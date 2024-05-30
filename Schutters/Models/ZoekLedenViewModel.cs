using System.ComponentModel.DataAnnotations;

namespace Schutters.Models
{
    public class ZoekLedenViewModel
    {
        [Display(Name = "Begin van de voornaam")]
        public string? DeelVoornaam { get; set; }

        [Display(Name = "Begin van de naam:")]
        public string? DeelNaam { get; set; }

        [Display(Name = "Geslacht:")]
        public string? Geslacht {  get; set; }

        [Display(Name = "Niveau:")]
        public string? Niveau { get; set; }

        [Display(Name = "Deel van de clubnaam:")]
        public string? DeelClubnaam { get; set; }

        public List<Lid> Leden { get; set; } = new List<Lid>();
    }
}
