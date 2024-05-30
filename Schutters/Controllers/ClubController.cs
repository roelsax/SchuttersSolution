using Microsoft.AspNetCore.Mvc;
using Schutters.Services;
using Schutters.Models;
using Microsoft.EntityFrameworkCore;

namespace Schutters.Controllers
{
    public class ClubController : Controller
    {
        private readonly ClubService clubService;
        public ClubController(ClubService clubService) 
        {
            this.clubService = clubService;
        }
        public IActionResult Index()
        {
            return View(clubService.GetClubs());
        }

        public IActionResult Add() 
        {
            var club = new Club();
            return View(club);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Club club)
        {
            if (ValideerStamnummer(club.Stamnummer) == false)
            {
                ModelState.AddModelError("Stamnummer", "Een club met dit stamnummer komt al voor in de database. Kies een ander nummer.");
            }
            
            if (ModelState.IsValid) 
            {
                clubService.Create(club);
                return RedirectToAction(nameof(Index));
            } else
            {
                return View(club);
            }
        }

        public bool ValideerStamnummer(int Stamnummer)
        {
            return !clubService.Bestaat(Stamnummer);
        }

        [HttpGet]
        public IActionResult Detail(int stamnummer)
        {
            
            var club = clubService.FindClub(stamnummer);

            if(club == null)
            {
                return NotFound();
            }

            return View(club);
        }


        public IActionResult Edit(int stamnummer)
        {
            var club = clubService.FindClub(stamnummer);

            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int stamnummer, Club club)
        {
            if(stamnummer != club.Stamnummer)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                clubService.Update(club);
                return RedirectToAction(nameof(Index));
            } else
            {
                return View(club);
            }
        }

        public IActionResult Delete(int stamnummer) 
        {
            if (clubService.FindClub(stamnummer) == null)
            {
                return NotFound();
            }

            clubService.Remove(stamnummer);
            return RedirectToAction(nameof(Index));
        }
    }
}
