using DocSignCallingAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using System.Web.Mvc;

namespace DocSignCallingAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class DocuSignController : Controller
    {

        public ActionResult SendDocumentforSign()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SendDocumentforSign(Recipient recipient, HttpPostedFileBase UploadDocument)
        {

            byte[] fileBytes;
            using (Stream inputStream = UploadDocument.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                fileBytes = memoryStream.ToArray();
            }

            var DocumentBase64 = System.Convert.ToBase64String(fileBytes);

            DocuSignRequestData inputModel = new DocuSignRequestData();
            inputModel.fileData = DocumentBase64;
            inputModel.Email = recipient.Email;
            inputModel.Name = recipient.Name;
            inputModel.Description = recipient.Description;

            //Note: be replcaed by actual webhook URL of caller API.
            inputModel.returnUrl = "http://callerapi.azurewebsites.net/api/SignedDocumentRecvd/ReciveSignedDoc";

            using (var client = new HttpClient())
            {

                //var res = await client.PostAsync("https://localhost:44349/api/DocuSignPost/SendDocumentforSign", new StringContent(JsonConvert.SerializeObject(inputModel), Encoding.UTF8, "application/json"));
                HttpResponseMessage res = client.PostAsync("https://dssapi.azurewebsites.net/api/DocuSignPost/SendDocumentforSign", new StringContent(JsonConvert.SerializeObject(inputModel), Encoding.UTF8, "application/json")).Result;

                // HttpResponseMessage res = client.PostAsync("https://localhost:44349/api/DocuSignPost/SendDocumentforSign", new StringContent(JsonConvert.SerializeObject(inputModel), Encoding.UTF8, "application/json")).Result;
                Task<string> responseesult = res.Content.ReadAsStringAsync();
                DocuSignResponse obj = JsonConvert.DeserializeObject<DocuSignResponse>(responseesult.Result.ToString());

                if (obj != null && !string.IsNullOrEmpty(obj.EnvelopeId))
                {
                    ViewBag.SucMessage = "Document sent successully";
                }
                try
                {
                    res.EnsureSuccessStatusCode();
                    //res.Result.EnsureSuccessStatusCode();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

            }



            return View();
        }
        
        [HttpGet]
        public ContentResult GetDocumentStatus(string EnvelopeId)
        {
            string resultMessage = "";
            using (var client = new HttpClient())
            {
                //var res = await client.PostAsync("https://localhost:44349/api/DocuSignPost/GetDocumentStatus", new StringContent(JsonConvert.SerializeObject(inputModel), Encoding.UTF8, "application/json"));
                //HttpResponseMessage res = client.GetAsync("https://dssapi.azurewebsites.net/api/DocuSignPost/GetDocumentStatus", new StringContent(JsonConvert.SerializeObject(inputModel), Encoding.UTF8, "application/json")).Result;

                HttpResponseMessage res = client.GetAsync("https://localhost:44349/api/DocuSignPost/GetDocumentStatus?EnvelopeId=" + EnvelopeId).Result;
                Task<string> responseesult = res.Content.ReadAsStringAsync();
                ViewBag.SucMessage = responseesult.Result.ToString();
                resultMessage = responseesult.Result.ToString();
            }
            return Content(resultMessage);
        }

    }
}
