namespace Library_Management_System.Models
{
    public class Transactions
    {
        public int TransactionsID {get; set;}
        public int UserId { get; set; }
        public int BookId { get; set; }
        public  DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int FineAmount { get; set; }
        
    }
}
