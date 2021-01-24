using LookaukwatApi.ViewModel;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LookaukwatApi.Services
{
    public class SendGridService
    {

        public static async Task<bool> configSendGridasync(contactUserViewModel message)
        {
           
            var apikey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apikey);
            var from = new EmailAddress("contact@lookaukwat.com", message.NameSender + "(Ne pas répondre ici)");
            var subject = message.SubjectSender;
            var to = new EmailAddress(message.RecieverEmail, message.RecieverName);
            var plainTextContent = "<a href='https://lookaukwat.com'><img src=" + "https://lookaukwat.com/UserImage/UserImage/logo_lookaukwat2.png" + " alt='lien vers le site' style='height: 50px;' /><br/><br/> <strong style='height: 20px;'>LookAuKwat</strong></a> " +
                "Hello <br/><br/> vous avez un nouveau message sur votre annonce dans <strong style='color:blue;Height:20px;'> LookAuKwat! </strong> <br/> " +
               " <a href =\"" + message.Linkshare + "\">" + message.Linkshare + "</a> <br/>" + message.Message + " <br/>" + "<br/>"
               + "Vous pouvez lui répondre aussi sur son email suivant : " + " <a href =\"mailto:" + message.EmailSender + "\">" + message.EmailSender + "</a>";

            var htmlContent = "<a href='https://lookaukwat.com'><img src=" + "https://lookaukwat.com/UserImage/ico.jpg" + " alt='lien vers le site' style='height: 50px;' /><br/><br/> <strong style='height: 20px;'>LookAuKwat</strong></a> "
               + "Hello <br/><br/> vous avez un nouveau message sur votre annonce dans <strong style='color:blue;Height:20px;'> LookAuKwat! </strong> <br/> " +
               " <a href =\"" + message.Linkshare + "\">" + message.Linkshare + "</a> <br/>" + message.Message + " <br/>" + "<br/>"
               + "Vous pouvez lui répondre aussi sur son email suivant : " + " <a href =\"mailto:" + message.EmailSender + "\">" + message.EmailSender + "</a>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

           
            var response = await client.SendEmailAsync(msg);

            return response.IsSuccessStatusCode;

        }
    }
}