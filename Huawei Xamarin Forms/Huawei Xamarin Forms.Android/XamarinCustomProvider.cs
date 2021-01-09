using Android.App;
using Android.Content;
using Android.Database;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Huawei.Agconnect.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Huawei_Xamarin_Forms.Droid
{
    [ContentProvider(new string[] { "com.hmscore.mapdemo.XamarinCustomProvider" })]
    class XamarinCustomProvider : ContentProvider
    {
        public override int Delete(Android.Net.Uri uri, string selection, string[] selectionArgs)
        {
            throw new NotImplementedException();
        }

        public override string GetType(Android.Net.Uri uri)
        {
            throw new NotImplementedException();
        }

        public override Android.Net.Uri Insert(Android.Net.Uri uri, ContentValues values)
        {
            throw new NotImplementedException();
        }

        public override bool OnCreate()
        {
            AGConnectServicesConfig config = AGConnectServicesConfig.FromContext(Context);
            config.OverlayWith(new HmsLazyInputStream(Context));

            return false;
        }

        public override ICursor Query(Android.Net.Uri uri, string[] projection, string selection, string[] selectionArgs, string sortOrder)
        {
            throw new NotImplementedException();
        }

        public override int Update(Android.Net.Uri uri, ContentValues values, string selection, string[] selectionArgs)
        {
            throw new NotImplementedException();
        }
    }
}