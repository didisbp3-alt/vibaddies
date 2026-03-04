
namespace MVC_Buddies.Dtos
{
    public class AnnouncementSearchResultDto
    {
        public int Id { get; set; }
        public int? PetSitterId { get; set; }
        public string? PetSitterName { get; set; }
        public string? PetSitterFullName { get; set; }
        public string? LocationName { get; set; }
        public string? AvatarUrl { get; set; }
        public double? Rating { get; set; }
        public int? ReviewsCount { get; set; }
        public decimal? PricePerDay { get; internal set; }
        public List<string> Services { get; internal set; }
    }

}
