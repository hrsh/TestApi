using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly UserManager<IdentityUser> _userManager;

        public BookController(
            ApplicationDbContext context,
            IJwtService jwtService,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _jwtService = jwtService;
            _userManager = userManager;
        }

        [HttpGet("token")]
        public async Task<ActionResult<string>> Token(
            string userId)
        {
            var t = await _userManager.FindByIdAsync(userId);
            if (null == t)
                return NotFound();

            var token = _jwtService.Generate(t);
            return token;
        }

        [HttpGet("index")]
        [Authorize]
        public async Task<ActionResult<List<BookEntity>>> GetAll(CancellationToken ct)
        {
            var t = await _context.Books
                .AsNoTracking()
                .OrderBy(a => a.Id)
                .Skip(0)
                .Take(128)
                .ToListAsync(ct);

            return t;
        }

        [HttpPost("add")]
        public async Task<ActionResult<BookEntity>> Create([FromBody] BookEntity model, CancellationToken ct)
        {
            var t = await _context.Books.AddAsync(model, ct);
            await _context.SaveChangesAsync(ct);
            return t.Entity;
        }
    }
}
