using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COR.FirstLook.Handlers
{
    public class NameValidationHandler : Handler<User>
    {
        public override void Handle(User request)
        {
            if (request.Name.Length <=1)
            {
                throw new InvalidOperationException();
            }
            base.Handle(request);
        }
    }
}
