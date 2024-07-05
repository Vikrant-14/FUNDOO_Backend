using BusinessLayer.Interface;
using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using Newtonsoft.Json;
using RepositoryLayer.CustomExecption;
using System.Runtime.InteropServices;

namespace Fundoo.Controllers
{
    [Route("api/label")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelBL labelBL;
        private readonly ResponseML responseML;
        private readonly ILogger<LabelController> _logger;
        private readonly ProducerConfig _prodConfig;
        private readonly IConfiguration _configuration;

        public LabelController(ILabelBL labelBL, ILogger<LabelController> logger, ProducerConfig prodConfig, IConfiguration configuration)
        {
            this.labelBL = labelBL; 
            responseML = new ResponseML();
            _logger = logger;
            _prodConfig = prodConfig;
            _configuration = configuration;
        }

        [HttpPost("create-label")]
        public async Task<ActionResult> CreateLabel(LabelML model)
        {
            var responseML = new ResponseML();
            try
            {
                var result = labelBL.CreateLabel(model); // Ensure labelBL is properly defined

                string serializedData = JsonConvert.SerializeObject(model);
                var topic = _configuration.GetValue<string>("TopicConfiguration:TopicName");

                using (var producer = new ProducerBuilder<Null, string>(_prodConfig).Build())
                {
                    await producer.ProduceAsync(topic, new Message<Null, string>
                    {
                        Value = serializedData
                    });

                    producer.Flush(TimeSpan.FromSeconds(10));
                }

                responseML.Success = true;
                responseML.Message = "Label added successfully";
                responseML.Data = result;

                _logger.LogInformation("Label Created and trace by NLOGGER");
                return StatusCode(201, responseML);
            }
            catch (LabelException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;

                return StatusCode(400, responseML);
            }
        }

        [HttpPut("update-label/{id}")]
        public async Task<ActionResult> UpdateLabel(int id,[FromBody] LabelML model)
        {
            try
            {
                var result = labelBL.UpdateLabel(id, model);

                int partition1 = 0;
                int partition2 = 1;

                string serializedData = JsonConvert.SerializeObject(result);
                var topic = _configuration.GetValue<string>("TopicConfiguration:TopicName");

                using ( var producer = new ProducerBuilder<int,string>(_prodConfig).Build() ) 
                {
                    if( result.Id % 2 == 0 )
                    {
                        await producer.ProduceAsync(new TopicPartition(topic,new Partition(partition1)), new Message<int, string>
                        {
                            Key = partition1,
                            Value = serializedData
                        });
                    }
                    else
                    {
                        await producer.ProduceAsync(new TopicPartition(topic, new Partition(partition2)), new Message<int, string>
                        {
                            Key = partition2,
                            Value = serializedData
                        });
                    }
                }

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