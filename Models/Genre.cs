﻿using System;
using System.Collections.Generic;

namespace Projekt.Models;

public partial class Genre
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Artist>? Artists { get; set; } = new List<Artist>();
}
