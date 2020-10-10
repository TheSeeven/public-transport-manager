using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Text;

namespace ManagerMap
{
    class Vehicle
    {

        private static bool check_id(int id)
        {
            if (((Int64)Connection.execute_procedure($"call check_id({id})")[0].ItemArray[0])==0)
            {
                return true;
            }
            return false;
        }

        public static int get_id()
        {
            for(int i=0;i<10001;i++)
            {
                if((Int64)Connection.execute_procedure($"call check_id({i})")[0].ItemArray[0]==0)
                {
                    return i;
                }
            }
            return -1;
        }

        public static bool insert(int id,string name, string type)
        { 
            if(check_id(id))
            {
                Connection.execute_procedure($"call insert_vehicle({id},'{name}','{type.ToUpper()}')");
                return true;
            }
            else
            {
                return false;
            }
        }
        public static List<DataRow> get_Vehicles()
        {
            return Connection.execute_procedure("call view_vehicles_manager()");
        }
        public static void delete(int id)
        {
            Connection.execute_procedure($"call delete_vehicle({id})");
        }
    }
}
