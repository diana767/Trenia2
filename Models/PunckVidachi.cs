using System;
using System.Collections.Generic;

namespace Trenia2.Models;

public partial class PunckVidachi
{
    public int IdPuncta { get; set; }

    public int Number { get; set; }

    public string Adres { get; set; } = null!;

    public virtual ICollection<Zakaz> Zakazs { get; set; } = new List<Zakaz>();
}
