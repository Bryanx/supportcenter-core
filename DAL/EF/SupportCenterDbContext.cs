using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using SC.BL.Domain;

namespace SC.DAL.EF {
    
    internal class SupportCenterDbContext /* : DbContext 'public' for testing with project 'DAL-Testing'! */ {
        
        public SupportCenterDbContext() {
            this.Tickets = new List<Ticket>();
            this.HardwareTickets = new List<HardwareTicket>();
            this.TicketResponses = new List<TicketResponse>();
            Seed();
        }

        public List<Ticket> Tickets { get; set; }
        public List<HardwareTicket> HardwareTickets { get; set; }
        public List<TicketResponse> TicketResponses { get; set; }
        
        protected void Seed() {
            // Create first ticket with three responses
            var t1 = new Ticket {
                TicketNumber = 0,
                AccountId = 1,
                Text = "Ik kan mij niet aanmelden op de webmail",
                DateOpened = new DateTime(2012, 9, 9, 13, 5, 59),
                State = TicketState.Open,
                Responses = new List<TicketResponse>()
            };
            Tickets.Add(t1);

            var t1r1 = new TicketResponse {
                Id = 0,
                Ticket = t1,
                Text = "Account was geblokkeerd",
                Date = new DateTime(2012, 9, 9, 13, 24, 48),
                IsClientResponse = false
            };
            t1.Responses.Add(t1r1);

            var t1r2 = new TicketResponse {
                Id = 1,
                Ticket = t1,
                Text = "Account terug in orde en nieuw paswoord ingesteld",
                Date = new DateTime(2012, 9, 9, 13, 29, 11),
                IsClientResponse = false
            };
            t1.Responses.Add(t1r2);

            var t1r3 = new TicketResponse {
                Id = 2,
                Ticket = t1,
                Text = "Aanmelden gelukt en paswoord gewijzigd",
                Date = new DateTime(2012, 9, 10, 7, 22, 36),
                IsClientResponse = true
            };
            t1.Responses.Add(t1r3);
            t1.State = TicketState.Closed;

            // Create second ticket with one response
            var t2 = new Ticket {
                TicketNumber = 1,
                AccountId = 1,
                Text = "Geen internetverbinding",
                DateOpened = new DateTime(2012, 11, 5, 9, 45, 13),
                State = TicketState.Open,
                Responses = new List<TicketResponse>()
            };
            Tickets.Add(t2);

            var t2r1 = new TicketResponse {
                Id = 0,
                Ticket = t2,
                Text = "Controleer of de kabel goed is aangesloten",
                Date = new DateTime(2012, 11, 5, 11, 25, 42),
                IsClientResponse = false
            };
            t2.Responses.Add(t2r1);
            t2.State = TicketState.Answered;

            // Create hardware ticket without response
            var ht1 = new HardwareTicket {
                TicketNumber = 2,
                AccountId = 2,
                Text = "Blue screen!",
                DateOpened = new DateTime(2012, 12, 14, 19, 5, 2),
                State = TicketState.Open,
                DeviceName = "PC-123456"
            };
            this.Tickets.Add(ht1);
        }
    }
}