using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MVC_Buddies.Dtos
{
    public class BreedCreateViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int SpeciesId { get; set; }

        public List<SelectListItem>? SpeciesList { get; set; }
    }
}
