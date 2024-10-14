using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace MyChatApp.Infrastructure
{
    public class BaseController : Controller
    {
        protected string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
