namespace AuthService.API.Extentions
{
    public static class CorsServiceExtensions
    {
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("DefaultCors", policy =>
                {
                    policy
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins(
                            "http://localhost:4200", // Angular
                            "http://localhost:3000"  // React
                        );
                });
            });

            return services;
        }
    }
}
