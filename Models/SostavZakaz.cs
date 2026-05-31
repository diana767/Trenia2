using System;
using System.Collections.Generic;

namespace Trenia2.Models;

public partial class SostavZakaz
{
    public int Id { get; set; }

    public int IdTovar { get; set; }

    public int IdZakaz { get; set; }

    public int KolZak { get; set; }

    public virtual Tovar IdTovarNavigation { get; set; } = null!;

    public virtual Zakaz IdZakazNavigation { get; set; } = null!;
}
