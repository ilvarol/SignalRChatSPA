using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRChatSPA.Controllers
{
    [Route("chat")]
    public class ChatController : Controller
    {
        [HttpGet]
        public IActionResult Index() 
        {
            return View();
        }
    }
}
