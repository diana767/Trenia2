using System;
using System.Collections.Generic;

namespace Trenia2.Models;

public partial class Zakaz
{
    public int IdZakaz { get; set; }

    public int IdAdres { get; set; }

    public int IdUser { get; set; }

    public int IdStatus { get; set; }

    public int Kod { get; set; }

    public DateOnly DataZak { get; set; }

    public DateOnly DataDost { get; set; }

    public virtual PunckVidachi IdAdresNavigation { get; set; } = null!;

    public virtual Status IdStatusNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<SostavZakaz> SostavZakazs { get; set; } = new List<SostavZakaz>();
}
