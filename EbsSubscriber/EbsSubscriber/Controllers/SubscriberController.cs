using EbsSubscriber.Models;
using EbsSubscriber.Services;
using Microsoft.AspNetCore.Mvc;

namespace EbsSubscriber.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubscriberController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriberController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet]
        public async Task<IActionResult> Subscribe()
        {
            _subscriptionService.SendSubscriptionsAsync(HttpContext.Connection.LocalPort);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> GetNotified([FromBody] PublicationDTO publicationDTO)
        {
            Console.WriteLine("Received " +
                string.Join(", ", "Company = " + publicationDTO.Company, "Value = " + publicationDTO.Value,
                            "Variation = " + publicationDTO.Variation, ", = " + publicationDTO.Drop,
                            "Date = " + publicationDTO.Date)
                + " from " + publicationDTO.Sender);

            return Ok();
        }
    }
}
