using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface ICollaboratorBL
    {
        public CollaboratorEntity AddCollaborator(CollaboratorML collaborator);
        public CollaboratorEntity RemoveCollaborator(CollaboratorML collaborator);
        public IList<CollaboratorEntity> GetCollaborators(int NoteId);
    }
}
