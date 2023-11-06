using Microsoft.AspNetCore.Authorization;

namespace IdentityUnderTheHood.Authorization
{
    public class HRManagerProbationRequirement : IAuthorizationRequirement
    {
        public int ProbationMonths { get; private set; }

        public HRManagerProbationRequirement(int probationMonth) 
        {
            ProbationMonths = probationMonth;
        }        
    }

    public class HRManagerProbationRequirementHandler : AuthorizationHandler<HRManagerProbationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HRManagerProbationRequirement requirement)
        {
            if (!context.User.HasClaim(claim => claim.Type == "EmploymentDate"))
                return Task.CompletedTask;

            if (DateTime.TryParse(context.User.FindFirst(claim => claim.Type == "EmploymentDate")?.Value, out DateTime employmentDate)) 
            {
                var period = DateTime.Now - employmentDate;
                if (period.Days > 30 * requirement.ProbationMonths)
                    context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
