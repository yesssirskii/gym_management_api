using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace gym_management_api.Middleware;

public class JwtMiddleware(RequestDelegate next, IConfiguration configuration)
{
    public async Task Invoke(HttpContext context, ApplicationDbContext dbContext)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            await AttachUserToContext(context, dbContext, token);
        }

        await next(context);
    }

    private async Task AttachUserToContext(HttpContext context, ApplicationDbContext dbContext, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:SecretKey"]);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = configuration["Jwt:Audience"],
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

            // Attach user to context
            var user = await dbContext.Users.FindAsync(userId);
            context.Items["User"] = user;
        }
        catch
        {
            // Token validation failed
        }
    }
}