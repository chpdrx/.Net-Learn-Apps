using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using resource_app.Data;
using resource_app.Models;
using resource_app.Service;

namespace resource_app.Endpoints
{
    public static class ApiEndpoints
    {
        public static void MapApiEndpoints(this WebApplication app)
        {
            // урлы методов api, и их соответствие функциональным методам
            app.UseSwagger();
            app.MapGet("/Resource/About", AboutApi);
            app.MapGet("/Resource/AllUsers",
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
                (IUserService service) => GetAllUsers(service))
                .Produces<List<Users>>(statusCode: 200, contentType: "application/json");
            app.MapDelete("/Resource/DeleteUser",
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
                (string user_id, IUserService service) => DeleteUser(user_id, service));
            app.MapGet("/Resource/GetUserByUsername",
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
                (string username, IUserService service) => GetUserByUsername(username, service))
                .Produces<Users>(statusCode: 200, contentType: "application/json");
            app.MapGet("/Resource/GetUserById",
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
                (string user_id, IUserService service) => GetUserById(user_id, service))
                .Produces<Users>(statusCode: 200, contentType: "application/json");
            app.MapPut("/Resource/UserUpdate",
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
                (string user_id, Users user, IUserService service) => UserUpdate(user_id, user, service))
                .Accepts<Users>(contentType: "application/json")
                .Produces<Users>(statusCode: 200, contentType: "application/json");
        }

        public static async Task<IResult> AboutApi()
        {
            string result = "Resource API for wb_analytics";
            return Results.Ok(result);
        }

        public static async Task<IResult> GetAllUsers(IUserService service)
        {
            List<Users> users = service.GetAllUsers();
            if (users == null) return Results.NotFound("Users not found");
            return Results.Ok(users);
        }

        public static async Task<IResult> DeleteUser(string user_id, IUserService service)
        {
            if (!service.DeleteUser(user_id)) return Results.NotFound("User not found");
            return Results.Ok();
        }

        public static async Task<IResult> GetUserByUsername(string username, IUserService service)
        {
            Users user = service.GetUserByUsername(username);
            if (user == null) return Results.NotFound("Username not found");
            return Results.Ok(user);
        }

        public static async Task<IResult> GetUserById(string user_id, IUserService service)
        {
            try
            {
                Guid userId = Guid.Parse(user_id);
                Users user = service.GetUserById(userId);
                if (user == null) return Results.NotFound("User Id not found");
                return Results.Ok(user);
            }
            catch
            {
                return Results.BadRequest("Id not valid");
            }   
        }

        public static async Task<IResult> UserUpdate(string user_id, Users user, IUserService service)
        {
            Users user_updated = service.UserUpdate(user_id, user);
            if (user_updated == null) return Results.NotFound("User not found");
            return Results.Ok(user_updated);
        }
    }
}
