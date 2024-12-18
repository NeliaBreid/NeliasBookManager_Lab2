using System;
using System.Collections.Generic;

namespace NeliasBookManager.Domain.ModelsDb;

public partial class BokFormat //TODO: Döp om mappen till entites
{
    public int FormatId { get; set; }

    public string? Format { get; set; }

    public virtual ICollection<Böcker> Böckers { get; set; } = new List<Böcker>();
}
