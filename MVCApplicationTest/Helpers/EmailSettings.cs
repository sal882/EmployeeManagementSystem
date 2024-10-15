using MVCApplicationTest.DAL.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MVCApplicationTestPL.Helpers
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl= true;

            client.Credentials = new NetworkCredential("badrsaeed85@gmail.com", "qpebbegjkughlgda");

            client.Send("badrsaeed85@gmail.com", email.To, email.Subject, email.Body);
        }
    }
}
