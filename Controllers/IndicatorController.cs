using BasicDmgAPI.DAO;
using BasicDmgAPI.Entity;
using BasicDmgAPI.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BasicDmgAPI.Controllers
{
    [Route("indicators")]
    [EnableCors("*", "*", "*")]
    public class IndicatorController : ApiController
    {
        private IndicatorService indicatorService = new IndicatorService(new IndicatorDAO());

        [HttpGet]
        public HttpResponseMessage GetAll()
        {
            List<Indicator> result = indicatorService.GetAll();
            return Request.CreateResponse<List<Indicator>>(HttpStatusCode.OK, result);
        }
    }
}
