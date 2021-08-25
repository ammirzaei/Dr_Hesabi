using System;
using System.Collections.Generic;
using System.Text;

namespace Dr_Hesabi.Classes.Class
{
   public static class CodeGeneratore
    {
        public static string ActiveCode()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
