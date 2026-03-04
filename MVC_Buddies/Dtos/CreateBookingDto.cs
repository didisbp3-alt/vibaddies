namespace MVC_Buddies.Dtos
{
    public class CreateBookingDto
    {
        public int PetOwnerId { get; set; }
        public int AnnouncementId { get; set; }
        public int PetId { get; set; }
        public string Status { get; set; } = "Pending"; // valor default
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; } = 0;
        public decimal Commission { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
