using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace SampleApi
{
    public static class AutoMapperRegistration
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            BusinessEntitiesToDataContracts();
            AgentToBusinessEntities();
			AgentToDataContracts();
            
            return services;
        }

        private static void BusinessEntitiesToDataContracts()
        {
         
        }

        private static void AgentToBusinessEntities()
        {

		}

        private static void AgentToDataContracts()
        {
            
        }
    }
}