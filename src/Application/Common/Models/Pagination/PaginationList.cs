using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Models.Pagination;

public sealed record PaginatedList<T>
{
    public IReadOnlyCollection<T> Items { get; }
    public int PageNumber { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }
    public int PageSize { get; init; }

    public PaginatedList(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items.ToList().AsReadOnly();

        TotalCount = totalCount;

        PageNumber = pageNumber;

        PageSize = pageSize;

        TotalPages = CalculateTotalPages(totalCount, pageSize);
    }

    [JsonConstructor]
    public PaginatedList(IReadOnlyCollection<T> items, int totalCount, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        TotalPages = CalculateTotalPages(totalCount, pageSize);
        TotalCount = totalCount;
        Items = items;
        PageSize = pageSize;
    }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var itemsAndCount = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new { Item = x, TotalCount = source.Count() })
            .ToListAsync();

        var items = itemsAndCount.Select(x => x.Item).ToList();
        var count = itemsAndCount.FirstOrDefault()?.TotalCount ?? 0;

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }

    public static PaginatedList<T> Create(IEnumerable<T> source, int pageNumber, int pageSize)
    {
        var sourceList = source.ToList();

        var count = sourceList.Count;

        var items = sourceList
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }

    public static PaginatedList<T> Create(List<T> source, int totalCount, int pageNumber, int pageSize)
    {
        return new PaginatedList<T>(source, totalCount, pageNumber, pageSize);
    }

    private static int CalculateTotalPages(int totalCount, int pageSize)
    {
        return (int)Math.Ceiling(totalCount / (double)pageSize);
    }
}