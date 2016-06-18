namespace Core.IO
{
    public class StringBuffer : Buffer<char>
    {
        public StringBuffer(int capacity) : base(capacity) {}

        public override char Get()
        {
            throw new System.NotImplementedException();
        }

        public override void Put(char data)
        {
            throw new System.NotImplementedException();
        }

    }
}