using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projekt.Controllers;
using Projekt.Models;

namespace Projekt.ViewComponents
{
    public class SongViewComponent : ViewComponent
    {
        private readonly MusicDatabaseContext _context;
        public SongViewComponent(MusicDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(){
            var longestSong = _context.Songs.OrderByDescending(song => song.Duration).Include(album => album.Album).FirstOrDefault();
            ViewData["longestSong"] = longestSong;
            return View("Default");
        }
    }
}
