using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CertificatesManager.Models
{
    public class CertificateViewModel
    {
        [Display(Name = "Certificate name")]
        public string Name { get; set; }

        [Display(Name = "Place of issue")]
        public string PlaceOfIssue { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Certificate contents")]
        public string Content { get; set; }

        [Display(Name = "EOS account name of certified person")]
        public string EOSOwnerAccount { get; set; }

        public string AuthorEOSPrivateKey { get; set; }
        
    }
}