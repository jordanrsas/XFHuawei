﻿using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Huawei.Hmf.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Huawei_Xamarin_Forms.Droid
{
    class LastLocationListener:
          Java.Lang.Object,
        IOnSuccessListener,
        IOnFailureListener,
        IOnCanceledListener,
        IOnCompleteListener
    {
        private const string TAG = "LastLocationListener";
        private Action<Location> _onSuccess;
        private Action<Java.Lang.Exception> _onFailure;

        public LastLocationListener(
           Action<Location> onSuccess,
           Action<Java.Lang.Exception> onFailure)
        {
            _onSuccess = onSuccess;
            _onFailure = onFailure;
        }

        public void OnFailure(Java.Lang.Exception exception)
        {
            Log.Info(TAG, "On failure received with {0}", exception?.ToString() ?? "null");
            _onFailure?.Invoke(exception);
        }

        public void OnSuccess(Java.Lang.Object parameter)
        {
            Location location = parameter as Location;

            Log.Info(TAG, "On success received with {0}", location?.ToString() ?? "null");

            _onSuccess?.Invoke(location);
        }

        public void OnCanceled()
        {
            Log.Info(TAG, "On canceled received");
        }

        public void OnComplete(Task task)
        {
            Log.Info(TAG, "On completed received");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _onSuccess = null;
            _onFailure = null;
        }
    }
}