using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC_Buddies.Dtos
{
    public class BreedViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? SpeciesId { get; set; }

        public string SpeciesName { get; set; }

        public List<SelectListItem>? SpeciesList { get; set; }
    }
  
}
