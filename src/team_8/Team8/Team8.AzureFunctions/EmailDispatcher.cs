using System.Net.Mail;
using System;
using System.Net;

namespace Team8.AzureFunctions
{
    public class EmailDispatcher
    {

        public void Send(string userName)
        {
            try
            {
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = true;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.Credentials = new NetworkCredential("team8ensonohackathon@gmail.com", "<redacted>");
                    // send the email



                    MailAddress from = new MailAddress("team8ensonohackathon@gmail.com", "People team");
                    MailAddress to = new MailAddress("edmar.monteiro@ensono.com", userName);
                    MailMessage myMail = new System.Net.Mail.MailMessage(from, to);

                    // add ReplyTo
                    MailAddress replyTo = new MailAddress("reply@example.com");
                    myMail.ReplyToList.Add(replyTo);

                    // set subject and encoding
                    myMail.Subject = "Sick Leave " + userName;
                    myMail.SubjectEncoding = System.Text.Encoding.UTF8;

                    // set body-message and encoding
                    myMail.Body = "<b>Hi,</b><br><br>This person is ill today and will be absent: <b>" + userName + "</b>.";
                    myMail.BodyEncoding = System.Text.Encoding.UTF8;
                    // text or html
                    myMail.IsBodyHtml = true;

                    smtp.Send(myMail);
                }

            }

            catch (SmtpException ex)
            {
                throw new ApplicationException
                  ("SmtpException has occured: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
