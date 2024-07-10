using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositoryLayer.CustomExecption;
using RepositoryLayer.Entity;
using RepositoryLayer.Utility;

namespace Fundoo.Controllers
{
    [Route("api/note")]
    [ApiController]
    [EnableCors]
    public class NoteController : ControllerBase
    {
        private readonly INoteBL noteBL;
        private readonly ResponseML responseML;
        private readonly RabbitMQProducer _rabbitMQProducer;
        public NoteController(INoteBL noteBL, RabbitMQProducer rabbitMQProducer)
        {
            this.noteBL = noteBL;
            responseML = new ResponseML();
            _rabbitMQProducer = rabbitMQProducer;
        }

        [HttpPost("createnote")]
        [Authorize(Roles ="Admin")]
        public IActionResult CreateNote([FromBody] NoteML noteML)
        {
            try
            {
                var note = noteBL.CreateNote(noteML);

                //send inserted note data to the queue and consumer will listening this data from queue
                //_rabbitMQProducer.SendMessage(note);

                responseML.Success = true;
                responseML.Message = "Note Created Successfully";
                responseML.Data = note;

                return StatusCode(201,responseML);
            }
            catch (NoteException ex)
            {
                responseML.Success = false;
                responseML.Message = "Error Occurred while creating note";

                return StatusCode(400, responseML);
            }
        }

        [HttpPost("create-note-async")]
        public async Task<ActionResult> CreateNoteAsync(NoteML model)
        {
            try
            {
                var note = await noteBL.CreateNoteAsync(model);

                responseML.Success = true;
                responseML.Message = "Note Created Successfully";
                responseML.Data = note;

                return StatusCode(201, responseML);
            }
            catch (NoteException ex)
            {
                responseML.Success = false;
                responseML.Message = "Error Occurred while creating note";

                return StatusCode(400, responseML);
            }
        }


        [HttpPut("updatenote/{id}")]
        public IActionResult UpdateNote(int id, [FromBody] NoteML noteML)
        {
            try
            {
                var updatedNote = noteBL.UpdateNote(id, noteML);

                responseML.Success = true;
                responseML.Message = "Note Updated Successfully";
                responseML.Data = updatedNote;

                return StatusCode(200, responseML);
            }
            catch (NoteException ex) 
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(404, responseML);
            }
        }

        [EnableCors("AnotherCorsPolicy")]
        [HttpGet("getallnotes")]
        //[Authorize(Roles = "Admin")]
        public ActionResult<List<Note>> GetNotes()
        {
            try
            {
                var notes = noteBL.GetNotes();

                responseML.Success = true;
                responseML.Message = "All Notes Fetched Successfully";
                responseML.Data = notes;

                return StatusCode(200, responseML);
            }
            catch (NoteException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(400, responseML);
            }
        }

        [HttpGet("getnotebyid/{id}")]
        public IActionResult GetNoteById(int id)
        {
            try
            {
                var note = noteBL.GetNoteById(id);

                responseML.Success = true;
                responseML.Message = "Note Fetch Successfully";
                responseML.Data = note;

                return StatusCode(200, responseML);
            }
            catch (NoteException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(404, responseML);
            }
        }

        [HttpDelete("deletebyid")]
        //[Authorize(Roles ="Admin")]
        public IActionResult DeleteNote(int id)
        {
            try
            {
                var note = noteBL.DeleteNote(id);

                responseML.Success = true;
                responseML.Message = "Note Deleted Successfully";
                responseML.Data = note;

                return StatusCode(200, responseML);
            }
            catch (NoteException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(404, responseML);
            }
        }

        [HttpPatch("archivedpost")]
        public IActionResult Archived(int id)
        {
            try
            {
                var note = noteBL.Archived(id);

                responseML.Success = true;
                responseML.Message = $"Note Id : {id} archived succesfully";
                responseML.Data = note;

                return StatusCode(200, responseML);
            }
            catch(NoteException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(400, responseML);
            }
        }

        [HttpPatch("trashedpost")]
        public IActionResult Trashed(int id)
        {
            try
            {
                var note = noteBL.Trashed(id);

                responseML.Success = true;
                responseML.Message = $"Note Id : {id} trashed succesfully";
                responseML.Data = note;

                return StatusCode(200, responseML);
            }
            catch (NoteException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(400, responseML);
            }
        }

        [HttpGet("get-all-trashed")]
        public IActionResult GetAllTrashedNotes()
        {
            try
            {
                var note = noteBL.GetAllTrashedNotes();

                responseML.Success = true;
                responseML.Message = "Note Fetch Successfully from Trashed";
                responseML.Data = note;

                return StatusCode(200, responseML);
            }
            catch (NoteException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(404, responseML);
            }
        }

        [HttpGet("get-all-archived")]
        public IActionResult GetAllArchivedNotes()
        {
            try
            {
                var note = noteBL.GetAllArchivedNotes();

                responseML.Success = true;
                responseML.Message = "Note Fetch Successfully from archived";
                responseML.Data = note;

                return StatusCode(200, responseML);
            }
            catch (NoteException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(404, responseML);
            }
        }

        [HttpGet("get-all-notes-with-labels")]
        public IActionResult GetAllNotesWithLabels()
        {
            try
            {
                var notes = noteBL.GetAllNotesWithLabels();

                responseML.Success = true;
                responseML.Message = "Note Fetch Successfully";
                responseML.Data = notes;

                return StatusCode(200, responseML);
            }
            catch (NoteException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(404, responseML);
            }
        }
    }
}
