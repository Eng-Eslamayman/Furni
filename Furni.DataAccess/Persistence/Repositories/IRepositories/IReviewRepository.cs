using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni.DataAccess.Persistence.Repositories.IRepositories
{
	public interface IReviewRepository : IBaseRepository<Review>
	{
		Task<ReviewDataViewModel>? GetReviewData(int id);
		Task<ReviewViewModel?> AddReviewAsync(ReviewFormViewModel model, string userId);
	}
}
