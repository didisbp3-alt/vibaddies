namespace MVC_Buddies.Dtos
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string Comment { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public string? PetName { get; set; }
        public List<string>? Services { get; set; } = new();
    }

    public class CreateReviewDto
    {
        public int BookingId { get; set; }
        public string Comment { get; set; } = "";
    }

    public class ReviewListViewModel
    {
        public List<ReviewDto> Reviews { get; set; } = new();
        public int? PetSitterId { get; set; }
    }
}
