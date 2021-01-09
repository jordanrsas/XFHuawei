using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Huawei.Hms.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Huawei_Xamarin_Forms.Droid
{
    class DelegateLocationCallback : LocationCallback
    {
        private Action<LocationResult> _onLocationResult;

        public DelegateLocationCallback(Action<LocationResult> onLocationResult)
        {
            _onLocationResult = onLocationResult;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _onLocationResult = null;
        }

        public override void OnLocationResult(LocationResult locationResult)
        {
            _onLocationResult?.Invoke(locationResult);
        }
    }
}