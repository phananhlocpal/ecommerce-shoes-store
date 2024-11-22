namespace backend.Dtos
{
    public class OrderItemReadDto
    {
        public int OrderItemID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; } 
        public int Quantity { get; set; }
        public int ProductSizeID { get; set; }
        public string ProductSizeName { get; set; } 
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
