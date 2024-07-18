using Furni.DataAccess.Persistence.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.DataAccess.Persistence.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
        public IEnumerable<Category> GetActiveCategories() =>
            _context.Categories.Where(a => !a.IsDeleted).OrderBy(a => a.Name).Select(c => new Category
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToList();

		public IEnumerable<ProductsAndCategoriesSearchViewModel> GetCategoryNames(string query, int count)
		{
			return _context.Categories
				.Where(c => c.Name.Contains(query))
                .Select(c => new ProductsAndCategoriesSearchViewModel { Name = c.Name, Type = "category" })
				.Take(count)
				.ToList();
		}


		public List<CustomCategoryViewModel> GetCustomCategories() => _context.Categories
                .Select(c => new CustomCategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    DisplayOrder = c.DisplayOrder,
                    ImageUrl = c.ImageUrl
                })
                .OrderBy(c => c.DisplayOrder)
                .Skip(0).Take(6).ToList();

    }
}
