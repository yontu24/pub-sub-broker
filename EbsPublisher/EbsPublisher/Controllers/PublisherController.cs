using EbsPublisher.Services;
using Microsoft.AspNetCore.Mvc;

namespace EbsPublisher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublisherController : ControllerBase
    {
        public readonly IPublicationService _publicationService;

        public PublisherController(IPublicationService publicationService)
        {
            _publicationService = publicationService;
        }

        [HttpGet] 
        public void Generate()
        {
            _publicationService.SendPublications(HttpContext.Connection.LocalPort);
        }
    }
}
