using BusinessLayer.Interface;
using ModelLayer;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class CollaboratorBL : ICollaboratorBL
    {
        private readonly ICollaboratorRL collaboratorRL;

        public CollaboratorBL(ICollaboratorRL collaboratorRL)
        {
            this.collaboratorRL = collaboratorRL;
        }

        public CollaboratorEntity AddCollaborator(CollaboratorML collaborator)
        {
            try
            {
                return collaboratorRL.AddCollaborator(collaborator);
            }
            catch
            {
                throw;
            }
        }

        public IList<CollaboratorEntity> GetCollaborators(int NoteId)
        {
            try
            {
                return collaboratorRL.GetCollaborators(NoteId);
            }
            catch
            {
                throw;
            }
        }

        public CollaboratorEntity RemoveCollaborator(CollaboratorML collaborator)
        {
            try
            {
                return collaboratorRL.RemoveCollaborator(collaborator);
            }
            catch
            {
                throw;
            }
        }
    }
}
