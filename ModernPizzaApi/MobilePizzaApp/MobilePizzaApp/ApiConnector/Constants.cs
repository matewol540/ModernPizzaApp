using System;
using System.Collections.Generic;
using System.Text;

namespace MobilePizzaApp.ApiConnector
{
    public static class Constants
    {
        private const String ApiUrl = "https://modernpizzaapi.azurewebsites.net/";

        public static String ConnectionApiUriPizza
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
        public static String ConnectionApiUriUser
        {
            get
            {
                return ApiUrl + "api/uzytkownik/";
            }
        }
    }
}
