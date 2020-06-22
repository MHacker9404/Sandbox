using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COR.PaymentProcessing.Business
{
    public class InvoiceProcessor : IPaymentProcessor
    {
        public void Finalize(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
