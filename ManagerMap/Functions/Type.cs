using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ManagerMap
{
    class Type
    {
        public static void insert(string name,byte rail)
        {
            if(((Int64)(Connection.execute_procedure($"call get_type('{name.ToUpper()}')")[0].ItemArray[0])==0))
            {
                Connection.execute_procedure($"call insert_vehicle_type('{name.ToUpper()}',{rail})");
            }
            else
            {
                Console.Write("Nu a putut fi inserat tipul de vehicul!");
            }
        }

        public static List<DataRow> get_types()
        {
            return Connection.execute_procedure("call get_types()");
        }
        public static void delete(string name)
        {
            Connection.execute_procedure($"call delete_vehicle_type('{name.ToUpper()}')");
        }

    }
}
