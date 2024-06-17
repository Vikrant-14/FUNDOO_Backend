using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ModelLayer;
using RepositoryLayer.CustomExecption;
using RepositoryLayer.Entity;

namespace Fundoo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteLabelController : ControllerBase
    {
        private readonly INoteLabelBL noteLabelBL;
        private readonly ResponseML responseML;

        public NoteLabelController(INoteLabelBL noteLabelBL)
        {
            this.noteLabelBL = noteLabelBL;
            responseML = new ResponseML();
        }

        [HttpPost("Add-notelabel")]
        public IActionResult AddLabelToNote(NoteLabelML model) 
        {
            try
            {
                var result = noteLabelBL.AddLabelToNote(model); 

                responseML.Success = true;
                responseML.Message = "Label added to note successfully";
                responseML.Data = result;

                return StatusCode(201, responseML);
            }
            catch (NoteLabelException ex) 
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(400, responseML);
            }
            catch(SqlException ex)
            {
                responseML.Success = false;
                responseML.Message = "You cannot add same note to label because it already Exists";

                return StatusCode(400, responseML);
            }
        }

        [HttpGet("getnotebylabel/{LabelID}")]
        public IActionResult GetNoteFromLabel(int LabelID)
        {
            try
            {
                var result = noteLabelBL.GetNoteFromLabel(LabelID);

                responseML.Success = true;
                responseML.Message = "Notes fetched successfully";
                responseML.Data = result;

                return StatusCode(200, responseML);
            }
            catch (NoteLabelException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(404, responseML);
            }
            catch (SqlException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(400, responseML);
            }
        }

        [HttpGet("getlabelbynote/{NoteID}")]
        public IActionResult GetLabelFromNote(int NoteID) 
        {
            try
            {
                var result = noteLabelBL.GetLabelFromNote(NoteID);

                responseML.Success = true;
                responseML.Message = "Label fetched successfully";
                responseML.Data = result;

                return StatusCode(200, responseML);
            }
            catch (NoteLabelException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(404, responseML);
            }
            catch (SqlException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(400, responseML);
            }
        }

        [HttpDelete("delete-notelabel")]
        public IActionResult RemoveLabelFromNote(NoteLabelML model)
        {
            try
            {
                var result = noteLabelBL.RemoveLabelFromNote(model);

                responseML.Success = true;
                responseML.Message = "Label remove from note successfully";
                responseML.Data = result;

                return StatusCode(200, responseML);
            }
            catch (NoteLabelException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(400, responseML);
            }
            catch(SqlException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(400, responseML);
            }
        }
    }
}
