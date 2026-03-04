namespace MVC_Buddies.Dtos
{
    public class LocationDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string District { get; set; }

        public string Country { get; set; }

        //public virtual ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();

        //public virtual ICollection<PetOwner> PetOwners { get; set; } = new List<PetOwner>();

        //public virtual ICollection<PetSitter> PetSitters { get; set; } = new List<PetSitter>();
    }
}
