using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COR.PaymentProcessing.Business;

namespace COR.PaymentProcessing.Handlers
{
    public class PayPalHandler : PaymentHandler
    {
        private PayPalProcessor _payPalProcessor { get; } = new PayPalProcessor();

        public override void Handle(Order order)
        {
            if(order.SelectedPayments.All(p => p.PaymentProvider != PaymentProvider.PayPal))
                _payPalProcessor.Finalize(order);

            base.Handle(order);
        }
    }
}
