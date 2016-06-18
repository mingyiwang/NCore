using System;

namespace Core.Aws {


    public class AmazonS3Object {

        public string    ParentKey    { get; set; }
        public string    Key          { get; set; }
        public string    Author       { get; set; }
        public byte[]    Data         { get; set; }
        public DateTime? DateModified { get; set; }
        public string    Url          { get; set; }
        public DateTime  DateExpired  { get; set; }

    }

}
