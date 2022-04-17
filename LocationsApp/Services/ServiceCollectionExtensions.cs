using LocationsApp.DataAccess.Repository;
using LocationsApp.DataAccess.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LocationsApp.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services)
        {
            services.AddTransient<ICountryRepository, CountryRepository>();
            services.AddTransient<IStateRepository, StateRepository>();
            services.AddTransient<ICityRepository, CityRepository>();
            services.AddTransient<IDistrictRepository, DistrictRepository>();
            services.AddTransient<IAddressRepository, AddressRepository>();
            return services;
        }
    }
}
