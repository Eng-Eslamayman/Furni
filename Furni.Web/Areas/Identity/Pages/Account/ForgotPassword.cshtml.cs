﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace Furni.Web.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
		private readonly IEmailBodyBuilder _emailBodyBuilder;
		private readonly IEmailSender _emailSender;

		public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IEmailBodyBuilder emailBodyBuilder, IEmailSender emailSender)
		{
			_userManager = userManager;
			_emailBodyBuilder = emailBodyBuilder;
			_emailSender = emailSender;
		}

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

				// Set place holders to send it to email Body Builder to replace holders in html file
				var placeHolders = new Dictionary<string, string>()
				{
					{ "imageUrl", "https://res.cloudinary.com/dzqc5nfai/image/upload/v1717874871/icon-positive-vote-2_ayuqhh.svg" },
					{ "header", $"Hey {user.FullName}," },
					{ "body", "Please click the below reset your password" },
					{ "url", $"{HtmlEncoder.Default.Encode(callbackUrl!)}" },
					{ "linkTitle", "Reset Password" }
				};

				// Email Body
				var body = _emailBodyBuilder.GetEmailBody(EmailTemplates.Email, placeHolders);

				// Send the email
				BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(Input.Email, "Reset Password", body));


				return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
