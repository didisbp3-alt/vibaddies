namespace MVC_Buddies.Dtos
{
    public class ConversationDto
    {
        public int ConversationId { get; set; }
    }

    public class MessageDto
    {
        public int Id { get; set; }
        public int SenderUserId { get; set; }
        public string Body { get; set; } = "";
    }

    public class SendMessageDto
    {
        public int ConversationId { get; set; }
        public string Body { get; set; } = "";
    }

    public class ChatViewModel
    {
        public int ConversationId { get; set; }
        public List<MessageDto> Messages { get; set; } = new();
        public SendMessageDto NewMessage { get; set; } = new();
    }
}
