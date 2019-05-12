
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextCMS.DbFileSystem
{
    public class EmptyDisposable : System.IDisposable
    {
        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
