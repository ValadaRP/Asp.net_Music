using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt.Models;

public partial class Artist
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime? DateOfBirth { get; set; }

    public string Nationality { get; set; } = null!;

    public int? Albums { get; set; }

    public virtual Album? AlbumsNavigation { get; set; } = null!;

    public virtual ICollection<Genre>? Genres { get; set; } = new List<Genre>();

    [NotMapped] 
    public List<int>? GenresIds { get; set; }

    [NotMapped]
    public string FullName {
        get { return FirstName + " " + LastName; }
    }
}
