namespace SportsStore.Models
{
    public class PageInfo
    {
        public int TotalItems { get; set; }
        public int ItensPerPage { get; set; }
        public int CurrentPage { get; set; }

        public int TotalPage => (int)Math.Ceiling((decimal)TotalItems / TotalItems);
    }
}
