using Furni.DataAccess.Persistence;

namespace Furni.Web.Background_Tasks
{
    public class HangfireTasks
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailBodyBuilder _emailBodyBuilder;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;

        public HangfireTasks(
            IWebHostEnvironment webHostEnvironment,
            IEmailBodyBuilder emailBodyBuilder,
            IEmailSender emailSender,
            IUnitOfWork unitOfWork)
        {
            _webHostEnvironment = webHostEnvironment;
            _emailBodyBuilder = emailBodyBuilder;
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
        }


        public async Task CleanupIncompleteOrders()
        {
            await _unitOfWork.Orders.CleanupIncompleteOrders();
        }

        public async Task ProcessCartAdjustmentsAsync()
        {
            var cartAdjustments = await _unitOfWork.ShoppingCarts.AdjustCartItemCountsAsync();

            foreach (var adjustment in cartAdjustments)
            {
                var user = await _unitOfWork.ApplicationUsers.GetByIdAsync(adjustment.UserId); 

                if (adjustment.Count == 0)
                {
                    var placeholders = new Dictionary<string, string>
                    {
                        { "imageUrl", "" },
                        { "header", $"Hello {user.FullName}," },
                        { "body", $"Product {adjustment.ProductName}, Removed from Your Shopping Cart, because it out of our stock!" } 
                    };

                    var body = _emailBodyBuilder.GetEmailBody("notification", placeholders);


                    
                    await _emailSender.SendEmailAsync(
                    user.Email!,
                    "Furnihuture Updating Shopping Cart", body);
                    

                }
                else
                {
                    var placeholders = new Dictionary<string, string>
                    {
                        { "imageUrl", "" },
                        { "header", $"Hello {user.FullName}," },
                        { "body", $"Product {adjustment.ProductName}, Quantity Updated in Your Shopping Cart and the new Quantity is: {adjustment.Count}." } 
                    };

                    var body = _emailBodyBuilder.GetEmailBody("notification", placeholders);

                    await _emailSender.SendEmailAsync(
                    user.Email!,
                    "Furnihuture Updating Shopping Cart", body);
                    
                }
            }
        }

    }
}
