namespace MVC_Buddies.Dtos
{
    public class AccountEditDto
    {
        //  User
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool? EmailConfirmed { get; set; }

        // 🔹 Tipo de perfil
        public string ProfileType { get; set; } // "PetSitter" ou "PetOwner"

        // PetSitter 
        public int? PetSitterId { get; set; }
        public string? FullName { get; set; }
        public string? PhotoUrl { get; set; }
        public int? LocationId { get; set; }
        public int? YearsExperience { get; set; }
        public string? SubscriptionType { get; set; }
        public List<int> SelectedSkillIds { get; set; } = new();
        public List<int> SelectedSpeciesIds { get; set; } = new();

        // PetOwner
        public int? PetOwnerId { get; set; }
    }
}
