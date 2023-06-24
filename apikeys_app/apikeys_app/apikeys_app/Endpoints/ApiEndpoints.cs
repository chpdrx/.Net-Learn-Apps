using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using apikeys_app.Model;
using apikeys_app.Service;
using System.Net.Http.Headers;
using Microsoft.Net.Http.Headers;

namespace apikeys_app.Endpoints
{
    public static class ApiEndpoints
    {
        public static void MapApiEndpoints(this WebApplication app)
        {
            // урлы методов api, и их соответствие функциональным методам
            app.UseSwagger();
            app.MapGet("/Apikeys/About", AboutApi);
            app.MapGet("/Apikeys/GetAllApiKeys",
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
                (string user_id, IApikeyService service) => GetAllKeys(user_id, service))
                .Produces<List<Apikey>>(statusCode: 200, contentType: "application/json");
            app.MapGet("/Apikeys/GetKey",
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
                (string key_id, IApikeyService service) => GetKey(key_id, service))
                .Produces<Apikey>(statusCode: 200, contentType: "application/json");
            app.MapPost("/Apikeys/CreateKey",
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
                (string user_id, Apikey key, IApikeyService service) => CreateKey(user_id, key, service))
                .Accepts<Apikey>(contentType: "application/json")
                .Produces<Apikey>(statusCode: 200, contentType: "application/json");
            app.MapPut("/Apikeys/UpdateKey",
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
                (Apikey key, IApikeyService service) => UpdateKey(key, service))
                .Accepts<Apikey>(contentType: "application/json")
                .Produces<Apikey>(statusCode: 200, contentType: "application/json"); ;
            app.MapDelete("/Apikeys/DeleteKey",
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
                (string key_id, IApikeyService service) => DeleteKey(key_id, service))
                .Produces(statusCode: 200);
        }

        // базовый метод, возвращающий строку с информацией об api
        public static async Task<IResult> AboutApi()
        {
            string result = "Apikeys API for wb_analytics";
            return Results.Ok(result);
        }

        // возвращает все apikeys для пользователя с user_id
        public static async Task<IResult> GetAllKeys(string user_id, IApikeyService service)
        {
            List<Apikey> keys = service.GetAll(user_id);
            if (keys == null) return Results.NotFound("Keys or User not found");
            return Results.Ok(keys);
        }

        // создаёт новый apikey, принадлежащий пользователю user_id
        public static async Task<IResult> CreateKey(string user_id, Apikey key, IApikeyService service)
        {
            Apikey apikey = service.Create(user_id, key);
            if (apikey == null) return Results.Problem("Apikey not created");
            return Results.Ok(apikey);
        }

        // обновляет информацию об apikey
        public static async Task<IResult> UpdateKey(Apikey key, IApikeyService service)
        {
            Apikey apikey = service.Update(key);
            if (apikey == null) return Results.NotFound("Apikey not found");
            return Results.Ok(apikey);
        }

        // удаляет apikey по его id
        public static async Task<IResult> DeleteKey(string key_id, IApikeyService service)
        {
            if (service.Delete(key_id)) return Results.Ok();
            else return Results.Problem("Key not deleted");
        }

        // возвращает все apikey по key_id
        public static async Task<IResult> GetKey(string key_id, IApikeyService service)
        {
            try
            {
                Guid.Parse(key_id);
            }
            catch 
            {
                return Results.BadRequest("Guid not valid");
            };
            Apikey apikey = service.GetKey(key_id.ToString());
            if (apikey == null) return Results.NotFound("Key not found");
            return Results.Ok(apikey);
        }
    }
}
