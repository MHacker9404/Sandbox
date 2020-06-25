namespace COR.PaymentProcessing.Business {
    public class Order {
        public double AmountDue { get; set; }
        public Payment[] SelectedPayments { get; set; }
    }
}