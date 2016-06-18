using System;

namespace MQ.Queue {

    public class Message {

        // start from 100000, keep it in order
        public long Id              { get; set; }

        public string Data          { get; set; }

        public int Retires          { get; set; }

        public MessageStatus Status { get; set; }
        
        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

    }
}
