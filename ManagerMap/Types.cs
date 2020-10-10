using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerMap
{
    class Types
    {
        public string name { get; set; }
        
        Types( ) { }

        Types (string Name)
        {
            name = Name;
            
        }

        public static ObservableCollection<Types> getType( )
        {
            ObservableCollection<Types> res = new ObservableCollection<Types>();
            foreach (DataRow row in MainPage.VehicleType)
            {
                res.Add(new Types(row.ItemArray[0].ToString() ));
            }
            return res;
        }
    }
}
