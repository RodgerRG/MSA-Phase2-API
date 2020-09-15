using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web_API.Data;
using Web_API.DTOs;
using Web_API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(ApiContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //I don't know why CORS doesn't seem to be working, despite being set up in Startup. I'm returning the 200 for the preflight here.
        /*[HttpOptions]
        [Route("login")]
        [AllowAnonymous]
        public IActionResult PreflightRoute()
        {
            return Ok();
        }*/

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserId(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if(user != null)
            {
                return Ok();
            } else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] UserDTO userDTO)
        {
            var userTest = _context.Users.Where(u => u.Email == userDTO.email).FirstOrDefault();
            if(userTest != null)
            {
                return BadRequest();
            }

            User newUser = new User(userDTO);

            var res = await _userManager.CreateAsync(newUser, userDTO.password);

            if(res.Succeeded)
            {
                await _context.SaveChangesAsync();

                User user = (User) _context.Users.Where(u => u.Email == userDTO.email).FirstOrDefault();

                return CreatedAtAction(nameof(GetUserId), new { id = user.Id }, userDTO);
            }

            return BadRequest();
        }

        // GET: api/<ValuesController>
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var result = await _signInManager.PasswordSignInAsync(loginDTO.username, loginDTO.password, false, lockoutOnFailure: false);

            if(result.Succeeded)
            {
                return Ok();
            }

            return Forbid();
        }
    }
}
