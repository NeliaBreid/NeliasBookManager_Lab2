using System;
using System.Collections.Generic;

namespace NeliasBookManager.Domain.ModelsDb;

public partial class Språk
{
    public int SpråkId { get; set; }

    public string Språk1 { get; set; } = null!;

    public virtual ICollection<Böcker> Böckers { get; set; } = new List<Böcker>();
}
