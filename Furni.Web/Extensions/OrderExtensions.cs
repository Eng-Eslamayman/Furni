using Furni.Utility.Models;

namespace Furni.Web.Extensions
{
	public static class CustomerListingViewModelExtensions
	{
		public static IQueryable GetDataPage(this IQueryable<CustomerListingViewModel> products, GetFilteredDto dto)
														 => products.Skip(dto.Skip).Take(dto.PageSize);
	}
}
