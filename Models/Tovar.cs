using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Trenia2.Models;

public partial class Tovar
{
    public int IdTovar { get; set; }

    public string Articul { get; set; } = null!;

    public string NameTovar { get; set; } = null!;

    public int IdEdIzm { get; set; }

    public decimal Price { get; set; }

    public int IdPost { get; set; }

    public int IdProiz { get; set; }

    public int IdCategory { get; set; }

    public int Sale { get; set; }

    public int KolSklad { get; set; }

    public string Opisanie { get; set; } = null!;

    public string? Photo { get; set; }

    public virtual Category IdCategoryNavigation { get; set; } = null!;

    public virtual EdIzm IdEdIzmNavigation { get; set; } = null!;

    public virtual Postavshik IdPostNavigation { get; set; } = null!;

    public virtual Proizvoditel IdProizNavigation { get; set; } = null!;

    public virtual ICollection<SostavZakaz> SostavZakazs { get; set; } = new List<SostavZakaz>();

    public bool HasDiscount => Sale > 0;

    public decimal MoneyDiscount => Price - Price * Sale / 100;

    public Brush RowColor => KolSklad == 0 ? Brushes.LightBlue : Sale > 15 ? new SolidColorBrush(Color.FromRgb(40, 140, 90)) : Brushes.White;
}
