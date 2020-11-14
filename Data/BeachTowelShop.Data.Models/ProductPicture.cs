namespace BeachTowelShop.Data.Models
{
    public class ProductPicture
    {
        public string ProductId { get; set; }
        public Product Product { get; set; }
        public string PictureId { get; set; }
        public Picture Picture { get; set; }
    }
}