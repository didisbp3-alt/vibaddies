using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC_Buddies.Dtos
{
    public class AnnouncementListViewModel
    {
       
            public List<AnnouncementSearchResultDto> Announcements { get; set; } = new List<AnnouncementSearchResultDto>();
            public int? SpeciesId { get; set; }
            public int? ServiceId { get; set; }
            public int? LocationId { get; set; }

            public List<SpeciesDto> Species { get; set; } = new();
            public List<ServiceOptionDto> Services { get; set; } = new();
            public List<LocationDto> Location { get; set; } = new();
        

    }
}
