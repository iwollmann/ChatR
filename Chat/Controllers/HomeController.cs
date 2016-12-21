using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Chat.Models;
using System.Configuration;
using System.Security.Cryptography;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Web.Security;

namespace Chat.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
                throw new UnauthorizedAccessException();

            var person = Person.getUserLogged("");

            return View(person);
        }

        public ActionResult Login(string name)
        {

            if (string.IsNullOrEmpty(name))
            {
                return View();
            }
            else
            {
                FormsAuthentication.SetAuthCookie(name, true);

                return RedirectToAction("Index", "Home");
            }
        }

        //public ActionResult SSO(string id)
        //{
        //    var idBytes = Convert.FromBase64String(id);

        //    RSAParameters certParameters = new RSAParameters();
        //    using (var stream = System.IO.File.Open(Server.MapPath("~/App_Data/cert.xml"), FileMode.Open))
        //    {
        //        certParameters = DeserializeObject<RSAParameters>(stream);
        //    }

        //    var myRsa = new RSACryptoServiceProvider();
        //    myRsa.ImportParameters(certParameters);

        //    byte[] decryptedMessage = myRsa.Decrypt(idBytes, false);

        //    var identity = Encoding.Unicode.GetString(decryptedMessage);

        //    FormsAuthentication.SetAuthCookie(identity, true);

        //    return RedirectToAction("Index", "Home");

        //}

        //public T DeserializeObject<T>(Stream stream)
        //{
        //    try
        //    {
        //        MemoryStream memoryStream = new MemoryStream();
        //        XmlSerializer xs = new XmlSerializer(typeof(T));
        //        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

        //        return (T)xs.Deserialize(stream);
        //    }

        //    catch (Exception e)
        //    {
        //        throw;
        //    }

        //} 
    }
}
