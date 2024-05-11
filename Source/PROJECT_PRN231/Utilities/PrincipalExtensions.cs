using System.Security.Claims;

namespace PROJECT_PRN231.Utilities
{
    public static class PrincipalExtensions
    {
        public static string GetUsernameFromClaim(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue("username");
        }
    }
}
