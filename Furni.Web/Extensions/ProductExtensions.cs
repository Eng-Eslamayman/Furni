namespace Furni.Web.Extensions
{
    public static class ProductExtensions
    {
        public static IQueryable GetDataPage(this IQueryable<Product> products, GetFilteredDto dto)
                                                         => products.Skip(dto.Skip).Take(dto.PageSize);
    }
}
