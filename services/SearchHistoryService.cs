using System.Collections.Generic;
using System.Linq;

namespace MovieSearchApp
{
    public class SearchHistoryService
    {
        private readonly Queue<string> _searchHistory = new Queue<string>(5);

        public void AddSearch(string query)
        {
            if (_searchHistory.Count == 5)
            {
                _searchHistory.Dequeue();
            }
            _searchHistory.Enqueue(query);
        }

        public IEnumerable<string> GetSearchHistory()
        {
            return _searchHistory.Reverse();
        }
    }
}