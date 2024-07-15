using Furni.DataAccess.Persistence.Repositories.IRepositories;
using System.Security.Claims;

namespace Furni.DataAccess.Persistence.Repositories
{
	public class ReviewRepository : BaseRepository<Review>, IReviewRepository
	{
		public ReviewRepository(ApplicationDbContext context) : base(context)
		{
		}

		public async Task<ReviewDataViewModel> GetReviewData(int id)
		{
			try
			{
				var product = await _context.Products
					.Where(p => p.Id == id)
					.Include(p => p.Reviews)
					.FirstOrDefaultAsync();

				if (product == null)
				{
					// Handle case where product with specified id is not found
					return null;
				}

				// Calculate review counts and average rating in memory
				var reviewCounts = product.Reviews
					.GroupBy(r => r.Rating)
					.ToDictionary(g => g.Key, g => g.Count());

				var totalReview = product.Reviews.Count();
				var ratingAverage = product.Reviews.Any() ? (float)product.Reviews.Average(r => r.Rating) : 0.0f;

				return new ReviewDataViewModel
				{
					ReviewCounts = reviewCounts,
					TotalReview = totalReview,
					RatingAverage = ratingAverage
				};
			}
			catch (Exception ex)
			{
				// Log the exception for debugging purposes
				Console.WriteLine($"Exception in GetReviewData for product Id {id}: {ex.Message}");

				// Handle or rethrow the exception based on your application's error handling strategy
				throw; // Rethrow the exception to propagate it up the call stack or handle it as appropriate
			}
		}



		public async Task<ReviewViewModel?> AddReviewAsync(ReviewFormViewModel model, string userId)
		{
			var review = new Review
			{
				ProductId = model.ProductId,
				ApplicationUserId = userId,
				Rating = model.Rating,
				Comment = model.Comment,
				ReviewDate = DateTime.Now,
				CreatedOn = DateTime.Now,
				CreatedById = userId,
			};

			await _context.Reviews.AddAsync(review);
			await _context.SaveChangesAsync();

			// Fetch the latest review after it's saved
			review = await _context.Reviews
				.Include(r => r.ApplicationUser) // Include ApplicationUser if needed for user details
				.FirstOrDefaultAsync(r => r.ReviewId == review.ReviewId); // Assuming Review has an Id property

			if (review == null)
				return null; // Return null if review is not found

			// Create ReviewViewModel
			var reviewViewModel = new ReviewViewModel
			{
				Rating = review.Rating,
				Comment = review.Comment,
				UserName = review.ApplicationUser?.FullName, // Example assuming ApplicationUser has a FullName property
				ReviewDate = review.CreatedOn
			};

			return reviewViewModel;
		}



	}
}
