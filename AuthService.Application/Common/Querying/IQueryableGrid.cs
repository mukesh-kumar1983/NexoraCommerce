using SharedKernel.Common.Models;

namespace AuthService.Application.Common.Querying;

public interface IQueryableGrid<T>
{
    IQueryable<T> Apply(IQueryable<T> query, GridQuery request);
}