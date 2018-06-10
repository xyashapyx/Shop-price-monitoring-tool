using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PriceMonitoring.Models.DataModel
{
    public class Price
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public decimal ProductPrice { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int ProductId { get; set; }
    }
}