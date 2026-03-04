namespace MVC_Buddies.Dtos
{
    public class PetSitterEditDto
    {
        public int Id { get; set; }

        public string FullName { get; set; }
        public string PhotoUrl { get; set; }
        public int? LocationId { get; set; }
        public int? YearsExperience { get; set; }

        public List<int> SelectedSkillIds { get; set; } = new();
        public List<int> SelectedSpeciesIds { get; set; } = new();
        public string SubscriptionType { get; set; }
    }
}
