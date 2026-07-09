using Auth.Core.DTO;
using Auth.Core.ServiceContracts;
using Auth.Core.Services;
using Auth.Core.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
 

namespace Auth.Core
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Extension method to add infrastructure services to the dependecy injection container.
        /// 
        ///</summary>
        ///>/// <param name="services">.</param>
        ///<returns></returns>
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            // Register your infrastructure services here
            // Example: services.AddSingleton<IMyService, MyService>();
             services.AddTransient<IUserService, UserService>();
             services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
             services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
           

            return services;
        }
    }
}
