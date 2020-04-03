using ModernPizzaApi.Models;
using System;
using System.Text;

namespace ModernPizzaApi.Utils
{
    public static class Utillities
    {
        
        public static String getHexGuid()
        {
            byte[] ba = Encoding.Default.GetBytes(Guid.NewGuid().ToString());
            var hexString = BitConverter.ToString(ba);
            return hexString.Replace("-", "").Remove(24);
        }

        internal static string PobierNowyKodWejscia()
        {
            byte[] ba = Encoding.Default.GetBytes(Guid.NewGuid().ToString());
            var hexString = BitConverter.ToString(ba);
            return hexString.Replace("-", "").Remove(8);
        }
    }
}
