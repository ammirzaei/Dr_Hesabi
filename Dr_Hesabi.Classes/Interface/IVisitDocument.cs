using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dr_Hesabi.Classes.Interface
{
    public interface IVisitDocument
    {
       Task<bool> ExistIP(string IP, string ID);
       Task AddVisit(string IP, string ID);
    }
}
