using System;
using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using Android.App;
using Android.Content;
using Android.OS;
using Huawei_Xamarin_Forms;
using Huawei_Xamarin_Forms.Droid;
using Huawei.Hms.Maps;
using Huawei.Hms.Maps.Model;
using Android.Util;
using Android.Widget;

[assembly: ExportRenderer(typeof(CustomMapControl), typeof(MapRenderer))]
namespace Huawei_Xamarin_Forms.Droid
{
    [Obsolete]
    public partial class MapRenderer : 
        ViewRenderer<CustomMapControl, MapView>,
        HuaweiMap.IOnMapLoadedCallback, 
        IOnMapReadyCallback
    {
        private const string TAG = nameof(MapRenderer);
        protected MapView NativeMap => Control;
        protected CustomMapControl Map => Element;
        
        static Bundle s_bundle;
        private Context _context;

        private HuaweiMap _hMap;
        private MapView _mapView;

        internal static Bundle Bundle
        {
            set { s_bundle = value; }
        }

        public MapRenderer(Context context): base(context)
        {
            _context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CustomMapControl> e)
        {
            base.OnElementChanged(e);
            if(null == Control)
            {
                var mapView = new MapView(Context);
                mapView.OnCreate(s_bundle);
                mapView.OnResume();
                SetNativeControl(mapView);
            }

            if (null != e.OldElement)
            {
                MapView oldMapView = Control;
                oldMapView.OnDestroy();
            }

            if (null != e.NewElement)
            {
                Control.GetMapAsync(this);
            }
        }

        public void OnMapLoaded()
        {
            throw new NotImplementedException();
        }

        public void OnMapReady(HuaweiMap hMap)
        {
            if (hMap == null)
            {
                return;
            }
            Log.Info(TAG, "OnMapReady.");
            _hMap = hMap;

            _hMap.MapType = HuaweiMap.MapTypeNormal;
            _hMap.UiSettings.MyLocationButtonEnabled = true;
            _hMap.UiSettings.ZoomControlsEnabled = true;
            _hMap.UiSettings.CompassEnabled = true;
            _hMap.UiSettings.MyLocationButtonEnabled = true;

            Toast.MakeText(_context, "OnMapReady done.", ToastLength.Short).Show();

            _hMap.MyLocationEnabled = true;
            LatLng PARIS = new LatLng(48.893478, 2.334595);
            _hMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(48.893478, 2.334595), 14));
            Marker mParis =
            _hMap.AddMarker(new MarkerOptions().Position(PARIS).Title("paris").Snippet("hello").Clusterable(true));
            
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("OnElementPropertyChanged: " + e.PropertyName);
            base.OnElementPropertyChanged(sender, e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(null != Element)
                {

                }
                NativeMap.OnDestroy();
            }
            base.Dispose(disposing);
        }


    }
}