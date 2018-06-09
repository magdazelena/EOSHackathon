using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CertificatesManager.Models
{
    public class RequestViewModel
    {
        public string EOSRequestorName { get; set; }

        public int CertificateId { get; set; }

        public string Email { get; set; }
    }
}