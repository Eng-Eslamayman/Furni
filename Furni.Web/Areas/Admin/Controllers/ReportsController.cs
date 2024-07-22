using AutoMapper;
using ClosedXML.Excel;
using Furni.Models.Enums;
using Furni.Utility.Reports;
using Furni.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenHtmlToPdf;
using System.Drawing.Printing;
using System.Net.Mime;
using ViewToHTML.Services;
using PaperSize = OpenHtmlToPdf.PaperSize;

namespace Furni.Web.Areas.Admin.Controllers
{

	[Area(AppRoles.Admin)]
    [Authorize(Policy = "ExtendedAccessPolicy")]
    public class ReportsController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IMapper _mapper;
        private readonly IViewRendererService _viewRendererService;

        private readonly string _logoPath;
        private readonly int _sheetStartRow = 7;

        public ReportsController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IMapper mapper, IViewRendererService viewRendererService)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _viewRendererService = viewRendererService;
            _logoPath = $"{_webHostEnvironment.WebRootPath}/assets/images/Logo.png";
        }

        public IActionResult Index()
		{
			return View();
		}

		#region Financial
		[HttpGet]
		public IActionResult Financials(string duration, int? pageNumber)
		{
			var viewModel = new FinancialsReportViewModel { Duration = duration };
			if (!string.IsNullOrEmpty(duration))
			{
				if (!DateTime.TryParse(duration.Split(" - ")[0], out DateTime from))
				{
					ModelState.AddModelError("Duration", Errors.InvalidStartDate);
					return View(viewModel);
				}

				if (!DateTime.TryParse(duration.Split(" - ")[1], out DateTime to))
				{
					ModelState.AddModelError("Duration", Errors.InvalidEndDate);
					return View(viewModel);
				}

				if (pageNumber is not null)
				{
					var model = _unitOfWork.OrderDetails.GetFinancialReports(from, to, (int)ReportsConfigurations.PageSize, pageNumber);
					viewModel = model;
				}
			}

			ModelState.Clear();

			return View(viewModel);
		}


		public async Task<IActionResult> ExportFinancialsToExcel(string duration)
		{
			if (string.IsNullOrEmpty(duration))
				return BadRequest("Duration parameter is required.");

			var from = DateTime.Parse(duration.Split(" - ")[0]);
			var to = DateTime.Parse(duration.Split(" - ")[1]);

			// Fetch financial reports
			var financialReports = _unitOfWork.OrderDetails.GetFinancialReports(from, to, int.MaxValue, 1); // Get all financial reports

			// Excel File
			using var workbook = new XLWorkbook();
			var sheet = workbook.AddWorksheet("Financials");

			sheet.AddLocalImage(_logoPath);

			// Define header
			var headerCells = new string[]
			{
		"Day", "Month", "Year", "Total Cost", "Total Revenue", "Total Profit"
			};

			sheet.AddHeader(headerCells);

			// Add financial reports data using for loop
			for (int i = 0; i < financialReports.Financials.Count; i++)
			{
				var report = financialReports.Financials[i];
				sheet.Cell(i + _sheetStartRow, 1).SetValue(report.Day);
				sheet.Cell(i + _sheetStartRow, 2).SetValue(report.Month);
				sheet.Cell(i + _sheetStartRow, 3).SetValue(report.Year);
				sheet.Cell(i + _sheetStartRow, 4).SetValue(report.TotalCost).Style.NumberFormat.Format = "#,##0.00"; // Format as currency
				sheet.Cell(i + _sheetStartRow, 5).SetValue(report.TotalRevenue).Style.NumberFormat.Format = "#,##0.00"; // Format as currency
				sheet.Cell(i + _sheetStartRow, 6).SetValue(report.TotalProfit).Style.NumberFormat.Format = "#,##0.00"; // Format as currency
			}

			// Add summary row with style
			sheet.Cell(_sheetStartRow + financialReports.Financials.Count + 1, 1).SetValue("Summary:");
			sheet.Cell(_sheetStartRow + financialReports.Financials.Count + 1, 4).SetValue("Total Cost:");
			sheet.Cell(_sheetStartRow + financialReports.Financials.Count + 1, 5).SetValue("Total Revenue:");
			sheet.Cell(_sheetStartRow + financialReports.Financials.Count + 1, 6).SetValue("Total Profit:");
			sheet.Cell(_sheetStartRow + financialReports.Financials.Count + 1, 4).Style.Font.Bold = true;
			sheet.Cell(_sheetStartRow + financialReports.Financials.Count + 1, 5).Style.Font.Bold = true;
			sheet.Cell(_sheetStartRow + financialReports.Financials.Count + 1, 6).Style.Font.Bold = true;
			sheet.Cell(_sheetStartRow + financialReports.Financials.Count + 1 + 1, 4).SetValue(financialReports.TotalCost).Style.NumberFormat.Format = "#,##0.00"; // Format as currency
			sheet.Cell(_sheetStartRow + financialReports.Financials.Count + 1 + 1, 5).SetValue(financialReports.TotalRevenue).Style.NumberFormat.Format = "#,##0.00"; // Format as currency
			sheet.Cell(_sheetStartRow + financialReports.Financials.Count + 1 + 1, 6).SetValue(financialReports.TotalProfit).Style.NumberFormat.Format = "#,##0.00"; // Format as currency

			// Apply formatting and table
			sheet.Format();
			sheet.AddTable(financialReports.Financials.Count() + 1, headerCells.Length); // Adjust the table creation

			// Disable gridlines
			sheet.ShowGridLines = false;

			await using var stream = new MemoryStream();
			workbook.SaveAs(stream);

			return File(stream.ToArray(), MediaTypeNames.Application.Octet, $"Financials_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
		}





		public async Task<IActionResult> ExportFinancialsToPDF(string duration)
		{
			if (string.IsNullOrEmpty(duration))
				return BadRequest("Duration parameter is required.");

			var from = DateTime.Parse(duration.Split(" - ")[0]);
			var to = DateTime.Parse(duration.Split(" - ")[1]);

			// Fetch financial reports
			var financialReports = _unitOfWork.OrderDetails.GetFinancialReports(from, to, int.MaxValue, 1); // Get all financial reports

			var templatePath = "~/Views/Shared/FinancialsTemplate.cshtml";
			var html = await _viewRendererService.RenderViewToStringAsync(ControllerContext, templatePath, financialReports);

			var pdf = Pdf
				.From(html)
				.EncodedWith("Utf-8")
				.OfSize(PaperSize.A4)
				.WithMargins(1.Centimeters())
				.Landscape()
				.Content();

			return File(pdf.ToArray(), MediaTypeNames.Application.Octet, $"Financials_{DateTime.Now:yyyyMMddHHmmss}.pdf");
		}

		#endregion


		#region Products
		[HttpGet]
		public async Task<IActionResult> Products(IList<int> selectedCategories, string? stock, int? pageNumber)
		{
			var page = pageNumber ?? 1;
			var productsReport = await _unitOfWork.Products.GetProductsReportAsync(selectedCategories, (int)ReportsConfigurations.PageSize, stock, page);
			var categories = _unitOfWork.Categories.GetActiveCategories();

			var viewModel = new ProductsReportsWithCategoriesViewModel
			{
				Categories = _mapper.Map<IEnumerable<SelectListItem>>(categories),
			};

			if (pageNumber is not null)
			{
				viewModel.Products = productsReport;
			}

			return View(viewModel);
		}

		public async Task<IActionResult> ExportProductsToExcel(string? categories, string? stock)
		{
			// Parse the selected categories and stock status from the query parameters
			var selectedCategories = categories?.Split(',').Select(int.Parse).ToList();
			var stockStatus = Enum.TryParse<StockStatus>(stock, out var parsedStockStatus) ? parsedStockStatus : (StockStatus?)null;

			// Fetch products using the repository pattern
			var data = await _unitOfWork.Products.GetProductsReportAsync(
				selectedCategories: selectedCategories,
				pageSize: int.MaxValue, // Fetch all products
				stock: stockStatus?.ToString()
			);

			// Excel File
			using var workbook = new XLWorkbook();

			// Add sheet to the workbook
			var sheet = workbook.AddWorksheet("Products");
            sheet.AddLocalImage(_logoPath);

            // Header of the sheet
            var headerCells = new string[]
			{
		"Title", "Category Name", "Price", "Cost Price", "Stock Status", "Discount Value"
			};

			sheet.AddHeader(headerCells);

			for (int i = 0; i < data.Count; i++)
			{
				sheet.Cell(i + _sheetStartRow, 1).SetValue(data[i].Title);
				sheet.Cell(i + _sheetStartRow, 2).SetValue(data[i].CategoryName);
				sheet.Cell(i + _sheetStartRow, 3).SetValue(data[i].Price);
				sheet.Cell(i + _sheetStartRow, 4).SetValue(data[i].CostPrice);
				sheet.Cell(i + _sheetStartRow, 5).SetValue(data[i].Quantity > 3 ? "InStock" : (data[i].Quantity > 0 && data[i].Quantity <= 3 ? "LowStock" : "OutOfStock"));
				sheet.Cell(i + _sheetStartRow, 6).SetValue(data[i].DiscountValue);
			}

			sheet.Format();
			sheet.AddTable(data.Count, headerCells.Length);
			sheet.ShowGridLines = false;

			await using var stream = new MemoryStream();
			workbook.SaveAs(stream);

			return File(stream.ToArray(), MediaTypeNames.Application.Octet, $"Products_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
		}

		public async Task<IActionResult> ExportProductsToPDF(string? categories, string? stock)
		{
			// Parse the selected categories and stock status from the query parameters
			var selectedCategories = categories?.Split(',').Select(int.Parse).ToList();
			var stockStatus = Enum.TryParse<StockStatus>(stock, out var parsedStockStatus) ? parsedStockStatus : (StockStatus?)null;

			// Fetch products using the repository pattern
			var products = await _unitOfWork.Products.GetProductsReportAsync(
				selectedCategories: selectedCategories,
				pageSize: int.MaxValue, // Fetch all products
				stock: stockStatus?.ToString()
			);

			var templatePath = "~/Views/Shared/ProductsTemplate.cshtml";

			// Render the HTML content using the view template
			string html;
			try
			{
				html = await _viewRendererService.RenderViewToStringAsync(ControllerContext, templatePath, products);
			}
			catch (Exception ex)
			{
				// Handle the exception if view rendering fails
				return BadRequest($"Error rendering view: {ex.Message}");
			}

			// Generate PDF from HTML content
			var pdf = Pdf.From(html).EncodedWith("Utf-8").OfSize(PaperSize.A4).WithMargins(0.Centimeters()).Landscape().Content();

			return File(pdf.ToArray(), MediaTypeNames.Application.Octet, $"Products_{DateTime.Now:yyyyMMddHHmmss}.pdf");
		}
		#endregion


		#region Orders
		[HttpGet]
		public IActionResult Orders(string duration, string? status, int? pageNumber)
		{
			var viewModel = new OrdersReportViewModel { Duration = duration };
			if (!string.IsNullOrEmpty(duration))
			{
				if (!DateTime.TryParse(duration.Split(" - ")[0], out DateTime from))
				{
					ModelState.AddModelError("Duration", Errors.InvalidStartDate);
					return View(viewModel);
				}

				if (!DateTime.TryParse(duration.Split(" - ")[1], out DateTime to))
				{
					ModelState.AddModelError("Duration", Errors.InvalidEndDate);
					return View(viewModel);
				}



				if (pageNumber is not null)
				{
					viewModel.Orders = _unitOfWork.Orders.GetOrdersReport(from, to, (int)ReportsConfigurations.PageSize, status, pageNumber);
					viewModel.Status = status;
				}
			}

			ModelState.Clear();

			return View(viewModel);
		}


		public async Task<IActionResult> ExportOrdersToExcel(string duration, string? status)
		{
			if (string.IsNullOrEmpty(duration))
				return BadRequest("Duration parameter is required.");
			var orderStatus = Enum.TryParse<OrderStatus>(status, out var parsedOrderStatus) ? parsedOrderStatus : (OrderStatus?)null;

			var from = DateTime.Parse(duration.Split(" - ")[0]);
			var to = DateTime.Parse(duration.Split(" - ")[1]);

			// Fetch order reports
			var orderReports = _unitOfWork.Orders.GetOrdersReport(from, to, int.MaxValue, orderStatus?.ToString()); // Get all financial reports


			// Excel File
			using var workbook = new XLWorkbook();
			var sheet = workbook.AddWorksheet("Orders");

			sheet.AddLocalImage(_logoPath);

			// Define header
			var headerCells = new string[]
			{
			  "Order ID", "Created On", "Customer Name", "Total Price", "Total Profit", "Status"
			};

			sheet.AddHeader(headerCells);

			// Add financial reports data using for loop
			for (int i = 0; i < orderReports.Count; i++)
			{
				var report = orderReports[i];
				sheet.Cell(i + _sheetStartRow, 1).SetValue(report.OrderId);
				sheet.Cell(i + _sheetStartRow, 2).SetValue(report.CreatedOn.ToString("d MMM yyyy"));
				sheet.Cell(i + _sheetStartRow, 3).SetValue(report.CustomerName);
				sheet.Cell(i + _sheetStartRow, 4).SetValue(report.TotalPrice).Style.NumberFormat.Format = "#,##0.00"; // Format as currency
				sheet.Cell(i + _sheetStartRow, 5).SetValue(report.TotalProfit).Style.NumberFormat.Format = "#,##0.00"; // Format as currency
				sheet.Cell(i + _sheetStartRow, 6).SetValue(report.Status);
			}

			// Apply formatting and table
			sheet.Format();
			sheet.AddTable(orderReports.Count() + 1, headerCells.Length); // Adjust the table creation

			// Disable gridlines
			sheet.ShowGridLines = false;

			await using var stream = new MemoryStream();
			workbook.SaveAs(stream);

			return File(stream.ToArray(), MediaTypeNames.Application.Octet, $"Orders_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
		}


		public async Task<IActionResult> ExportOrdersToPDF(string duration, string? status)
		{
			if (string.IsNullOrEmpty(duration))
				return BadRequest("Duration parameter is required.");
			var orderStatus = Enum.TryParse<OrderStatus>(status, out var parsedOrderStatus) ? parsedOrderStatus : (OrderStatus?)null;

			var from = DateTime.Parse(duration.Split(" - ")[0]);
			var to = DateTime.Parse(duration.Split(" - ")[1]);

			// Fetch order reports
			var orderReports = _unitOfWork.Orders.GetOrdersReport(from, to, int.MaxValue, orderStatus?.ToString()); // Get all financial reports


			// Excel File
			using var workbook = new XLWorkbook();
			var sheet = workbook.AddWorksheet("Orders");

			sheet.AddLocalImage(_logoPath);


			var templatePath = "~/Views/Shared/OrdersTemplate.cshtml";
			var html = await _viewRendererService.RenderViewToStringAsync(ControllerContext, templatePath, orderReports);

			var pdf = Pdf
				.From(html)
				.EncodedWith("Utf-8")
				.OfSize(PaperSize.A4)
				.WithMargins(1.Centimeters())
				.Landscape()
				.Content();

			return File(pdf.ToArray(), MediaTypeNames.Application.Octet, $"Orders_{DateTime.Now:yyyyMMddHHmmss}.pdf");
		}
		#endregion


		[HttpGet]
		public IActionResult Customers(int? pageNumber)
		{
			var page = pageNumber ?? 1;
			var customersReport = _unitOfWork.ApplicationUsers.GetCustomersReport((int)ReportsConfigurations.PageSize, page);
			var viewModel = new CustomersReportViewModel();

			if (pageNumber is not null)
				viewModel.Customers = customersReport;

			return View(viewModel);
		}

        public async Task<IActionResult> ExportCustomersToExcel()
        {
            var customersReport = _unitOfWork.ApplicationUsers.GetCustomersReport(pageSize: int.MaxValue);

            // Excel File
            using var workbook = new XLWorkbook();
            var sheet = workbook.AddWorksheet("Customers");

            sheet.AddLocalImage(_logoPath);

            // Define header
            var headerCells = new string[]
            {
        "Full Name", "Total Buying Price", "Number of Orders"
            };

            sheet.AddHeader(headerCells);

            // Add customer data using for loop
            for (int i = 0; i < customersReport.Count; i++)
            {
                var report = customersReport[i];
                sheet.Cell(i + _sheetStartRow, 1).SetValue(report.FullName);
                sheet.Cell(i + _sheetStartRow, 2).SetValue(report.TotalBuyingPrice).Style.NumberFormat.Format = "#,##0.00"; // Format as currency
                sheet.Cell(i + _sheetStartRow, 3).SetValue(report.NumberOfOrders);
            }

            // Apply formatting and table
            sheet.Format();
            sheet.AddTable(customersReport.Count + 1, headerCells.Length); // Adjust the table creation

            // Disable gridlines
            sheet.ShowGridLines = false;

            await using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return File(stream.ToArray(), MediaTypeNames.Application.Octet, $"Customers_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
        }

        public async Task<IActionResult> ExportCustomersToPDF()
        {
            var customersReport = _unitOfWork.ApplicationUsers.GetCustomersReport(pageSize: int.MaxValue);

            var templatePath = "~/Views/Shared/CustomersTemplate.cshtml";
            var html = await _viewRendererService.RenderViewToStringAsync(ControllerContext, templatePath, customersReport);

            var pdf = Pdf
                .From(html)
                .EncodedWith("Utf-8")
                .OfSize(PaperSize.A4)
                .WithMargins(1.Centimeters())
                .Landscape()
                .Content();

            return File(pdf.ToArray(), MediaTypeNames.Application.Octet, $"Customers_{DateTime.Now:yyyyMMddHHmmss}.pdf");
        }

    }

}
