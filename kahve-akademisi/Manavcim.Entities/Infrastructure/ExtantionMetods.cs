using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Linq;

namespace KahveAkademisi.Entities.Infrastructure
{
    public static class ExtantionMetods
    {

        public static string GetDescription(this Enum value)
        {
            var descriptionAttribute = (DescriptionAttribute)value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(false)
                .Where(a => a is DescriptionAttribute)
                .FirstOrDefault();

            return descriptionAttribute != null ? descriptionAttribute.Description : value.ToString();
        }

        public static string DeleteTre(this string telefonNo)
        {

            string telno= telefonNo.Remove(telefonNo.IndexOf("-"), 1);
            return telno;



        }
    }
}
