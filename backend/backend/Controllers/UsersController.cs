using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Entities;
using backend.Repositories.EntitiesRepo;
using AutoMapper;
using backend.Dtos;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IRepo<User> _userRepo;
        private readonly IMapper _mapper;

        public UsersController(IRepo<User> userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReadDto>>> GetUsers()
        {
            var users = await _userRepo.GetAllAsync();
            var usersDto = _mapper.Map<IEnumerable<User>>(users);
            return Ok(usersDto);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadDto>> GetUser(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = _mapper.Map<UserReadDto>(user);
            return Ok(userDto);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserUpdateDto user)
        {
            var existingUser = await _userRepo.GetByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            var userEntity = _mapper.Map<User>(user);
            bool updated = await _userRepo.UpdateAsync(userEntity);
            if (!updated)
            {
                return BadRequest("Unable to update the user.");
            }

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserCreateDto user)
        {
            var userEntity = _mapper.Map<User>(user);
            var added = await _userRepo.AddAsync(userEntity);
            if (added == null)
            {
                return BadRequest("Unable to add the user.");
            }

            return CreatedAtAction("GetUser", new { id = added.Id }, added);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            bool deleted = await _userRepo.DeleteAsync(id);
            if (!deleted)
            {
                return BadRequest("Unable to delete the user.");
            }

            return NoContent();
        }
    }
}
