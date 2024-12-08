using System.Linq.Expressions;

namespace TollCalculator.Data.Interface;

public interface ICustomDatabaseClient
{
    Task<T?> ReadOne<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T: class;
}