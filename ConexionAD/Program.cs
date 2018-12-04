using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConexionAD
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Console.Clear();
                string user = "";
                string pass = "";
                Console.WriteLine("Usuario: ");
                user = Console.ReadLine();
                Console.WriteLine("Contraseña: ");
                pass = Console.ReadLine();
                Console.Clear();

                if (ValidateCredentials(user, pass, "ARTHA.local") == true)
                {
                    Console.WriteLine("Autenticacion exitosa");
                }
                else
                {
                    Console.WriteLine(ERROR_LOGON_FAILURE);
                    Console.WriteLine("Autenticacion fallida");
                }
                Console.WriteLine("\n\rPresiona <ESC> para salir... ");
            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }

        private const int ERROR_LOGON_FAILURE = 0x31;
        private static bool ValidateCredentials(string username, string password, string domain)
        {
            NetworkCredential credentials = new NetworkCredential(username, password, domain);

            LdapDirectoryIdentifier id = new LdapDirectoryIdentifier(domain);

            using (LdapConnection connection = new LdapConnection(id, credentials, AuthType.Kerberos))
            {
                connection.SessionOptions.Sealing = true;
                connection.SessionOptions.Signing = true;

                try
                {
                    connection.Bind();
                }
                catch (LdapException lEx)
                {
                    if (ERROR_LOGON_FAILURE == lEx.ErrorCode)
                    {
                        return false;
                    }
                    throw;
                }
            }
            return true;
        }
    }
}
