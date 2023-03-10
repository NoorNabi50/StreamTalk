using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PersistCommunication.Models;
using PersistCommunicator.Abstractions;
using System.ComponentModel;
using System.Diagnostics;

namespace PersistCommunication.Controllers
{
    public class ChatController : Controller
    {
        private readonly ILogger<ChatController> _logger;

        private readonly IChatManager chatManager;
        public ChatController(IChatManager _chatManager)
        {
            chatManager = _chatManager;
        }

        public IActionResult Index()
        {
            return View(chatManager.GetRooms());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}