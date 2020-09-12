using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlazorWebAssemblyApp.Server.Data;
using BlazorWebAssemblyApp.Server.Models;
using BlazorWebAssemblyApp.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BlazorWebAssemblyApp.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]//enables validation, inferrs where param comes from
	public class AuthController : ControllerBase
	{
		private readonly IAuthRepository _repo;
		private readonly IConfiguration _config;
		private readonly IMapper _mapper;
		public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
		{
			_mapper = mapper;
			_config = config;
			_repo = repo;
		}

		[HttpPost("register")]
		//object from body url or form, you can inferr with frombody tag in param
		public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
		{
			//validate req
			// if(!ModelState.IsValid)
			//     return BadRequest(ModelState);

			userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

			if (await _repo.UserExists(userForRegisterDto.Username))
				return BadRequest(new RegisterResult { 
					Successful = false,
					Error = "Username already exists",
					User = null
				});

			var userToCreate = _mapper.Map<User>(userForRegisterDto);

			var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);
			var userToReturn = _mapper.Map<UserForDetailedDto>(createdUser);

			return CreatedAtRoute("GetUser", new { controller = "Users", id = createdUser.ID }, new RegisterResult
			{
				Successful = true,
				Error = null,
				User = userToReturn
			});
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
		{
			var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

			if (userFromRepo == null)
				return Unauthorized(new LoginResult
				{
					Successful = false,
					Error = "Incorrect username or password!"
				});

			var claims = new[]
			{
				new Claim(ClaimTypes.NameIdentifier, userFromRepo.ID.ToString()),
				new Claim(ClaimTypes.Name, userFromRepo.Username)
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(1),
				SigningCredentials = creds
			};

			var tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.CreateToken(tokenDescriptor);

			var user = _mapper.Map<UserForDetailedDto>(userFromRepo);

			return Ok(new LoginResult
			{
				Successful = true,
				Token = tokenHandler.WriteToken(token),
				User = user
			});
		}

	}
}