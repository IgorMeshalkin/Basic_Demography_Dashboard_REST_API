using BasicDmgAPI.DAO;
using BasicDmgAPI.Entity;
using BasicDmgAPI.Service;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BasicDmgAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    public class DistrictController : ApiController
    {
        DistrictServise districtServise = new DistrictServise(new DistrictDAO(), new PeriodDAO(), new DataDAO());

        [HttpGet, Route("districts")]
        public HttpResponseMessage GetAll()
        {
            District result = new District();
            result.FullName = "Полное имя";
            return Request.CreateResponse<District>(HttpStatusCode.OK, result);

            //List<District> result = districtServise.GetAll();
            //return Request.CreateResponse<List<District>>(HttpStatusCode.OK, result);
        }

        [HttpGet, Route("districts/without_data")]
        public HttpResponseMessage GetAllWithoutData()
        {
            List<District> result = districtServise.GetAllWithoutData();
            return Request.CreateResponse<List<District>>(HttpStatusCode.OK, result);
        }
    }
}
