﻿using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface INoteLabelRL
    {
        public NoteLabelEntity AddNoteToLabel(NoteLabelML model);
        public NoteLabelEntity RemoveNoteFromLabel(NoteLabelML model);
        public IEnumerable<Note> GetNoteFromLabel(int LabelID);
        public IEnumerable<Label> GetLabelFromNote(int NoteID);
    }
}
