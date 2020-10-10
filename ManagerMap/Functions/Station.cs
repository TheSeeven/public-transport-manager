using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ManagerMap
{
    class Station
    {
        public static void insert(string name, string lat, string l,string type)
        {
            if (((Int64)(Connection.execute_procedure($"call get_type('{type.ToUpper()}')")[0].ItemArray[0]) != 0))
            {
                Connection.execute_procedure($"call insert_station('{name}',{lat},{l},'{type.ToUpper()}')");
            }
            else
            {
                Console.Write("Nu a putut fi inserata statia!!");
            }
        }

        public static void delete(int id)
        {
            Connection.execute_procedure($"call delete_station({id})");
        }

        public static List<DataRow> get_vehicle_stations(string name)
        {
            return Connection.execute_procedure($"call stations('{name}')");
        }

    }
}
