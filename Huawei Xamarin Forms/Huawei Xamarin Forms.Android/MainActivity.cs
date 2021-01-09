using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Huawei.Hms.Maps;
using AndroidX.Core.App;
using Huawei.Hms.Location;
using Huawei.Hmf.Tasks;
using Huawei.Hms.Maps.Model;
using Huawei.Hms.Api;
using AndroidX.Core.Content;

namespace Huawei_Xamarin_Forms.Droid
{
    [Activity(Label = "Huawei_Xamarin_Forms", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity,
        IOnSuccessListener, IOnFailureListener, IOnCompleteListener
    {
        private const string TAG = nameof(MainActivity);
       
        private LatLng _latLng;
        private FusedLocationProviderClient _fusedLocationProviderClient;
        private LastLocationListener _fusedLocationProviderClientLastLocationListener;
        private DelegateLocationCallback _locationCallback;

        private static readonly string[] Permissions = {
                Android.Manifest.Permission.AccessCoarseLocation,
                Android.Manifest.Permission.AccessFineLocation,
                Android.Manifest.Permission.Internet };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            if (HuaweiApiAvailability.Instance.IsHuaweiMobileServicesAvailable(this) != ConnectionResult.Success)
                return;
               
            //ActivityCompat.RequestPermissions(this, permissions, 100);

            // Initialise huawei fused location provider
            _fusedLocationProviderClientLastLocationListener = new LastLocationListener(location =>
            {
                switch (location)
                {
                    case null:
                        GetLocation();
                        break;
                    default:
                        Toast.MakeText(this, location.ToString(), ToastLength.Long).Show();
                        //MoveToUserLocation(new LatLng(location.Latitude, location.Longitude));
                        break;
                }
            }, exception =>
            {
                Toast.MakeText(this, exception.Message, ToastLength.Long).Show();
            });

            _fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(this);

            Android.Util.Log.Info(TAG, "CheckPermission");

            // Get last known location if permissions are granted
            if (CheckPermission(Permissions, 100))
            {
                GetLastLocation();
            }

            MapsInitializer.Initialize(this);

        }

        private void GetLastLocation()
        {
            Android.Util.Log.Info(TAG, "GetLastLocation");
            var client = _fusedLocationProviderClient.GetLastLocation();
            if (client == null)
                Android.Util.Log.Info(TAG, "GetLastLocation : Client is null");

            client?.AddOnSuccessListener(_fusedLocationProviderClientLastLocationListener);
            client?.AddOnFailureListener(_fusedLocationProviderClientLastLocationListener);
            Android.Util.Log.Info(TAG, "GetLastLocation - End");
        }

        public void GetLocation()
        {
            LocationRequest locationRequest = new LocationRequest();
            locationRequest.SetInterval(1000).SetPriority(LocationRequest.PriorityHighAccuracy);

            _locationCallback = new DelegateLocationCallback(locationResult =>
            {
                if (locationResult != null && locationResult.HWLocationList.Count > 0)
                {
                    var location = locationResult.HWLocationList[0];
                    Toast.MakeText(this, location.ToString(), ToastLength.Long).Show();
                    //MoveToUserLocation(new LatLng(location.Latitude, location.Longitude));
                    _fusedLocationProviderClient.RemoveLocationUpdates(_locationCallback);
                }
            });

            _fusedLocationProviderClient.RequestLocationUpdates(locationRequest, _locationCallback, MainLooper);
        }

        public void OnSuccess(Java.Lang.Object p0)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(Java.Lang.Exception p0)
        {
            throw new NotImplementedException();
        }

        public void OnComplete(Task p0)
        {
            throw new NotImplementedException();
        }

        #region Permissions
        public bool CheckPermission(string[] permissions, int requestCode)
        {
            var hasAllPermissions = true;
            foreach (string permission in permissions)
            {
                if (ContextCompat.CheckSelfPermission(this, permission) == Permission.Denied)
                {
                    hasAllPermissions = false;
                    ActivityCompat.RequestPermissions(this, permissions, requestCode);
                }
            }

            return hasAllPermissions;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            bool hasAllPermissions = true;
            for (int i = 0; i < permissions.Length; i++)
            {
                if (grantResults[i] == Permission.Denied)
                {
                    hasAllPermissions = false;
                    break;
                }
            }


            if (hasAllPermissions)
            {
                GetLastLocation();
            }
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _fusedLocationProviderClientLastLocationListener?.Dispose();
            _fusedLocationProviderClientLastLocationListener = null;

            _fusedLocationProviderClient?.Dispose();
            _fusedLocationProviderClient = null;

            _latLng?.Dispose();
            _latLng = null;

        }
    }
}