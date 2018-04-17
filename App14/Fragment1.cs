using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace App14
{
    public class Fragment1 : Fragment, View.IOnClickListener
    {
        public void OnClick(View v)
        {
            Intent intent = new Intent(Intent.ActionGetContent);
            intent.SetType("*/*");
            intent.AddCategory(Intent.CategoryOpenable);
            ((MainActivity)Activity).StartActivityForResult(intent, 1);
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view=inflater.Inflate(Resource.Layout.layout1, container, false);
            Button btn = view.FindViewById<Button>(Resource.Id.btn);
            btn.SetOnClickListener(this);
            return view;

        }
    }
}