using auth_app.Data;
using auth_app.Models;
using auth_app.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Net.Http.Headers;

namespace auth_app.Endpoints
{
    // точки входа в api
    public static class ApiEndpoints
    {
        public static void MapApiEndpoints(this WebApplication app)
        {
            // урлы методов api, и их соответствие функциональным методам
            app.UseSwagger();
            app.MapGet("/About", AboutApi);
            app.MapPost("/Auth/SignUp", SignUp)
                .Accepts<User>("application/json")
                .Produces<User>(statusCode: 200, contentType: "application/json");
            app.MapPost("/Auth/SignIn", SignIn)
                .Accepts<Login>("application/json")
                .Produces<Login>(statusCode: 200, contentType: "application/json");
            app.MapGet("/Auth/CurrentUserInfo", 
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
                (ITokenService service, IHttpContextAccessor accessor) => GetCurrentUser(service, accessor))
                .Produces<User>(statusCode: 200, contentType: "application/json");
            app.MapPut("/Auth/TokenProlongate", 
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
                (ITokenService service, IHttpContextAccessor accessor) => TokenProlongate(service, accessor));
            app.MapPost("/Auth/Logout",
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
                (IHttpContextAccessor accessor) => LogoutUser(accessor));
            app.MapGet("/Auth/TokenInfo",
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
                (ITokenService service, IHttpContextAccessor accessor) => TokenInfo(service, accessor))
                .Produces<Token>(statusCode: 200, contentType: "application/json");
        }

        // базовый метод, возвращающий строку с информацией об api
        public static async Task<IResult> AboutApi()
        {
            string result = "Auth API for wb_analytics";
            return Results.Ok(result);
        }

        // метод создания нового пользователя
        public static async Task<IResult> SignUp(User user, IUserService service)
        {
            if (!string.IsNullOrEmpty(user.Username) && !string.IsNullOrEmpty(user.Password))
            {
                if (PgData.GetUsername(user.Username)) return Results.BadRequest("Username already exist");
                var db_response = service.Create(user);
                if (db_response != null) return Results.Ok(db_response);
                else return Results.BadRequest(db_response);
            }
            return Results.BadRequest("Empty input user credentials"); 
        }

        // авторизация пользователя, проверка данных, возвращает токен
        public static async Task<IResult> SignIn(Login user, ILoginService service)
        {
            if (!string.IsNullOrEmpty(user.Username) && !string.IsNullOrEmpty(user.Password))
            {
                if (!PgData.GetUsername(user.Username)) return Results.BadRequest("Username not found");
                // возвращает True, если пароль подошёл, иначе False
                var db_response = service.Get(user);
                if (!db_response) return Results.BadRequest("Password incorrect");

                // генерация токена для пользователя
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, PgData.GetId(user.Username))
                };

                var token = new JwtSecurityToken
                (
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(2),
                    notBefore: DateTime.UtcNow,
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(FileData.JsonParse().JwtKey)),
                        SecurityAlgorithms.HmacSha256)
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return Results.Ok(tokenString);
            }
            return Results.BadRequest("Empty input user credentials");
        }

        // возвращает все данные о пользователе по токену в хедере
        public static async Task<IResult> GetCurrentUser(ITokenService serivce, IHttpContextAccessor accessor)
        {
            var authHeader = accessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString();
            string token = authHeader.Split(new char[] { ' ' })[1];
            string user_id = serivce.GetToken(token).UserId;
            if (user_id == null) return Results.NotFound("User not found");
            User user = PgData.GetCurrentUser(user_id);
            return Results.Ok(user);
        }

        // нужно подумать как реализовать продление токена
        public static async Task<IResult> TokenProlongate(ITokenService serivce, IHttpContextAccessor accessor)
        {
            return Results.Ok("Work in progress!");
        }

        // очищает хедер и делает редирект на гугл
        public static async Task<IResult> LogoutUser(IHttpContextAccessor accessor)
        {
            accessor.HttpContext.Response.Clear();
            return Results.Redirect("https://www.google.com/");
        }

        // возвращает id пользователя по его токену
        public static async Task<IResult> TokenInfo(ITokenService serivce, IHttpContextAccessor accessor)
        {
            var authHeader = accessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString();
            string token = authHeader.Split(new char[] { ' ' })[1];
            Token user_id = serivce.GetToken(token);
            if (user_id == null) return Results.NotFound("User not found or token invalid");
            return Results.Ok(user_id);
        }
    }
}
