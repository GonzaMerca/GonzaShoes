﻿using System.ComponentModel.DataAnnotations.Schema;

namespace GonzaShoes.Model.Entities.Product
{
    [Table("Product")]
    public class Product : ContextualProps
    {
        public int Id { get; set; }
        //TODO: Ver de utilizar marca + model + color + talle
        //public string Name { get; set; } = string.Empty;

        // Relationship with Model
        public int ModelId { get; set; }
        public ModelProduct Model { get; set; } = null!;

        // Relationship with Color
        public int ColorId { get; set; }
        public Color Color { get; set; } = null!;

        // Relationship with Size
        public int SizeId { get; set; }
        public Size Size { get; set; } = null!;

        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
