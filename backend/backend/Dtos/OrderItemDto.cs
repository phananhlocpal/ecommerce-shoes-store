namespace backend.Dtos
{
    public class OrderItemReadDto
    {
        public int OrderItemID { get; set; }
        public ProductReadDto Product { get; set; }
        public int Quantity { get; set; }
        public ProductSizeReadDto ProductSize { get; set; }
    }

    public class OrderItemCreateDto
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public int ProductSizeID { get; set; }
    }

    public class OrderItemUpdateDto
    {
        public int Quantity { get; set; }
        public int ProductSizeID { get; set; }
    }
}
