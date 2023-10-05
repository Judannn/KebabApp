using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Essentials;

using System.Collections.Generic;
using System.Linq;
using TopKebab.ViewModels;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Android.Locations;
using static Android.Provider.MediaStore.Audio;
using XFGoogleMapSample;
using Location = Xamarin.Essentials.Location;
using TopKebab.Services;
using Org.Json;
using TopKebab.Models;
using static Android.Provider.DocumentsContract;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Java.Util;

namespace TopKebab.Views
{
    public partial class MapPage : ContentPage
    {
        List<MapType> mapTypeValues = new List<MapType>();
        private Position center;
        private KebabShops MapKebabShops { get; set; }

        public string BootLocation { get; set; }

        public MapPage()
        {
            InitializeComponent();
            BindingContext = new MapViewModel();
            EventSubscriptions();
            PrepareMap();
        }

        private void EventSubscriptions()
        {
            // Functionality
            SearchButton.Pressed += async (sender, e) =>
            {
                var p = map.CameraPosition;
                CameraPosition cameraPosition = new CameraPosition(new Position(p.Target.Latitude, p.Target.Longitude), p.Zoom);

                await CreatePins(cameraPosition);
            };

            // On map type change
            pickerMapType.SelectedIndexChanged += (sender, e) =>
            {
                map.MapType = mapTypeValues[pickerMapType.SelectedIndex];
            };
            pickerMapType.SelectedIndex = 0;

            // Map Clicked (Turn this into accessing a kebab store card
            map.MapClicked += (sender, e) =>
            {
                //var lat = e.Point.Latitude.ToString("0.000");
                //var lng = e.Point.Longitude.ToString("0.000");
                //this.DisplayAlert("MapClicked", $"{lat}/{lng}", "CLOSE");
            };

            // Map changed
            map.CameraChanged += async (sender, args) =>
            {
                //var p = args.Position;
                //CameraPosition cameraPosition = new CameraPosition(new Position(p.Target.Latitude,p.Target.Longitude),p.Zoom);

                //await CreatePins(cameraPosition);

                //this.DisplayAlert("MapClicked", $"Lat={p.Target.Latitude:0.00}, Long={p.Target.Longitude:0.00}, Zoom={p.Zoom:0.00}, Bearing={p.Bearing:0.00}, Tilt={p.Tilt:0.00}", "CLOSE");
            };
        }

        async void PrepareMap()
        {

            await RequestLocationPermission();

            // MapTypes
            foreach (var mapType in Enum.GetValues(typeof(MapType)))
            {
                mapTypeValues.Add((MapType)mapType);
                pickerMapType.Items.Add(Enum.GetName(typeof(MapType), mapType));
            }

            // Enable Compass
            map.UiSettings.CompassEnabled = true;

            // Enable RotateGestures
            map.UiSettings.RotateGesturesEnabled = true;

            // Enable Scroll Gestures
            map.UiSettings.ScrollGesturesEnabled = true;

            // Enable MyLocation
            map.UiSettings.MyLocationButtonEnabled = true;

            // Enable Tilt Gestures
            map.UiSettings.TiltGesturesEnabled = true;

            // Enable Zoom Gestures
            map.UiSettings.ZoomGesturesEnabled = true;

            // Disable Zoom Control Button
            map.UiSettings.ZoomControlsEnabled = false;

        }

        async Task CreatePins(CameraPosition cameraPosition)
        {

            await APISearch(cameraPosition);

            map.Pins.Clear();

            foreach (KebabShops.Result kebabShop in MapKebabShops.results)
            {
                map.Pins.Add(new Pin()
                {
                    Type = PinType.Place,
                    Label = kebabShop.name,
                    Address = kebabShop.vicinity,
                    Position = new Position(kebabShop.geometry.location.lat, kebabShop.geometry.location.lng),
                    Rotation = 0,
                    Tag = "id_tokyo",
                    IsVisible = true
                });
            }
        }

        async Task APISearch(CameraPosition cameraPosition)
        {
            if(await CheckPermissions())
            {
                // Convert lat or lng from decimal degrees into radians (divide by 57.2958)
                var lat2 = map.Region.NearLeft.Latitude;
                var lon2 = map.Region.NearLeft.Longitude;
                var lat1 = cameraPosition.Target.Latitude;
                var lon1 = cameraPosition.Target.Longitude;

                // distance = circle radius from center to Northeast corner of bounds
                var dis = distance(lat1, lat2, lon1, lon2);

                string keyword = "kebab";
                string location = $"{cameraPosition.Target.Latitude}%2C{cameraPosition.Target.Longitude}";
                string radius = $"{dis}";
                string type = "resteraunt";
                string rankby = "distance";
                string key = Variables.GOOGLE_MAPS_ANDROID_API_KEY;
                string request = $"?keyword={keyword}&location={location}&type={type}&rankby={rankby}&key={key}";

                HttpResponseMessage response = await HTTPClient.sharedClient.GetAsync(request);
                response.EnsureSuccessStatusCode();
                HTTPClient.WriteRequestToConsole(response);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"{jsonResponse}\n");
                MapKebabShops = JsonConvert.DeserializeObject<KebabShops>(jsonResponse);
            }
        }

        static double toRadians(double angleIn10thofaDegree)
        {
            // Angle in 10th
            // of a degree
            return (angleIn10thofaDegree * Math.PI) / 180;
        }

        static double distance(double lat1,double lat2,double lon1,double lon2)
        {

            // The math module contains
            // a function named toRadians
            // which converts from degrees
            // to radians.
            lon1 = toRadians(lon1);
            lon2 = toRadians(lon2);
            lat1 = toRadians(lat1);
            lat2 = toRadians(lat2);

            // Haversine formula
            double dlon = lon2 - lon1;
            double dlat = lat2 - lat1;
            double a = Math.Pow(Math.Sin(dlat / 2), 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(dlon / 2), 2);
            double c = 2 * Math.Asin(Math.Sqrt(a));

            // Radius of earth in
            // kilometers. Use 3956
            // for miles
            double r = 4000000;

            // calculate the result
            return (c * r);
        }

        async Task<bool> CheckPermissions()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status == PermissionStatus.Granted)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        async Task RequestLocationPermission()
        {
            // Request Location
            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                //await this.DisplayAlert("Permission Error", "This feature needs Location permission.", "CLOSE");
            }
            else
            {
                map.MyLocationEnabled = true;
                Position position = await GetUserLocation();
                map.InitialCameraUpdate = new CameraUpdate(new CameraPosition(position, 12));
                ZoomToUser();
            }
        }

        async Task<Position> GetUserLocation()
        {
            Location location = await Geolocation.GetLastKnownLocationAsync();
            return new Position(location.Latitude, location.Longitude);
        }

        private async void ZoomToUser()
        {
            if (await CheckPermissions())
            {
                Position position = await GetUserLocation();
                await map.MoveCamera(new CameraUpdate(new CameraPosition(position, 12)));
            }
        }
    }
}