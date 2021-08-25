using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dr_Hesabi.Classes.Interface;
using Dr_Hesabi.DataLayers.Context;
using Dr_Hesabi.DataLayers.Entity;
using Microsoft.EntityFrameworkCore;

namespace Dr_Hesabi.Classes.Service
{
    public class ConnectionsService : IConnections, IDisposable
    {
        private readonly DataBaseContext db;

        public ConnectionsService(DataBaseContext context)
        {
            this.db = context;
        }

        public async Task AddConnection(Connections connection)
        {
            await db.AddAsync(connection);
            await db.SaveChangesAsync();
        }

        public async Task<string> GetEmailSupport()
        {
            return await Task.FromResult(db.Setting.FirstAsync().Result.EmailSupport);
        }

        public void Dispose()
        {
            db?.Dispose();
        }
    }
}
