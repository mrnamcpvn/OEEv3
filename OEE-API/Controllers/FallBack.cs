using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace OEE_API.Controllers
{
    public class FallBack : Controller
    {
        public IActionResult Index()
        {
            // Controller page navigation
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(),
            "wwwroot", "index.html"), "text/HTML");
        }
    }
}