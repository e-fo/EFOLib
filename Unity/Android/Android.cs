#if UNITY_ANDROID

using UnityEngine;


namespace EFO.Unity.Android {

    public static class AndroidApplication {

        private const string _CLASS_NAME = "[EFOLib.cs] --> AndroidApplication.";
        public static bool IsPackageInstalled(string packageName) {
            string methodName = "IsPackageInstalled(): ";
            bool ret = false;

#if UNITY_EDITOR

            Debug.LogFormat(_CLASS_NAME + methodName +
                "You can not check {0} package installation state on editor", packageName);
            ret = true;

#else // UNITY_EDITOR

            AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");

            AndroidJavaObject launchIntent = null;
            //if the app is installed, no errors. Else, doesn't get past next line
            try
            {
                launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", packageName);
                //ca.Call("startActivity",launchIntent);        
                if(launchIntent != null)
                    ret = true;
            }
            catch (Exception ex)
            {
                Debug.Log("exception"+ex.Message);
                ret = false;
            }
#endif // UNITY_EDITOR
            return ret;
        }
    }
}


#endif // UNITY_ANDROID
