using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COR.FirstLook.Handlers
{
    public class AgeValidationHandler : Handler<User>
    {
        public override void Handle(User request)
        {
            if (request.Age < 18)
            {
                throw new InvalidOperationException();
            }
            base.Handle(request);
        }
    }
}
