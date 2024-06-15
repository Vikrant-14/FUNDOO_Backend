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
    public class NoteLabelBL : INoteLabelBL
    {
        private readonly INoteLabelRL noteLabelRL;

        public NoteLabelBL(INoteLabelRL noteLabelRL)
        {
            this.noteLabelRL = noteLabelRL;
        }
        public NoteLabelEntity AddNoteToLabel(NoteLabelML model)
        {
            try
            {
                return noteLabelRL.AddNoteToLabel(model);
            }
            catch (NoteLabelException)
            {
                throw;
            }
        }

        public IEnumerable<Label> GetLabelFromNote(int NoteID)
        {
            try
            {
                return noteLabelRL.GetLabelFromNote(NoteID);
            }
            catch (NoteLabelException)
            {
                throw;
            }
        }

        public IEnumerable<Note> GetNoteFromLabel(int LabelID)
        {
            try
            {
                return noteLabelRL.GetNoteFromLabel(LabelID);
            }
            catch (NoteLabelException)
            {
                throw;
            }
        }

        public NoteLabelEntity RemoveNoteFromLabel(NoteLabelML model)
        {
            try
            {
                return noteLabelRL.RemoveNoteFromLabel(model);
            }
            catch (NoteLabelException)
            {
                throw;
            }
        }
    }
}
