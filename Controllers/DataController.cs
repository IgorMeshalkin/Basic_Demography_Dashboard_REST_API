using BasicDmgAPI.DAO;
using BasicDmgAPI.Entity;
using BasicDmgAPI.Models;
using BasicDmgAPI.Service;
using BasicDmgAPI.Util;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using FromBodyAttribute = System.Web.Http.FromBodyAttribute;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace BasicDmgAPI.Controllers
{
    [Route("data")]
    [EnableCors("*", "*", "*")]
    public class DataController : ApiController
    {
        DataService dataService = new DataService(new DataDAO(), new PeriodDAO());
        UserService userService = new UserService(new UserDAO());

        [HttpGet]
        public HttpResponseMessage Get([FromQuery] int districtID, [FromQuery] int periodID)
        {
            Data result = dataService.GetData(districtID, periodID);
            return Request.CreateResponse<Data>(HttpStatusCode.OK, result);
        }

        [HttpPost]
        public HttpResponseMessage Update([FromBody] DataWithCredentials dataWithCredentials)
        {
            Data data = dataWithCredentials.GetData();
            User user = new User();
            user.Password = dataWithCredentials.Password;

            if (!userService.IsAuth(user))
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            Data updatedData = dataService.Update(data);

            if (updatedData != null && updatedData.Equals(data))
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            } 
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }

}
