namespace Company524.API.Models.Supplier
{
    public record SupplierForGettingDto
    {
        public Guid Id { get; set; }
        public string SupplierName { get; set; }
    }
}
