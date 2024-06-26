﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TollBooth.Models;

namespace TollBooth
{
    public class SendToEventGrid
    {
        private readonly HttpClient _client;
        private readonly ILogger _log;

        public SendToEventGrid(ILogger log, HttpClient client)
        {
            _log = log;
            _client = client;
        }

        public async Task SendLicensePlateData(LicensePlateData data)
        {
            // Will send to one of two routes, depending on success.
            // Event listeners will filter and act on events they need to
            // process (save to database, move to manual checkup queue, etc.)
            if (data.LicensePlateFound)
            {
                // TODO 3: Modify send method to include the proper eventType name value for saving plate data.
                await Send("SaveLicensePlateData", "license-plate-data", data);
            }
            else
            {
                // TODO 4: Modify send method to include the proper eventType name value for queuing plate for manual review.
                await Send("QueueManualReview", "manual-review", data);
            }
        }

        private async Task Send(string eventType, string subject, LicensePlateData data)
        {
            // Get the API URL and the API key from settings.
            var uri = Environment.GetEnvironmentVariable("eventGridTopicEndpoint");
            var key = Environment.GetEnvironmentVariable("eventGridTopicKey");

            _log.LogInformation($"Sending license plate data to the {eventType} Event Grid type");
            
            var events = new List<Event<LicensePlateData>>
            {
                new Event<LicensePlateData>()
                {
                    Data = data,
                    EventTime = DateTime.UtcNow,
                    EventType = eventType,
                    Id = Guid.NewGuid().ToString(),
                    Subject = subject
                }
            };

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("aeg-sas-key", key);
            await _client.PostAsJsonAsync(uri, events);

            _log.LogInformation($"Sent the following to the Event Grid topic: {events[0]}");
        }
    }
}
