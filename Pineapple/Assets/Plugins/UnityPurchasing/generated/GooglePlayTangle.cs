#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("8RJfZFMYUH3YlRwUU+iLAqHurT2DdgRKfkJ5tAOfPgE28b5xjiKDkkxd5TEEVuI9kM0Wnwm3yMOcmPoQMwS7KHKHi0zKVu3DmFtRpn95/lwwWdjtQeHCJRnBmSW3wX+/Gzt2jMWwPAwvOSlWojrZltsHuMwR9qtSEwI0PtuAjNEzqz7R69WzGKfb5nq3BYalt4qBjq0BzwFwioaGhoKHhMuIR7n3XVzPGdM52EboiJopeth9RxIz8SSjhYW0JNjQp+jZxpmzKgUFhoiHtwWGjYUFhoaHGCoBScX6rM68m5iukIWkWQNoAJQhUlrc+Tu2g0Bx0+iOxDYkytvfB4GZpkt61SqGOh6i3vmsWap+9Y33S+TuJAPlPBT+GJzdR+p5uIWEhoeG");
        private static int[] order = new int[] { 4,8,2,9,7,5,6,7,13,9,13,11,12,13,14 };
        private static int key = 135;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
