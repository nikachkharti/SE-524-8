namespace Company524.Application.Models.Supplier
{
    public record SupplierForGettingDto
    {
        public Guid Id { get; set; }
        public string SupplierName { get; set; }
    }
}
