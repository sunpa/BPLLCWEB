using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using BPLLCWEB.Domain.Abstract;
using BPLLCWEB.Domain.Entities;
using System.Net;
using System.Configuration;

namespace BPLLCWEB.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "";
        public string MailFromAddress = ConfigurationManager.AppSettings["Email.From"];
        public string MailToTechAddress = ConfigurationManager.AppSettings["Email.Tech"];
        public string MailToAlertGroup = ConfigurationManager.AppSettings["Email.Alert"];
        //public bool UseSsl = true;
        //public string Username = "MySmtpUsername";
        //public string Password = "MySmtpPassword";
        public string ServerName = ConfigurationManager.AppSettings["SMTP.Server"];
        public int ServerPort = Convert.ToInt16(ConfigurationManager.AppSettings["SMTP.Port"]);
        public bool WriteAsFile = false;
        public string FileLocation = @"c:\bpllcweb_store_emails";
    }

    public class SubmitProcessor : IProcessor
    {
        private EmailSettings emailSettings;

        public SubmitProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }

        public void ProcessContactInfo(ContactInfo contactInfo)
        {
            using (var smtpClient = new SmtpClient())
            {
                emailSettings.MailToAddress = contactInfo.MailToAddress;

                //smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                //smtpClient.UseDefaultCredentials = false;
                //smtpClient.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);

                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod
                    = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder()
                    .AppendLine("Credit Union Name: " + contactInfo.CreditunionName + "\n")
                    .AppendLine("Contact Name: " + contactInfo.ContactName + "\n")
                    .AppendLine("Phone Number: " + contactInfo.PhoneNumber + "\n")
                    .AppendLine("Email Address: " + contactInfo.EmailAddress + "\n")
                    .AppendLine("Problem/Issue: ")
                    .AppendLine(contactInfo.ProblemIssue);

                MailMessage mailMessage = new MailMessage(
                    emailSettings.MailFromAddress, // From
                    emailSettings.MailToAddress, // To
                    contactInfo.Subject, // Subject
                    body.ToString()); // Body

                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }
                smtpClient.Send(mailMessage);
            }
        }

        public void ProcessBrokerInfo(BrokerInfo brokerInfo)
        {
            using (var smtpClient = new SmtpClient())
            {

                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;

                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod
                    = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                emailSettings.MailToAddress = brokerInfo.MailToAddress;

                StringBuilder body = new StringBuilder()
                    .AppendLine("Brokerage Firm: " + brokerInfo.BrokerageName + "\n")
                    .AppendLine("Broker Contact: " + brokerInfo.BrokertName + "\n")
                    .AppendLine("Phone Number: " + brokerInfo.PhoneNumber + "\n")
                    .AppendLine("Email Address: " + brokerInfo.EmailAddress + "\n");

                MailMessage mailMessage = new MailMessage(
                    emailSettings.MailFromAddress, // From
                    emailSettings.MailToAddress, // To
                    brokerInfo.Subject, // Subject
                    body.ToString()); // Body

                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }

                smtpClient.Send(mailMessage);
            }

        }

        public void ProcessParticipantInfo(ParticipantInfo participantInfo)
        {
            using (var smtpClient = new SmtpClient())
            {

                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;

                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod
                    = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                emailSettings.MailToAddress = participantInfo.MailToAddress;

                StringBuilder body = new StringBuilder()
                    .AppendLine("Credit Union Name: " + participantInfo.CreditunionName + "\n")
                    .AppendLine("Contact Name: " + participantInfo.ContactName + "\n")
                    .AppendLine("Phone Number: " + participantInfo.PhoneNumber + "\n")
                    .AppendLine("Email Address: " + participantInfo.EmailAddress + "\n")
                    .AppendLine("Interested in: " + "\n")
                    .AppendLine(participantInfo.InterestedIn);

                MailMessage mailMessage = new MailMessage(
                    emailSettings.MailFromAddress, // From
                    emailSettings.MailToAddress, // To
                    participantInfo.Subject, // Subject
                    body.ToString()); // Body

                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }

                smtpClient.Send(mailMessage);
            }

        }

        //public void ProcessNewPassword(ValidateUser user)
        //{

        //}

        public void ProcessHackAttemptEmail(string ipaddress, string username, string password)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html><body><font face=Arial, Sans-Serif><p align=left>");
            sb.Append("THIS SHOULD REQUIRE NO WORK ON YOUR PART" + "<br /><br />" + "<br /><br />");
            sb.Append("BPLLC POSSIBLE HACK ATTEMPT - IP: " + ipaddress + "<br /><br />");
            sb.Append("Attempted Username: " + username + "<br /><br />");
            sb.Append("Attempted Password: " + password + "<br /><br />" + "<br /><br />");
            sb.Append("The credit union users who are part of this organization will see the error message ");
            sb.Append("and will be provided a link to unlock their own organization. This 'should' require no work ");
            sb.Append("on your part. If you are called to unlock their account, please verify that this person ");
            sb.Append("is a valid member, and then use the application to unlock them. Thanks!</p></font></body></html>");

            string emailBody = sb.ToString();

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;

                MailMessage mailMessage = new MailMessage(
                    emailSettings.MailFromAddress,
                    emailSettings.MailToAlertGroup,
                    "BPLLC Web Site Has Reported a POSSIBLE HACK ATTEMPT!",
                    emailBody);
                mailMessage.IsBodyHtml = true;
                mailMessage.Priority = MailPriority.Normal;

                smtpClient.Send(mailMessage);
            }
        }

        public void ProcessNewPasswordSendEmail(string recipient, string username, string newPasword)
        {
            string emailBody = @"<html><body><font face=""Arial, Sans-Serif""><p align=""left"">Your password has been reset on the Business Partners, LLC Website.</p><BR><p align=""left"">Your Username is: <font size=""+2"">" + username + @"</font></p><p align=""left"">Your NEW Password is: <font size=""+2"">" + newPasword + @"</font></p><BR><BR><p align=""left""><strong>Please note: We will not use the characters Uppercase 'O', Zero, the number One, Lowercase 'L' and Uppercase 'Q' in our passwords for your convenience. For security purposes, Business Partners does not have access to your password. Please store your password in a safe place.</strong></p><BR><BR><p align=""left"">You can modify your username and password once you have logged in, as well as set up a secret question and answer for account authentication by clicking on the 'My Options' link at the top of the page.</p></font></body></html>";

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;

                MailMessage mailMessage = new MailMessage(
                    emailSettings.MailFromAddress,
                    recipient,
                    "Your Password has been reset",
                    emailBody);
                mailMessage.IsBodyHtml = true;
                mailMessage.Priority = MailPriority.Normal;

                smtpClient.Send(mailMessage);
            }
        }

        public void ProcessSendErrorEmail(string errorMessage, string location)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;

                MailMessage mailMessage = new MailMessage(
                    emailSettings.MailFromAddress,
                    emailSettings.MailToTechAddress,
                    "Error from " + location,
                    errorMessage);
                mailMessage.Priority = MailPriority.High;

                smtpClient.Send(mailMessage);
            }
        }

    }

}
