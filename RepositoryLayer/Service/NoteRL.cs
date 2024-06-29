using Microsoft.AspNetCore.Mvc;
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

            _cache.Remove(cacheKey);

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

           _cache.Remove(cacheKey);

            return note;
        }
        
        public Note UpdateNote(int id, NoteML model)
        {
           var note = _context.Notes.Find(id);

            if (note != null)
            {
                note.Title = model.Title;
                note.Description = model.Description;
                note.IsArchived = model.IsArchived;
                note.IsTrashed = model.IsTrashed;

                _context.SaveChanges();
                _cache.Remove(cacheKey);

                return note;
            }
            else
            {
                throw new NoteException($"Note Id : {id} does not exists");
            }

        }

        public NoteWithLabelDTO GetNoteById(int id)
        {
            var cachedNotes = RedisCacheHelper.GetFromCache<List<NoteWithLabelDTO>>(cacheKey, _cache);
            NoteWithLabelDTO note;

            if (cachedNotes != null)
            {
                note = cachedNotes.FirstOrDefault(n => n.NoteId == id);

                if (note != null)
                {
                    return note;
                }
            }

            var notes = _context.Notes.Include(n => n.NoteLabel)
                                    .ThenInclude(l => l.Label)
                                    .Select(n => new NoteWithLabelDTO
                                    {
                                        NoteId = n.Id,
                                        Title = n.Title,
                                        Description = n.Description,
                                        IsTrashed = n.IsTrashed,
                                        IsArchived = n.IsArchived,
                                        Labels = n.NoteLabel.Select(l => new LabelDTO
                                        {
                                            LableId = l.Label.Id,
                                            LabelName = l.Label.LabelName
                                        }).ToList()
                                    }).ToList();

            if (notes.Count == 0)
            {
                throw new NoteException("No Notes found");
            }
            else
            {
                RedisCacheHelper.SetToCache(cacheKey, _cache, notes, 30, 15);
                var note1 = notes.FirstOrDefault(n => n.NoteId == id);

                if(note1 == null)
                {
                    throw new NoteException($"Note ID : {id} does not exists");
                }

                return note1;
            }
        }

        public IList<NoteWithLabelDTO> GetNotes()
        {
            var cachedNotes = RedisCacheHelper.GetFromCache<List<NoteWithLabelDTO>>(cacheKey, _cache);
            IList<NoteWithLabelDTO> notes;

            if (cachedNotes != null)
            {
                notes = cachedNotes.Where(n => n.IsArchived == false && n.IsTrashed == false).ToList();

                if (notes.Count != 0)
                {
                    return notes;  
                }
            }

            notes = _context.Notes.Include(n => n.NoteLabel)
                                    .ThenInclude(l => l.Label)
                                    .Select(n => new NoteWithLabelDTO
                                    {
                                        NoteId = n.Id,
                                        Title = n.Title,
                                        Description = n.Description,
                                        IsTrashed = n.IsTrashed,
                                        IsArchived = n.IsArchived,
                                        Labels = n.NoteLabel.Select(l => new LabelDTO
                                        {
                                            LableId = l.Label.Id,
                                            LabelName = l.Label.LabelName
                                        }).ToList()
                                    }).ToList();

            if (notes.Count == 0)
            {
                throw new NoteException("No Notes found");
            }
            else
            {
                RedisCacheHelper.SetToCache(cacheKey, _cache, notes, 30, 15);
                return notes.Where(n => n.IsArchived == false && n.IsTrashed == false).ToList();
            }
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

            _cache.Remove(cacheKey);

            return note;
        }

        public Note Archived(int id)
        {
            //var note = _context.Notes.FirstOrDefault(n => n.Id == id);
            var noteList = _context.Notes.ToList();
            var note = noteList.FirstOrDefault(n => n.Id == id);

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

            _cache.Remove(cacheKey);

            return note;
        }

        public Note Trashed(int id)
        {
            //var note = _context.Notes.FirstOrDefault(n => n.Id == id);
            var noteList = _context.Notes.ToList();
            var note = noteList.FirstOrDefault(n => n.Id == id);

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

            _cache.Remove(cacheKey);

            return note;
        }

        public IList<NoteWithLabelDTO> GetAllTrashedNotes() 
        {
            var cachedNotes = RedisCacheHelper.GetFromCache<List<NoteWithLabelDTO>>(cacheKey, _cache);
            IList<NoteWithLabelDTO> notes;

            if (cachedNotes != null)
            {
                notes = cachedNotes.Where(n => n.IsTrashed == true).ToList();

                if (notes.Count != 0)
                {
                    return notes;
                }
            }

            notes = _context.Notes.Include(n => n.NoteLabel)
                                    .ThenInclude(l => l.Label)
                                    .Select(n => new NoteWithLabelDTO
                                    {
                                        NoteId = n.Id,
                                        Title = n.Title,
                                        Description = n.Description,
                                        IsTrashed = n.IsTrashed,
                                        IsArchived = n.IsArchived,
                                        Labels = n.NoteLabel.Select(l => new LabelDTO
                                        {
                                            LableId = l.Label.Id,
                                            LabelName = l.Label.LabelName
                                        }).ToList()
                                    }).ToList();

            if (notes.Count == 0)
            {
                throw new NoteException("No Notes found");
            }
            else
            {
                RedisCacheHelper.SetToCache(cacheKey, _cache, notes, 30, 15);
                return notes.Where(n => n.IsTrashed == true).ToList();
            }
        }

        public IList<NoteWithLabelDTO> GetAllArchivedNotes() 
        {
            var cachedNotes = RedisCacheHelper.GetFromCache<List<NoteWithLabelDTO>>(cacheKey, _cache);
            IList<NoteWithLabelDTO> notes;

            if (cachedNotes != null)
            {
                notes = cachedNotes.Where(n => n.IsArchived == true).ToList();

                if (notes.Count != 0)
                {
                    return notes;
                }
            }

            notes = _context.Notes.Include(n => n.NoteLabel)
                                    .ThenInclude(l => l.Label)
                                    .Select(n => new NoteWithLabelDTO
                                    {
                                        NoteId = n.Id,
                                        Title = n.Title,
                                        Description = n.Description,
                                        IsTrashed = n.IsTrashed,
                                        IsArchived = n.IsArchived,
                                        Labels = n.NoteLabel.Select(l => new LabelDTO
                                        {
                                            LableId = l.Label.Id,
                                            LabelName = l.Label.LabelName
                                        }).ToList()
                                    }).ToList();

            if (notes.Count == 0)
            {
                throw new NoteException("No Notes found");
            }
            else
            {
                RedisCacheHelper.SetToCache(cacheKey, _cache, notes, 30, 15);
                return notes.Where(n => n.IsArchived == true).ToList();
            }
        }


        public IEnumerable<NoteWithLabelDTO> GetAllNotesWithLabels()
        {
            //string cacheKey = "getNotesWithLabels";

            // Try to get the data from cache
            var notesWithLabelsFromCache = RedisCacheHelper.GetFromCache<List<NoteWithLabelDTO>>(cacheKey, _cache);

            // If cache is not empty, return the cached data
            if (notesWithLabelsFromCache != null && notesWithLabelsFromCache.Any())
            {
                return notesWithLabelsFromCache;
            }

            // Fetch data from database if cache is empty
            var notesWithLabelsFromDb = _context.Notes.Include(n => n.NoteLabel)
                                                      .ThenInclude(l => l.Label)
                                                      .Select(n => new NoteWithLabelDTO
                                                      {
                                                          NoteId = n.Id,
                                                          Title = n.Title,
                                                          Description = n.Description,
                                                          IsTrashed = n.IsTrashed,
                                                          IsArchived = n.IsArchived,
                                                          Labels = n.NoteLabel.Select(l => new LabelDTO
                                                          {
                                                              LableId = l.Label.Id,
                                                              LabelName = l.Label.LabelName
                                                          }).ToList()
                                                      }).ToList();

            // If no notes and labels found in the database, throw an exception
            if (!notesWithLabelsFromDb.Any())
            {
                throw new NoteException("No notes and label found");
            }

            // Set the data to cache
            RedisCacheHelper.SetToCache(cacheKey, _cache, notesWithLabelsFromDb, 30, 15);

            return notesWithLabelsFromDb;
        }

    }
}
