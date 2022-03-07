using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ddos.Interfaces
{
    public interface ITools
    {
        Task<IPEndPoint> GetIP(string target);
        Task<byte[]> CreateLoad(int size);
    }
}
