using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocSignCallingAPI.Models
{
    public class DocuSignResponse
    {
        public int Status { get; set; }
        public string EnvelopeId { get; set; }
        public string Message { get; set; }
    }
}