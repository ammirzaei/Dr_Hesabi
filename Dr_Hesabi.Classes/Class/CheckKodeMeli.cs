using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dr_Hesabi.Classes.Class
{
    public class CheckKodeMeli
    {
        public static bool Check(string Kode)
        {
            if (Kode.Length == 10)
            {
                char[] List = Kode.ToArray();

                int Counter = int.Parse(List.GetValue(9).ToString());

                var SumMax = List.Max() * List.Count();
                var SumList = List.Sum(s => s);
                if (SumMax == SumList)
                {
                    return false;
                }

                int value = 10;
                int sum = 0;
                foreach (var item in List)
                {
                    if (value == 1)
                    {
                        break;
                    }
                    sum += int.Parse(item.ToString()) * value;
                    value--;
                }

                int Result = sum % 11;
                if (Result <= 2)
                {
                    if (Result == Counter)
                    {
                        return true;
                    }
                }
                else
                {
                    int res = 11 - Result;
                    if (res == Counter)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
