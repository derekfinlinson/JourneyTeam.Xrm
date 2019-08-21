using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace Xrm
{
    public static class ParallelOrganizationService
    {
        public static IEnumerable<Entity> Create(IEnumerable<Entity> entities, Guid? userId, ParallelOptions options, IExtendedExecutionContext context)
        {
            ExecuteParallel<Entity>(entities, userId, options,
                (entity, service) =>
                {
                    entity.Id = service.Create(entity);
                },
                context
            );

            return entities;
        }

        public static void Update(IEnumerable<Entity> entities, Guid? userId, ParallelOptions options, IExtendedExecutionContext context)
        {
            ExecuteParallel<Entity>(entities, userId, options,
                (entity, service) =>
                {
                    service.Update(entity);
                },
                context
            );
        }

        public static void Delete(IEnumerable<Entity> entities, Guid? userId, ParallelOptions options, IExtendedExecutionContext context)
        {
            ExecuteParallel<Entity>(entities, userId, options,
                (entity, service) =>
                {
                    service.Delete(entity.LogicalName, entity.Id);
                },
                context
            );
        }

        public static void Execute(IEnumerable<OrganizationRequest> requests, Guid? userId, ParallelOptions options, IExtendedExecutionContext context)
        {
            ExecuteParallel<OrganizationRequest>(requests, userId, options,
                (request, service) =>
                {
                    service.Execute(request);
                },
                context
            );
        }

        public static void ExecuteParallel<TRequest>(IEnumerable<TRequest> requests, Guid? userId, ParallelOptions options,
            Action<TRequest, IOrganizationService> execute, IExtendedExecutionContext context)
        {
            Func<IOrganizationService> serviceInit = () =>
            {
                return context.CreateOrganizationService(userId);
            };

            using (var threadLocal = new ThreadLocal<IOrganizationService>(serviceInit, true))
            {
                Parallel.ForEach(requests, options,
                    (request, loopState, index) =>
                    {
                        try
                        {
                            execute(request, threadLocal.Value);
                        }
                        catch (FaultException<OrganizationServiceFault>)
                        {
                            throw;
                        }
                    }
                );
            }
        }
    }
}