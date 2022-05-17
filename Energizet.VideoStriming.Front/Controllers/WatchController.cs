using Microsoft.AspNetCore.Mvc;

namespace Energizet.VideoStriming.Front.Controllers
{
	public class WatchController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
