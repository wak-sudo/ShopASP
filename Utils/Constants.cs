using System.Globalization;

namespace ShopASP.Utils
{
    public class Constants
    {
        public const string AdminRoleName = "admin";

        public const string CartCookieName = "ShoppingCartCookie";

        public const string CartCookiePurpose = CartCookieName;

        public const string TempDataInfo = "Info";

        public const int ItemsPerPage = 5;

        public static readonly CultureInfo CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

        public enum RetCode
        {
            INVALID_PRODUCT_ID,
            OK,
            UNK_ERROR,
            INVALID_ORDER,
        }

        public static readonly Dictionary<RetCode, string> Map = new()
        {
        };
    }
}
