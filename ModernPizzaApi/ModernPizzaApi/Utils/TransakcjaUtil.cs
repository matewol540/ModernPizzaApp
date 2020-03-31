using ModernPizzaApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernPizzaApi.Utils
{
    public static class TransakcjaUtil
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
    }
}
