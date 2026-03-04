using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_Buddies.Dtos
{
    public class MainViewModel
    {
        public string? Location { get; set; }
        public List<SitterCardDto> Sitters { get; set; } = new();

        public List<ServiceDto> Services { get; set; } = new(); 
        public int? SelectedServiceId { get; set; }
        public bool IsSearch { get; set; }

        


    }


}
