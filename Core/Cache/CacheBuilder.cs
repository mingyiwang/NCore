namespace Core.Cache
{

    /// <summary>
    /// CacheBuilder should be Thread Safe
    /// </summary>
    public sealed class CacheBuilder
    {
        private ICache _cache;

        public ICache Build()
        {
            return _cache;
        }

    }

}