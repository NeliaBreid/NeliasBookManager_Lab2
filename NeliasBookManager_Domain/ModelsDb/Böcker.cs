using System;
using System.Collections.Generic;

namespace NeliasBookManager.Domain.ModelsDb;

public partial class Böcker
{
    public string Isbn { get; set; } = null!;

    public string Titel { get; set; } = null!;

    public int SpråkId { get; set; }

    public int GenreId { get; set; }

    public int Pris { get; set; }

    public int FörlagId { get; set; }

    public int Utgivningsår { get; set; }

    public int FormatId { get; set; }

    public virtual BokFormat Format { get; set; } = null!;

    public virtual BokFörlag Förlag { get; set; } = null!;

    public virtual Genre Genre { get; set; } = null!;

    public virtual ICollection<LagerSaldo> LagerSaldos { get; set; } = new List<LagerSaldo>();

    public virtual Språk Språk { get; set; } = null!;

    public virtual ICollection<Författare> Författars { get; set; } = new List<Författare>();
}
