namespace MVC_Buddies.Dtos
{
    public partial class AnnouncementDto
    {
        public int Id { get; set; }

        public int? PetSitterId { get; set; }

        public int? IdLocation { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string Title { get; set; }



        //public virtual ICollection<Availability> Availabilities { get; set; } = new List<Availability>();

        //public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        //public virtual Location IdLocationNavigation { get; set; }

        //public virtual PetSitter IdPetsitterNavigation { get; set; }
    }
}
