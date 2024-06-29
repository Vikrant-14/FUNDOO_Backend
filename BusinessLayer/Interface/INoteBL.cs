using ModelLayer;
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
        public IList<NoteWithLabelDTO> GetNotes();
        public NoteWithLabelDTO GetNoteById(int id);
        public Note CreateNote(NoteML model);
        public Task<Note> CreateNoteAsync(NoteML model);
        public Note UpdateNote(int id, NoteML model);
        public Note DeleteNote(int id);
        public Note Archived(int id);
        public Note Trashed(int id);
        public IList<NoteWithLabelDTO> GetAllTrashedNotes();
        public IList<NoteWithLabelDTO> GetAllArchivedNotes();
        public IEnumerable<NoteWithLabelDTO> GetAllNotesWithLabels();
    }
}
