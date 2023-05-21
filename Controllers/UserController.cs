using BasicDmgAPI.DAO;
using BasicDmgAPI.Entity;
using BasicDmgAPI.Service;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BasicDmgAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    public class UserController : ApiController
    {
        private UserService userService = new UserService(new UserDAO());

        [HttpPost, Route("users")]
        public HttpResponseMessage Auth([FromBody] User user)
        {
            if (userService.IsAuth(user))
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            } 
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        //[HttpPost, Route("users/create")]
        //public HttpResponseMessage Create([FromBody] User user)
        //{
        //    if (userService.Create(user) != null)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK);
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest);
        //    }
        //}
    }
}
