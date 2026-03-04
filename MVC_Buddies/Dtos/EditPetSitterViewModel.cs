namespace MVC_Buddies.Dtos
{
    public class EditPetSitterViewModel
    {
        public int Id { get; set; }

        // USER
        public string UserName { get; set; }
        public string Email { get; set; }

        // PETSITTER
        public string FullName { get; set; }
        public string PhotoUrl { get; set; }
        public int? YearsExperience { get; set; }
        public int? LocationId { get; set; }
        public string SubscriptionType { get; set; }

        // CHECKBOX LISTS
        public List<int> SelectedSkillIds { get; set; } = new();
        public List<int> SelectedSpeciesIds { get; set; } = new();

        // DATA SOURCES
        public List<SkillItemDto> Skills { get; set; } = new();
        public List<SpeciesDto> Species { get; set; } = new();
        public List<LocationDto> Locations { get; set; } = new();

        // OPTIONAL NEW ITEMS
        public List<string> NewSkills { get; set; } = new();
        public List<string> NewSpecies { get; set; } = new();


    }
}
