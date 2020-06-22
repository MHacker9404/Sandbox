using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COR.FirstLook.Handlers
{
    public class RegionValidationHandler : Handler<User>
    {
        public override void Handle(User request)
        {
            if (request.RegionInfo.TwoLetterISORegionName == "NO")
            {
                throw new InvalidOperationException();
            }

            base.Handle(request);
        }
    }
}
