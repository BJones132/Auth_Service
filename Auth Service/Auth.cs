using Auth_Service.Data;
using Auth_Service.Data.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Auth_Service
{
    public class Auth
    {
        private Crypto crypt;
        public Auth() {
            crypt = new Crypto();
        }

        public async Task<Results<JsonHttpResult<Token>, UnauthorizedHttpResult>> Authenticate(AuthDb db, UserDTO u)
        {
            var selectedUser = await db.Users.Where(e => e.username == u.username).FirstOrDefaultAsync();

            if (selectedUser == null || selectedUser.locked_out || u.password == string.Empty)
                return TypedResults.Unauthorized();

            if(!crypt.VerifyPassword(u.password, selectedUser.password))
            {
                selectedUser.failed_attempts += 1;
                if (selectedUser.failed_attempts >= 3)
                    selectedUser.locked_out = true;
                await db.SaveChangesAsync();
                return TypedResults.Unauthorized();
            }

            if (selectedUser.access_token != null && selectedUser.token_expiry != null && selectedUser.token_expiry > DateOnly.FromDateTime(DateTime.Now))
            {
                if (selectedUser.failed_attempts > 0)
                {
                    selectedUser.failed_attempts = 0;
                    selectedUser.locked_out = false;
                    await db.SaveChangesAsync();
                }
                return TypedResults.Json(new Token { access_token = selectedUser.access_token });
            }

            string tokenString = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            selectedUser.token_expiry = DateOnly.FromDateTime(DateTime.Now).AddDays(14);
            selectedUser.access_token = tokenString;
            selectedUser.failed_attempts = 0;
            selectedUser.locked_out = false;
            await db.SaveChangesAsync();
            return TypedResults.Json(new Token { access_token = selectedUser.access_token });
        }

        public async Task<Results<JsonHttpResult<int>, UnauthorizedHttpResult>> IdFromToken(AuthDb db, HttpContext context)
        {
            var authHeader = context.Request.Headers.Authorization.ToString();

            if (!authHeader.StartsWith("Bearer "))
                return TypedResults.Unauthorized();

            var token = authHeader.Replace("Bearer ", "");

            var user = await db.Users.Where(e => e.access_token == token).FirstOrDefaultAsync();
            if (user == null || user.locked_out || user.token_expiry < DateOnly.FromDateTime(DateTime.Now))
                return TypedResults.Unauthorized();

            return TypedResults.Json(user.id);
        }

        //PLACEHOLDER FOR DEVELOPMENT
        public async Task<Results<Created<User>, Conflict>> Register(AuthDb db, UserDTO u)
        {
            if(await db.Users.Where(e=>e.username == u.username).FirstOrDefaultAsync() != null)
                return TypedResults.Conflict();

            User createdUser = new User
            {
                username = u.username,
                password = crypt.HashPassword(u.password)
            };

            db.Users.Add(createdUser); 
            await db.SaveChangesAsync();

            return TypedResults.Created($"/register/{u.username}", createdUser);
        }
    }
}
