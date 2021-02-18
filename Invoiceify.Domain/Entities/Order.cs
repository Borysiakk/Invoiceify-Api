using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoiceify.Domain.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string InvoiceId { get; set; }
        public string Gtu { get; set; }
        public string Count { get; set; }
        public string Jm { get; set; }
        public string NetPrice { get; set; }
        public string NetValue { get; set; }
        public string VatPercent { get; set; }
        public string VatAmount { get; set; }
        public string GrossValue { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }

        [ForeignKey("InvoiceId")]
        public virtual Invoice Invoice { get; set; }
        [ForeignKey("ProductId")]
        public virtual ICollection<Product> Product { get; set; }
        
    }
}