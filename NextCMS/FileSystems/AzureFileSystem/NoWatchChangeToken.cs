
namespace BlogSample_ASPNET5_ViewsInAzureStorage.AzureFileSystem
{


    public class NoWatchChangeToken 
        : Microsoft.Extensions.Primitives.IChangeToken
    {
        public System.IDisposable RegisterChangeCallback(System.Action<object> callback, object state)
        {
            return new EmptyDisposable();
        }

        public bool HasChanged => false;
        public bool ActiveChangeCallbacks => false;
    }


}
