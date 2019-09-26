
namespace EFO.Unity.Network {
    public static class NetworkTools {

        public static bool IsNetworkReachable()
        {
#if false
            return Application.internetReachability != NetworkReachability.NotReachable;
#endif
            return true;
        }
    }
}
