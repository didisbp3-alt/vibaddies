namespace MVC_Buddies.Dtos
{
    public class SitterCardDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string City { get; set; } = "";
        public string AvatarUrl { get; set; } = "";
        public decimal PricePerDay { get; set; }
        public double Rating { get; set; }
        public int ReviewsCount { get; set; }
        public List<string>? Services { get; set; }
        public List<int> AnnouncementIds { get; set; } = new();

    }
}
