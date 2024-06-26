using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.DataAccess.Persistence.Repositories.IRepositories
{
	public interface IProductImageRepository: IBaseRepository<ProductImage>
	{
		List<string> GetImagesUrl(int id);
	}
}
