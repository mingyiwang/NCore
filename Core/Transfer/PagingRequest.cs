namespace Core.Transfer {

    public class PagingRequest {

        public int    Limit    { get; set; }
        public int    OffSet   { get; set; }
        public string Keyword  { get; set; }

    }
}