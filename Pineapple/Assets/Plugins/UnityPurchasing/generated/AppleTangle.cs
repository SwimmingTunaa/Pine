#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class AppleTangle
    {
        private static byte[] data = System.Convert.FromBase64String("JAcqLSkpKy4tOjJEWFhcXxYDA1sjsRHfB2UENuTS4pmVIvVyMPrnEQMcre8qJAcqLSkpKy4uHK2aNq2fmRaB2CMiLL4nnQ06Alj5ECH3TjppUjNgR3y6baXoWE4nPK9tqx+mrZLYX7fC/kgj51VjGPSOEtVU00fkGR4dGBwfGnY7IR8ZHB4cFR4dGBwKHAgqL3koJz8xbVxcQEkMb0leWPUaU+2refWLtZUebtf0+V2yUo1+VhyuLVocIioveTEjLS3TKCgvLi2Hj12+a3957YMDbZ/U189c4cqPYAaqZKrbIS0tKSksHE4dJxwlKi95WERDXkVYVR06HDgqL3koLz8hbVwIzsf9m1zzI2nNC+bdQVTBy5k7O1UMTV9fWUFJXwxNT09JXFhNQk9JfklARU1CT0kMQ0IMWERFXwxPSV4MQ0oMWERJDFhESUIMTVxcQEVPTSgqPy55fx0/HD0qL3koJj8mbVxcKhwjKi95MT8tLdMoKRwvLS3THDErwFEVr6d/DP8U6J2TtmMmR9MH0DO99zJrfMcpwXJVqAHHGo57YHnAmzeRv24IPgbrIzGaYbByT+RnrDtTbYS01f3mSrAIRz38j5fINwbvMwxNQkgMT0leWEVKRU9NWEVDQgxcXk1PWEVPSQxfWE1YSUFJQlhfAhynNaXy1WdA2SuHDhwuxDQS1Hwl/0ujJJgM2+eAAAxDXJoTLRygm2/jEQpLDKYfRtshruPyx48D1X9Gd0jsTx9b2xYrAHrH9iMNIvaWXzVjmTOpr6k3tRFrG96Ft2yiAPidvD70ISolBqpkqtshLS0pKSwvri0tLHBFSkVPTVhFQ0IMbVlYRENeRVhVHQJsittrYVMkchwzKi95MQ8oNBw6nRx0wHYoHqBEn6Mx8klf00tySZDlNV7ZcSL5U3O33gkvlnmjYXEh3WX0WrMfOEmNW7jlAS4vLSwtj64tTkBJDF9YTUJITV5IDFhJXkFfDE11iyklUDtsej0yWP+bpw8Xa4/5QyRyHK4tPSoveTEMKK4tJByuLSgcHD0qL3koJj8mbVxcQEkMZUJPAh0qL3kxIig6KDgH/EVruFol0thHoaw4B/xFa7haJdLYR6ECbIrba2FTubJWIIhrp3f4Ohsf5+gjYeI4Rf0atWABVJvBoLfw31u33lr+Wxxj7VxASQx+Q0NYDG9tHDI7IRwaHBgeW1sCTVxcQEkCT0NBA01cXEBJT01CSAxPQ0JIRVhFQ0JfDENKDFlfSVxASQxvSV5YRUpFT01YRUNCDG1ZSBkPOWc5dTGfuNvasLLjfJbtdHxYRUpFT01YSQxOVQxNQlUMXE1eWAxvbRyuLQ4cISolBqpkqtshLS0tOhw4Ki95KC8/IW1cXEBJDH5DQ1gcriiXHK4vj4wvLi0uLi0uHCEqJSksL64tIywcri0mLq4tLSzIvYUlHxp2HE4dJxwlKi95KCo/Lnl/HT+E8FIOGeYJ+fUj+kf4jggPPduNgAAMT0leWEVKRU9NWEkMXENARU9VQEkMZUJPAh0KHAgqL3koJz8xbVyuLSwqJQaqZKrbT0gpLRyt3hwGKqNfrUzqN3clA76e1Ghk3EwUsjnZfIam+fbI0PwlKxucWVkN");
        private static int[] order = new int[] { 35,22,37,23,25,50,7,10,58,28,24,22,50,53,40,38,41,37,29,51,38,59,37,45,46,41,59,56,57,49,47,37,37,59,51,56,42,52,59,57,47,52,49,48,46,49,48,52,59,57,53,53,56,56,58,58,59,57,59,59,60 };
        private static int key = 44;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
