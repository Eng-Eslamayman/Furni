using Furni.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using QRCoder;
using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;

namespace Furni.Web.Areas.Admin.Controllers
{
	[Area(AppRoles.Admin)]
	[Authorize(Roles = AppRoles.Admin)]
	public class ManagerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ManagerController> _logger;
        private readonly UrlEncoder _urlEncoder;
        private readonly IImageService _imageService;
        //private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";


        public ManagerController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IImageService imageService,
            ILogger<ManagerController> logger,
            UrlEncoder urlEncoder)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _imageService = imageService;
            _logger = logger;
            _urlEncoder = urlEncoder;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var managerView = new ManagerViewModel
            {
                PhoneNumber = phoneNumber,
                FullName = user.FullName
            };

            return View(managerView);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ManagerViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
                var managerView = new ManagerViewModel
                {
                    PhoneNumber = phoneNumber,
                    FullName = user.FullName
                };

                return View(managerView);
            }


            if (model.FullName != user.FullName)
            {
                user.FullName = model.FullName;
                var setFullName = await _userManager.UpdateAsync(user);
                if (!setFullName.Succeeded)
                {
                    TempData["StatusMessage"] = "Unexpected error when trying to set full name.";
                    return RedirectToPage(nameof(Index));
                }
            }

            var phoneNumberCheck = await _userManager.GetPhoneNumberAsync(user);
            if (model.PhoneNumber != phoneNumberCheck)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    TempData["StatusMessage"] = "Unexpected error when trying to set phone number.";
                    return View(nameof(Index));
                }
            }


            if (model.Avatar is not null)
            {
                _imageService.Delete($"/images/users/{user.Id}.png");
                var (isUploaded, errorMessage) = await _imageService.UploadeAsynce(model.Avatar, $"{user.Id}.png", "/images/users", hasThumbnail: false);
                if (!isUploaded)
                {
                    ModelState.AddModelError("model.Avatar", errorMessage!);
                    var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
                    var managerView = new ManagerViewModel
                    {
                        PhoneNumber = phoneNumber,
                        FullName = user.FullName
                    };

                    return View(managerView);
                }
            }
            else if (model.ImageRemoved)
                _imageService.Delete($"/images/users/{user.Id}.png");

            await _signInManager.RefreshSignInAsync(user);
            TempData["StatusMessage"] = "Your profile has been updated";
            return View(nameof(Index));
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
