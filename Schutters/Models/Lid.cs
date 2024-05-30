using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Schutters.Models;

public class Lid
{
    [Required]
    public long Lidnummer { get; set; }
    [Required]
    [StringLength(55)]
    public string Naam { get; set; } = null!;
    [Required]
    [StringLength(55)]
    public string Voornaam { get; set; } = null!;

    public string? Geslacht { get; set; }

    public string? Niveau { get; set; }
    [Required]
    public int ClubStamnummer { get; set; }
    
    public virtual Club? Club { get; set; } = null!;
}
