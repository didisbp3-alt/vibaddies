namespace MVC_Buddies.Dtos
{
    public class AccountProfileDto
    {
        public int UserId { get; set; }          // Id do usuário
        public string UserName { get; set; }     // Nome de usuário
        public string Email { get; set; }        // Email
        public string RoleName { get; set; }     // "PetSitter" ou "PetOwner"

        public string Bio {  get; set; }
        // 🔹 Campos específicos de PetSitter
        public string? FullName { get; set; }       // Nome completo do PetSitter
        public string? PhotoUrl { get; set; }       // Foto
        public int? YearsExperience { get; set; }   // Anos de experiência
        public int? LocationId { get; set; }        // Localização
        public string? SubscriptionType { get; set; } // Tipo de subscrição
        public List<int> SelectedSkillIds { get; set; } = new();
        public List<int> SelectedSpeciesIds { get; set; } = new();
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
