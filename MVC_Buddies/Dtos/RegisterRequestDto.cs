using System.ComponentModel.DataAnnotations;

namespace MVC_Buddies.Dtos
{
   
        public class RegisterRequestDto
        {
            [Required(ErrorMessage = "O nome de utilizador é obrigatório")]
            [StringLength(50, MinimumLength = 3)]
            public string UserName { get; set; } = string.Empty;

            [Required(ErrorMessage = "O email é obrigatório")]
            [EmailAddress(ErrorMessage = "Email inválido")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "A palavra-passe é obrigatória")]
            [StringLength(100, MinimumLength = 6, ErrorMessage = "A palavra-passe deve ter pelo menos 6 caracteres")]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            // ← Esta propriedade é essencial para a view + validação
            [Required(ErrorMessage = "Confirme a palavra-passe")]
            [Compare("Password", ErrorMessage = "As palavras-passe não coincidem")]
            [DataType(DataType.Password)]
            [Display(Name = "Confirmar palavra-passe")]
            public string ConfirmPassword { get; set; } = string.Empty;

            [Required(ErrorMessage = "Selecione o tipo de conta")]
            public string AccountType { get; set; } = string.Empty; // "PetOwner" ou "PetSitter"
        }
    

    public class CreatePetOwnerViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nome completo")]
        [StringLength(100, MinimumLength = 3)]
        public string? FullName { get; set; }

        public string Email { get; set; } 

        [Display(Name = "Foto de perfil")]
        public string? PhotoUrl { get; set; }

        [Display(Name = "Escolher foto de perfil")]
        public IFormFile? ProfilePicture { get; set; }

        [Display(Name = "Sobre mim")]
        [StringLength(800)]
        public string? Bio { get; set; }

        [Display(Name = "Localização")]
        public int? SelectedLocationId { get; set; }
        public List<LocationDto> Locations { get; set; } = new();

        [Display(Name = "Animais")]
        public List<int> SelectedPetIds { get; set; } = new();
        public List<PetDto> Pets { get; set; } = new();

        [Display(Name = "Morada")]
        public string? Address { get; set; }

        [Display(Name = "Código Postal")]
        public string? PostalCode { get; set; }

        [Display(Name = "Telefone")]
        public string? PhoneNumber { get; set; }

    }



        public class CreatePetSitterViewModel
    {
        // Identificador do utilizador autenticado
        public int Id { get; set; }

        // =========================
        // DADOS BÁSICOS
        // =========================

        [Display(Name = "Nome completo")]
        [Required(ErrorMessage = "O nome completo é obrigatório")]
        [StringLength(100, MinimumLength = 3)]
        public string? FullName { get; set; }

        [Display(Name = "Foto de perfil")]
        public string? PhotoUrl { get; set; }

        [Display(Name = "Escolher foto de perfil")]
        public IFormFile? ProfilePicture { get; set; }

        [Display(Name = "Sobre mim / Apresentação")]
        [StringLength(1500)]
        public string? Bio { get; set; }

        // =========================
        // CAMPOS ESPECÍFICOS PETSITTER
        // =========================

        [Display(Name = "Preço por dia (€)")]
        [Required(ErrorMessage = "O preço por dia é obrigatório")]
        [Range(0, 10000, ErrorMessage = "Preço inválido")]
        public decimal? PricePerDay { get; set; }

        [Display(Name = "Anos de experiência")]
        [Range(0, 50, ErrorMessage = "Anos de experiência entre 0 e 50")]
        public int? YearsExperience { get; set; }

        [Display(Name = "Localização")]
        [Required(ErrorMessage = "A localização é obrigatória")]
        public int? LocationId { get; set; }

        [Display(Name = "Tipo de subscrição")]
        [Required(ErrorMessage = "O tipo de subscrição é obrigatório")]
        public string? SubscriptionType { get; set; }

        // =========================
        // SERVIÇOS / ESPÉCIES
        // =========================

        [Display(Name = "Serviços que oferece")]
        [MinLength(1, ErrorMessage = "Selecione pelo menos um serviço")]
        public List<int> SelectedSkillIds { get; set; } = new();

        [Display(Name = "Animais que aceita")]
        [MinLength(1, ErrorMessage = "Selecione pelo menos uma espécie")]
        public List<int> SelectedSpeciesIds { get; set; } = new();

        // =========================
        // DISPONIBILIDADE
        // =========================

        [Display(Name = "Disponível durante o dia")]
        public bool AvailableForDaytime { get; set; }

        [Display(Name = "Disponível para pernoita")]
        public bool AvailableForOvernight { get; set; }

        [Display(Name = "Disponível aos fins de semana")]
        public bool AvailableForWeekends { get; set; }

        // =========================
        // LISTAS PARA DROPDOWNS
        // =========================

        public List<SkillItemDto> Skills { get; set; } = new();
        public List<SpeciesDto> Species { get; set; } = new();
        public List<LocationDto> Locations { get; set; } = new();

        // Opcional: se quiseres dropdown fixo de subscrições
        public List<string> SubscriptionTypes { get; set; } = new()
    {
        "Basic",
        "Premium",
        "Pro"
    };
    }

}
