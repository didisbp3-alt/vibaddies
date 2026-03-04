namespace MVC_Buddies.Dtos
{
    public class PetDto
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; } = "";
        public int Age { get; set; }
        public int BreedId { get; set; }
        public string? BreedName { get; set; }
        public string OwnerFullName { get; set; } = "";
        public string UserName { get; set; } = "";
        public bool HasSpecialNeeds { get; set; }
        public string? SpecialNeedsNotes { get; set; }
        public string? MedicalNotes { get; set; }
        public List<string>? PhotoUrls { get; set; }
    }

    public class BreedDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }

    public class CreatePetDto
    {
        public int PetOwnerId { get; set; }
        public string Name { get; set; } = "";
        public int Age { get; set; }
        public int BreedId { get; set; }
        public bool HasSpecialNeeds { get; set; }
        public string? SpecialNeedsNotes { get; set; }
        public string? MedicalNotes { get; set; }
        public List<string>? PhotoUrls { get; set; }
    }

    public class UpdatePetDto
    {
        public string Name { get; set; } = "";
        public int Age { get; set; }
        public int BreedId { get; set; }
        public bool HasSpecialNeeds { get; set; }
        public string? SpecialNeedsNotes { get; set; }
        public string? MedicalNotes { get; set; }
        public List<string>? PhotoUrls { get; set; }
    }
}
