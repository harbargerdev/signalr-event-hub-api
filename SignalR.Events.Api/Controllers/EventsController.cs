using Microsoft.AspNetCore.Mvc;
using SignalR.Events.Api.Models;

namespace SignalR.Events.Api.Controllers
{
    [ApiController]
    [Route("events")]
    public class EventsController : Controller
    {
        private readonly ILogger<EventsController> _logger;

        public EventsController(ILogger<EventsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("query/{userId}/")]
        public ActionResult<EventDetails> QueryEventDetailsByUserId([FromRoute] string userId)
        {
            var eventDetails = new EventDetails
            {
                UserId = userId,
                EventId = Guid.NewGuid().ToString(),
                EventName = "New Event",
                EventDescription = $"This is a new event that happened at {DateTime.UtcNow.ToString()}"
            };

            _logger.LogInformation("Queried event details for user {UserId}: {@EventDetails}", userId, eventDetails);
            return Ok(eventDetails);
        }
    }
}
