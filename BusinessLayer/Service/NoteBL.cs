using BusinessLayer.Interface;
using ModelLayer;
using RepositoryLayer.CustomExecption;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class NoteBL : INoteBL
    {
        private readonly INoteRL noteRL;

        public NoteBL(INoteRL noteRL)
        {
            this.noteRL = noteRL;
        }
        public Note CreateNote(NoteML model)
        {
            try
            {
                return noteRL.CreateNote(model);
            }
            catch(NoteException)
            {
                throw;
            }
        }

        public Task<Note> CreateNoteAsync(NoteML model) 
        {
            try
            {
                return noteRL.CreateNoteAsync(model);
            }
            catch (NoteException)
            {
                throw;
            }
        }
        public Note UpdateNote(int id, NoteML model)
        {
            try
            {
                return noteRL.UpdateNote(id, model);
            }
            catch (NoteException) 
            {
                throw;
            }
        }
        public NoteWithLabelDTO GetNoteById(int id)
        {
            try
            {
                return noteRL.GetNoteById(id);
            }
            catch (NoteException)
            {
                throw;
            }
        }

        public IList<NoteWithLabelDTO> GetNotes()
        {
            try
            {
                return noteRL.GetNotes();
            }
            catch (NoteException)
            {
                throw;
            }
        }

        public Note DeleteNote(int id)
        {
            try
            {
                return noteRL.DeleteNote(id);
            }
            catch (NoteException)
            {
                throw;
            }
        }

        public Note Archived(int id)
        {
            try
            {
                return noteRL.Archived(id);
            }
            catch (NoteException)
            {
                throw;
            }
        }

        public Note Trashed(int id)
        {
            try
            {
                return noteRL.Trashed(id);
            }
            catch (NoteException)
            {
                throw;
            }
        }

        public IList<NoteWithLabelDTO> GetAllTrashedNotes()
        {
            try
            {
                return noteRL.GetAllTrashedNotes();
            }
            catch (NoteException)
            {
                throw;
            }
        }

        public IList<NoteWithLabelDTO> GetAllArchivedNotes()
        {
            try
            {
                return noteRL.GetAllArchivedNotes();
            }
            catch (NoteException)
            {
                throw;
            }
        }

        public IEnumerable<NoteWithLabelDTO> GetAllNotesWithLabels()
        {
            try
            {
                return noteRL.GetAllNotesWithLabels();
            }
            catch (NoteException)
            {
                throw;
            }
        }
    }
}
