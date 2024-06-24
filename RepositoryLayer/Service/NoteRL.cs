using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using ModelLayer;
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

            return note;
        }
        
        public Note UpdateNote(int id, NoteML model)
        {
            var note = _context.Notes.FirstOrDefault(n => n.Id == id);

            if (note == null)
            {
                throw new NoteException("No note available");
            }

            note.Title = model.Title;
            note.Description = model.Description;
            note.IsArchived = model.IsArchived;
            note.IsTrashed = model.IsTrashed;

            _context.Notes.Update(note);
            _context.SaveChanges();

            _cache.Remove("GET_ALL_NOTES");
            
            return note;
        }

        public Note GetNoteById(int id)
        {
            var cacheKey = $"Note_{id}";

            var note = RedisCacheHelper.GetFromCache<Note>(cacheKey, _cache);

            if (note != null)
            {
                return note;
            }

            note = _context.Notes.FirstOrDefault(n => n.Id == id);

            if (note == null)
            {
                throw new NoteException($"Note ID : {id} does not exists");
            }
            
            RedisCacheHelper.SetToCache(cacheKey, _cache, note, 30, 15);

            return note;
        }

        public List<Note> GetNotes()
        {
            var cacheKey = "GET_ALL_NOTES";

            var notes = RedisCacheHelper.GetFromCache<List<Note>>(cacheKey, _cache);

            if (notes != null)
            {
                return notes.ToList();
            }

            notes = _context.Notes.ToList();

            if (notes == null)
            {
                throw new NoteException("No Note List Exists");
            }

            RedisCacheHelper.SetToCache(cacheKey,_cache,notes);

            return notes;
        }

        public Note DeleteNote(int id)
        {
            var note = _context.Notes.FirstOrDefault(n => n.Id == id);

            if (note == null)
            {
                throw new NoteException($"Note ID : {id} does not exists");
            }

            _context.Notes.Remove(note);
            _context.SaveChanges();

            _cache.Remove("GET_ALL_NOTES");

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
