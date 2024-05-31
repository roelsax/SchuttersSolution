using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Schutters.Models;
using Schutters.Services;
using System.Diagnostics.Eventing.Reader;
using System.Numerics;

namespace Schutters.Controllers
{
    public class LedenController : Controller
    {
        private readonly LedenService ledenService;
        private readonly ClubService clubService;

        public LedenController(LedenService ledenService, ClubService clubService)
        {
            this.ledenService = ledenService;
            this.clubService = clubService;
        }
        [Authorize(Roles = "Leden,Admin")]
        public IActionResult Index()
        {
            return View(ledenService.GetLeden());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            ViewData["Club"] = new SelectList(clubService.GetClubs(), "Stamnummer", "Naam");
            List<SelectListItem> Geslachten = new List<SelectListItem>()
            {
                new SelectListItem() { Value = "M", Text = "M" },
                new SelectListItem() { Value = "V", Text = "V" },
                new SelectListItem() { Value = "X", Text = "X" }
            };

            ViewData["GeslachtOptions"] = new SelectList(Geslachten, "Value", "Text");

            List<SelectListItem> Niveaus = new List<SelectListItem>();
            Niveaus.Add(new SelectListItem { Value = "C", Text = "C" });
            Niveaus.Add(new SelectListItem { Value = "R", Text = "R" });
            Niveaus.Add(new SelectListItem { Value = "J", Text = "J" });
            ViewBag.NiveauOptions = new SelectList(Niveaus, "Value", "Text");
            var lid = new Lid();
            return View(lid);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Add(Lid lid)
        {
            if (!ValideerLidnummer(lid.Lidnummer)) {
                ModelState.AddModelError("Lidnummer", "Een club met dit stamnummer komt al voor in de database. Kies een ander nummer.");
            }

            lid.Club = clubService.FindClub(lid.ClubStamnummer);

            if (lid.Club == null)
            {
                ModelState.AddModelError("Club", "De club met dit stamnummer bestaat niet.");
            }

            if (ModelState.IsValid)
            {
                ledenService.Create(lid);
                return RedirectToAction(nameof(Index));
            } else
            {
                return View(lid);
            }
        }

        public bool ValideerLidnummer(long? Lidnummer)
        {
            return !ledenService.Bestaat(Lidnummer);
        }
        [Authorize(Roles = "Leden,Admin")]
        public IActionResult Detail(long lidnummer)
        {
            var lid = ledenService.FindLid(lidnummer);

            if(lid == null)
            {
                return NotFound();
            }

            return View(lid);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(long lidnummer)
        {
            var lid = ledenService.FindLid(lidnummer);

            if (lid == null)
            {
                return NotFound();
            }
            ViewData["Club"] = new SelectList(clubService.GetClubs(), "Stamnummer", "Naam");

            List<SelectListItem> Geslachten = new List<SelectListItem>()
            {
                new SelectListItem() { Value = "M", Text = "M" },
                new SelectListItem() { Value = "V", Text = "V" },
                new SelectListItem() { Value = "X", Text = "X" }
            };

            ViewData["GeslachtOptions"] = Geslachten;

            List <SelectListItem> Niveaus = new List<SelectListItem>();
            Niveaus.Add(new SelectListItem { Value = "C", Text = "C" });
            Niveaus.Add(new SelectListItem { Value = "R", Text = "R" });
            Niveaus.Add(new SelectListItem { Value = "J", Text = "J" });
            ViewBag.NiveauOptions = new SelectList(Niveaus, "Value", "Text");

            return View(lid);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(long lidnummer, Lid lid)
        {
            if (lidnummer != lid.Lidnummer)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                ledenService.Update(lid);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(lid);
            }
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(long lidnummer)
        {
            if(ledenService.FindLid(lidnummer) == null)
            {
                return NotFound();
            }

            ledenService.Remove(lidnummer);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Leden,Admin")]
        public IActionResult Search()
        {
            ZoekLedenViewModel zoekLedenViewModel = new ZoekLedenViewModel();
            return View(zoekLedenViewModel);
        }

        [Authorize(Roles = "Leden,Admin")]
        public IActionResult SearchLeden(ZoekLedenViewModel form)
        {
            if(ModelState.IsValid)
            {
                var leden = new List<Lid>();
                var AllLeden = ledenService.GetLeden();

                if(form.DeelNaam != null)
                {
                    AllLeden = AllLeden.Where(l => l.Naam.Contains(form.DeelNaam));
                }

                if(form.DeelVoornaam != null)
                {
                    AllLeden = AllLeden.Where(l => l.Voornaam.Contains(form.DeelVoornaam));
                }

                if(form.Geslacht != null)
                {
                    AllLeden = AllLeden.Where(l => l.Geslacht == form.Geslacht);
                }

                if (form.Niveau != null)
                {
                    AllLeden = AllLeden.Where(l => l.Niveau == form.Niveau);
                }

                if (form.DeelClubnaam != null)
                {
                    AllLeden = AllLeden.Where(l => l.Club.Naam.Contains(form.DeelClubnaam));
                }
                
                leden = AllLeden.ToList();
                form.Leden = leden;
                if (leden.Count == 0)
                    ViewBag.ErrorMessage =
                    $"Er zijn geen leden gevonden.";
                else
                    ViewBag.ErrorMessage = String.Empty;
            }
            return View("Search", form);
        }
    }
}
