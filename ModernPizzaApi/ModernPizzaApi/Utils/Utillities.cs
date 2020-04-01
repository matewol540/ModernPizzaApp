using ModernPizzaApi.Models;
using System;
using System.Text;

namespace ModernPizzaApi.Utils
{
    public static class Utillities
    {

        public static double ObliczPodatekPrzedmiotu(IPrzedmiotMenu przedmiot)
        {
            if (przedmiot.RodzajPodatku == Enums.RodzajPodatku.Napoje)
                return Math.Round(przedmiot.Cena * 0.23, 2);
            else if (przedmiot.RodzajPodatku == Enums.RodzajPodatku.Żywnosc)
                return Math.Round(przedmiot.Cena * 0.08, 2);
            else
                return 0;
        }
        public static String getHexGuid()
        {
            byte[] ba = Encoding.Default.GetBytes(Guid.NewGuid().ToString());
            var hexString = BitConverter.ToString(ba);
            return hexString.Replace("-", "").Remove(24);
        }
    }
}
