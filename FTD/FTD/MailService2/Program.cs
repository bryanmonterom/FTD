using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailService2
{
    class Program
    {
        static void Main(string[] args)
        {
            MailService2.Services.MailSender("Prueba Correo", "Esta es una prueba de correo", "bryanmonterom@gmail.com");
        }
    }
}
