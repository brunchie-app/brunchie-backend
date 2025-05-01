using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using brunchie_backend.Repositories;
using brunchie_backend.Models;
using Microsoft.AspNetCore.Http.Features;

namespace brunchie_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuRepository _menuRepository;

        public MenuController(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Vendor")]

        public async Task<IActionResult> AddMenu([FromBody] IEnumerable<MenuItemAddDto> MenuItems)
        {
            try
            {
                await _menuRepository.AddItems(MenuItems);
                return Ok();
            }

            catch (ApplicationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Vendor")]

        public async Task<IActionResult> Delete([FromBody] IEnumerable<int> ItemIds)
        {

            IEnumerable<int> failed = new List<int>();

            failed = await _menuRepository.RemoveItems(ItemIds);

            if (failed.Any())
            {
                return StatusCode(207, new
                {
                    failedItems = failed,
                    message = "Some deletions failed, check details."
                });
            }

            return Ok();

        }

        [HttpGet("{Id}")]

        public async Task<IActionResult> VendorMenu(string VendorId)
        {

            if (string.IsNullOrEmpty(VendorId))
            {
                return BadRequest();
            }

            var Items = await _menuRepository.GetMenu( VendorId);
            return Ok(Items);
            
        }

    }
}
