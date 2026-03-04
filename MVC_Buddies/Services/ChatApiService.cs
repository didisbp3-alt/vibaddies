using MVC_Buddies.Dtos;

namespace MVC_Buddies.Services
{
    public interface IChatService
    {
        Task<ConversationDto> OpenConversationAsync(int bookingId);
        Task<List<MessageDto>> GetMessagesAsync(int conversationId);
        Task SendMessageAsync(SendMessageDto dto);
    }

    public class ChatService : IChatService
    {
        private readonly HttpClient _api;

        public ChatService(IHttpClientFactory factory)
        {
            _api = factory.CreateClient("API_Buddies");
        }

        public async Task<ConversationDto> OpenConversationAsync(int bookingId)
        {
            var result = await _api.GetFromJsonAsync<ConversationDto>($"chat/open/{bookingId}");
            if (result == null) throw new Exception("Erro ao abrir conversa");
            return result;
        }

        public async Task<List<MessageDto>> GetMessagesAsync(int conversationId)
        {
            var messages = await _api.GetFromJsonAsync<List<MessageDto>>($"chat/{conversationId}");
            return messages ?? new List<MessageDto>();
        }

        public async Task SendMessageAsync(SendMessageDto dto)
        {
            var response = await _api.PostAsJsonAsync("chat/send", dto);
            response.EnsureSuccessStatusCode();
        }
    }
}
