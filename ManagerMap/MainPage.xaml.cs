using System;
using System.Collections.Generic;
using System.Data;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using System.Threading;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ManagerMap
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Timer timer;
        bool UnassignedStationsON = false;
        bool tappingON = false;
        double lat = 0;
        double lng = 0;
        string type;
        string vname;
        public static List<DataRow> VehicleList = new List<DataRow>(Vehicle.get_Vehicles());
        public static List<DataRow> VehicleStations = new List<DataRow>();
        public static List<DataRow> VehiclePath = new List<DataRow>(Path.get_path());
        public static List<DataRow> VehicleType = new List<DataRow>(Type.get_types());
        public static List<DataRow> UnassignedStations = new List<DataRow>(VehicleStation.unassigned_stations());
        public MainPage()
        {
            InitializeComponent();

            ListPath.ItemsSource = VPath.get_VPath();
            ListType.ItemsSource = Types.getType();
            MapControl1.StyleSheet = MapStyleSheet.RoadDark();
            BasicGeoposition cityPosition = new BasicGeoposition() { Latitude = 45.756646, Longitude = 21.228666 };
            Geopoint cityCenter = new Geopoint(cityPosition);
            MapControl1.Center = cityCenter;
            MapControl1.ZoomLevel = 15;
            MapControl1.LandmarksVisible = true;
            MapControl1.MapTapped += MyMap_MapTapped;
            
        }
        
        private void MyMap_MapTapped(Windows.UI.Xaml.Controls.Maps.MapControl sender, Windows.UI.Xaml.Controls.Maps.MapInputEventArgs args)
        {
            if (tappingON == true)
            {
                string nume = Station_Name.Text;
                var MyLandmarks = new List<MapElement>();
                var tappedGeoPosition = args.Location.Position;
                string status = "MapTapped at \nLatitude:" + tappedGeoPosition.Latitude + "\nLongitude: " + tappedGeoPosition.Longitude;
                lat = tappedGeoPosition.Latitude;
                lng = tappedGeoPosition.Longitude;

                BasicGeoposition snPosition = new BasicGeoposition { Latitude = tappedGeoPosition.Latitude, Longitude = tappedGeoPosition.Longitude };
                
                Geopoint snPoint = new Geopoint(snPosition);
                
                var spaceNeedleIcon = new MapIcon
                {
                    Location = snPoint,
                    NormalizedAnchorPoint = new Point(0.5, 1.0),
                    ZIndex = 0,
                    Title = nume
                };
                
                MapControl1.MapElements.Add(spaceNeedleIcon);
            }
        }

        private async void timerCallback(object state)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                StationValid.Visibility = Visibility.Collapsed;
                TextID.Visibility = Visibility.Collapsed;
                DeletStat.Visibility = Visibility.Collapsed;
                DeletVeh.Visibility = Visibility.Collapsed;
                PathBlock.Visibility = Visibility.Collapsed;
                DelType.Visibility = Visibility.Collapsed;
                InsType.Visibility = Visibility.Collapsed;
                DeleteStatVeh.Visibility = Visibility.Collapsed;
                insertStatVeh.Visibility = Visibility.Collapsed;
                DeletePathWrong.Visibility = Visibility.Collapsed;
            });
           
                
        }
        // Insert Station //
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string Name_Statie = Station_Name.Text;
            string Type_Statie  = type ;
            string Lat_Statie = lat.ToString();
            string Long_Statie = lng.ToString();
            if (type != null && Name_Statie != null && Name_Statie != "" && double.TryParse(Lat_Statie, out _) && double.TryParse(Long_Statie, out _) && lat !=0 && lng!=0)
            {
                

                Station.insert(Name_Statie, Lat_Statie, Long_Statie, Type_Statie);
                StationValid.Text = "Station was inserted";
                StationValid.Foreground = new SolidColorBrush(Colors.LightGreen);
                StationValid.Visibility = Visibility.Visible;
                Station_Name.Text = "";
                MapControl1.MapElements.Clear();
                

            }
            else
            {
                StationValid.Text = "No type was selected or Name was not declared";
                StationValid.Foreground = new SolidColorBrush(Colors.OrangeRed);

                StationValid.Visibility = Visibility.Visible;

                
            }

            timer = new Timer(timerCallback, null, (int)TimeSpan.FromMilliseconds(5000).TotalMilliseconds, Timeout.Infinite);
            tappingON = false;
            Activation.Text = "Selection on map is disabeld";
            Activation.Foreground = new SolidColorBrush(Colors.LightGreen);
            lat = 0;
            lng = 0;
        }

        // Activate selecting location on map //
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            if (tappingON == false)
            {
                tappingON = true;
                Activation.Text = "Selection on map is active";
                Activation.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                tappingON = false;
                Activation.Text = "Selection on map is disabeld";
                Activation.Foreground = new SolidColorBrush(Colors.LightGreen); 
            }
            
        }

        // Insert Vehicle //
        private void AddVheiclie_Click(object sender, RoutedEventArgs e)
        {
            string VehicleType = type;
            string VehicleName;
            VehicleName = VehName.Text;
            
            if (VehID.Text != null && VehicleName != null && VehicleType != null && type != null && int.TryParse(VehID.Text, out _) && VehicleName != "" && VehicleName.Length<5) 
            {
                
                bool x = Vehicle.insert(Convert.ToInt32(VehID.Text), VehicleName, VehicleType);
                if (x == false)
                {
                    TextID.Text = " The id is already used, try: " + Vehicle.get_id().ToString();
                    TextID.Foreground = new SolidColorBrush(Colors.OrangeRed);
                    TextID.Visibility = Visibility.Visible;
                }
                else
                {
                    
                    TextID.Text = "Vehicle was inserted";
                    TextID.Foreground = new SolidColorBrush(Colors.LightGreen);
                    TextID.Visibility = Visibility.Visible;
                }
                VehicleList = new List<DataRow>(Vehicle.get_Vehicles());
                ListVeh.ItemsSource = Veh.getVeh(type);
                VehName.Text = "";
                VehID.Text = "";
            }
            else
            {
                TextID.Text = "The name must be less then 5 characters, you have to select a type and the id must be a number ";
                TextID.Foreground = new SolidColorBrush(Colors.OrangeRed);
                TextID.Visibility = Visibility.Visible;
            }
            timer = new Timer(timerCallback, null, (int)TimeSpan.FromMilliseconds(5000).TotalMilliseconds, Timeout.Infinite);
        }

        // Delete Station //
        private void DeleteStation_Click(object sender, RoutedEventArgs e)
        {
            MapControl1.MapElements.Clear();
            if (Station_Id.Text != null && int.TryParse(Station_Id.Text, out _))
            {
                Station.delete(Convert.ToInt32(Station_Id.Text));
                if (vname != null)
                {
                    VehicleStations = new List<DataRow>(Station.get_vehicle_stations(vname));
                    MarkerPlacement(VehicleStations);
                    
                }
                Station_Id.Text = "";
                DeletStat.Text = "Station was deleted";
                DeletStat.Foreground = new SolidColorBrush(Colors.LightGreen);
                DeletStat.Visibility = Visibility.Visible;
                VehiclePath = new List<DataRow>(Path.get_path());
                ListPath.ItemsSource = VPath.get_VPath();
            }
            else
            {
                DeletStat.Text = "Wrong insert, the id must be number";
                DeletStat.Foreground = new SolidColorBrush(Colors.OrangeRed);
                DeletStat.Visibility = Visibility.Visible;
            }
            timer = new Timer(timerCallback, null, (int)TimeSpan.FromMilliseconds(5000).TotalMilliseconds, Timeout.Infinite);
        }

        // Delete Vehicle //
        private void DeleteVehicle_Click(object sender, RoutedEventArgs e)
        {
            if (VehicleID.Text != null && int.TryParse(VehicleID.Text, out _))
            {
                Vehicle.delete(Convert.ToInt32(VehicleID.Text));
                VehicleList = new List<DataRow>(Vehicle.get_Vehicles());
                ListVeh.ItemsSource = Veh.getVeh(type);
                VehicleID.Text = "";
                DeletVeh.Text = "Vehicle was deleted";
                DeletVeh.Foreground = new SolidColorBrush(Colors.LightGreen);
                DeletVeh.Visibility = Visibility.Visible;
            }
            else
            {
                DeletVeh.Text = "Wrong insert, the id must be number";
                DeletVeh.Foreground = new SolidColorBrush(Colors.OrangeRed);
                DeletVeh.Visibility = Visibility.Visible;
            }

            timer = new Timer(timerCallback, null, (int)TimeSpan.FromMilliseconds(5000).TotalMilliseconds, Timeout.Infinite);
        }

        // Insert Path //
        private void Inser_Path_Click(object sender, RoutedEventArgs e)
        {
            if (Station_From.Text != null && Station_To.Text != null && int.TryParse(Station_From.Text, out _) && int.TryParse(Station_To.Text, out _))
            {
                bool x = Path.insert(Convert.ToInt32(Station_From.Text), Convert.ToInt32(Station_To.Text));
                if (x == false)
                {
                    PathBlock.Text = "Path already exists or the id was incorrect";
                    PathBlock.Foreground = new SolidColorBrush(Colors.OrangeRed);
                    PathBlock.Visibility = Visibility.Visible;
                }
                else
                {
                    PathBlock.Text = "Path was inserted";
                    PathBlock.Foreground = new SolidColorBrush(Colors.LightGreen);
                    PathBlock.Visibility = Visibility.Visible;
                }
                Station_From.Text = "";
                Station_To.Text = "";
                VehiclePath = new List<DataRow>(Path.get_path());
                ListPath.ItemsSource = VPath.get_VPath();
            }
            else
            {
                PathBlock.Text = "Path already exists or the id was incorrect";
                PathBlock.Foreground = new SolidColorBrush(Colors.OrangeRed);
                PathBlock.Visibility = Visibility.Visible;
            }
            timer = new Timer(timerCallback, null, (int)TimeSpan.FromMilliseconds(5000).TotalMilliseconds, Timeout.Infinite);
        }

        // Delete Path //
        private void Delete_Path_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(Station_FromD.Text, out _) && int.TryParse(Station_ToD.Text, out _) && Station_FromD.Text != null && Station_ToD.Text != null)
            {
                Path.delete(Convert.ToInt32(Station_FromD.Text), Convert.ToInt32(Station_ToD.Text));
                DeletePathWrong.Text = "Path was deleted";
                DeletePathWrong.Foreground = new SolidColorBrush(Colors.LightGreen);
                DeletePathWrong.Visibility = Visibility.Visible;
                Station_FromD.Text = "";
                Station_ToD.Text = "";
                VehiclePath = new List<DataRow>(Path.get_path());
                ListPath.ItemsSource = VPath.get_VPath();
            }
            else
            {
                DeletePathWrong.Text = "Wrong insert, the id must be number@or the station dose not exist";
                DeletePathWrong.Text = DeletePathWrong.Text.Replace("@", System.Environment.NewLine);
                DeletePathWrong.Foreground = new SolidColorBrush(Colors.OrangeRed);
                DeletePathWrong.Visibility = Visibility.Visible;

            }
            timer = new Timer(timerCallback, null, (int)TimeSpan.FromMilliseconds(5000).TotalMilliseconds, Timeout.Infinite);
        }

        // Selecting the Type //
        public void Item_Tapped(object sender, TappedRoutedEventArgs e)
        {

            type = ((TextBlock)sender).Text;
            ListVeh.ItemsSource = Veh.getVeh(type);
            TypeLabel.Text = $"Selected type:{((TextBlock)sender).Text}";
            //if (((SolidColorBrush)(((TextBlock)sender).Foreground)).Color == (new SolidColorBrush(Colors.White)).Color)
            //{
            //    ((TextBlock)sender).Foreground = new SolidColorBrush(Colors.Red);
            //}
            //else ((TextBlock)sender).Foreground = new SolidColorBrush(Colors.White);
        }

        // Showing the Stations of a Vehicle on map //
        public void Item_Name_Stations(object sender, TappedRoutedEventArgs e)
        {
            MapControl1.MapElements.Clear();
            vname = ((TextBlock)sender).Text.ToString();
            VehicleStations = new List<DataRow>(Station.get_vehicle_stations(vname));
            MarkerPlacement(VehicleStations);
        }

        // Inserting a station to a Vehicle //
        public void Insert_Station_Vehicle(object sender, RoutedEventArgs e)
        {
            if (NameInsert.Text != null && IdInsert.Text != null && int.TryParse(IdInsert.Text, out _) && NameInsert.Text != "")
            {
                VehicleStation.insert(NameInsert.Text, Convert.ToInt32(IdInsert.Text));
                insertStatVeh.Text = "Station was inserted to vehicle";
                insertStatVeh.Foreground = new SolidColorBrush(Colors.LightGreen);
                insertStatVeh.Visibility = Visibility.Visible;
                
                UnassignedStations = new List<DataRow>(VehicleStation.unassigned_stations());
                MarkerPlacement(UnassignedStations);
                NameInsert.Text = "";
                IdInsert.Text = "";
            }
            else
            {
                insertStatVeh.Text = "Wrong insert, the id must be number@or the station,vehicle name dose not exist";
                insertStatVeh.Text = insertStatVeh.Text.Replace("@",System.Environment.NewLine);
                insertStatVeh.Foreground = new SolidColorBrush(Colors.OrangeRed);
                insertStatVeh.Visibility = Visibility.Visible;
            }
            timer = new Timer(timerCallback, null, (int)TimeSpan.FromMilliseconds(5000).TotalMilliseconds, Timeout.Infinite);
        }

        // Deleting a station of a Vehicle //
        public void Delete_Station_Vehicle(object sender, RoutedEventArgs e)
        {
            if (NameDelete.Text != null && IdDelete.Text != null && int.TryParse(IdDelete.Text, out _) && NameDelete.Text != "")
            {
                VehicleStation.delete(NameDelete.Text, Convert.ToInt32(IdDelete.Text));
                DeleteStatVeh.Text = "Vehicle station was deleted";
                DeleteStatVeh.Foreground = new SolidColorBrush(Colors.LightGreen);
                DeleteStatVeh.Visibility = Visibility.Visible;
                VehicleStations = new List<DataRow>(Station.get_vehicle_stations(vname));
                MarkerPlacement(VehicleStations);
                NameDelete.Text = "";
                IdDelete.Text = "";
            }
            else
            {
                DeleteStatVeh.Text = "Wrong insert, the id must be number@or the station,vehicle name dose not exist";
                DeleteStatVeh.Text = DeleteStatVeh.Text.Replace("@",System.Environment.NewLine);
                DeleteStatVeh.Foreground = new SolidColorBrush(Colors.OrangeRed);
                DeleteStatVeh.Visibility = Visibility.Visible;
                
            }
            timer = new Timer(timerCallback, null, (int)TimeSpan.FromMilliseconds(5000).TotalMilliseconds, Timeout.Infinite);
        }

        // Add a Type //
        public void Add_Type_Click(object sender, RoutedEventArgs e)
        {
            if (TypeNameInsert.Text != null && TypeNameInsert.Text.Length<5 && TypeNameInsert.Text != "")
            {
                if (SwitchType.IsOn)
                {
                    Type.insert(TypeNameInsert.Text, 1);
                }
                else { Type.insert(TypeNameInsert.Text, 0); }
                InsType.Text = "Type was inserted";
                InsType.Foreground = new SolidColorBrush(Colors.LightGreen);
                InsType.Visibility = Visibility.Visible;
                TypeNameInsert.Text = "";
                VehicleType = new List<DataRow>(Type.get_types());
                ListType.ItemsSource = Types.getType();
                
            }
            else
            {
                InsType.Text = "Wrong type name";
                InsType.Foreground = new SolidColorBrush(Colors.OrangeRed);
                InsType.Visibility = Visibility.Visible;
               
            }
            timer = new Timer(timerCallback, null, (int)TimeSpan.FromMilliseconds(5000).TotalMilliseconds, Timeout.Infinite);
        }
        
        // Delete a Type //
        public void Delete_Type_Click(object sender, RoutedEventArgs e)
        {
            if (TypeNameDelete.Text != null && TypeNameDelete.Text.Length < 5 && TypeNameDelete.Text != "")
            {
                Type.delete(TypeNameDelete.Text);
                DelType.Text = "Type was deleted";
                DelType.Foreground = new SolidColorBrush(Colors.LightGreen);
                DelType.Visibility = Visibility.Visible;
                TypeNameDelete.Text = "";
                VehicleType = new List<DataRow>(Type.get_types());
                ListType.ItemsSource = Types.getType();
                VehiclePath = new List<DataRow>(Path.get_path());
                ListPath.ItemsSource = VPath.get_VPath();
            }
            else
            {
                DelType.Text = "Wrong type name";
                DelType.Foreground = new SolidColorBrush(Colors.OrangeRed);
                DelType.Visibility = Visibility.Visible;

            }
           timer = new Timer(timerCallback, null, (int)TimeSpan.FromMilliseconds(5000).TotalMilliseconds, Timeout.Infinite);
        }

        // Show the Stations that have no vehicle assigned //
        public void Unassigned_Stations_Click(object sender, RoutedEventArgs e)
        {
            if (UnassignedStationsON == false)
            {
                UnassignedStations = new List<DataRow>(VehicleStation.unassigned_stations());
                MarkerPlacement(UnassignedStations);
                UnassignedStationsON = true;
                ButtonUnisigned.Content = "Hide Stations Whitout a Vehicle";
            }
            else
            {
                MapControl1.MapElements.Clear();
                UnassignedStationsON = false;
                ButtonUnisigned.Content = "Show Stations Whitout a Vehicle";
            }
        }

        // Basic Function for showing the markers on map //
        private void MarkerPlacement(List<DataRow> lista)
        {
            MapControl1.MapElements.Clear();
            
            foreach (DataRow row in lista)
            {
                string nume = row.ItemArray[0].ToString() + " : " + row.ItemArray[1].ToString();
                var MyLandmarks = new List<MapElement>();
                BasicGeoposition station_pos = new BasicGeoposition { Latitude = Convert.ToDouble(row.ItemArray[2]), Longitude = Convert.ToDouble(row.ItemArray[3]) };
                Geopoint snPoint = new Geopoint(station_pos);
                var spaceNeedleIcon = new MapIcon
                {
                    Location = snPoint,
                    NormalizedAnchorPoint = new Point(0.5, 1.0),
                    ZIndex = 0,
                    Title = nume
                };


                MapControl1.MapElements.Add(spaceNeedleIcon);
            }
        }

        private void SwitchType_Toggled(object sender, RoutedEventArgs e)
        {

        }
    }
}

