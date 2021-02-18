using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoiceify.Domain.Entities
{
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public bool IsCompany { get; set; }
        public string Nip { get; set; }
        public string Company { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostCode { get; set; }
        public string Bank { get; set; }
        public string AccountNumber { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }

        [InverseProperty("BuyerClient")]
        public virtual ICollection<Invoice> InvoicesA { get; set; }

        [InverseProperty("SellerClient")]
        public virtual ICollection<Invoice> InvoicesB { get; set; }
    }
}