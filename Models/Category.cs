using System;
using System.Collections.Generic;

namespace Trenia2.Models;

public partial class Category
{
    public int IdCategory { get; set; }

    public string Category1 { get; set; } = null!;

    public virtual ICollection<Tovar> Tovars { get; set; } = new List<Tovar>();
}
