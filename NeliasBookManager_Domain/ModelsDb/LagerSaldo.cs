using System;
using System.Collections.Generic;

namespace NeliasBookManager.Domain.ModelsDb;

public partial class LagerSaldo
{
    public int ButikId { get; set; }

    public string Isbn { get; set; } = null!;

    public int? AntalBöcker { get; set; }

    public virtual Butiker Butik { get; set; } = null!;

    public virtual Böcker IsbnNavigation { get; set; } = null!;
}
