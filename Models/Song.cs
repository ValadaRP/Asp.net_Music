using System;
using System.Collections.Generic;

namespace Projekt.Models;

public partial class Song
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Duration { get; set; } = null!;

    public int? AlbumId { get; set; }

    public virtual Album? Album { get; set; }
}
