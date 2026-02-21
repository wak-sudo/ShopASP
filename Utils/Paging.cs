using Microsoft.EntityFrameworkCore;

namespace ShopASP.Utils
{
    public class Paging<T> where T : class
    {
        public readonly int TotalPages;

        public readonly int CurrentPage;

        public readonly IReadOnlyCollection<T> Items;

        public Paging(IQueryable<T> query, int? pageNumber, int itemsPerPage = Constants.ItemsPerPage)
        {
            const int defaultPage = 1;

            if (itemsPerPage < 0) itemsPerPage = 10;

            int totalPageCount = GetTotalPageCount(query, itemsPerPage);

            int pageNumberFinal = pageNumber == null || pageNumber < 1 || pageNumber > totalPageCount ? defaultPage : pageNumber.Value;

            TotalPages = totalPageCount;
            CurrentPage = pageNumberFinal;
            Items = query.AsNoTracking()
                    .Skip((pageNumberFinal - 1) * itemsPerPage)
                    .Take(itemsPerPage)
                    .ToList();
        }

        private static int GetTotalPageCount(int totalItems, int itemsPerPage) =>
            (int)Math.Ceiling((float)totalItems / itemsPerPage);

        private static int GetTotalPageCount<K>(IQueryable<K> query, int itemsPerPage) =>
            GetTotalPageCount(query.Count(), itemsPerPage);
    }
}
