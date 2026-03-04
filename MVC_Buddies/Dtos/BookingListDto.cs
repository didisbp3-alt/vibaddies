namespace MVC_Buddies.Dtos
{
    public class BookingListDto
    {
        public int BookingId { get; set; }
        public string PetName { get; set; }
        public string PetSitterName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public decimal? TotalPrice { get; set; }

        public int? AnnouncementId { get; set; }

        public int? PetOwnerId { get; set; }

        public int? PetId { get; set; }

        public decimal? PricePaid { get; set; }

        public decimal? Commission { get; set; }

        public List<string> Services { get; set; }

        //public DateTime? CreatedAt { get; set; }

        //public virtual Announcement Announcement { get; set; }

        //public virtual Pet Pet { get; set; }

        //public virtual PetOwner PetOwner { get; set; }

        //public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
