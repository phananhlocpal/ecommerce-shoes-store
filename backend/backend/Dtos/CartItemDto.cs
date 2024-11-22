namespace backend.Dtos
{
    public class CartItemReadDto
    {
        public int CartItemID { get; set; }
        public int CustomerID { get; set; }
        public ProductReadDto Product { get; set; }
        public decimal ProductPrice { get; set; } 
        public int? ProductSizeID { get; set; }
        public string ProductSizeName { get; set; }
        public int Quantity { get; set; }
    }

    public class CartItemCreateDto
    {
        public int CustomerID { get; set; }
        public int ProductSizeID { get; set; }
        public int Quantity { get; set; }
    }

    public class CartItemUpdateDto
    {
        public int CartItemID { get; set; }
        public int CustomerID { get; set; }
        public int? ProductSizeID { get; set; }
        public int Quantity { get; set; }
    }
}
