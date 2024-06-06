using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositoryLayer.CustomExecption;

namespace Fundoo.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBL userBL;
        private readonly ResponseML responseML;

        public UserController(IUserBL userBL)
        {
            this.userBL = userBL;
            responseML = new ResponseML();
        }

        [HttpPost("adduser")]
        public IActionResult AddUser(UserML model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Where(x => x.Value.Errors.Any())
                                .ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage).ToArray());
                    return BadRequest(new ResponseML { Success = false, Message = "Validation Occured in {Phone number}", Data = null });
                }

                var result = userBL.AddUsers(model);
                responseML.Success = true;
                responseML.Message = "User Added Successfully";
                responseML.Data = result;

                if (result == null)
                {
                    responseML.Success = false;
                    responseML.Message = "Error  Ocurred while adding user";
                    return StatusCode(500, responseML);
                }
            }
            catch (UserException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;
                return StatusCode(500, responseML);
            }

            return StatusCode(200, responseML);
        }


        [HttpPost("register")]
        public IActionResult RegisterNewUser(UserML model)
        {
            try
            {
                var result = userBL.RegisterNewUser(model);

                if( result != null)
                {
                    responseML.Success = true;
                    responseML.Message = "User Registered Successfully";
                    responseML.Data = result;
                }

            }
            catch (UserException ex)
            {
                responseML.Success = false;
                responseML.Message = ex.Message;
                return StatusCode(400, responseML);
            }

            return StatusCode(201, responseML);
        }
    }
}
