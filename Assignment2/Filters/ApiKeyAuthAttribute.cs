using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ShortestRouteApi.Filters
{
    public class ApiKeyAuthAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _requiredKey;

        public ApiKeyAuthAttribute(string requiredKey)
        {
            _requiredKey = requiredKey;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("X-Api-Key", out var providedKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (providedKey != _requiredKey)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
