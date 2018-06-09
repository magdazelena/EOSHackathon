using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CertificatesManager.Models
{
    public class Certificate
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public string PlaceOfIssue { get; set; }

        public string Content { get; set; }

        public string EOSAuthorAccount { get; set; }

        public string EOSOwnerAccount { get; set; }

        public ICollection<Request> Requests { get; set; }
    }
}