using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ddos.Interfaces
{
    public interface INuke
    {
        Task Boom(string destination, bool useProxy, CancellationToken cancellationToken);
        string WhoAmI();
    }
}
