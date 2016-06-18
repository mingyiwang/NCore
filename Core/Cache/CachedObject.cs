namespace Core.Cache
{


    public class CachedObject<TKey, TValue>
    {

        public TKey Key
        {
            get; set;
        }
        public TValue Item
        {
            get; set;
        }
        public bool Exist
        {
            get; set;
        }

    }


}