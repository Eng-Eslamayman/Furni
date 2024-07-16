using Furni.Web.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using SixLabors.ImageSharp;

namespace Furni.Web.Areas.Customer.Controllers
{
    [Area(AppRoles.Customer)]
    [Authorize(Roles = AppRoles.Customer)]
    public class ManagerController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly ILogger<ManagerController> _logger;
		private readonly UrlEncoder _urlEncoder;
		private readonly IImageService _imageService;
        //private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        private readonly IEmailBodyBuilder _emailBodyBuilder;
        private readonly IEmailSender _emailSender;

        public ManagerController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IImageService imageService,
            ILogger<ManagerController> logger,
            UrlEncoder urlEncoder,
            IEmailBodyBuilder emailBodyBuilder,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _imageService = imageService;
            _logger = logger;
            _urlEncoder = urlEncoder;
            _emailBodyBuilder = emailBodyBuilder;
            _emailSender = emailSender;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Get user's phone number
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            // Initialize ManagerUserViewModel with existing user data
            var managerView = new ManagerUserViewModel
            {
                Id = user.Id,
                PhoneNumber = phoneNumber,
                FullName = user.FullName,
                Street = user.StreetNumber,
                City = user.City,
                State = user.State,
                PostalCode = user.PostalCode,
                About = user.About,
                Email = user.Email!,
                UserName = user.UserName!
            };

