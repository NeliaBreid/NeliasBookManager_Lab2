using System;
using System.Collections.Generic;

namespace NeliasBookManager.Domain.ModelsDb;

public partial class Författare
{
    public int FörfattarId { get; set; }

    public string Förnamn { get; set; } = null!;

    public string Efternamn { get; set; } = null!;

    public DateTime Födelsedatum { get; set; }

    public DateTime? Dödsdatum { get; set; }

    public virtual ICollection<Böcker> Isbns { get; set; } = new List<Böcker>();
}
