using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using SC.BL.Domain;
// add reference to 'System.Net.Http'

// install NuGet-package 'Newtonsoft.Json'

namespace SC.UI.CA {
    internal class Service {
        private const string baseUri = "http://localhost:51150/api/";
        //private const string baseUri = "http://localhost.fiddler:51150/api/"; // use this when using fiddler to capture traffic! 

        public IEnumerable<TicketResponse> GetTicketResponses(int ticketNumber) {
            IEnumerable<TicketResponse> responses = null;

            using (var http = new HttpClient()) {
                var uri = baseUri + "TicketResponse?ticketNumber=" + ticketNumber;
                var httpRequest = new HttpRequestMessage(HttpMethod.Get, uri);
                //Verwachte content-type van de response meegeven
                httpRequest.Headers.Add("Accept", "application/json");
                //Request versturen en wachten op de response
                var httpResponse = http.SendAsync(httpRequest).Result;
                if (httpResponse.IsSuccessStatusCode) {
                    //Body van de response uitlezen als een string
                    var responseContentAsString = httpResponse.Content.ReadAsStringAsync().Result;
                    //Body-string (in json-format) deserializeren (omzetten) naar een verzameling van TicketResponse-objecten
                    responses = JsonConvert.DeserializeObject<List<TicketResponse>>(responseContentAsString);
                } else {
                    throw new Exception(httpResponse.StatusCode + " " + httpResponse.ReasonPhrase);
                }
            }

            return responses;
        }

        public TicketResponse AddTicketResponse(int ticketNumber, string response, bool isClientResponse) {
            TicketResponse tr = null;

            using (var http = new HttpClient()) {
                var uri = baseUri + "TicketResponse";
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, uri);
                //Request data toevoegen aan body, via anonymous object dat je serialiseert naar json-formaat
                object data = new {
                    TicketNumber = ticketNumber,
                    ResponseText = response,
                    IsClientResponse = isClientResponse
                };
                var dataAsJsonString = JsonConvert.SerializeObject(data);
                httpRequest.Content = new StringContent(dataAsJsonString, Encoding.UTF8, "application/json");
                //Verwachte content-type van de response meegeven
                httpRequest.Headers.Add("Accept", "application/json");
                //Request versturen en wachten op de response
                var httpResponse = http.SendAsync(httpRequest).Result;
                if (httpResponse.IsSuccessStatusCode) {
                    //Body van de response uitlezen als een string
                    var responseContentAsString = httpResponse.Content.ReadAsStringAsync().Result;
                    //Body-string (in json-format) deserializeren (omzetten) naar een TicketResponse-object
                    tr = JsonConvert.DeserializeObject<TicketResponse>(responseContentAsString);
                } else {
                    throw new Exception(httpResponse.StatusCode + " " + httpResponse.ReasonPhrase);
                }
            }

            return tr;
        }

        public void ChangeTicketStateToClosed(int ticketNumber) {
            using (var http = new HttpClient()) {
                var uri = baseUri + "Ticket/" + ticketNumber + "/State/Closed";
                var httpRequest = new HttpRequestMessage(HttpMethod.Put, uri);
                //Request versturen en wachten op de response
                var httpResponse = http.SendAsync(httpRequest).Result;
                if (!httpResponse.IsSuccessStatusCode)
                    throw new Exception(httpResponse.StatusCode + " " + httpResponse.ReasonPhrase);
            }
        }
    }
}