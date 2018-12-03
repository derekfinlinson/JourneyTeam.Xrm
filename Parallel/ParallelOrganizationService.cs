using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using System.Collections.Concurrent;

namespace Xrm
{
    public class ParallelOrganizationService
    {
        private IExtendedExecutionContext _context;

        public ParallelOrganizationService(IExtendedExecutionContext context)
        {
            _context = context;
        }

        public IEnumerable<Entity> Create(IEnumerable<Entity> entities, Guid userId, ParallelOptions options)
        {
            ExecuteParallel<Entity>(entities, userId, options,
                (entity, service) =>
                {
                    entity.Id = service.Create(entity);
                }
            );

            return entities;
        }

        public void Update(IEnumerable<Entity> entities, Guid userId, ParallelOptions options)
        {
            ExecuteParallel<Entity>(entities, userId, options,
                (entity, service) =>
                {
                    service.Update(entity);
                }
            );
        }

        public void Delete(IEnumerable<Entity> entities, Guid userId, ParallelOptions options)
        {
            ExecuteParallel<Entity>(entities, userId, options,
                (entity, service) =>
                {
                    service.Delete(entity.LogicalName, entity.Id);
                }
            );
        }

        public void ExecuteParallel<TRequest>(IEnumerable<TRequest> requests, Guid userId, ParallelOptions options,
            Action<TRequest, IOrganizationService> execute)
        {
            Func<IOrganizationService> serviceInit = () =>
            {
                return _context.CreateOrganizationService(userId);
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