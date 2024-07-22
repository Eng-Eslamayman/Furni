using AutoMapper;
using Furni.Web.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Furni.Web.Extensions;
using System.Text.Encodings.Web;

namespace Furni.Web.Areas.Admin.Controllers
{
	[Area(AppRoles.Admin)]
    [Authorize(Policy = "ManagerOnly")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailBodyBuilder _emailBodyBuilder;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        public UsersController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment
            , IEmailBodyBuilder emailBodyBuilder
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _emailBodyBuilder = emailBodyBuilder;
        }


        // Show List of Users that have Admin Role
        public async Task<IActionResult> Index()
        {
            // Retrieve the current user
            var user = await _userManager.GetUserAsync(User);

            if(user is null)
                return NotFound();

            // Retrieve the user's claims
            var claims = await _userManager.GetClaimsAsync(user);

            // Check if the user has the 'NoAccess' claim with the value 'UsersController'
            if (claims.Any(c => c.Type == "NoAccess" && c.Value == "UsersController"))
            {
                return Forbid(); // Deny access if the claim exists
            }

            // Return users that have Admin Role
            var users = await _userManager.GetUsersInRoleAsync(roleName: AppRoles.Admin);
            var viewModel = _mapper.Map<IEnumerable<UserViewModel>>(users);
            ActionName();
            return View(viewModel);
        }

        [HttpGet]
        [AjaxOnly]
        public async Task<IActionResult> Create()
        {
            //var roles = await _roleManager.Roles.ToListAsync();
            var viewModel = new UserFormViewModel
            {
                Roles = await _roleManager.Roles
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name // r.Id
                }).Where(r => r.Value != AppRoles.Customer).ToListAsync() // Here Admin Can not create customer accounts.
            };
            return PartialView("_Form", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserFormViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            if (model.SelectedRoles.Contains(AppRoles.Customer)) return BadRequest();

            ApplicationUser user = new() // Initail Id to new GUID
            {
                FullName = model.FullName,
                UserName = model.UserName,
                Email = model.Email,
                CreatedById = User.GetUserId(),
                CreatedOn = DateTime.UtcNow // Set admin date
            };
            var identityResult = await _userManager.CreateAsync(user, model.Password!);

            if (identityResult.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, model.SelectedRoles);


                // Add default claims based on role or other criteria
                if (model.SelectedRoles.Contains(AppRoles.Admin))
                {
                    // Assuming new admins should have "Initial" access
                    await _userManager.AddClaimAsync(user, new Claim("Access", "Initial"));
                    await _userManager.AddClaimAsync(user, new Claim("CreatedDate", user.CreatedOn.ToString("o"))); // Add created date for probation check
                }


                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = user.Id, code },
                    protocol: Request.Scheme);

                // Set place holders to send it to email Body Builder to replace holders in html file
                var placeHolders = new Dictionary<string, string>()
                {
                    { "imageUrl", "https://res.cloudinary.com/dzqc5nfai/image/upload/v1717798788/qkp9tphwwlqkrmkdukpx.svg" },
                    { "header", $"Hey {user.FullName}, thanks for joining up!" },
                    { "body", "Please confirm your email" },
                    { "url", $"{HtmlEncoder.Default.Encode(callbackUrl!)}" },
                    { "linkTitle", "Active Account" }
                };

                // Email Body
                var body = _emailBodyBuilder.GetEmailBody(EmailTemplates.Email, placeHolders);

                // Send the email
                await _emailSender.SendEmailAsync(user.Email, "Confirm your email", body);

                var viewModel = _mapper.Map<UserViewModel>(user);
                return PartialView("_UserRow", viewModel);
            }

            return BadRequest(string.Join(',', identityResult.Errors.Select(e => e.Description)));
        }



        [HttpGet]
        [AjaxOnly]
        public async Task<IActionResult> ResetPassword(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound();

            var viewModel = new ResetPasswordFormViewModel { Id = user.Id };

            return PartialView("_ResetPasswordForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordFormViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user is null)
                return NotFound();

            // If password not valid then return to the old password 
            var currentPasswordHash = user.PasswordHash;
            await _userManager.RemovePasswordAsync(user); // Here remove the password from database

            var identityResult = await _userManager.AddPasswordAsync(user, model.Password);
            if (identityResult.Succeeded)
            {
                user.LastUpdatedById = User.GetUserId();
                user.LastUpdatedOn = DateTime.Now;

                await _userManager.UpdateAsync(user);

                var viewModel = _mapper.Map<UserViewModel>(user);
                return PartialView("_UserRow", viewModel);
            }

            // If password not valid then return to the old password because we remove the passord
            user.PasswordHash = currentPasswordHash;
            await _userManager.UpdateAsync(user);

            return BadRequest(string.Join(',', identityResult.Errors.Select(e => e.Description)));

        }


        [HttpGet]
        [AjaxOnly]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();

            var viewModel = _mapper.Map<UserFormViewModel>(user);

            viewModel.SelectedRoles = await _userManager.GetRolesAsync(user);
            viewModel.Roles = await _roleManager.Roles
                                    .Select(r => new SelectListItem
                                    {
                                        Text = r.Name,
                                        Value = r.Name
                                    }).Where(r => r.Value != AppRoles.Customer).ToListAsync();


            return PartialView("_Form", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserFormViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            if (model.SelectedRoles.Contains(AppRoles.Customer)) return BadRequest();

            var user = await _userManager.FindByIdAsync(model.Id!);

            if (user is null)
                return NotFound();

            user = _mapper.Map(model, user);
            user.LastUpdatedById = User.GetUserId();
            user.LastUpdatedOn = DateTime.Now;

            var identityResult = await _userManager.UpdateAsync(user);

            if (identityResult.Succeeded)
            {
                var currentRoles = await _userManager.GetRolesAsync(user);

                var rolesUpdated = !currentRoles.SequenceEqual(model.SelectedRoles); // If old roles == newRoles => it will not go to database

                if (rolesUpdated)
                {
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    await _userManager.AddToRolesAsync(user, model.SelectedRoles);
                }

                await _userManager.UpdateSecurityStampAsync(user);

                var viewModel = _mapper.Map<UserViewModel>(user);
                return PartialView("_UserRow", viewModel);
            }


            return BadRequest(string.Join(',', identityResult.Errors.Select(e => e.Description)));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound();

            user.IsDeleted = !user.IsDeleted;
            user.LastUpdatedById = User.GetUserId();
            user.LastUpdatedOn = DateTime.Now;

            await _userManager.UpdateAsync(user);

            if (user.IsDeleted)
                await _userManager.UpdateSecurityStampAsync(user);

            return Ok(user.LastUpdatedOn.ToString());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unlock(string id)
        {
            var user = await _userManager.FindByIdAsync($"{id}");
            if (user is null) return NotFound();

            var isLocked = await _userManager.IsLockedOutAsync(user);

            if (isLocked)
                await _userManager.SetLockoutEndDateAsync(user, null);

            return Ok();
        }


        public async Task<IActionResult> AllowUserName(UserFormViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            var isAllowed = user is null || user.Id.Equals(model.Id);
            return Json(isAllowed);
        }
        public async Task<IActionResult> AllowEmail(UserFormViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var isAllowed = user is null || user.Id.Equals(model.Id);
            return Json(isAllowed);
        }

        private void ActionName()
        {
            ViewBag.ControllerName = RouteData.Values["controller"]!.ToString();
            ViewBag.ActionName = RouteData.Values["action"]!.ToString();
        }
    }
}
