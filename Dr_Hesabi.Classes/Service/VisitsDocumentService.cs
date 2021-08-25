using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Class;
using Dr_Hesabi.Classes.Interface;
using Dr_Hesabi.DataLayers.Context;
using Dr_Hesabi.DataLayers.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Dr_Hesabi.Classes.Service
{
    public class VisitsDocumentService : IVisitDocument,IDisposable
    {
        private readonly DataBaseContext db;

        public VisitsDocumentService(DataBaseContext context)
        {
            db = context;
        }

        public async Task AddVisit(string IP, string ID)
        {
            db.Add(new VisitsDocument()
            {
                VisitID = CodeGeneratore.ActiveCode(),
                IP = IP,
                TableID = ID,
                DateTime = DateTime.Now
            });
            await db.SaveChangesAsync();
        }

        public async Task<bool> ExistIP(string IP, string ID)
        {
            return await db.VisitsDocuments.AnyAsync(s => s.IP == IP && s.TableID == ID);
        }

        public void Dispose()
        {
            db?.Dispose();
        }
    }
}
