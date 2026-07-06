namespace Company524.Application.Models.Cloudinary
{
    public record ImageUploadResultDto
    {
        public string PublicId { get; set; }
        public string Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public long Bytes { get; set; }
    }
}
