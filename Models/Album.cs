using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;

namespace Projekt.Models;

public partial class Album
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public DateTime? ReleaseYear { get; set; }

    public string RecordLabel { get; set; } = null!;

    public virtual ICollection<Artist>? Artists { get; set; } = new List<Artist>();

    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();

}
