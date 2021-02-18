using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoiceify.Domain.Entities
{
    public class Invoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public bool IsPaid { get; set; }
        public string Type { get; set; }
        
        public string Paid { get; set; }
        public string Number { get; set; }
        public string Comments { get; set; }
        public string NetValue { get; set; }
        public string VatAmount { get; set; }
        public string GrossValue { get; set; }
        public string GrossValueDescription { get; set; }
        public DateTime DateIssue { get; set; }
        public DateTime DateSell { get; set; }
        public DateTime DatePayment { get; set; }
        public string MethodPayment { get; set; }
        public string BuyerClientId { get; set; }
        public string SellerClientId { get; set; }

        [ForeignKey("BuyerClientId")]
        public virtual Client BuyerClient { get; set; }
        [ForeignKey("SellerClientId")]
        public virtual Client SellerClient { get; set; }
        
        public virtual ICollection<Order> Orders { get; set; }
    }
}