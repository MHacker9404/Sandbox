using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COR.PaymentProcessing.Business;

namespace COR.PaymentProcessing.Handlers
{
    public abstract class PaymentHandler: IHandler<Order>
    {
        private IHandler<Order> Next { get; set; }

        public IHandler<Order> SetNext(IHandler<Order> next )
        {
            Next = next;
            return Next;
        }

        public virtual void Handle(Order order)
        {
            if ((Next == null) && (order.AmountDue > 0))
            {
                throw new InvalidOperationException();
            }

            if (order.AmountDue > 0)
            {
                Next.Handle(order);
            }
        }
    }
}
