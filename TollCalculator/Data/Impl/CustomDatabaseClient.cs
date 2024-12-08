using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TollCalculator.Data.Interface;

namespace TollCalculator.Data.Impl;

public class CustomDatabaseClient : ICustomDatabaseClient
{
    private readonly DbContext _context;

    public CustomDatabaseClient(DbContext context)
    {
        _context = context;
    }

    public async Task<T?> ReadOne<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        where T : class
    {
        var dbSet = _context.Set<T>();

        IQueryable<T> query = dbSet;
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        T? item = await query.FirstOrDefaultAsync(predicate);
        return item;
    }
}