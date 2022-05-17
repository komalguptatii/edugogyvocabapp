// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("1puGvYJKgeM5HmF6LpKxBPFA3lo7zH637Y4pMm+fCk3487d5pAqkbe9d3v3v0tnW9VmXWSjS3t7e2t/cUxXLXdHf+YcoZmrj2Kb/ePJPZfUmNKQuebiZXrlIszsNeqpCKYOiPTjXsdAcEg1CMPuwN9jFAz5jabvAw/dRwvPZHUxU6d8nUbkEyQj6Tqcp+6sZWyCuAaieZ8qQvvEEz1raa2gDxPB17cl9rQP/xDBQRwmhg7tei/hv+6t8jmpmTeDnkGwjOFEi8atd3tDf713e1d1d3t7fXDlqFvo8r1IrfQAngiKm8fgGo0aKRchX4C1pyBBZn0U3SFIYQu6UjT7YCGWnh2WMnhYUA1is2Whw5fXbsv9FpUC5kx81fF4tlzM4Lt3c3t/e");
        private static int[] order = new int[] { 3,9,3,6,6,8,12,12,10,10,10,12,12,13,14 };
        private static int key = 223;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
