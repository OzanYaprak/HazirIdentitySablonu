using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AspNetCoreIdentityApp.Extensions
{
    public static class ModelStateExtensions
    {
        public static void AddModelErrorList(this ModelStateDictionary modelState, List<string> errors )
        {
            errors.ForEach(error => modelState.AddModelError(string.Empty, error));
        }

        //Yukarıdaki metotun overload u
        public static void AddModelErrorList(this ModelStateDictionary modelState, IEnumerable<IdentityError> errors)
        {
            errors.ToList().ForEach(error => modelState.AddModelError(string.Empty, error.Description));
        }
    }
}
