using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using SC.BL.Domain;

namespace SC.DAL.EF {
    public class TicketRepository : ITicketRepository {
        private readonly SupportCenterDbContext ctx;

        public TicketRepository() {
            ctx = new SupportCenterDbContext();
        }

        public IEnumerable<Ticket> ReadTickets() {
            //IEnumerable<Ticket> tickets = ctx.Tickets.AsEnumerable<Ticket>();

            // Eager-loading
            //IEnumerable<Ticket> tickets = ctx.Tickets.Include(t => t.Responses).AsEnumerable<Ticket>();

            // Lazy-loading
            //IEnumerable<Ticket> tickets = ctx.Tickets.AsEnumerable<Ticket>(); // needs 'Multiple Active Result Sets' (MARS) for lazy-loading (connectionstring)
            IEnumerable<Ticket>
                tickets = ctx.Tickets
                    .ToList(); // all (parent-)entities are loaded before lazy-loading associated data (doesn't need MARS)

            return tickets;
        }

        public Ticket ReadTicket(int ticketNumber) {
            var ticket = ctx.Tickets.Find(t => t.TicketNumber==ticketNumber);
            return ticket;
        }

        public Ticket CreateTicket(Ticket ticket)
        {
            ticket.TicketNumber = ReadTickets().ToList().Count;
            ctx.Tickets.Add(ticket);

            return ticket; // 'TicketNumber' has been created by the database!
        }

        public void UpdateTicket(Ticket ticket) {
            // Make sure that 'ticket' is known by context
            // and has state 'Modified' before updating to database
            //TODO
        }

        public void UpdateTicketStateToClosed(int ticketNumber) {
            var ticket = ReadTicket(ticketNumber);
            ticket.State = TicketState.Closed;
        }

        public void DeleteTicket(int ticketNumber)
        {
            var ticket = ReadTicket(ticketNumber);
            ctx.Tickets.Remove(ticket);
        }

        public IEnumerable<TicketResponse> ReadTicketResponsesOfTicket(int ticketNumber) {
            var ticket = ReadTicket(ticketNumber);
            var response = ticket.Responses.ToList();
            //var responses = ctx.TicketResponses.Where(r => r.Ticket.TicketNumber == ticketNumber).AsEnumerable();

            return response;
        }

        public TicketResponse CreateTicketResponse(TicketResponse response)
        {
            response.Id = ReadTicketResponsesOfTicket(response.Ticket.TicketNumber).ToList().Count;
            //ReadTicket(response.Ticket.TicketNumber).Responses.Add(response);
            //ctx.TicketResponses.Add(response);

            return response; // 'Id' has been created by the database!
        }
    }
}