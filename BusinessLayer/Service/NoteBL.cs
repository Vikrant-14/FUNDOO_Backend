﻿using BusinessLayer.Interface;
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
        public Note GetNoteById(int id)
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

        public List<Note> GetNotes()
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


    }
}