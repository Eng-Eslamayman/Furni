using Microsoft.AspNetCore.Authorization;

namespace Furni.Web.Authorization
{
    public class AdminProbationRequirement : IAuthorizationRequirement
    {
        public int ProbationMonths { get; }

        public AdminProbationRequirement(int probationMonths)
        {
            ProbationMonths = probationMonths;
        }
    }

    public class AdminProbationRequirementHandler : AuthorizationHandler<AdminProbationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminProbationRequirement requirement)
        {
            var createdDateClaim = context.User.FindFirst(c => c.Type == "CreatedDate");
            if (createdDateClaim != null && DateTime.TryParse(createdDateClaim.Value, out DateTime createdDate))
            {
                var period = DateTime.UtcNow - createdDate;
                if (period.TotalDays > 30 * requirement.ProbationMonths)
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }


}
