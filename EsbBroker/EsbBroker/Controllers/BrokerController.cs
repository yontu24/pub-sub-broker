using EsbBroker.Models;
using EsbBroker.Services;
using Microsoft.AspNetCore.Mvc;

namespace EsbBroker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BrokerController : ControllerBase
    {
        private readonly IRoutingService _routingService;

        public BrokerController(IRoutingService routingService)
        {
            _routingService = routingService;
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] SubscriptionDTO subscriptionDTO)
        {
            string companyString = subscriptionDTO.Company == null ? "Company = {}" : "Company = { " + subscriptionDTO.Company.Value + " " + subscriptionDTO.Company.Operator + "}";
            string valueString = subscriptionDTO.Value == null ? "Value = {}" : "Value = { " + subscriptionDTO.Value.Value + " " + subscriptionDTO.Value.Operator + "}";
            string variationString = subscriptionDTO.Variation == null ? "Variation = {}" : "Variation = { " + subscriptionDTO.Variation.Value + " " + subscriptionDTO.Variation.Operator + "}";
            string dropString = subscriptionDTO.Drop == null ? "Drop = {}" : "Drop = { " + subscriptionDTO.Drop.Value + " " + subscriptionDTO.Drop.Operator + "}";
            string dateString = subscriptionDTO.Date == null ? "Date = {}" : "Date = { " + subscriptionDTO.Date.Value + " " + subscriptionDTO.Date.Operator + "}";

            Console.WriteLine("Received " +
                string.Join(", ", companyString, valueString, variationString, dropString, dateString)
                + " from subscriber " + subscriptionDTO.Sender);

            _routingService.RegisterSubscriptionFromSubscriber(subscriptionDTO, HttpContext.Connection.LocalPort);

            return Ok();
        }

        [HttpPost("forward")]
        public async Task<IActionResult> Forward([FromBody] SubscriptionDTO subscriptionDTO)
        {
            string companyString = subscriptionDTO.Company == null ? "Company = {}" : "Company = { " + subscriptionDTO.Company.Value + " " + subscriptionDTO.Company.Operator + "}";
            string valueString = subscriptionDTO.Value == null ? "Value = {}" : "Value = { " + subscriptionDTO.Value.Value + " " + subscriptionDTO.Value.Operator + "}";
            string variationString = subscriptionDTO.Variation == null ? "Variation = {}" : "Variation = { " + subscriptionDTO.Variation.Value + " " + subscriptionDTO.Variation.Operator + "}";
            string dropString = subscriptionDTO.Drop == null ? "Drop = {}" : "Drop = { " + subscriptionDTO.Drop.Value + " " + subscriptionDTO.Drop.Operator + "}";
            string dateString = subscriptionDTO.Date == null ? "Date = {}" : "Date = { " + subscriptionDTO.Date.Value + " " + subscriptionDTO.Date.Operator + "}";

            Console.WriteLine("Received " +
                string.Join(", ", companyString, valueString, variationString, dropString, dateString)
                + " from subscriber " + subscriptionDTO.Sender);

            _routingService.RegisterSubscriptionFromBroker(subscriptionDTO, HttpContext.Connection.LocalPort);

            return Ok();
        }

        [HttpPost("publish")]
        public async Task<IActionResult> Publish([FromBody] PublicationDTO publicationDTO) 
        {
            Console.WriteLine("Received " +
                string.Join(", ", "Company = " + publicationDTO.Company, "Value = " + publicationDTO.Value,
                            "Variation = " + publicationDTO.Variation, ", = " + publicationDTO.Drop,
                            "Date = " + publicationDTO.Date)
                + " from " + publicationDTO.Sender);
            _routingService.AddPublication(publicationDTO);
            _routingService.MatchPublicationAsync(publicationDTO, true, HttpContext.Connection.LocalPort);
            _routingService.MatchPublicationAsync(publicationDTO, false, HttpContext.Connection.LocalPort);

            await Task.CompletedTask;
            return Ok();
        }

        [HttpGet("print")]
        public IActionResult Print()
        {
            _routingService.PrintPublications(HttpContext.Connection.LocalPort);
            _routingService.PrintSubscriptions(HttpContext.Connection.LocalPort);

            return Ok();
        }
    }
}
