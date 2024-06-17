using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositoryLayer.CustomExecption;

namespace Fundoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollaboratorController : ControllerBase
    {
        private readonly ICollaboratorBL collaboratorBL;
        private readonly ResponseML responseML;

        public CollaboratorController(ICollaboratorBL collaboratorBL)
        {
            this.collaboratorBL = collaboratorBL;
            responseML = new ResponseML();
        }

        [HttpPost("add-collaborator")]
        public IActionResult AddCollaborator(CollaboratorML collaborator) 
        {
            try
            {
                var result =  collaboratorBL.AddCollaborator(collaborator);

                responseML.Success = true;
                responseML.Message = "Collaborator Added Successfully";
                responseML.Data = result;

                return StatusCode(201, responseML);
            }
            catch (CollaboratorException ex) 
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(400, responseML);
            }

        }

        [HttpDelete("remove-collaborator")]
        public IActionResult RemoveCollaborator(CollaboratorML collaborator) 
        {
            try
            {
                var result = collaboratorBL.RemoveCollaborator(collaborator);
                responseML.Success = true;
                responseML.Message = "Collaborator removed successfully";
                responseML.Data = result;

                return StatusCode(200, responseML);
            }
            catch (CollaboratorException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(404, responseML);
            }
        }

        [HttpGet("get-collaborators/{NoteId}")]
        public IActionResult GetCollaborators(int NoteId) {
            try
            {
                var result = collaboratorBL.GetCollaborators(NoteId);
                responseML.Success = true;
                responseML.Message = "Collaborator fetch successfully";
                responseML.Data = result;

                return StatusCode(200, responseML);
            }
            catch (CollaboratorException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(400, responseML);
            }
        }
    }
}
