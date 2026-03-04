using System.ComponentModel.DataAnnotations;

namespace MVC_Buddies.Dtos
{
    //  KPIs 
    public record AdminKpisDto(
        int TotalUsers,
        int TotalPetSitters,
        int TotalOwners,
        int TotalAnnouncements,
        int TotalBookings,
        decimal TotalRevenue,
        decimal RevenueLast30Days
    );

    //  Users List 
    public record AdminUserListItemDto(
        int Id,
        string FullName,
        string Email,
        string Role,
        bool IsActive,
        DateTime CreatedAt
    );

    public record PagedResult<T>(int Total, int Page, int PageSize, List<T> Items);

    public record UpdateUserStatusDto(bool IsActive);

    // PetSitters Pending 
    public record PendingPetSitterItemVm(
        int PetSitterId,
        int UserId,
        string Name,
        string Email,
        int? ExperienceYears,
        string? Bio,
        bool IsApproved
    );

    public record ApprovePetSitterDto(bool IsApproved);

    // Announcements 
    public record AnnouncementAdminItemVm(
        int Id,
        string? Description,
        DateTime CreatedAt,
        bool IsActive,
        string? Location,
        int? PetSitterId,
        bool? PetSitterApproved
    );

    public record SetAnnouncementActiveDto(bool IsActive);

    // CRUD refs 
    public record RefItemDto(int Id, string Name);
    public record RefCreateDto(string Name);
    public record RefUpdateDto(string Name);

    // Species/Breeds
    public record BreedItemDto(int Id, int SpeciesId, string Name);
    public record BreedCreateDto(int SpeciesId, string Name);

    //  Page VMs 
    public class AdminDashboardVm
    {
        public AdminKpisDto? Kpis { get; set; }
    }


    //location 
    public record LocationCreateDto(
    string Name,
    string District,
    string Country
);

    public record LocationUpdateDto(
        string Name,
        string District,
        string Country
    );

    //services
    public record ServiceCreateDto(string Name);

    //skills 
    public record SkillCreateDto(string Name);

    //species
    public record SpeciesCreateDto(string Name);


    public class ProfitReportViewModel
    {
        [Display(Name = "Ano")]
        public int Year { get; set; }

        public ProfitSummaryDto Summary { get; set; } = new();
        public List<ProfitHistoryDto> History { get; set; } = new();
        public List<int> AvailableYears { get; set; } = new();
    }

    public class ProfitHistoryDto
    {
        [Display(Name = "Lucro Líquido")]
        public int NetProfit { get; set; }

        [Display(Name = "Período")]
        [DataType(DataType.Date)]
        public DateTime Period { get; set; }

        [Display(Name = "Receita")]
        public int Revenue { get; set; }
    }

    public class ProfitSummaryDto
    {
        [Display(Name = "Lucro Líquido")]
        public int NetProfit { get; set; }

        [Display(Name = "Receita Total")]
        public int TotalRevenue { get; set; }

        [Display(Name = "Comissões")]
        public int TotalCommission { get; set; }
    }


}

