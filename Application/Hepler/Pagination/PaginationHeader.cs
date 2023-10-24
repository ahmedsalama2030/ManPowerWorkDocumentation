namespace Application.Common.Pagination
{
    public class PaginationHeader
    {
        // public int  PageNumber { get; set; }
        // public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public PaginationHeader( int totalItems, int totalPages)
        {
             this.TotalItems = totalItems;
            this.TotalPages = totalPages;
          }
    }
}