using ddos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ddos.Services
{
    public class Tools : ITools
    {
        public Task<byte[]> CreateLoad(int size)
        {
            return Task.Factory.StartNew<byte[]>(() =>
            {
                var r = new Random();
                var b = new byte[size];
                r.NextBytes(b);

                return b;
            });
        }

        public async Task<IPEndPoint> GetIP(string target)
        {
            var u = new Uri(target);
            var ips = (await Dns.GetHostAddressesAsync(u.Host)).FirstOrDefault();

            if (ips == null)
                return null;

            return new IPEndPoint(ips, u.Port);
        }
    }
}
