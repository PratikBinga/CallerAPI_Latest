using DocSignCallingAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DocSignCallingAPI.Controllers
{
    [RoutePrefix("api/[controller]/[action]")]
    
    public class SignedDocumentRecvdController : ApiController
    {
        [HttpPost]
        public IHttpActionResult ReciveSignedDoc([FromBody] PayLoadData payLoadData)
        {
            //var addr = new System.Net.Mail.MailAddress(email.Split('@')[0]);
            // var addr = new System.Net.Mail.MailAddress(email);
            //string directorypath = Server.MapPath("~/App_Data/" + "Files/");
            string directorypath = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/" + "Files/");
            if (!Directory.Exists(directorypath))
            {
                Directory.CreateDirectory(directorypath);
            }
            var serverpath = directorypath + payLoadData.UsernameFile + ".pdf";
            System.IO.File.WriteAllBytes(serverpath, Convert.FromBase64String(payLoadData.docBase64));
            // return View(serverpath);
            return Ok();
        }

        [HttpGet]
        
        public IHttpActionResult Welcome()
        {
            //return Content("WElocme");
            return Ok(new { Name = "Sanjay", Age = "30", City = "Pune" });
        }


    }
}
