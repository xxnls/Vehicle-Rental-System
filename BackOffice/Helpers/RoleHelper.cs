using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackOffice.Models.DTOs.Employees;
using BackOffice.Models.Employees;
using BackOffice.Services;

namespace BackOffice.Helpers
{
    public static class RoleHelper
    {
        /// <summary>
        /// Checks if the currently logged user has a specific role.
        /// </summary>
        /// <param name="roleName">The name of the role to check.</param>
        /// <returns>True if the user has the role; otherwise, false.</returns>
        public static bool HasRole(string roleName)
        {
            var roles = SessionManager.Get("Roles") as List<string>;
            return roles != null && roles.Contains(roleName);
        }

        /// <summary>
        /// Checks if the current user has a specific permission.
        /// </summary>
        /// <param name="permission">The permission to check (e.g., ManageVehicles).</param>
        /// <returns>True if the user has the permission; otherwise, false.</returns>
        public static async Task<bool> HasPermission(string permission)
        {
            if (SessionManager.Get("User") is not EmployeeDto user)
            {
                return false;
            }

            var apiClient = new ApiClient();
            var result = await apiClient.GetAsync<bool>($"EmployeeRoles/user/{user.Id}/has-permission/{permission}");
            return result;
        }

        /// <summary>
        /// Checks if the currently logged user has any of the specified roles.
        /// </summary>
        /// <param name="roleNames">The names of the roles to check.</param>
        /// <returns>True if the user has any of the roles; otherwise, false.</returns>
        public static bool HasAnyRole(params string[] roleNames)
        {
            var roles = SessionManager.Get("Roles") as List<string>;
            return roles != null && roles.Any(roleNames.Contains);
        }

        ///// <summary>
        ///// Checks if the current user has all of the specified roles.
        ///// </summary>
        ///// <param name="roleNames">The names of the roles to check.</param>
        ///// <returns>True if the user has all of the roles; otherwise, false.</returns>
        //public static bool HasAllRoles(params string[] roleNames)
        //{
        //    var user = SessionManager.Get("User") as Employee;
        //    return user?.Roles?.All(role => roleNames.Contains(role)) ?? false;
        //}

        ///// <summary>
        ///// Gets the current user's roles.
        ///// </summary>
        ///// <returns>A list of roles assigned to the current user.</returns>
        //public static List<string> GetUserRoles()
        //{
        //    var user = SessionManager.Get("User") as Employee;
        //    return user?.Roles?.ToList() ?? new List<string>();
        //}

        ///// <summary>
        ///// Gets the current user's permissions.
        ///// </summary>
        ///// <returns>A list of permissions assigned to the current user.</returns>
        //public static List<string> GetUserPermissions()
        //{
        //    var user = SessionManager.Get("User") as Employee;
        //    return user?.Permissions?.ToList() ?? new List<string>();
        //}
    }
}
