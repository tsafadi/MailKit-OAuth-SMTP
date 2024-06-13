using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schrott;

public static class MailService
{
    public static void Send(SaslMechanism accessToken, string host, int port, string from, string to)
    {
        using (var client = new SmtpClient(new ProtocolLogger(Console.OpenStandardOutput())))
        {
            try
            {
                client.Connect(host, port, SecureSocketOptions.Auto);
                client.Authenticate(accessToken);
                var msg = new MimeMessage();
                msg.From.Add(MailboxAddress.Parse(from));
                msg.To.Add(MailboxAddress.Parse(to));
                msg.Subject = "Testing SMTP";
                msg.Body = new TextPart("plain") { Text = "This is a test message." };
                client.Send(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
