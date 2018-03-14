using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using SC.BL;
using SC.UI.Web.MVC.Models;
using System.Collections.Generic;
using SC.BL.Domain;


namespace SC.UI.Web.MVC.Controllers.Api {
    [Route("api/[controller]")]
    public class TicketResponseController : ControllerBase {
        private readonly ITicketManager mgr = new TicketManager();

        [HttpGet("{ticketNumber?}")]
        public IEnumerable<TicketResponse> Get(int ticketNumber) {
            var responses = mgr.GetTicketResponses(ticketNumber);

            if (responses == null || responses.Count() == 0)
                return null;
            

            foreach (var r in responses)
                r.Ticket = null;
            return responses;
        }
        
        [HttpPost]
        public IActionResult Post(NewTicketResponseDTO response) {
            var createdResponse =
                mgr.AddTicketResponse(response.TicketNumber, response.ResponseText, response.IsClientResponse);


            //// Circulaire referentie!! (TicketResponse <-> Ticket) -> can't be serialized!!
            //return CreatedAtRoute("DefaultApi",
            //                      new { Controller = "TicketResponse", id = createdResponse.Id },
            //                      createdResponse);

            // Gebruik DTO (Data Transfer Object)
            var responseData = new TicketResponseDTO {
                Id = createdResponse.Id,
                Text = createdResponse.Text,
                Date = createdResponse.Date,
                IsClientResponse = createdResponse.IsClientResponse,
                TicketNumberOfTicket = createdResponse.Ticket.TicketNumber
            };

            return CreatedAtRoute("DefaultApi",
                new {Controller = "TicketResponse", id = responseData.Id},
                responseData);
                
            
        }
    }
}