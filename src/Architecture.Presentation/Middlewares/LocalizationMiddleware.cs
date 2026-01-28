using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace Architecture.Presentation.Middlewares;

public sealed class LocalizationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var provider = new AcceptLanguageHeaderRequestCultureProvider();
        var result = await provider.DetermineProviderCultureResult(context);

        if (result != null)
        {
            var culture = result.Cultures.FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(culture.Value))
            {
                var cultureInfo = CultureInfo
                    .GetCultures(CultureTypes.AllCultures)
                    .FirstOrDefault(c => c.Name.Equals(culture.Value, StringComparison.OrdinalIgnoreCase));

                if (cultureInfo != null)
                {
                    CultureInfo.CurrentCulture = cultureInfo;
                    CultureInfo.CurrentUICulture = cultureInfo;
                }
            }
        }

        await next(context);
    }
}
