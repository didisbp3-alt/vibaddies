namespace MVC_Buddies.Dtos
{
    public class AnnouncementListDto
    {
        public string? PetSitterName { get; set; } = null!;
        public string? PetSitterFullName { get; set; }


        public string? AvatarUrl { get; set; }

        public double? Rating { get; set; }
        public int? ReviewsCount { get; set; }
        //public List<string> Services { get; set; }


        //public List<string>? Services { get; set; }
        //public List<int> AnnouncementIds { get; set; } = new();

        public int Id { get; set; }
        public int? PetSitterId { get; set; }
        public string? LocationName { get; set; } = "";
        public List<string> Services { get; set; } = new();
        public decimal? PricePerDay { get; set; }



        public int? LocationId { get; set; }

        public List<string> Species { get; set; } = new();

    }


}
