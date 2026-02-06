using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected int GetUserId()
        {
            var claims = User.Claims.ToList();

            var userIdClaim = claims.FirstOrDefault(c => c.Type == "nameid")?.Value
                             ?? claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim))
                throw new UnauthorizedAccessException("Usuário não identificado no token.");

            if (!int.TryParse(userIdClaim, out int id))
            {
                throw new FormatException($"Esperado ID numérico, mas recebeu: '{userIdClaim}'.");
            }

            return id;
        }
    }
}
