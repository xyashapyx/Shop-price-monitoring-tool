using System;

namespace PriceMonitoring.Helpers
{
    public static class ByteArrayToBase64Convertor
    {
        public static string ConvertToBase64(byte[] imageBytes)
        {
            string imreBase64Data = Convert.ToBase64String(imageBytes);
            return $"data:image/png;base64,{imreBase64Data}";
        }
    }
}