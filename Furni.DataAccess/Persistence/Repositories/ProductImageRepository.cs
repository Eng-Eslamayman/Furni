﻿using Furni.DataAccess.Persistence.Repositories.IRepositories;
using Furni.Models.Entities;
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

        public (List<string>, List<string>) GetImagesUrl(int id)
        {
			var imageUrls = _context.ProductImages
            .Where(pi => pi.ProductId == id)
            .Select(pi => new
            {
                pi.ImageUrl,
                pi.ImageThumbnailUrl
            })
            .ToList();

            List<string> ImageUrl = imageUrls.Select(pi => pi.ImageUrl).ToList();
            List<string> ImageThumbnailUrl = imageUrls.Select(pi => pi.ImageThumbnailUrl).ToList();

            return (ImageUrl, ImageThumbnailUrl);
        }

        public void RemoveByThumbnailUrl(string thumbnailUrl)
        {
            var image = _context.ProductImages.FirstOrDefault(pi => pi.ImageThumbnailUrl == thumbnailUrl);
            if (image != null)
            {
                _context.ProductImages.Remove(image);
                _context.SaveChanges(); // Save changes to database
            }
            // Optionally, delete the image file from storage here
        }

    }
}
