namespace MVC_Buddies.Dtos
{
    public class EditPetOwnerViewModel
    {
        public int Id { get; set; }

        // USER
        public string UserName { get; set; }
        public string Email { get; set; }

        // PET OWNER (se tiver campos futuros)
        public string FullName { get; set; }
    }
}
