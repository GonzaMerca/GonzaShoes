﻿using System.ComponentModel.DataAnnotations.Schema;

namespace GonzaShoes.Model.Entities.Product
{
    [Table("Color")]
    public class Color : ContextualProps
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string HexCode { get; set; } = string.Empty;

    }
}
