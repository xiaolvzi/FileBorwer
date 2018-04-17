using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Content;
using Android.Runtime;
using Android.Net;
using Android.Util;
using Android.Database;
using Android.Provider;

namespace App14
{
    [Activity(Label = "App14", MainLauncher = true)]
    public class MainActivity : Activity
    {


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var tra = FragmentManager.BeginTransaction();
            tra.Replace(Resource.Id.fl,new Fragment1());
            tra.Commit();
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (resultCode == Result.Ok)
            {
                Uri uri = data.Data;
                Log.Error("uri====",uri.ToString());
                if ("file".Equals(uri.Scheme))
                {
                    Toast.MakeText(this, uri.Path + "11111", ToastLength.Short).Show();
                    return;
                }
                if (Build.VERSION.SdkInt > BuildVersionCodes.Kitkat)
                {   //  >4.4
                    string path = getPath(this, uri);
                    // this will show the uri not file's path
                    //string path = uri.ToString();
                    Toast.MakeText(this, path, ToastLength.Short).Show();
                }
                else
                {  //  <4.4
                    Toast.MakeText(this, getRealPathFromURI(uri) + "222222", ToastLength.Short).Show();
                }
            }


        }
        public string getRealPathFromURI(Uri contentUri)
        {
            string res = null;
            string[] proj = { MediaStore.Images.Media.InterfaceConsts.Data };
            ICursor cursor = ContentResolver.Query(contentUri, proj, null, null, null);
            if (null != cursor && cursor.MoveToFirst())
            {
                ;
                int column_index = cursor.GetColumnIndexOrThrow(MediaStore.Images.Media.InterfaceConsts.Data);
                res = cursor.GetString(column_index);
                cursor.Close();
            }
            return res;
        }
        
         
    public string getPath( Context context,  Uri uri)
        {

             bool isKitKat = Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat;

            // DocumentProvider
            if (isKitKat && DocumentsContract.IsDocumentUri(context, uri))
            {
                // ExternalStorageProvider
                if (isExternalStorageDocument(uri))
                {
                    string docId = DocumentsContract.GetDocumentId(uri);
                    string[] split = docId.Split(':');
                    string type = split[0];

                    if ("primary".Equals(type))
                    {
                        return Environment.ExternalStorageDirectory + "/" + split[1];
                    }
                }
                // DownloadsProvider
                else if (isDownloadsDocument(uri))
                {

                    string id = DocumentsContract.GetDocumentId(uri);
                     Uri contentUri = ContentUris.WithAppendedId(
                            Uri.Parse("content://downloads/public_downloads"), long.Parse(id));

                    return getDataColumn(context, contentUri, null, null);
                }
                // MediaProvider
                else if (isMediaDocument(uri))
                {
                    string docId = DocumentsContract.GetDocumentId(uri);
                    string[] split = docId.Split(':');
                    string type = split[0];

                    Uri contentUri = null;
                    if ("image".Equals(type))
                    {
                        contentUri = MediaStore.Images.Media.ExternalContentUri;
                    }
                    else if ("video".Equals(type))
                    {
                        contentUri = MediaStore.Video.Media.ExternalContentUri;
                    }
                    else if ("audio".Equals(type))
                    {
                        contentUri = MediaStore.Audio.Media.ExternalContentUri;
                    }

                    string selection = "_id=?";
                    string[] selectionArgs = new string[] { split[1] };

                    return getDataColumn(context, contentUri, selection, selectionArgs);
                }
            }
            // MediaStore (and general)
            if ("content".Equals(uri.Scheme))
            {
                //start with "content://", it database's path,
                return getDataColumn(context, uri, null, null);
            }
            // File
            if ("file".Equals(uri.Scheme))
            {
                return uri.Path;
            }
            return null;
        }

        /**
         * Get the value of the data column for this Uri. This is useful for
         * MediaStore Uris, and other file-based ContentProviders.
         *
         * @param context       The context.
         * @param uri           The Uri to query.
         * @param selection     (Optional) Filter used in the query.
         * @param selectionArgs (Optional) Selection arguments used in the query.
         * @return The value of the _data column, which is typically a file path.
         */
        public string getDataColumn(Context context, Uri uri, string selection,
                                    string[] selectionArgs)
        {
            string str = null;
            ICursor cursor = null;
             string column = "_data";
             string[] projection = { column};

            try
            {
                cursor = context.ContentResolver.Query(uri, projection, selection, selectionArgs,
                        null);
                if (cursor != null && cursor.MoveToFirst())
                {
                     int column_index = cursor.GetColumnIndexOrThrow(column);
                    str = cursor.GetString(column_index);
                    return str;
                }
            }
            finally
            {
                if (cursor != null)
                    cursor.Close();
            }
            return str;
        }

        /**
         * @param uri The Uri to check.
         * @return Whether the Uri authority is ExternalStorageProvider.
         */
        public bool isExternalStorageDocument(Uri uri)
        {
            return "com.android.externalstorage.documents".Equals(uri.Authority);
        }

        /**
         * @param uri The Uri to check.
         * @return Whether the Uri authority is DownloadsProvider.
         */
        public bool isDownloadsDocument(Uri uri)
        {
            return "com.android.providers.downloads.documents".Equals(uri.Authority);
        }

        /**
         * @param uri The Uri to check.
         * @return Whether the Uri authority is MediaProvider.
         */
        public bool isMediaDocument(Uri uri)
        {
            return "com.android.providers.media.documents".Equals(uri.Authority);
        }
    }
}