            return View(managerView);
        }


        [HttpPost]
        public async Task<IActionResult> Index(ManagerUserViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = $"Unable to load user with ID '{_userManager.GetUserId(User)}'." });
            }

            if (!ModelState.IsValid)
            {
                var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
                var managerView = new ManagerUserViewModel
                {
                    PhoneNumber = phoneNumber,
                    FullName = user.FullName
                };

                return Json(new { success = false, message = "Invalid model state." });
            }

            if (model.FullName != user.FullName)
            {
                user.FullName = model.FullName;
                var setFullName = await _userManager.UpdateAsync(user);
                if (!setFullName.Succeeded)
                {
                    return Json(new { success = false, message = "Unexpected error when trying to set full name." });
                }
            }

            if (model.Email != user.Email)
            {
                user.Email = model.Email;
                var setEmail = await _userManager.UpdateAsync(user);
                if (!setEmail.Succeeded)
                {
                    return Json(new { success = false, message = "Unexpected error when trying to set email." });
                }

                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, model.Email);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Home",
                    pageHandler: null,
                    values: new { area = "Customer", userId = userId, email = model.Email, code = code },
                    protocol: Request.Scheme);

                var placeHolders = new Dictionary<string, string>()
                {
                    { "imageUrl", "https://res.cloudinary.com/dzqc5nfai/image/upload/v1717798788/qkp9tphwwlqkrmkdukpx.svg" },
                    { "header", $"Hey {user.FullName}," },
                    { "body", "Your Profile Updated Successfully!" },
                    { "url", $"{HtmlEncoder.Default.Encode(callbackUrl!)}" },
                    { "linkTitle", "Login" }
                };

                var body = _emailBodyBuilder.GetEmailBody(EmailTemplates.Email, placeHolders);

                await _emailSender.SendEmailAsync(model.Email, "Profile Updated", body);
            }

            if (model.UserName != user.UserName)
            {
                user.UserName = model.UserName;
                var setUserName = await _userManager.UpdateAsync(user);
                if (!setUserName.Succeeded)
                {
                    return Json(new { success = false, message = "Unexpected error when trying to set username." });
                }
            }

            user.StreetNumber = model.Street;
            user.City = model.City;
            user.State = model.State;
            user.PostalCode = model.PostalCode;
            user.About = model.About;

            var updateUserResult = await _userManager.UpdateAsync(user);
            if (!updateUserResult.Succeeded)
            {
                return Json(new { success = false, message = "Unexpected error when trying to update profile." });
            }

            var phoneNumberCheck = await _userManager.GetPhoneNumberAsync(user);
            if (model.PhoneNumber != phoneNumberCheck)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    return Json(new { success = false, message = "Unexpected error when trying to set phone number." });
                }
            }

            if (model.Avatar is not null)
            {
                _imageService.Delete($"/images/users/{user.Id}.png");
                var (isUploaded, errorMessage) = await _imageService.UploadeAsynce(model.Avatar, $"{user.Id}.png", "/images/users", hasThumbnail: false);
                if (!isUploaded)
                {
                    ModelState.AddModelError("model.Avatar", errorMessage!);
                    return Json(new { success = false, message = "Error uploading avatar." });
                }
                user.ImageUrl = $"/images/users/{user.Id}.png";
            }
            else if (model.ImageRemoved)
            {
                _imageService.Delete($"/images/users/{user.Id}.png");
                user.ImageUrl = null;
            }

            var updatedUserResult = await _userManager.UpdateAsync(user);
            if (!updatedUserResult.Succeeded)
            {
                return Json(new { success = false, message = "Unexpected error when trying to update profile." });
            }

            await _signInManager.RefreshSignInAsync(user);

            return Json(new { success = true, message = "Your profile has been updated." });
        }



        public async Task<IActionResult> AllowUserName(ManagerUserViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            var isAllowed = user is null || user.Id.Equals(model.Id);
            return Json(isAllowed);
        }

        public async Task<IActionResult> AllowEmail(ManagerUserViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var isAllowed = user == null || user.Id.Equals(model.Id);
            return Json(isAllowed);
        }


        [AjaxOnly]
		[HttpGet]
		public async Task<IActionResult> TwoFactor()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound(new { message = $"Unable to load user with ID '{_userManager.GetUserId(User)}'." });
			}

			// Check if 2FA is already enabled
			if (await _userManager.GetTwoFactorEnabledAsync(user))
			{
				return BadRequest(new { message = "Two-factor authentication is already enabled." });
			}


			TwoFactorViewModel twoFactorViewModel = await LoadSharedKeyAndQrCodeUriAsync(user);

			return PartialView("_TwoFactorForm", twoFactorViewModel);
		}


		[HttpPost]
		public async Task<IActionResult> TwoFactorAuthentication(TwoFactorViewModel model)
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound(new { message = $"Unable to load user with ID '{_userManager.GetUserId(User)}'." });
			}

			// Check if 2FA is already enabled
			if (await _userManager.GetTwoFactorEnabledAsync(user))
			{
				return BadRequest(new { message = "Two-factor authentication is already enabled." });
			}

			if (!ModelState.IsValid)
				return BadRequest(new { message = "Invalid model state." });

			// Strip spaces and hyphens
			var verificationCode = model.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

			var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
				user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

			if (!is2faTokenValid)
			{
				ModelState.AddModelError("model.Code", "Verification code is invalid.");
				TwoFactorViewModel twoFactorViewModel = await LoadSharedKeyAndQrCodeUriAsync(user);

				return BadRequest(new { message = "Verification code is invalid." });
			}

			await _userManager.SetTwoFactorEnabledAsync(user, true);
			var userId = await _userManager.GetUserIdAsync(user);
			_logger.LogInformation("User with ID '{UserId}' has enabled 2FA with an authenticator app.", userId);

			TempData["StatusMessage"] = "Your authenticator app has been verified.";

			if (await _userManager.CountRecoveryCodesAsync(user) == 0)
			{
				var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
				model.RecoveryCodes = recoveryCodes.ToArray();

				string url = Url.Action("ShowRecoveryCodes", "Manage", new { area = "Identity/Account" })!;
				return Redirect(url);
			}
			else
			{
				//var isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
				return PartialView("_TwoFactorComponent");
			}
		}


		[HttpPost]
		public async Task<IActionResult> DisableTwoFactorAuthentication()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound(new { message = $"Unable to load user with ID '{_userManager.GetUserId(User)}'." });
			}

			// Check if 2FA is already disabled
			if (!await _userManager.GetTwoFactorEnabledAsync(user))
			{
				return BadRequest(new { message = "Two-factor authentication is already disabled." });
			}

			var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
			if (!disable2faResult.Succeeded)
			{
				throw new InvalidOperationException($"Unexpected error occurred disabling 2FA.");
			}

			_logger.LogInformation("User with ID '{UserId}' has disabled 2fa.", _userManager.GetUserId(User));
			//var isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
			return PartialView("_TwoFactorComponent");
		}



		private async Task<TwoFactorViewModel> LoadSharedKeyAndQrCodeUriAsync(ApplicationUser user)
		{

			// Load the authenticator key & QR code URI to display on the form
			await _userManager.ResetAuthenticatorKeyAsync(user);
			var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
			if (string.IsNullOrEmpty(unformattedKey))
			{
				await _userManager.ResetAuthenticatorKeyAsync(user);
				unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
			}


			var email = await _userManager.GetEmailAsync(user);

			var twoFactorViewModel = new TwoFactorViewModel
			{
				SharedKey = FormatKey(unformattedKey!),
				QRCodeBytes = GenerateQrCodeBytes("Furnihuture", FormatKey(unformattedKey!), email!)
			};

			return twoFactorViewModel;
		}


		private string FormatKey(string unformattedKey)
		{
			var result = new StringBuilder();
			int currentPosition = 0;
			while (currentPosition + 4 < unformattedKey.Length)
			{
				result.Append(unformattedKey.AsSpan(currentPosition, 4)).Append(' ');
				currentPosition += 4;
			}
			if (currentPosition < unformattedKey.Length)
			{
				result.Append(unformattedKey.AsSpan(currentPosition));
			}

			return result.ToString().ToLowerInvariant();
		}

		private Byte[] GenerateQrCodeBytes(string provider, string unformattedKey, string userEmail)
		{
			var qrCodeGenerator = new QRCodeGenerator();
			var qrCodeData = qrCodeGenerator.CreateQrCode($"otpauth://totp/{provider}:{userEmail}?secret={unformattedKey}&issur={provider}", QRCodeGenerator.ECCLevel.Q);
			var qrCode = new PngByteQRCode(qrCodeData);

			return qrCode.GetGraphic(20);
		}

	}
}
