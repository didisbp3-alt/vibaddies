namespace MVC_Buddies.Dtos
{
    public class BookingViewModel
    {
        public int AnnouncementId { get; set; }

        public int PetId { get; set; }
        public int SkillId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int SelectedServiceId { get; set; }
        public List<PetOptionDto> Pets { get; set; } = new();
        public List<SkillOptionDto> Skills { get; set; } = new();

        public List<ServiceOptionDto> Services { get; set; } = new();


        public decimal TotalPrice { get; set; } = 0;
        public decimal Commission { get; set; } = 0;

    }

}
