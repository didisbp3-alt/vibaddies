
using System.ComponentModel.DataAnnotations;

namespace MVC_Buddies.Dtos
{

    public class PetSitterDashboardVm
    {
        public List<PetSitterSkillDto> Skills { get; set; } = new();
        public List<PetSitterSpeciesDto> Species { get; set; } = new();
        public List<PetsitterAnnouncementDto> Announcements { get; set; } = new();
        public List<PetsitterReviewDto> Reviews { get; set; } = new();

        public UpdatePetSitterSkillsDto AddSkills { get; set; } = new();
        public UpdatePetsitterSpeciesDto AddSpecies { get; set; } = new();
        public CreateAnnouncementDto NewAnnouncement { get; set; } = new();
    }


    public partial class PetSitterSkillDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }

    public partial class PetSitterProfileCreateDto
    {
        [Required]
        public string FullName { get; set; } = "";

        public string? Bio { get; set; }
        public decimal? PricePerDay { get; set; }
    }


    public partial class UpdatePetSitterSkillsDto
    {
        public List<int> SkillIds { get; set; } = new();

    }

    public partial class PetSitterSpeciesDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }

    public partial class UpdatePetsitterSpeciesDto
    {
        public List<int> SpeciesIds { get; set; } = new();
    }

    public partial class PetsitterAnnouncementDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }

    public partial class CreateAnnouncementDto
    {
        public int LocationId { get; set; }

        public List<int> ServiceIds { get; set; } = new();

        //public List<DateTime> SelectedDates { get; set; } = new();

        public List<LocationDto> Locations { get; set; } = new();

        public List<ServiceDto> Services { get; set; } = new();

        public List<int> SpeciesIds { get; set; } = new();   // 🔥 FALTA ISTO

        public List<SpeciesDto> Species { get; set; } = new();

        public List<int>? SelectedServiceIds { get; set; }
        public List<int>? SelectedSpeciesIds { get; set; }

        public List<string>? NewServices { get; set; }
        public List<string>? NewSpecies { get; set; }

    }

    public partial class PetsitterReviewDto
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = "";
        public DateTime CreatedAtUtc { get; set; }
    }

}
