using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ddos.Interfaces
{
    public interface ITargetsService
    {
        IEnumerable<string> GetTargets();
        string GetOne();
    }
}
