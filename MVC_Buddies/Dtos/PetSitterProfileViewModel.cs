namespace MVC_Buddies.Dtos
{
    public class PetSitterProfileViewModel
    {
        public int Id { get; set; }                 // UserId
        public string UserName { get; set; } = "";  // Nome de usuário
        public string Email { get; set; } = "";     // Email

        public string? FullName { get; set; }       // Nome completo
        public string? PhotoUrl { get; set; }       // Foto
        public int? YearsExperience { get; set; }   // Anos de experiência
        public string? SubscriptionType { get; set; } // Tipo de subscrição

        public string? LocationName { get; set; }   // Nome do local
        public List<string> Skills { get; set; } = new();   // Lista de skills em texto
        public List<string> Species { get; set; } = new();  // Lista de espécies em texto
    }
}
