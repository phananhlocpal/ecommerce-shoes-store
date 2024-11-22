namespace backend.Dtos
{
    public class ProductReadDto
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } 
        public List<ProductSizeReadDto> ProductSizes { get; set; }
    }

    public class ProductCreateDto
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public int CategoryID { get; set; }
    }

    public class ProductUpdateDto
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public int CategoryID { get; set; }
    }
}
