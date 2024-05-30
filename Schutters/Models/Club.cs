using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Schutters.Models;

public class Club
{
    [Required]
    public int Stamnummer { get; set; }
    [Required]
    [StringLength(55)]
    public string Naam { get; set; } = null!;
    [Required]
    public string Postcode { get; set; } = null!;
    [Required]
    [StringLength(55)]
    public string Gemeente { get; set; } = null!;
    [StringLength(70)]
    public string? Adres { get; set; }

    public virtual ICollection<Lid> Leden { get; set; } = new List<Lid>();
}
