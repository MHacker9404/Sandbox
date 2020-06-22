using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COR.FirstLook.Validators;

namespace COR.FirstLook.Handlers
{
    public class IdNbrValidator : Handler<User>
    {
        private readonly SocialSecurityNumberValidator _socialSecurityNumberValidator = new SocialSecurityNumberValidator();
        public override void Handle(User request)
        {
            if (!_socialSecurityNumberValidator.Validate(request.Id, request.RegionInfo))
            {
                throw new InvalidOperationException();
            }

            base.Handle(request);
        }
    }
}
