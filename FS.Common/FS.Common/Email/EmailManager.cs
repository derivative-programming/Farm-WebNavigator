using System;
using System.Collections.Generic;
using System.Text;

namespace HM.Common.Email
{
    public class EmailManager
    {

        public static void SendEmail(string sMTPServer, string port, string username, string password, string toAddress, string fromAddress,
string ccAddress, string bCCAddress,
string subject, string messageBody, bool isBodyHtml, System.Net.Mail.MailPriority priority)
        {
            SendEmail(sMTPServer,port,username,password, toAddress, fromAddress, ccAddress, bCCAddress,
                subject, messageBody, isBodyHtml, priority, new List<string>());
        }

        public static void SendEmail(string sMTPServer, string port, string username, string password, string toAddress, string fromAddress, 
            string ccAddress, string bCCAddress, string subject, string messageBody, bool isBodyHtml, System.Net.Mail.MailPriority priority,
    List<string> fileAttachments)
        {
            SendEmail(sMTPServer, port, username, password, toAddress, fromAddress, ccAddress, bCCAddress,
                subject, messageBody, isBodyHtml, priority, fileAttachments, new List<EmailInlineImages>());
        }

        public static void SendEmail(string sMTPServer, string port, string username, string password, string toAddress, string fromAddress,
string ccAddress, string bCCAddress,
string subject, string messageBody, bool isBodyHtml, System.Net.Mail.MailPriority priority,
            List<string> fileAttachments, List<HM.Common.Email.EmailInlineImages> InlineImages)
        {
            string subjectPrefix = HM.Common.Configuration.ApplicationSetting.ReadApplicationSetting("EmailSubjectPrefix", string.Empty);
            if (subjectPrefix.Length > 0)
                subject = subjectPrefix + ": " + subject;

            System.Net.Mail.SmtpClient client = null;
                int portVal = 0;
            if(port.Length > 0 &&
                int.TryParse(port,out portVal))
            { 
                client = new System.Net.Mail.SmtpClient(sMTPServer,portVal); 
            }
            else
                client = new System.Net.Mail.SmtpClient(sMTPServer); 
            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
            mailMessage.Body = messageBody;
            //mailMessage.BodyEncoding;
            mailMessage.DeliveryNotificationOptions = System.Net.Mail.DeliveryNotificationOptions.None;
            if (fromAddress.Length > 0)
            {
                mailMessage.From = new System.Net.Mail.MailAddress(fromAddress);
            }
            mailMessage.IsBodyHtml = isBodyHtml;
            mailMessage.Priority = priority;
            // mailMessage.ReplyTo;
            // mailMessage.Sender;
            mailMessage.Subject = subject;
            //mailMessage.SubjectEncoding;
            string[] toAddressArray = toAddress.Split(";".ToCharArray()[0]);
            for (int i = 0; i < toAddressArray.Length; i++)
            {
                if(toAddressArray[i].Contains("@"))
                    mailMessage.To.Add(toAddressArray[i]);
            }
            if (bCCAddress.Length > 0)
            {

                string[] bCCAddressArray = bCCAddress.Split(";".ToCharArray()[0]);
                for (int i = 0; i < bCCAddressArray.Length; i++)
                {
                    if (bCCAddressArray[i].Contains("@"))
                        mailMessage.Bcc.Add(bCCAddressArray[i]);
                }
            }
            if (ccAddress.Length > 0)
            {

                string[] ccAddressArray = ccAddress.Split(";".ToCharArray()[0]);
                for (int i = 0; i < ccAddressArray.Length; i++)
                {
                    if (ccAddressArray[i].Contains("@"))
                        mailMessage.CC.Add(ccAddressArray[i]);
                }

            }
            for (int i = 0; i < fileAttachments.Count; i++)
            {
                mailMessage.Attachments.Add(new System.Net.Mail.Attachment(fileAttachments[i]));                
            }

            if (InlineImages.Count > 0)
            {
                for (int i = 0; i < InlineImages.Count; i++)
                {
                    System.Net.Mail.Attachment att = new System.Net.Mail.Attachment(InlineImages[i].ImageMemoryStream, InlineImages[i].ImageName.Replace(".",""));
                    att.ContentDisposition.Inline = true;
                    att.ContentDisposition.DispositionType = System.Net.Mime.DispositionTypeNames.Inline;
                    att.ContentId = InlineImages[i].ContentId;
                    att.ContentType.MediaType = InlineImages[i].MediaType;
                    att.ContentType.Name = InlineImages[i].ImageName;
                    mailMessage.Attachments.Add(att);
                }
            }
            if (username.Length > 0)
            {
                client.Credentials = new System.Net.NetworkCredential(username, password);
                client.EnableSsl = true;
            }
            client.Send(mailMessage);
            mailMessage.Dispose();
        }
    }

    public class EmailInlineImages
    {
        public string ContentId { get; set; }
        public string MediaType { get; set; }
        public System.IO.MemoryStream ImageMemoryStream { get; set; }
        public string ImageName { get; set; }
    }
}
