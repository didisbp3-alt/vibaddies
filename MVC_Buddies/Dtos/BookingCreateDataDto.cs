namespace MVC_Buddies.Dtos
{
    public class BookingCreateDataDto
    {
        public List<PetOptionDto> Pets { get; set; } = new();
        public List<SkillOptionDto> Skills { get; set; } = new();
    }

    public class PetOptionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class SkillOptionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ServiceOptionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        //public bool Selected { get; set; } // opcional, se quiser marcar checkboxes
    }


}
