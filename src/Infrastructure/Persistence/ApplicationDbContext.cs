using System.Reflection;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MissBot.Infrastructure.Common;
using MissBot.Infrastructure.Persistence.Interceptors;
using MissCore.DataAccess;

namespace MissBot.Infrastructure.Persistence;
public class ApplicationDbContext : DbContext/*ApiAuthorizationDbContext<ApplicationUser>*/, IApplicationGenericRepository
{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IOptions<OperationalStoreOptions> operationalStoreOptions, IMediator mediator, AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
    //    : base(options, operationalStoreOptions)
    //{
    //    _mediator = mediator;
    //    _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    //}
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator, AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
        : base(options)
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Chat> Chats => Set<Chat>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }

    public IEnumerable<TEntity> GetRepository<TEntity>() where TEntity:class
        => Set<TEntity>().AsNoTracking();
}
