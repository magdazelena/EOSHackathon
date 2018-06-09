using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CertificatesManager.Models
{
    public class Request
    {
        public int Id { get; set; }

        public string EOSRequestorName { get; set; }

        public int CertificateId { get; set; }

        public string Email { get; set; }
    }
}