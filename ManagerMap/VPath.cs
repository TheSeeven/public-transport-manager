using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerMap
{
    class VPath
    {
        public string from { get; set; }
        public string to { get; set; }
        VPath() { }

        VPath(string From, string To)
        {
            from = From;
            to = To;
        }

        public static ObservableCollection<VPath> get_VPath()
        {
            ObservableCollection<VPath> res = new ObservableCollection<VPath>();
            foreach (DataRow row in MainPage.VehiclePath)
            {
                res.Add(new VPath(row.ItemArray[0].ToString(),row.ItemArray[1].ToString()));
            }
            return res;
        }
    }
}
