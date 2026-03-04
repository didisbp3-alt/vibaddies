using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC_Buddies.Dtos
{
    public class BreedEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? SpeciesId { get; set; }

        public List<SelectListItem>? SpeciesList { get; set; }
    }
}
