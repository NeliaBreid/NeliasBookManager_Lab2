using System;
using System.Collections.Generic;

namespace NeliasBookManager.Domain.ModelsDb;

public partial class BokFörlag
{
    public int FörlagId { get; set; }

    public string BokFörlagNamn { get; set; } = null!;

    public string Adress { get; set; } = null!;

    public string Postkod { get; set; } = null!;

    public string Stad { get; set; } = null!;

    public virtual ICollection<Böcker> Böckers { get; set; } = new List<Böcker>();
}
