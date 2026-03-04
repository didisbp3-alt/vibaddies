namespace MVC_Buddies.Dtos
{
    public class AnnouncementDetailsDto
    {
        public int Id { get; set; }

        public string LocationName { get; set; }

        public int? LocationId { get; set; }

        public int? IdPetsitter { get; set; }

        public string FullName { get; set; }

        public string PhotoUrl { get; set; }

        public int? YearsExperience { get; set; }

        public bool? IsApproved { get; set; }

        public bool? IsActive { get; set; }

        public List<SkillOptionDto> Skills { get; set; } = new();

        public List<ReviewDto> Reviews { get; set; } = new();
    }
}


