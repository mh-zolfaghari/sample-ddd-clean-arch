namespace Architecture.Application;

public static class AppServiceConfiguration
{
    public static void ConfigureApplicationServices(this WebApplicationBuilder builder)
    {
        // Register Application Mediator
        builder.Services.AddScoped<IMediator, Mediator>();

        // Register Fluent Validation Validators
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Configure Wolverine with Fluent Validation and CQRS Handlers
        builder.Services.AddWolverine(opts =>
        {
            // Integrating Fluent Validation
            opts.UseFluentValidation();

            // Handling Custom Validation Failure Action
            opts.Services.AddSingleton(typeof(IFailureAction<>), typeof(CustomFailureValidationAction<>));

            // Configuring Code Generation Mode Based on Environment
            if (builder.Environment.IsDevelopment())
            {
                opts.CodeGeneration.TypeLoadMode = JasperFx.CodeGeneration.TypeLoadMode.Dynamic;
            }

            // Customizing Handler Discovery for CQRS
            opts.Discovery.CustomizeHandlerDiscovery(discovery =>
            {
                // Command Handlers
                discovery.Includes.Implements(typeof(ICommandRequestHandler<>));
                discovery.Includes.Implements(typeof(ICommandRequestHandler<,>));

                // Query Handlers
                discovery.Includes.Implements(typeof(IQueryRequestHandler<,>));
                discovery.Includes.Implements(typeof(ICollectionQueryRequestHandler<,>));

                // Event Handler
                discovery.Includes.Implements(typeof(IEventHandler<>));

            }).IncludeAssembly(typeof(AppServiceConfiguration).Assembly);
        });
    }
}
