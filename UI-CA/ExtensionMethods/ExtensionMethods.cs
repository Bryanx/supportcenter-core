using SC.BL.Domain;

namespace SC.UI.CA.ExtensionMethods {
    internal static class ExtensionMethods {
        internal static string GetInfo(this Ticket t) {
            return string.Format("[{0}] {1} ({2} antwoorden)", t.TicketNumber, t.Text,
                t.Responses == null ? 0 : t.Responses.Count);
        }

        internal static string GetInfo(this TicketResponse r) {
            return string.Format("{0:dd/MM/yyyy} {1}{2}", r.Date, r.Text, r.IsClientResponse ? " (client)" : "");
        }
    }
}