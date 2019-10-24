using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AppEmpresa.Domain.Identity;
using AppEmpresa.Dtos;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AppEmpresa.Controllers
{
    [Route("api/[auth]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
       // public IConfiguration _config { get; }
        public UserManager<User> _userManager { get; }
        public   IMapper _mapper { get; }
        public SignInManager<User> _signInManager { get; }

        public AuthController(
                                UserManager<User> userManager,
                                SignInManager<User> signInManager,
                                IMapper mapper)
        {
            _signInManager = signInManager;
            //_config = config;
            _userManager = userManager;
            _mapper = mapper;
        }


        [HttpGet("GetUser")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUser()
        {
            return Ok(new UserDto());
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                var result = await _userManager.CreateAsync(user, userDto.Password);
                var userToResult = _mapper.Map<UserDto>(user);

                if (result.Succeeded)
                {
                    return Created("GetUser", userToResult);
                }
                return BadRequest(result.Errors);
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha {e.Message}");
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userLogin.UserName);
                //False = Indica que não será efetuado o bloqueio da conta do usuário
                var result = await _signInManager.CheckPasswordSignInAsync(user, userLogin.Password, false);

                if (result.Succeeded)
                {
                    var appuser = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.NormalizedUserName == userLogin.UserName.ToUpper());

                    var userToResult = _mapper.Map<UserLoginDto>(appuser);

                    return Ok(new
                    {
                        token = GenerateJwToken(appuser).Result,
                        user = userToResult
                    });
                }
                return Unauthorized();
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Falha {e.Message}");
            }
        }

        private async Task<string> GenerateJwToken(User user)
        {
            //Criando as autorizações do usuário
            List<Claim> claims = new List<Claim>{
              new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
              new Claim(ClaimTypes.Name,user.UserName.ToString())
            };

        //Obtendo as autorizações que o usuário possui
        IList<string> roles = await _userManager.GetRolesAsync(user);

            //Adicionando as autorizações que o usuário possui
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

    //Obtendo os dados da key das configurações, porém 
    SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.ASCII
           .GetBytes("AppSettings:Token"));

    //Criando a chave criptografada
    SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

    SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(claims),
        //Atenção nesse tipo de forma pois pode ser utilizado data de caducidade atrazando o relógio
        Expires = DateTime.Now.AddDays(1),
        SigningCredentials = creds
    };

    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}

    
