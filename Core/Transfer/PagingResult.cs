using System.Collections.Generic;

namespace Core.Transfer {

    public class PagingResult<TModel> {

        public List<TModel> Data { get; set; }
        public int Offset        { get; set; }
        public int Total         { get; set; }

    }
}