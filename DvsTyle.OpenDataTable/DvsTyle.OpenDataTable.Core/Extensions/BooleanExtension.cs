using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TagHelpers.Extensions
{
    public static class BooleanExtension
    {
        public static string Translate(this bool value, CultureInfo culture)
        {
            string result = string.Empty;
            switch (culture.Name)
            {
                case "fr-FR":
                    result = value == true ? "Oui" : "Non";
                    break;
                default:
                    result = value == true ? "Yes" : "No";
                    break;

            }
            return result;
        }
    }
}
