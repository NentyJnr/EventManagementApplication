using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NCSEvent.API.Commons.DTO;
using NCSEvent.API.Commons.Responses;
using NCSEvent.API.Entities;
using NCSEvent.API.Services.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NCSEvent.API.Services.Implementations
{
    public class AccountService : IAccount
    {
        private readonly ILogger<AccountService> _logger;
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly ISessionService _sessionService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly AppDbContext _context;

        public AccountService(AppDbContext dbContext, ILogger<AccountService> logger, SignInManager<Users> signInManager, UserManager<Users> userManager, ISessionService sessionService, IHttpContextAccessor httpContext, AppDbContext context)
        {
            _httpContext = httpContext;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _signInManager = signInManager;
            _userManager = userManager;
            _sessionService = sessionService;
            _context = context;
        }

        public async Task<ServerResponse<LoginResponse>> Login(string email, string password)
        {
            var response = new ServerResponse<LoginResponse>();

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.RECORD_DOES_NOT_EXISTS,
                    ResponseDescription = "No User Found"
                };

                return response;
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);

            if (signInResult != null && signInResult.Succeeded)
            {
                UserDTO users = user.Adapt<UserDTO>();

                var userRoles = await _userManager.GetRolesAsync(user);

                var tokens = GenerateToken(users);

                var loginResponse = new LoginResponse
                {
                    UserDTO = users,
                    Roles = userRoles,
                    token = tokens
                };

                var sessionRequest = new SessionDTO
                {
                    DateCreated = DateTime.Now,
                    Token = tokens,
                    UserId = users.Id,
                };

                var sessionResponse = await _sessionService.CreateSessionAsync(sessionRequest);
                if (sessionResponse != null && sessionResponse.IsSuccessful)
                {
                    response.IsSuccessful = true;
                    response.Data = loginResponse;
                    response.IsSuccessful = true;
                }
            }

            else
            {
                _logger.LogError($"Error during login: ");
                response.IsSuccessful = false;
                response.Data = null;
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.REQUEST_NOT_SUCCESSFUL,
                    ResponseDescription = "Login Unsuccessful"
                };
            }

            return response;
        }

        public async Task<ServerResponse<List<UserDTO>>> GetAllRecord()
        {
            var response = new ServerResponse<List<UserDTO>>();

            try
            {
                var data = await _context.Users
                    .Join(
                        _context.UserRoles,
                        user => user.Id,
                        userRole => userRole.UserId,
                        (user, userRole) => new { User = user, UserRole = userRole }
                    )
                    .Join(
                        _context.Roles,
                        userRole => userRole.UserRole.RoleId,
                        role => role.Id,
                        (userRole, role) => new UserDTO
                        {
                            Id = userRole.User.Id,
                            FirstName = userRole.User.FirstName,
                            LastName = userRole.User.LastName,
                            Email = userRole.User.Email,
                            Roles = new List<string> { role.Name }
                        }
                    )
                    .ToListAsync();

                response.Data = data;
                response.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.RECORD_DOES_NOT_EXISTS,
                    ResponseDescription = "Failed to fetch User."
                };
            }

            return response;
        }

            public async Task<ServerResponse<bool>> LogOut()
        {
            var response = new ServerResponse<bool>();

            var token = GetTokenFromHttpContext();
            if (token == null)
            {
                return response;
            }

            var userId = token.UserId;

            var logout = await _sessionService.DeleteSessionAsync(userId);

            if (logout != null)
            {
                response.Data = true;
                response.IsSuccessful = true;
                response.SuccessMessage = "Logout Successful";
            }
            else
            {
                response.Data = false;
                response.IsSuccessful = false;

                response.Error = new ErrorResponse
                {
                    ResponseCode = ResponseCodes.REQUEST_NOT_SUCCESSFUL,
                    ResponseDescription = "Logout Failed"
                };
            }
            return response;
        }

        public TokenConvert GetTokenFromHttpContext()
        {
            var httpContext = _httpContext.HttpContext;

            if (httpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                var token = authorizationHeader.ToString().Replace("Bearer ", "");

                var tokenHandler = new JwtSecurityTokenHandler();
                var decodedToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (decodedToken != null)
                {
                    var tokenModel = new TokenConvert
                    {
                        UserId = decodedToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value,
                        FirstName = decodedToken.Claims.FirstOrDefault(c => c.Type == "first_name")?.Value,
                        LastName = decodedToken.Claims.FirstOrDefault(c => c.Type == "last_name")?.Value,
                        Email = decodedToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value
                    };

                    return tokenModel;
                }
            }

            return null;
        }


        private string GenerateToken(UserDTO user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("ApplicationSettings:SecurityKey");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserId", user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim(ClaimTypes.Name, user.LastName)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
