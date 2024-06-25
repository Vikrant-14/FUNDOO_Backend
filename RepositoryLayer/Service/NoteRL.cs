using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using ModelLayer;
using Org.BouncyCastle.Utilities;
using RepositoryLayer.Context;
using RepositoryLayer.CustomExecption;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class NoteRL : INoteRL
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _cache;
        string cacheKey = "GET_ALL_NOTES";
        public NoteRL(ApplicationDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public  Note CreateNote(NoteML model)
        {
            Note note = new Note();

            note.Title = model.Title;
            note.Description = model.Description;
            note.IsArchived = model.IsArchived;
            note.IsTrashed = model.IsTrashed;

            _context.Notes.Add(note);
            _context.SaveChanges();

            var noteList = _context.Notes.ToList();
            RedisCacheHelper.SetToCache(cacheKey, _cache, noteList, 30, 15);

            return note;
        }

        public async Task<Note> CreateNoteAsync(NoteML model)
        {
            Note note = new Note();

            note.Title = model.Title;
            note.Description = model.Description;
            note.IsArchived = model.IsArchived;
            note.IsTrashed = model.IsTrashed;

            await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();

            var noteList = _context.Notes.ToList();
            RedisCacheHelper.SetToCache(cacheKey, _cache, noteList, 30, 15);
            return note;
        }
        
        public Note UpdateNote(int id, NoteML model)
        {
            var notes = RedisCacheHelper.GetFromCache<List<Note>>(cacheKey, _cache);
            Note note;

            if (notes != null)
            {
                note = notes.FirstOrDefault(n => n.Id == id);
                if (note != null)
                {
                    // Update the note in the cached list
                    note.Title = model.Title;
                    note.Description = model.Description;
                    note.IsArchived = model.IsArchived;
                    note.IsTrashed = model.IsTrashed;

                    // Update the note in the database
                    _context.Notes.Update(note);
                    _context.SaveChanges();

                    // Update the note in the cache
                    foreach (var note2 in notes)
                    {
                        if (note2.Id == id)
                        {
                            note2.Title = model.Title;
                            note2.Description = model.Description;
                            note2.IsArchived = model.IsArchived;
                            note2.IsTrashed = model.IsTrashed;
                        }
                    }

                    // Update the cache with the modified note list
                    RedisCacheHelper.SetToCache(cacheKey, _cache, notes, 30, 15);

                    return note;
                }
            }

            // If the note was not found in the cache, get it from the database
            note = _context.Notes.FirstOrDefault(n => n.Id == id);
            if (note == null)
            {
                throw new NoteException("No note available");
            }

            // Update the note properties
            note.Title = model.Title;
            note.Description = model.Description;
            note.IsArchived = model.IsArchived;
            note.IsTrashed = model.IsTrashed;

            // Update the note in the database
            _context.Notes.Update(note);
            _context.SaveChanges();

            var noteList = _context.Notes.ToList();

            RedisCacheHelper.SetToCache(cacheKey, _cache, noteList, 30, 15);

            return note;
        }

        public Note GetNoteById(int id)
        {
            var notes = RedisCacheHelper.GetFromCache<List<Note>>(cacheKey, _cache);
            Note note;

            if (notes != null)
            {
               note = notes.FirstOrDefault(n => n.Id == id);
                if (note != null)
                {
                    return note;
                }
            }

            note = _context.Notes.FirstOrDefault(n => n.Id == id);
            notes = _context.Notes.ToList();
            if (note == null)
            {
                throw new NoteException($"Note ID : {id} does not exists");
            }
            

            RedisCacheHelper.SetToCache(cacheKey, _cache, notes, 30, 15);

            return note;
        }

        public List<Note> GetNotes()
        {
            var notes = RedisCacheHelper.GetFromCache<List<Note>>(cacheKey, _cache);
            
            if (notes != null)
            {
                return notes.Where(n => n.IsTrashed == false && n.IsArchived == false).ToList();
            }

            notes = _context.Notes.Where(n => n.IsTrashed == false && n.IsArchived == false).ToList();
            var noteList = _context.Notes.ToList();

            if (notes == null)
            {
                throw new NoteException("No Note List Exists");
            }

            RedisCacheHelper.SetToCache(cacheKey,_cache, noteList);

            return notes;
        }

        public Note DeleteNote(int id)
        {
            var noteList = _context.Notes.ToList();
            var note = noteList.FirstOrDefault(n => n.Id == id);

            if (note == null)
            {
                throw new NoteException($"Note ID : {id} does not exists");
            }

            _context.Notes.Remove(note);
            _context.SaveChanges();

            RedisCacheHelper.SetToCache(cacheKey, _cache, noteList, 30, 15);

            return note;
        }

        public Note Archived(int id)
        {
            var note = _context.Notes.FirstOrDefault(n => n.Id == id);

            if (note == null)
            {
                throw new NoteException($"Note ID : {id} does not exists");
            }

            if(note.IsTrashed == true)
            {
                throw new NoteException($"Note ID : {id} is already trashed; cannot archived");
            }

            note.IsArchived = !note.IsArchived;
  
            // note.IsArchived = note.IsArchived ? false : true;

            _context.Notes.Update(note);
            _context.SaveChanges();

            return note;
        }

        public Note Trashed(int id)
        {
            var note = _context.Notes.FirstOrDefault(n => n.Id == id);

            if (note == null)
            {
                throw new NoteException($"Note ID : {id} does not exists");
            }

            if (note.IsArchived == true)
            {
                note.IsArchived = !note.IsArchived;
            }

            note.IsTrashed = !note.IsTrashed;

            if(note.IsTrashed == false) 
            {
                note.IsArchived = !note.IsArchived;
            }

            // note.IsTrashed = note.IsTrashed ? false : true;

            _context.Notes.Update(note);
            _context.SaveChanges();

            return note;
        }

        public IList<Note> GetAllTrashedNotes() 
        {
            var notes = _context.Notes.Where(n => n.IsTrashed == true).ToList();

            if(notes.Count == 0)
            {
                throw new NoteException("No Notes inside trashed");
            }

            return notes;
        }

        public IList<Note> GetAllArchivedNotes() 
        {
            var notes = _context.Notes.Where(n => n.IsArchived == true).ToList();

            if (notes.Count == 0)
            {
                throw new NoteException("No Notes archived");
            }

            return notes;
        }
    }
}
