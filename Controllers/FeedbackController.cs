using brunchie_backend.Models;
using brunchie_backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace brunchie_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackController(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }


        [HttpPost]
        [Authorize(Roles ="Student")]

        public async Task<IActionResult> PostFeedback([FromBody] FeedbackDto feedback)
        {
            try
            {
                await _feedbackRepository.Post(feedback);
                return Ok();
            }

            catch (Exception ex)
            { 
               
              return StatusCode(500, ex.Message);

            }
            
        }

        [HttpGet]
        [Authorize(Roles = "Vendor")]

        public async Task<IActionResult> GetFeedback([FromQuery] string VendorId)
        {
            if (string.IsNullOrEmpty(VendorId))
            {
                return BadRequest("No Vendor Id Recieved");
            }

            var feedbacks = await _feedbackRepository.Get(VendorId);

            return Ok(feedbacks);
        }
    }
}
