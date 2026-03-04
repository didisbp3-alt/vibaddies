namespace MVC_Buddies.Dtos
{
    public class PetSitterProfileDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;

        public string? AvatarUrl { get; set; }

        public decimal PricePerDay { get; set; }

        public double Rating { get; set; }
        public int ReviewsCount { get; set; }

        public List<string> Services { get; set; } = new();

        public string? Description { get; set; }
    }
}
