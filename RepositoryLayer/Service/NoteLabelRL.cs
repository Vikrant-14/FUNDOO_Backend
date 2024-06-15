using Microsoft.EntityFrameworkCore;
using ModelLayer;
using RepositoryLayer.Context;
using RepositoryLayer.CustomExecption;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class NoteLabelRL : INoteLabelRL
    {
        private readonly ApplicationDbContext _context;

        public NoteLabelRL(ApplicationDbContext context)
        {
            _context = context;
        }

        public NoteLabelEntity AddNoteToLabel(NoteLabelML model)
        {
            var note = _context.Notes.FirstOrDefault(n => n.Id == model.NoteId);
            var label = _context.Labels.FirstOrDefault(l => l.Id == model.LabelId);

            if (note == null || label == null) 
            {
                throw new NoteLabelException("invalid noteID/LabelID");
            }

            NoteLabelEntity noteLabel = new NoteLabelEntity()
            {
                NoteId = model.NoteId,
                LabelId = model.LabelId
            };

            _context.NoteLabelEntities.Add(noteLabel);
            _context.SaveChanges();
            return noteLabel;
        }

        public IEnumerable<Label> GetLabelFromNote(int NoteID)
        {
            var matchLabelNote = _context.Notes.Include(n => n.NoteLabel).ThenInclude(l => l.Label).FirstOrDefault(n => n.Id == NoteID);

            if (matchLabelNote == null)
            {

                throw new NoteLabelException("No note available");
            }

            var labels = matchLabelNote.NoteLabel.Select(n => n.Label).ToList();
            
            return labels;
        }

        public IEnumerable<Note> GetNoteFromLabel(int LabelID)
        {
            var matchNoteByLabel = _context.Labels.Include(l => l.NoteLabel).ThenInclude(n => n.Note).FirstOrDefault(l => l.Id == LabelID);

            if (matchNoteByLabel == null)
            {
                throw new NoteLabelException("No label available");
            }

            var notes = matchNoteByLabel.NoteLabel.Select(n => n.Note).ToList();

            return notes;
        }

        public NoteLabelEntity RemoveNoteFromLabel(NoteLabelML model)
        {
            var note = _context.Notes.FirstOrDefault(n => n.Id == model.NoteId);
            var label = _context.Labels.FirstOrDefault(l => l.Id == model.LabelId);

            if (note == null || label == null)
            {
                throw new NoteLabelException("invalid noteID/LabelID");
            }

            NoteLabelEntity noteLabel = new NoteLabelEntity()
            {
                NoteId = model.NoteId,
                LabelId = model.LabelId
            };

            _context.NoteLabelEntities.Remove(noteLabel); 
            _context.SaveChanges();
            return noteLabel;
        }
    }
}
