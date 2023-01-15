using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Projekt.Models;

public partial class MusicDatabaseContext : DbContext
{

    public MusicDatabaseContext(DbContextOptions<MusicDatabaseContext> options): base(options){}

    public DbSet<Projekt.Models.Album> Albums { get; set; }
    public DbSet<Projekt.Models.Artist> Artists { get; set; }
    public DbSet<Projekt.Models.Genre> Genres { get; set; }
    public DbSet<Projekt.Models.Song> Songs { get; set; }

}
