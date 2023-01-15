using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projekt.Controllers;
using Projekt.Models;


namespace Projekt.ViewComponents
{
    public class ArtistViewComponent:ViewComponent
    {
        private readonly MusicDatabaseContext _context;
        public ArtistViewComponent(MusicDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync() { 
            var liczba = _context.Artists.Count();
            ViewData["liczbaArtystow"] = liczba;
            return View("Default");
        }

    }
}
