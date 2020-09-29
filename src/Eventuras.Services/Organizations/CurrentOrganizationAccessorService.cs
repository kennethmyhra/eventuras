using Eventuras.Domain;
using Eventuras.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Eventuras.Services.Organizations
{
    internal class CurrentOrganizationAccessorService : ICurrentOrganizationAccessorService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentOrganizationAccessorService(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<Organization> GetCurrentOrganizationAsync(OrganizationRetrievalOptions options = null)
        {
            var host = _httpContextAccessor.HttpContext.Request.Host;
            if (!host.HasValue)
            {
                return null;
            }

            return await _context.Organizations
                .AsNoTracking()
                .UseOptions(options ?? new OrganizationRetrievalOptions())
                .Where(o => o.Hostnames.Any(h => h.Active && h.Hostname == host.Value))
                .FirstOrDefaultAsync();
        }

        public async Task<Organization> RequireCurrentOrganizationAsync(OrganizationRetrievalOptions options = null)
        {
            return await GetCurrentOrganizationAsync(options) ??
                   throw new OrganizationMisconfigurationException(_httpContextAccessor.HttpContext.Request.Host.Value);
        }
    }
}
