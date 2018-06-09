using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CertificatesManager.Models
{
    public class CertificateViewModel
    {
        public string Name { get; set; }
        
        public string PlaceOfIssue { get; set; }

        public string Content { get; set; }
        
        public string EOSOwnerAccount { get; set; }
        
    }
}