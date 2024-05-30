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

        public IActionResult Index()
        {
            return View(ledenService.GetLeden());
        }

        public IActionResult Add()
        {
            ViewData["Club"] = new SelectList(clubService.GetClubs(), "Stamnummer", "Naam");
            var lid = new Lid();
            return View(lid);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

        public bool ValideerLidnummer(long Lidnummer)
        {
            return !ledenService.Bestaat(Lidnummer);
        }

        public IActionResult Detail(long lidnummer)
        {
            var lid = ledenService.FindLid(lidnummer);

            if(lid == null)
            {
                return NotFound();
            }

            return View(lid);
        }

        public IActionResult Edit(long lidnummer)
        {
            var lid = ledenService.FindLid(lidnummer);

            if (lid == null)
            {
                return NotFound();
            }
            ViewData["Club"] = new SelectList(clubService.GetClubs(), "Stamnummer", "Naam");

            return View(lid);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        public IActionResult Delete(long lidnummer)
        {
            if(ledenService.FindLid(lidnummer) == null)
            {
                return NotFound();
            }

            ledenService.Remove(lidnummer);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Search()
        {
            ZoekLedenViewModel zoekLedenViewModel = new ZoekLedenViewModel();
            return View(zoekLedenViewModel);
        }

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
