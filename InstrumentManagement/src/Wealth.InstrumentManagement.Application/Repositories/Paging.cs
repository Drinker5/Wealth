namespace Wealth.InstrumentManagement.Application.Repositories;

public record struct PageRequest(int Page, int PageSize);

public record PaginatedResult<T>(IReadOnlyCollection<T> Items, int TotalCount);