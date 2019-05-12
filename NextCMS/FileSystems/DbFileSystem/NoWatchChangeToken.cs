
namespace NextCMS.DbFileSystem
{


    public class NoWatchChangeToken
        : Microsoft.Extensions.Primitives.IChangeToken
    {
        bool Microsoft.Extensions.Primitives.IChangeToken.HasChanged => false;

        bool Microsoft.Extensions.Primitives.IChangeToken.ActiveChangeCallbacks => false;

        System.IDisposable Microsoft.Extensions.Primitives.IChangeToken.RegisterChangeCallback(System.Action<object> callback, object state)
        {
            return new EmptyDisposable();
        }


    }


}
