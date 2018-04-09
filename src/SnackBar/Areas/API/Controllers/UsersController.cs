using AutoMapper;
using SnackBar.BLL.Interfaces;
using SnackBar.ViewModels;
using Swashbuckle.Swagger.Annotations;
using System.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Http;

namespace SnackBar.Areas.API.Controllers
{
    public class UsersController : BaseApiController
    {
        public UsersController(IServiceFactory factory) : base(factory)
        {
        }

        /// <summary>
        /// Add new User by email
        /// </summary>
        /// <param name="email">New User's Email</param>
        [HttpPost]
        [Route("Api/Users")]
        [SwaggerResponse(200, Description = "Add New user by Email or resend User's PIN", Type = (typeof(UserApiViewModel)))]
        [SwaggerResponse(400, Description = "Invalid Email address")]
        public async Task<IHttpActionResult> Post([FromBody]string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var address = new MailAddress(email);
            string emailDomen = ConfigurationManager.AppSettings["EmailDomen"];
            if (!emailDomen.Equals(address.Host.ToLower()))
            {
                var errorMessage = $"Your email is not valid. Please use {ConfigurationManager.AppSettings["CompanyName"]} email.";
                return BadRequest(errorMessage);
            }

            var user = await Factory.GetUserService().RegisterUser(email);
            var returnResult = Mapper.Map<UserApiViewModel>(user);
            return Ok(returnResult);
        }

        /// <summary>
        /// Get User info by PIN
        /// </summary>
        /// <param name="pin">User PIN</param>
        [HttpGet]
        [Route("Api/Users/{pin}/info")]
        [SwaggerResponse(200, Description = "Get User's Info by PIN", Type = (typeof(UserIndexPageViewModel)))]
        [SwaggerResponse(400, Description = "Invalid input")]
        public IHttpActionResult GetUserInfo([FromUri]string pin)
        {
            if (string.IsNullOrEmpty(pin) || pin.Length != 4)
            {
                return BadRequest("Inccorect PIN");
            }

            var result = Factory.GetUserService().GetIndexPageInfo(pin);
            if (result == null)
            {
                return BadRequest("Incorrect PIN");
            }

            return Ok(result);
        }
    }
}