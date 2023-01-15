using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projekt.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Proje.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly MusicDatabaseContext _context;

        public ArtistsController(MusicDatabaseContext context)
        {
            _context = context;
        }

        // GET: Artists
        public async Task<IActionResult> Index()
        {
            var albums = _context.Albums.ToList();

            var musicDatabaseContext = _context.Artists.Include(a => a.Genres).Include(albums => albums.AlbumsNavigation);

            ViewBag.Albums = albums;
            return View(await musicDatabaseContext.ToListAsync());
        }

        // GET: Artists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Artists == null)
            {
                return NotFound();
            }

            var artist = await _context.Artists
                .Include(a => a.AlbumsNavigation).Include(g => g.Genres)
                .FirstOrDefaultAsync(m => m.Id == id);

            /*var artistAlbums = _context.Artists.Find(id);*/
            var album = _context.Albums.FirstOrDefault(a => a.Id == artist.Albums);
            ViewBag.Album = album;

            if (artist == null)
            {
                return NotFound();
            }
            var selectedGenres = _context.Genres.Where(g => artist.Genres.Contains(g)).ToList(); // Gatunki należące do artysty
            ViewBag.selectedGenres = selectedGenres;

            return View(artist);
        }

        // GET: Artists/Create
        public IActionResult Create()
        {
            ViewData["Albums"] = new SelectList(_context.Albums, "Id", "Title");
            ViewBag.Genres = _context.Genres.ToList();
            return View();
        }

        // POST: Artists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,DateOfBirth,Nationality,Albums,GenresIds")] Artist artist)
        {
            if (ModelState.IsValid)
            {
                if (artist.GenresIds != null)
                {
                    var selectedGenre = _context.Genres.Where(a => artist.GenresIds.Contains(a.Id)).ToList();
                    artist.Genres = selectedGenre;
                }
                _context.Add(artist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            /*ViewData["Albums"] = new SelectList(_context.Albums, "Id", "Title", artist.Albums);*/
            return View(artist);
        }

        // GET: Artists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Artists == null)
            {
                return NotFound();
            }

            var artist = await _context.Artists.Include(g => g.Genres).SingleAsync(g => g.Id == id);
            if (artist == null)
            {
                return NotFound();
            }


            ViewData["Albums"] = new SelectList(_context.Albums, "Id", "Title", artist.Albums);
            var selectedGenres = _context.Genres.Where(g => artist.Genres.Contains(g)).ToList(); // Gatunki należące do artysty
            var allGenres = _context.Genres.ToList(); // Wszystkie gatunki

            ViewBag.Genres = selectedGenres;
            ViewBag.AllGenres = allGenres;

            return View(artist);
        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,DateOfBirth,Nationality,Albums")] Artist artist, int[] GenreIds)
        {
            if (id != artist.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var currentArtist = await _context.Artists
                        .Include(a => a.Genres)
                        .FirstOrDefaultAsync(a => a.Id == id);

                    currentArtist.FirstName = artist.FirstName;
                    currentArtist.LastName = artist.LastName;
                    currentArtist.DateOfBirth = artist.DateOfBirth;
                    currentArtist.Nationality = artist.Nationality;
                    currentArtist.Albums = artist.Albums;

                    if (GenreIds == null)
                    {
                        currentArtist.Genres.Clear();
                    }
                    else
                    {
                        // Pobieramy aktualnie przypisane gatunki do artysty
                        var currentGenres = currentArtist.Genres;

                        // Pobieramy wszystkie gatunki z bazy danych
                        var allGenres = await _context.Genres.ToListAsync();

                        // Iterujemy po wszystkich gatunkach
                        foreach (var genre in allGenres)
                        {
                            // Jeśli gatunek jest obecny w tablicy GenreIds (czyli jest zaznaczony), to oznacza, że należy go przypisać do artysty
                            // Jeśli gatunek nie jest obecny w tablicy GenreIds (czyli nie jest zaznaczony), to oznacza, że należy go usunąć z artysty
                            if (GenreIds.Contains(genre.Id))
                            {
                                // Jeśli gatunek nie jest jeszcze przypisany do artysty, to go dodajemy
                                if (!currentGenres.Contains(genre))
                                {
                                    currentArtist.Genres.Add(genre);
                                }
                            }
                            else
                            {
                                // Jeśli gatunek jest już przypis any do artysty, to go usuwamy
                                if (currentGenres.Contains(genre))
                                {
                                    currentArtist.Genres.Remove(genre);
                                }
                            }
                        }
                    }
                    _context.Update(currentArtist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistExists(artist.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(artist);
        }

        // GET: Artists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Artists == null)
            {
                return NotFound();
            }

            var artist = await _context.Artists
                .Include(a => a.AlbumsNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Artists == null)
            {
                return Problem("Entity set 'MusicDatabaseContext.Artists'  is null.");
            }
            var artist = await _context.Artists.Include(a => a.AlbumsNavigation).FirstOrDefaultAsync(m => m.Id == id);

            if (artist.Albums != null)
            {
                artist.Albums = null;
                _context.Artists.Remove(artist);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtistExists(int id)
        {
            return (_context.Artists?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
