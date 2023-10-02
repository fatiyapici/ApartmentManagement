using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApartmentManagement
{
    public class Invoice
    {
        public int Id { get; set; }

        public DateTime Session { get; set; }
        public string Residence { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        public string Description { get; set; }
    }
}
