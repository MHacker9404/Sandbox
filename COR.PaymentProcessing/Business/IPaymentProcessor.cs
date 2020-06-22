using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COR.PaymentProcessing.Business
{
    public interface IPaymentProcessor
    {
        void Finalize(Order order);
    }
}
