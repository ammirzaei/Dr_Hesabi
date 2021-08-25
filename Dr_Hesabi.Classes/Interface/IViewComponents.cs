using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.ViewModel;
using Dr_Hesabi.DataLayers.Entity;

namespace Dr_Hesabi.Classes.Interface
{
    public interface IViewComponents
    {
       Task<IEnumerable<Staffs>> GetAllStaffs();
       Task<IEnumerable<GetAllMajorsViewModel>> GetAllMajors();
    }
}
