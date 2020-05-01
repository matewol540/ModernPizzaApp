using System;
using System.Collections.Generic;
using System.Text;

namespace MobilePizzaApp.ApiConnector
{
    public static class Constants
    {
        public const String ApiUrl = "https://192.168.0.24:45455/";

        public static String ConnectionApiUri
        {
            get
            {
                return ApiUrl + "pizzamain";
            }
        }
        public static String ConnectionApiUriArtykul
        {
            get
            {
                return ApiUrl + "artykul/";
            }
        }
    }
}
