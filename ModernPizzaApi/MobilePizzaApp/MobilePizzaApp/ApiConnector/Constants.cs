using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        public static String ConnectionApiUriRestauracja
        {
            get
            {
                return ApiUrl + "api/Restauracja/";
            }
        }
        public static String ConnectionApiUriRezerwacja
        {
            get
            {
                return ApiUrl + "api/Rezerwacja/";
            }
        }
        public static String ConnectionApiUriNapoj
        {
            get
            {
                return ApiUrl + "Napoj";
            }
        }
    }
}
