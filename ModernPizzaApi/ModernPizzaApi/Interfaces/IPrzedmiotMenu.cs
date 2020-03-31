using ModernPizzaApi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModernPizzaApi.Models
{
    public interface IPrzedmiotMenu
    {
        public double Nazwa { get; set; }
        public double Cena { get; set; }
        public RodzajPodatku RodzajPodatku { get; set; }
    }
}
