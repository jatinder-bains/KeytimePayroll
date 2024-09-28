using System;
using System.Net;
using System.Net.Mail;

class Program
{
    static void Main()
    {
        // SMTP server details
        string smtpServer = "smtp.gmail.com"; // "smtp -mail.outlook.com"; // "smtp.office365.com";  // For Outlook, use Office365 SMTP
        //string smtpServer = "smtp.office365.com";  // For Outlook, use Office365 SMTP
        int smtpPort = 587;  // Port for TLS
        string fromEmail = "bainstech.software@gmail.com"; // "bainstech-payroll@hotmail.com";

        string emailPassword = "rlvz izec lnsg swvr"; // "wyxopsghizkpbquc"; // "btpr#38WC";  // Use a secure method for storing passwords
        string toEmail = "jsbains74@gmail.com";
        string subject = "Test Email";
        string body = "This is a test email sent using .NET 4.5.2 and SmtpClient.";

        // Create a new SmtpClient object
        using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
        {
            smtpClient.EnableSsl = true; // Enable SSL encryption
            smtpClient.Credentials = new NetworkCredential(fromEmail, emailPassword);

            try
            {
                // Create a MailMessage object
                MailMessage mailMessage = new MailMessage(fromEmail, toEmail, subject, body);
                mailMessage.IsBodyHtml = false;  // Set to true if sending HTML email

                // Send the email
                smtpClient.Send(mailMessage);

                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
        }
    }
}
