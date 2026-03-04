namespace MVC_Buddies.Dtos
{
    public class PendingPetSitterViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhotoUrl { get; set; }
        public int? YearsExperience { get; set; }
        public string SubscriptionType { get; set; }
    }
}
