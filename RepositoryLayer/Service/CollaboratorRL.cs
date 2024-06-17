using ModelLayer;
using Org.BouncyCastle.Crypto.Prng;
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
    public class CollaboratorRL : ICollaboratorRL
    {
        private readonly ApplicationDbContext _context;

        public CollaboratorRL(ApplicationDbContext context)
        {
            _context = context;
        }
        public CollaboratorEntity AddCollaborator(CollaboratorML collaborator)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == collaborator.CollaboratorEmail);
            var note = _context.Notes.FirstOrDefault(n => n.Id == collaborator.NoteId);

            if (user == null)
            {
                throw new CollaboratorException("No such Email exists");
            }

            if (note == null)
            {
                throw new CollaboratorException("No such Note exists");
            }

            CollaboratorEntity collaborator1 = new CollaboratorEntity()
            {
                CollaboratorEmail = collaborator.CollaboratorEmail, 
                NoteId = collaborator.NoteId
            };

            _context.Collaborators.Add(collaborator1);
            _context.SaveChanges();

            return collaborator1;
        }

        public IList<CollaboratorEntity> GetCollaborators(int NoteId)
        {
            var note = _context.Notes.FirstOrDefault(n => n.Id == NoteId);

            var collaborators = _context.Collaborators.Where(n => n.NoteId == NoteId).ToList();
            if (collaborators.Count() == 0)
            {
                throw new NoteException("No Note/Collaborators exists");
            }

            return collaborators;
        }

        public CollaboratorEntity RemoveCollaborator(CollaboratorML collaborator)
        {
            var collab = _context.Collaborators.Where(u => u.CollaboratorEmail == collaborator.CollaboratorEmail && u.NoteId == collaborator.NoteId).FirstOrDefault();

            if (collab == null)
            {
                throw new CollaboratorException("No collaborator exists");
            }

            _context.Collaborators.Remove(collab);
            _context.SaveChanges();

            return collab;
        }
    }
}
