namespace Core.Extension
{

    public static class LazyExtension
    {

        public static T Get<T>(this System.Lazy<T> lazy)
        {
            return lazy.Value;
        }        
    
    }

}