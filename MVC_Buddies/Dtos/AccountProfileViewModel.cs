using System.ComponentModel.DataAnnotations;

namespace MVC_Buddies.Dtos
{
    public class AccountProfileViewModel
    {
        //  User

        public string UserName { get; set; }
        public string Email { get; set; }


        public string ProfileType { get; set; }

        public int Id { get; set; }

        public string FullName { get; set; }
        public string PhotoUrl { get; set; }
        public string? LocationName { get; set; }
        public int? YearsExperience { get; set; }
        public string SubscriptionType { get; set; }

        public List<string> Skills { get; set; } = new();
        public List<string> Species { get; set; } = new();


    }

}
