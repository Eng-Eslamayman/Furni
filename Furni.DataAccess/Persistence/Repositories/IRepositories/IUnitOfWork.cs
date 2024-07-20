using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.DataAccess.Persistence.Repositories.IRepositories
{
    public interface IUnitOfWork
    {
        ICategoryRepository Categories { get; }
        IProductRepository Products { get; }
        IShoppingCartRepository ShoppingCarts {  get; }
		IOrderRepository Orders { get; }
		IOrderDetailsRepository OrderDetails { get; }
		IReviewRepository Reviews { get; }
		IProductImageRepository ProductImages {  get; }
        IApplicationUserRepository ApplicationUsers { get; }

		int Complete();
    }
}
