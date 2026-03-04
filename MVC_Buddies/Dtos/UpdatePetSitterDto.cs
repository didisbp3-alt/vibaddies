namespace MVC_Buddies.Dtos
{
    public class UpdatePetSitterDto
    {
        public int Id { get; set; }

        public string? FullName { get; set; }
        public string? PhotoUrl { get; set; }
        public int? LocationId { get; set; }
        public int? YearsExperience { get; set; }

        public List<int>? SelectedSkillIds { get; set; } = new();
        public List<int>? SelectedSpeciesIds { get; set; } = new();
        public string? SubscriptionType { get; set; }

        public List<string>? NewSkills { get; set; } = new();
        public List<string>? NewSpecies { get; set; } = new();
    }
}
