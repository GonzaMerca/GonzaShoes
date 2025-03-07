namespace GonzaShoes.Model.DTOs.Product
{
    public class ProductGrouping
    {
        public int Id { get; set; }
        //TODO: Ver de utilizar marca + model + color + talle
        public string Name => $"{BrandName} - {ModelProductName}".Trim();
        public int ModelProductId { get; set; }
        public string ModelProductName { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public List<ColorGrouping> Colors { get; set; } = new List<ColorGrouping>();
    }

    public class ColorGrouping
    {
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public string ColorHexCode { get; set; }
        public List<SizeGrouping> Sizes { get; set; } = new List<SizeGrouping>();
    }

    public class SizeGrouping
    {
        public int SizeId { get; set; }
        public decimal SizeNumber { get; set; }
    }

}
