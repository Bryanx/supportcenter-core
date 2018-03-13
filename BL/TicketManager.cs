using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SC.BL.Domain;
using SC.DAL;
using SC.DAL.EF;

namespace SC.BL {
    public class TicketManager : ITicketManager {
        private readonly ITicketRepository repo;

        public TicketManager() {
            repo = new TicketRepository();
        }

        public IEnumerable<Ticket> GetTickets() {
            return repo.ReadTickets();
        }

        public Ticket GetTicket(int ticketNumber) {
            return repo.ReadTicket(ticketNumber);
        }

        public Ticket AddTicket(int accountId, string question) {
            var t = new Ticket {
                AccountId = accountId,
                Text = question,
                DateOpened = DateTime.Now,
                State = TicketState.Open
            };
            return AddTicket(t);
        }

        public Ticket AddTicket(int accountId, string device, string problem) {
            Ticket t = new HardwareTicket {
                AccountId = accountId,
                Text = problem,
                DateOpened = DateTime.Now,
                State = TicketState.Open,
                DeviceName = device
            };
            return AddTicket(t);
        }

        public void ChangeTicket(Ticket ticket) {
            Validate(ticket);
            repo.UpdateTicket(ticket);
        }

        public void RemoveTicket(int ticketNumber) {
            repo.DeleteTicket(ticketNumber);
        }

        public IEnumerable<TicketResponse> GetTicketResponses(int ticketNumber) {
            return repo.ReadTicketResponsesOfTicket(ticketNumber);
        }

        public TicketResponse AddTicketResponse(int ticketNumber, string response, bool isClientResponse) {
            var ticketToAddResponseTo = GetTicket(ticketNumber);
            if (ticketToAddResponseTo != null) {
                // Create response
                var newTicketResponse = new TicketResponse();
                newTicketResponse.Date = DateTime.Now;
                newTicketResponse.Text = response;
                newTicketResponse.IsClientResponse = isClientResponse;
                newTicketResponse.Ticket = ticketToAddResponseTo;

                // Add response to ticket
                var responses = GetTicketResponses(ticketNumber);
                if (responses != null)
                    ticketToAddResponseTo.Responses = responses.ToList();
                else
                    ticketToAddResponseTo.Responses = new List<TicketResponse>();
                ticketToAddResponseTo.Responses.Add(newTicketResponse);

                // Change state of ticket
                if (isClientResponse)
                    ticketToAddResponseTo.State = TicketState.ClientAnswer;
                else
                    ticketToAddResponseTo.State = TicketState.Answered;


                // Validatie van ticketResponse en ticket afdwingen!!!
                Validate(newTicketResponse);
                Validate(ticketToAddResponseTo);

                // Bewaren naar db
                repo.CreateTicketResponse(newTicketResponse);
                repo.UpdateTicket(ticketToAddResponseTo);

                return newTicketResponse;
            }

            throw new ArgumentException("Ticketnumber '" + ticketNumber + "' not found!");
        }

        public void ChangeTicketStateToClosed(int ticketNumber) {
            repo.UpdateTicketStateToClosed(ticketNumber);
        }

        private Ticket AddTicket(Ticket ticket) {
            Validate(ticket);
            return repo.CreateTicket(ticket);
        }

        private void Validate(Ticket ticket) {
            //Validator.ValidateObject(ticket, new ValidationContext(ticket), validateAllProperties: true);

            var errors = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(ticket, new ValidationContext(ticket), errors, true);

            if (!valid)
                throw new ValidationException("Ticket not valid!");
        }

        private void Validate(TicketResponse response) {
            //Validator.ValidateObject(response, new ValidationContext(response), validateAllProperties: true);

            var errors = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(response, new ValidationContext(response), errors, true);

            if (!valid)
                throw new ValidationException("TicketResponse not valid!");
        }
    }
}