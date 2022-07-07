using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace SwarmPortal.Context;

public static class DbSetExtensions
{
    public static async Task<T> AddModelAsync<T>(this DbSet<T> dbSet, T model, CancellationToken ct = default) where T : class
    {
        var dbEntity = await dbSet.AddAsync(model, ct);
        return dbEntity.Entity;
    }
    public static async IAsyncEnumerable<T> AddModelRangeAsync<T>(this DbSet<T> dbSet, IAsyncEnumerable<T> models, [EnumeratorCancellation] CancellationToken ct = default) where T : class
    {
        //TODO: check if this is the best way to do this
        //I think it is, because then as items are passed through this method,
        // they are "pulled" out the other side of the IAsyncEnumerable.
        await foreach (var item in models)
        {
            var dbEntity = await dbSet.AddAsync(item, ct);
            yield return dbEntity.Entity;
        }
    }
    public static async Task<T> AddToDbSet<T>(this T model, DbSet<T> dbSet, CancellationToken ct = default) where T : class
    {
        var dbEntity = await dbSet.AddAsync(model, ct);
        return dbEntity.Entity;
    }
    public static async IAsyncEnumerable<T> AddRangeToDbSet<T>(this IAsyncEnumerable<T> models, DbSet<T> dbSet, [EnumeratorCancellation] CancellationToken ct = default) where T : class
    {
        //TODO: check if this is the best way to do this
        //I think it is, because then as items are passed through this method,
        // they are "pulled" out the other side of the IAsyncEnumerable.
        await foreach (var item in models)
        {
            var dbEntity = await dbSet.AddAsync(item, ct);
            yield return dbEntity.Entity;
        }
    }
    public static T Alter<T>(this T model, Action<T> modification)
        where T : class
    {
        modification(model);
        return model;
    }
    public static async IAsyncEnumerable<T> OnEach<T>(this IAsyncEnumerable<T> set, Func<T, Task> action, [EnumeratorCancellation] CancellationToken ct = default)
        where T : class
    {
        await foreach (var item in set.WithCancellation(ct))
        {
            await action(item);
            yield return item;
        }
    }
    public static async IAsyncEnumerable<T> OnEach<T>(this IAsyncEnumerable<T> set, Action<T> action, [EnumeratorCancellation] CancellationToken ct = default)
        where T : class
    {
        await foreach (var item in set.WithCancellation(ct))
        {
            action(item);
            yield return item;
        }
    }


    
    public static async Task<T> AddOrUpdateAsync<T>(this DbSet<T> dbSet, Expression<Func<T, bool>> predicate, Action<T> update, Func<T> getSingle, CancellationToken ct = default)
        where T : class
    {
        var existing = await dbSet.SingleOrDefaultAsync(predicate);
        if (existing == null)
        {
            var dbItem = getSingle();
            await dbSet.AddAsync(dbItem, ct);
            return dbItem;
        }
        else
        {
            update(existing);
            return existing;
        }
    }
}