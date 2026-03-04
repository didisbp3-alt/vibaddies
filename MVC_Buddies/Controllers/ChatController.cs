using Microsoft.AspNetCore.Mvc;
using MVC_Buddies.Dtos;
using MVC_Buddies.Models;

namespace MVC_Buddies.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MVC_Buddies.Services;

    public class ChatController : Controller
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        // Abrir conversa para um booking
        public async Task<IActionResult> Open(int bookingId)
        {
            var conversation = await _chatService.OpenConversationAsync(bookingId);

            var messages = await _chatService.GetMessagesAsync(conversation.ConversationId);

            var model = new ChatViewModel
            {
                ConversationId = conversation.ConversationId,
                Messages = messages,
                NewMessage = new SendMessageDto { ConversationId = conversation.ConversationId }
            };

            return View(model);
        }

        // Enviar mensagem
        [HttpPost]
        public async Task<IActionResult> Send(ChatViewModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.NewMessage.Body))
            {
                await _chatService.SendMessageAsync(model.NewMessage);
            }

            return RedirectToAction("Open", new { bookingId = model.NewMessage.ConversationId });
        }
    }
}
