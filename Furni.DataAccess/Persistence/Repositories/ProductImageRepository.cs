using Furni.DataAccess.Persistence.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.DataAccess.Persistence.Repositories
{
	public class ProductImageRepository : BaseRepository<ProductImage>, IProductImageRepository
	{
		public ProductImageRepository(ApplicationDbContext context) : base(context)
		{
		}

        public List<string> GetImagesUrl(int id) => _context.ProductImages
			.Where(pi => pi.ProductId == id)
            .Select(pi => pi.ImagePath)
            .ToList();

    }
}
