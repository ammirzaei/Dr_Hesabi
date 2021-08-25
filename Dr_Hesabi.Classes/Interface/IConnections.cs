﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dr_Hesabi.DataLayers.Entity;

namespace Dr_Hesabi.Classes.Interface
{
    public interface IConnections
    {
        Task AddConnection(Connections connection);
        Task<string> GetEmailSupport();
    }
}
