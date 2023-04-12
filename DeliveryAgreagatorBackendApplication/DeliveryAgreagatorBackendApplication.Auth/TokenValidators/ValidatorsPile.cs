using DeliveryAgreagatorApplication.Auth.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace DeliveryAgreagatorBackendApplication.Auth.TokenValidators
{
    public class ValidatorsPile
    {
        public static Task ValidateTokenParent(TokenValidatedContext context)
        {
            string refreshTokenId = context.Principal.Claims.FirstOrDefault(c => c.Type == "RefreshIdClaim").Value;
            Guid refreshTokenGuidId;
            Guid.TryParse(refreshTokenId, out refreshTokenGuidId);
            var dbcontext = context.HttpContext.RequestServices.GetService<AuthDbContext>();

            if (dbcontext.RefreshTokens.FirstOrDefault(x => x.Id == refreshTokenGuidId && x.Expires > DateTime.UtcNow) == null)
                context.Fail("This token has been revoked!");

            return Task.CompletedTask;

        }
    }
}

