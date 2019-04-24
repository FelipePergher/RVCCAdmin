namespace LigaCancer.Code
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public static class ControllerHelper
    {
        public static void AddErrors(this ModelStateDictionary dic, IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                dic.AddModelError(string.Empty, error.Description);
            }
        }

        public static void AddErrors(this ModelStateDictionary dic, TaskResult result)
        {
            foreach (var error in result.Errors)
            {
                dic.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
