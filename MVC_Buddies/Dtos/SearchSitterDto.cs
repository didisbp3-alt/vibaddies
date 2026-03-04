using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_Buddies.Dtos
{
    public class SearchSitterDto
    {
        public int PetSitterId { get; set; }

        [Column("PetSitterFullName")]
        public string FullName { get; set; } = null!;

        public string City { get; set; } = null!;
        public List<int> AnnouncementId { get; set; } = new();
    }
}
