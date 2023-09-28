namespace DentalClinic.API.Models
{
    public class QueryParameters
    {
        private int _page = 1;
        private int _pageSize = 10;
        private const int maxPageSize = 100;

        public int Page
        {
            get => _page;
            set => _page = (value < 1) ? 1 : value;
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }

        public string OrderBy { get; set; }

        public string Search { get; set; }

       
    }
}
