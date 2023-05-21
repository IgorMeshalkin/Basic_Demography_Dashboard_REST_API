using BasicDmgAPI.DAO;
using BasicDmgAPI.Entity;
using BasicDmgAPI.Service;
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
    [EnableCors("*", "*", "*")]
    public class PeriodController : ApiController
    {
        private PeriodService periodService = new PeriodService(new PeriodDAO(), new DistrictDAO(), new DataDAO());

        [HttpGet, Route("periods")]
        public HttpResponseMessage GetAll()
        {
            List<Period> result = periodService.GetAll();
            return Request.CreateResponse<List<Period>>(HttpStatusCode.OK, result);
        }

        [HttpGet, Route("periods/united")]
        public HttpResponseMessage GetAllUnited()
        {
            List<Period> result = periodService.GetAllUnited();
            return Request.CreateResponse<List<Period>>(HttpStatusCode.OK, result);
        }

        [HttpPost, Route("periods")]
        public HttpResponseMessage CreatePeriod([FromBody] Period period)
        {
            Period result = periodService.Create(period);
            return Request.CreateResponse<Period>(HttpStatusCode.OK, result);
        }
    }
}
