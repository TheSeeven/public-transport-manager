using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ManagerMap
{
    class Path
    {
        private static bool check_path(int f, int to) {
            if((Int64)(Connection.execute_procedure($"call check_path({f},{to})")[0].ItemArray[0]) == 0)            {
                return true;
            }
            return false;
        }

        private static bool check_station(int i) {
            if( ! ((Int64)Connection.execute_procedure($"call check_station({i})")[0].ItemArray[0] == 0))            {
                return true;
            }
            return false;
        }

        public static bool insert(int f, int to) {
            if (check_station(f) && check_station(to) && check_path(f, to))            {
                for (int i = 1; i < 5; i++) {
                    Connection.execute_procedure($"call insert_path({f},{to},{i})");
                }
                return true;
            }
            else {
                return false;
            }            
        }

        public static List<DataRow> get_path()
        {
            return Connection.execute_procedure($"call paths()");
        }
        public static void delete(int f,int to) {
            Connection.execute_procedure($"call delete_path({f},{to})");
        }
    }
}
