using HashidsNet;
using Microsoft.AspNetCore.WebUtilities;

namespace Bookify.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<HomeController> _logger;
        private readonly IHashids _hahsids;

        public HomeController(ILogger<HomeController> logger, IApplicationDbContext context, IMapper mapper, IHashids hahsids)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _hahsids = hahsids;
        }
        public IActionResult Index()
        {
            if (User.Identity!.IsAuthenticated)
                return RedirectToAction(nameof(Index), controllerName: "Dashboard");

            var lastAddedBooks = _context.Books
                .Include(b => b.Author)
                .Where(b => !b.IsDeleted)
                .OrderByDescending(b => b.ID)
                .Take(10)
                .ToList();

            var viewModel = _mapper.Map<IEnumerable<BookViewModel>>(lastAddedBooks);

            foreach (var bookViewModel in viewModel)
                bookViewModel.Key = _hahsids.EncodeHex(bookViewModel.ID.ToString());

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode = 500)
        {
            return View(new ErrorViewModel { ErrorCode = statusCode, ErrorDescription = ReasonPhrases.GetReasonPhrase(statusCode) });
        }
    }
}