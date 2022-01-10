namespace TRMDataManager.Library.Models
{
    using System;

    public class SaleDbModel
    {
        public int Id { get; set; }
        public string CashierId { get; set; }
        public DateTime SaleDate { get; set; } = DateTime.UtcNow;
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
    }
}
