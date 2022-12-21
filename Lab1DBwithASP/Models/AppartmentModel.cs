using Org.BouncyCastle.Asn1.Mozilla;
using System.ComponentModel;

namespace Lab1DBwithASP.Models
{
    public class ApartmentModel
    {
        [DisplayName("Apartment")]
        public UInt32 Id { get; set; }
        [DisplayName("Initial Payment")]
        public double First { get; set; }
        public int MonthId { get; set; }
        [DisplayName("Additional Payment")]
        public double Additional { get; set; }
        [DisplayName("Paid this month")]
        public double Paid { get; set; }
        [DisplayName("Remaining payment")]
        public double Left { get; set; }
        public UInt32 Year { get; set; }
        public string? Month { get; set; }

    }
}
