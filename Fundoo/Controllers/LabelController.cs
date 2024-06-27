using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositoryLayer.CustomExecption;

namespace Fundoo.Controllers
{
    [Route("api/label")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelBL labelBL;
        private readonly ResponseML responseML;

        private readonly ILogger<LabelController> _logger;   

        public LabelController(ILabelBL labelBL, ILogger<LabelController> logger)
        {
            this.labelBL = labelBL; 
            responseML = new ResponseML();
            _logger = logger;
        }

        [HttpPost("create-label")]
        public IActionResult CreateLabel(LabelML model)
        {
            try
            {

  
                var result = labelBL.CreateLabel(model);

                responseML.Success = true;
                responseML.Message = "Label added successfully";
                responseML.Data = result;

                _logger.LogInformation("Label Created and trace by NLOGGER");
                return StatusCode(201, responseML);
            }
            catch(LabelException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(400, responseML);
            }
        }

        [HttpPut("update-label")]
        public IActionResult UpdateLabel(string labelName, LabelML model)
        {
            try
            {
                var result = labelBL.UpdateLabel(labelName, model);

                responseML.Success = true;
                responseML.Message = "Label updated successfully";
                responseML.Data = result;

                return StatusCode(200, responseML);
            }
            catch (LabelException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(404, responseML);
            }
        }

        [HttpGet("getById/{id}")]
        public IActionResult GetLabelById(int id)
        {
            try
            {
                var result = labelBL.GetLabelById(id);

                responseML.Success = true;
                responseML.Message = $"Label ID : {id} fetched successfully";
                responseML.Data = result;

                return StatusCode(200, responseML);
            }
            catch (LabelException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(404, responseML);
            }
        }

        [HttpGet("getall")]
        public IActionResult GetAllLabels()
        {
            try
            {
                var result = labelBL.GetAllLabels();

                responseML.Success = true;
                responseML.Message = $"All Label fetched successfully";
                responseML.Data = result;

                return StatusCode(200, responseML);
            }
            catch (LabelException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(404, responseML);
            }
        }

        [HttpDelete("deleteById/{id}")]
        public IActionResult DeleteLabel(int id)
        {
            try
            {
                var result = labelBL.DeleteLabel(id);

                responseML.Success = true;
                responseML.Message = $"Label ID : {id} deleted successfully";
                responseML.Data = result;

                return StatusCode(200, responseML);
            }
            catch (LabelException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(404, responseML);
            }
        }
    }
}
