using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dr_Hesabi.DataLayers.Entity;

namespace Dr_Hesabi.Classes.Interface
{
    public interface IContents
    {
        Task<Majors> GetMajor(string title);
        Task<IEnumerable<Contents>> GetAllContentsMajor(string majorID);
        Task<IEnumerable<Contents>> GetAllContents(string majorID, string contentID);
        Task<Contents> GetContent(string contentID);
        Task<Contents> GetContentForMajor(string majorTitle, string contentID);
        Task<bool> IsContentinList(string contentID);
        Task<IEnumerable<Contents>> GetSearchContents(string majorID, string q);
    }
}
