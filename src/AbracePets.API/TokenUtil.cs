using System.Security.Claims;

namespace AbracePets.API
{
    internal class TokenUtil
    {

        public static Guid GetUsuarioId(ClaimsPrincipal principal)
        {
            return Guid.Parse(principal.Identities.First().Claims.ToList()[0].Value);
        }

    }
}
