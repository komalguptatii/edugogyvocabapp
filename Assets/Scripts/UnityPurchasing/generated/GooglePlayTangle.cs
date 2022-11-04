// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("P0PW/BK/HsjUNFnDd4yS3j4SXFRDKYKXUr9kGatqvmh5Veu2+SXlHqNumWXBXeN6l7d/5xRmdYZzFaCVjpvkQFFFgL8Oj/n9n+5yHuc1cGnN4sq9UpVEO3+9eytIGHXXUHhZbW1RrSupnk5gcF/KFsX2JIt9jiGDwnDz0ML/9PvYdLp0Bf/z8/P38vGYNX/SM7odd3vScqMYZYnUemAdtVPXcD0pyxAXjPli49jc22OrV66XClgDtbNxMCCSgkv1eQ3qwWiaDhX9lwwL8Wlioh1rTflhRrpCusqg9HnH/j4XdJbQ+Qo7iD8ufWaxzMOmYP8ij2wWH/ZcR2OadkKJkoJpJCpw8/3ywnDz+PBw8/PyI0xb+24wLd2C8+/TOTzoP/Dx8/Lz");
        private static int[] order = new int[] { 5,7,13,9,8,13,13,10,9,11,12,11,13,13,14 };
        private static int key = 242;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
