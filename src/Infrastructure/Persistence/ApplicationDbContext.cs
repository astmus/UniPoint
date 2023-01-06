using System.Reflection;
using Duende.IdentityServer.EntityFramework.Options;
using MediatR;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MissBot.Application.Common.Interfaces;
using MissBot.Domain.Entities;
using MissBot.Infrastructure.Identity;
using MissBot.Infrastructure.Persistence.Interceptors;

namespace MissBot.Infrastructure.Persistence;
public class ApplicationDbContext : DbContext/*ApiAuthorizationDbContext<ApplicationUser>*/, IApplicationDbContext
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

    public DbSet<TodoList> TodoLists => Set<TodoList>();

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

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
}
