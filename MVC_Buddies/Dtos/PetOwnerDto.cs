using System.ComponentModel.DataAnnotations;

namespace MVC_Buddies.Dtos
{
    public class PetOwnerDashboardVm
    {
        public List<PetOwnerPetDto> Pets { get; set; } = new();
        public List<ServiceDto> Services { get; set; } = new();
        public List<PetOwnerBookingDto> Bookings { get; set; } = new();
        public CreateServiceDto NewService { get; set; } = new();
    }

    public class PetOwnerProfileCreateDto
    {
        [Required]
        public string FullName { get; set; } = "";

        public string? Phone { get; set; }
        public string? Location { get; set; }
    }

    public class PetOwnerPetDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime? BirthDate { get; set; }
        public string? Notes { get; set; }
        public int BreedId { get; set; }
        public string BreedName { get; set; } = "";
        public int SpeciesId { get; set; }
        public string SpeciesName { get; set; } = "";
    }

    //public class ServiceDto
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; } = "";
    //    public string? Description { get; set; }
    //}

    public class CreateServiceDto
    {
        public string Name { get; set; } = "";
        public string? Description { get; set; }
    }

    public class PetOwnerBookingDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = "";
        public int StatusId { get; set; }
        public string StatusName { get; set; } = "";
    }

}
