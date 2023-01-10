using Org.BouncyCastle.Asn1.Mozilla;
using System.ComponentModel;

namespace Lab1DBwithASP.Models
{
    public class ApartmentModel
    {
        [DisplayName("Apartment")]
        public UInt32 Id { get; set; }
        [DisplayName("Initial Charges")]
        public decimal First { get; set; }
        [DisplayName("Month Number")]
        public int MonthId { get; set; }
        [DisplayName("Additional Charges")]
        public decimal Additional { get; set; }
        [DisplayName("Paid")]
        public decimal Paid { get; set; }
        [DisplayName("Remaining payment")]
        public decimal Left { get; set; }
        public UInt32 Year { get; set; }
        public string? Month { get; set; }

    }
}
