using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerMap
{
    class Veh
    {
        public string nume { get; set; }
        public string id { get; set; }


        Veh( ) { }

        Veh(string Nume,string Id)
        {
            nume = Nume;
            id = Id;
        }

        public static ObservableCollection<Veh> getVeh(string type)
        {
            ObservableCollection<Veh> res = new ObservableCollection<Veh>();
            foreach (DataRow row in MainPage.VehicleList)
            {
                if (row.ItemArray[2].ToString() == type)
                {
                    res.Add(new Veh(row.ItemArray[1].ToString(), row.ItemArray[0].ToString()));
                }
            }
            return res;
        }
    }
}
