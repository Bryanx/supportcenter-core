using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SC.BL;
using SC.BL.Domain;
using SC.UI.Web.MVC.Models;

namespace SC.UI.Web.MVC.Controllers {
    public class TicketController : Controller {
        private readonly ITicketManager mgr = new TicketManager();

        // GET: Ticket
        public ActionResult Index() {
            var tickets = mgr.GetTickets();
            return View(tickets);
        }

        // GET: Ticket/Details/5
        public ActionResult Details(int id) {
            var ticket = mgr.GetTicket(id);

            var responses = mgr.GetTicketResponses(id).ToList();

            ticket.Responses = responses;
            // OF: via ViewBag
            //ViewBag.Responses = responses;

            return View(ticket);
        }

        // GET: Ticket/Create
        public ActionResult Create() {
            return View();
        }

        // POST: Ticket/Create
        [HttpPost]
        /* public ActionResult Create(Ticket ticket)
        {
          if (ModelState.IsValid)
          {
            ticket = mgr.AddTicket(ticket.AccountId, ticket.Text);

            return RedirectToAction("Details", new { id = ticket.TicketNumber });
          }

          return View();
        } */
        // ADHV view-model 'CreateTicketVM'
        public ActionResult Create(CreateTicketVM newTicket) {
            if (ModelState.IsValid) {
                var ticket = mgr.AddTicket(newTicket.AccId, newTicket.Problem);

                return RedirectToAction("Details", new {id = ticket.TicketNumber});
            }

            return View();
        }

        // GET: Ticket/Edit/5
        public ActionResult Edit(int id) {
            var ticket = mgr.GetTicket(id);
            return View(ticket);
        }

        // POST: Ticket/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Ticket ticket) {
            if (ModelState.IsValid) {
                mgr.ChangeTicket(ticket);

                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: Ticket/Delete/5
        public ActionResult Delete(int id) {
            var ticket = mgr.GetTicket(id);
            return View(ticket);
        }

        // POST: Ticket/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection) {
            try {
                mgr.RemoveTicket(id);

                return RedirectToAction("Index");
            }
            catch {
                return View();
            }
        }
    }
}