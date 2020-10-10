using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ManagerMap
{
    class VehicleStation
    {
        public static void insert(string name, int id)
        {
            Connection.execute_procedure($"call insert_vehicle_station('{name}',{id})");
        }

        public static List<DataRow> unassigned_stations()
        {
            return Connection.execute_procedure("call unassigned_stations()");
        }
        public static void delete(string name, int id)
        {
            Connection.execute_procedure($"call delete_vehicle_station('{name}',{id})");
        }

    }
}
