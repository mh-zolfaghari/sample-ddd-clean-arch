using Architecture.Presentation.Middlewares;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace Architecture.Presentation.Configurations;

public static class ConfigureLocalization
{
    internal static IServiceCollection AddConfiguredLocalization(this IServiceCollection services)
    {
        services.AddLocalization(options =>
        {
            options.ResourcesPath = "Localization/Resources";
        });

        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[]
            {
                new CultureInfo("ar-SA"),
                new CultureInfo("en-US"),
                new CultureInfo("fa-IR")
            };

            options.DefaultRequestCulture = new RequestCulture("en-US");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.RequestCultureProviders = [new AcceptLanguageHeaderRequestCultureProvider()];

        });

        return services;
    }

    public static void UseAppLocalization(this IApplicationBuilder app)
    {
        app.UseRequestLocalization();
        app.UseMiddleware<LocalizationMiddleware>();
    }
}
