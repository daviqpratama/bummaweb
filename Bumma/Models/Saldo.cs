namespace Bumma.Models
{
    public class Saldo
    {
        public int Id { get; set; }
        public string NamaAkun { get; set; } = string.Empty;
        public decimal Debit { get; set; }
        public decimal Kredit { get; set; }
    }
}
