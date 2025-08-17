using Microsoft.AspNetCore.Authorization;

namespace gym_management_api
{
    // ===== ROLE-BASED AUTHORIZATION =====
    public static class Roles
    {
        public const string Owner = "Owner";
        public const string Manager = "Manager";
        public const string Receptionist = "Receptionist";
        public const string Admin = "Owner,Manager"; // Composite role
    }

    public static class Policies
    {
        public const string AdminOnly = "AdminOnly";
        public const string StaffOnly = "StaffOnly";
        public const string TrainerOrStaff = "TrainerOrStaff";
    }

    public class AuthorizationPolicyProvider
    {
        public static void ConfigurePolicies(AuthorizationOptions options)
        {
            options.AddPolicy(Policies.AdminOnly, policy =>
                policy.RequireRole(Roles.Owner, Roles.Manager));

            options.AddPolicy(Policies.StaffOnly, policy =>
                policy.RequireRole(Roles.Owner, Roles.Manager, Roles.Receptionist));

            options.AddPolicy(Policies.TrainerOrStaff, policy =>
                policy.RequireAssertion(context =>
                    context.User.HasClaim("UserType", "Trainer") ||
                    context.User.IsInRole(Roles.Owner) ||
                    context.User.IsInRole(Roles.Manager) ||
                    context.User.IsInRole(Roles.Receptionist)));
        }
    }
}