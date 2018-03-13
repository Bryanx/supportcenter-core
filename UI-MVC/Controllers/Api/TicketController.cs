using System.Net;
using System.Web.Http;
using SC.BL;

namespace SC.UI.Web.MVC.Controllers.Api {
    public class TicketController : ApiController {
        private readonly ITicketManager mgr = new TicketManager();

        [HttpPut]
        [Route("api/Ticket/{id}/State/Closed")]
        public IHttpActionResult PutTicketStateToClosed(int id) {
            mgr.ChangeTicketStateToClosed(id);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}