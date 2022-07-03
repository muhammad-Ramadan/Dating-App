using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using API.Data;
using API.Entities;
using System.Security.Cryptography;
using System.Text;
using API.DTOs;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ITokenService _tokenService;

        public DataContext _context { get; }

        public AccountController(ILogger<AccountController> logger, DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(await userExists(registerDto.UserName)) return BadRequest("the user is taken");
            using var hmac = new HMACSHA512();
            var user = new AppUser(){
                UserName = registerDto.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto{
                UserName = user.UserName,
                Token = _tokenService.GenerateToken(user)
            };
        }

        private async Task<bool> userExists(string userName){
            return await _context.Users.AnyAsync(x => x.UserName.ToLower() == userName.ToLower());
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto){
            var user = await _context.Users.
            FirstOrDefaultAsync(x => x.UserName == loginDto.UserName);
            if(user == null) return Unauthorized("invalid username");

            var hmac = new HMACSHA512(user.PasswordSalt);
            var PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for(int i=0; i<PasswordHash.Length;i++){
                if(PasswordHash[i]!=user.PasswordHash[i]) return Unauthorized("invalid password");
            }
             return new UserDto{
                UserName = user.UserName,
                Token = _tokenService.GenerateToken(user)
            };
        }

        [HttpPost("register2")]
        public async Task<UserDto> RegisterUsingParameters(string userName, string password)
        {
            using var hmac = new HMACSHA512();
            var user = new AppUser(){
                UserName = userName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto{
                UserName = user.UserName,
                Token = _tokenService.GenerateToken(user)
            };
        }
    }
}