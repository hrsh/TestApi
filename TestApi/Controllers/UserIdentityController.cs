using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("account/u")]
    public class UserIdentityController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserIdentityController(
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Register([FromBody]UserModel model)
        {
            var user = new IdentityUser
            {
                 Email = model.Email,
                  PhoneNumber = model.PhoneNumber,
                   UserName = model.UserName
            };

            var t = await _userManager.CreateAsync(user, model.Password);

            if (t.Succeeded)
                return Ok();

            return BadRequest();
        }

        [HttpGet("users")]
        public async Task<List<IdentityUser>> Users(CancellationToken ct)
        {
            var t = await _context.Set<IdentityUser>().AsNoTracking().ToListAsync(ct);
            return t;
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<IdentityUser>> GetById(string id, CancellationToken ct)
        {
            var t = await _context.Set<IdentityUser>().FirstOrDefaultAsync(a => a.Id == id, ct);

            if (null != t)
                return t;

            return NotFound();
        }


    }
}
