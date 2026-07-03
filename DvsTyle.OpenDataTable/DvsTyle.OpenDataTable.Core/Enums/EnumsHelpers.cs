using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DvStyle.OpenDataTable.Enums
{
    public static class EnumsHelpers
    {
        public static String GetEnumValue(Object value)
        {
            var type = value.GetType();
            if (!type.IsEnum)
                throw new ArgumentException(string.Format("Type {0} is not an enum", type));
            return ((Enum)value).ToString("D");
        }

        public static List<SelectListItem> EnumsToList(this Enum enumeration)
        {
            var items = new List<SelectListItem> { };

            foreach (var e in Enum.GetValues(enumeration.GetType()))
            {
                items.Add(new SelectListItem
                {
                    Value = GetEnumValue(e),
                    Text = GetDisplayName(e)
                });
            }
            return items.OrderBy(s=>s.Text).ToList();
        }

        public static List<SelectListItem> EnumsToList<TModel>()
        {
            return Enum.GetValues(typeof(TModel)).Cast<TModel>()
                .Select(e => new SelectListItem { Text = GetDisplayName(e), Value = GetEnumValue(e) }).OrderBy(j=>j.Text).ToList();
        }

        public static List<SelectListItem> EnumsToList<TModel>(TModel exludeValue)
        {
            return Enum.GetValues(typeof(TModel)).Cast<TModel>()
                .Select(e => new SelectListItem { Text = GetDisplayName(e), Value = GetEnumValue(e) })
                .Where(s => s.Value != GetEnumValue(exludeValue)).OrderBy(j => j.Text).ToList();
        }


        public static List<TModel> EnumValuesToList<TModel>()
        {
            return Enum.GetValues(typeof(TModel)).Cast<TModel>().Select(e => e).ToList();
        }

        public static string GetDisplayName(object value)
        {
            var type = value.GetType();
            if (!type.IsEnum)
                throw new ArgumentException(string.Format("Type {0} is not an enum", type));
            // Get the enum field.
            var field = type.GetField(value.ToString());
            if (field == null)
                return value.ToString();
            var atts = field.GetCustomAttributes(typeof(DisplayAttribute), true);
            if (atts.Length == 0)
                return value.ToString();
            return (atts[0] as DisplayAttribute).Name;
        }


        public static string EnumDisplayValueName(this Enum value)
        {
            var type = value.GetType();
            if (!type.IsEnum)
                throw new ArgumentException(string.Format("Type {0} is not an enum", type));
            // Get the enum field.
            var field = type.GetField(value.ToString());
            if (field == null)
                return value.ToString();
            var atts = field.GetCustomAttributes(typeof(DisplayAttribute), true);
            if (atts.Length == 0)
                return value.ToString();
            return (atts[0] as DisplayAttribute).Name;
        }

      
    }
}
