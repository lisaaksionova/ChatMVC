using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyChatApp.Database;
using MyChatApp.Models;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;

namespace MyChatApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private AppDbContext _ctx;
        public HomeController(AppDbContext ctx) => _ctx = ctx;
        public IActionResult Index()
        {
            var chats = _ctx.Chats
                .Include(x=>x.Users)
                .Where(x=>!x.Users
                .Any(y=>y.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value))
                .ToList();
            return View(chats);
        }

        public IActionResult Find()
        {
            var users = _ctx.Users
                .Where(x => x.Id != User.FindFirst(ClaimTypes.NameIdentifier).Value)
                .ToList();
            return View(users);
        }
        public IActionResult Private()
        {
            var chats = _ctx.Chats
                .Include(x => x.Users)
                .ThenInclude(x=>x.User)
                .Where(x => x.Type == ChatType.Private
                && x.Users.Any(y => y.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value))
                .ToList();
            return View(chats);
        }
        public async Task<IActionResult> CreatePrivateRoom(string userId)
        {
            var chat = new Chat
            {
                Name = userId,
                Type = ChatType.Private
            };
            chat.Users.Add(new ChatUser
            {
                UserId = userId
            });
            chat.Users.Add(new ChatUser
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value
            });
            _ctx.Chats.Add(chat);
            await _ctx.SaveChangesAsync();
            return RedirectToAction("Chat", new {id = chat.Id});
        }
        [HttpGet("{id}")]
        public IActionResult Chat(int id)
        {
            var chat = _ctx.Chats
                .Include(x=>x.Messages)
                .FirstOrDefault(x => x.Id == id);
            return View(chat);
        }
        [HttpGet]
        public async Task<IActionResult> JoinRoom(int id)
        {
            var chatUser = new ChatUser
            {
                ChatId = id,
                UserRole = UserRole.Member,
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value
            };
            _ctx.ChatUsers.Add(chatUser);
            await _ctx.SaveChangesAsync();
            return RedirectToAction("Chat", "Home", new { id = id });
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(string name)
        {
            var chat = new Chat
            {
                Name = name,
                Type = ChatType.Room
            };
            chat.Users.Add(new ChatUser
            {
                UserRole = UserRole.Admin,
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value
            });
            _ctx.Chats.Add(chat);
            await _ctx.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> CreateMessage(int chatId, string message)
        {
            _ctx.Messages.Add(new Message
            {
                ChatId = chatId,
                Text = message,
                Timestamp = DateTime.Now,
                UserName = User.Identity.Name
            });
            await _ctx.SaveChangesAsync();
            return RedirectToAction("Chat", new {id=chatId});
        }
    }
}
