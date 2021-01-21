using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LookaukwatApi.Services
{
    public class ConvertDate
    {
        public static string Convert(DateTime date)
        {
            TimeSpan elapsTime = DateTime.UtcNow - date;
            string period = null;
            int time = 0;

            if (elapsTime.TotalMinutes < 60)
            {
                time = elapsTime.Minutes;
                period = "minutes";
            }
            else if (elapsTime.TotalMinutes > 60 && elapsTime.TotalMinutes < 1440)
            {
                time = elapsTime.Hours;
                period = "Heures";
            }
            else if (elapsTime.TotalMinutes > 1440)
            {
                if(elapsTime.Days > 31 && elapsTime.Days < 365)
                {
                    time = elapsTime.Days/31;
                    period = "Mois";
                }else if(elapsTime.Days >= 365)
                {
                    time = elapsTime.Days / 365;
                    period = "Ans";
                }
                else if(elapsTime.Days <= 31)
                {
                    time = elapsTime.Days;
                    period = "Jours";
                }
                
            }

            return "Ajoutée il y'a " + time.ToString() + " " + period;
        }
    }
}