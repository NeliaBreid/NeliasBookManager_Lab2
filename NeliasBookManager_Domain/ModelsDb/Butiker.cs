using System;
using System.Collections.Generic;

namespace NeliasBookManager.Domain.ModelsDb;

public partial class Butiker
{
    public int ButikId { get; set; }

    public string ButikNamn { get; set; } = null!;

    public string Adress { get; set; } = null!;

    public string Postkod { get; set; } = null!;

    public string Postort { get; set; } = null!;

    public virtual ICollection<LagerSaldo> LagerSaldos { get; set; } = new List<LagerSaldo>();
}
