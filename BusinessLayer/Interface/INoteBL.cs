﻿using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface INoteBL
    {
        public List<Note> GetNotes();
        public Note GetNoteById(int id);
        public Note CreateNote(NoteML model);
        public Task<Note> CreateNoteAsync(NoteML model);
        public Note UpdateNote(int id, NoteML model);
        public Note DeleteNote(int id);
        public Note Archived(int id);
        public Note Trashed(int id);
        public IList<Note> GetAllTrashedNotes();
    }
}
