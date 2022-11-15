using logic_hawks.logic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace logic_hawks.utils
{
    public class SessionHelper
    {
        public static bool ExistUserInSession()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }
        public static void DestroyUserSession()
        {
            FormsAuthentication.SignOut();
        }
        public static string GetUser()
        {
            string user = "";
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity is FormsIdentity)
            {
                FormsAuthenticationTicket ticket = ((FormsIdentity)HttpContext.Current.User.Identity).Ticket;
                if (ticket != null)
                {
                    user = ticket.UserData;
                }
            }
            return user ;
        }

        public static string GetEmpresaUser()
        {
            string empresa = "";
            var objUsuario = new LDatoUsuario();
            string msgError = "";
            var userData = GetUser();
            var userDes = JsonConvert.DeserializeObject<AuthHelper>(userData);

            if (userDes.CodigoUsuario != 0)
            {
                var empresaObj = objUsuario.GetUser(userDes.CodigoUsuario, ref msgError);
                empresa = empresaObj.NOMBRE_EMPRESA;
            }
            else
            {
                empresa = "Empresa";
            }

            return empresa;
        }

        public static long GetCodigoEmpresaUser()
        {
            long empresa = 0;
            var objUsuario = new LDatoUsuario();
            string msgError = "";
            var userData = GetUser();
            var userDes = JsonConvert.DeserializeObject<AuthHelper>(userData);

            if (userDes.CodigoUsuario != 0)
            {
                var empresaObj = objUsuario.GetUser(userDes.CodigoUsuario, ref msgError);
                empresa = (long)empresaObj.CODIGO_EMPRESA;
            }
            else
            {
                empresa = 0;
            }

            return empresa;
        }

        public static int GetProfileUser()
        {
            int profile_id = 0;
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity is FormsIdentity)
            {
                FormsAuthenticationTicket ticket = ((FormsIdentity)HttpContext.Current.User.Identity).Ticket;
                if (ticket != null)
                {
                    profile_id = Convert.ToInt32(ticket.UserData);
                }
            }
            return profile_id;
        }

        public static void AddUserToSession(string id, string profile)
        {
            var dictionary = new AuthHelper();
            dictionary.CodigoUsuario = Convert.ToInt32(id);
            dictionary.CodigoPerfilUserio = Convert.ToInt32(profile);

            bool persist = true;
            var cookie = FormsAuthentication.GetAuthCookie("usuario", persist);

            cookie.Name = FormsAuthentication.FormsCookieName;
            cookie.Expires = Extra.GetCurrentTime().AddMonths(3);

            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            var newTicket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration, ticket.IsPersistent, JsonConvert.SerializeObject(dictionary));

            cookie.Value = FormsAuthentication.Encrypt(newTicket);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static void AddProfileUserToSession(string id)
        {
            bool persist = true;
            var cookie = FormsAuthentication.GetAuthCookie("perfil", persist);

            cookie.Name = FormsAuthentication.FormsCookieName;
            cookie.Expires = Extra.GetCurrentTime().AddMonths(3);

            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            var newTicket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration, ticket.IsPersistent, id);

            cookie.Value = FormsAuthentication.Encrypt(newTicket);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}
