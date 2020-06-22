using System;
using COR.FirstLook.Handlers;

namespace COR.FirstLook
{
    public class UserProcessor
    {
        //private readonly SocialSecurityNumberValidator _socialSecurityNumberValidator = new SocialSecurityNumberValidator();

        public bool Register(User user)
        {
            //if (!_socialSecurityNumberValidator.Validate(user.Id, user.RegionInfo))
            //    return false;
            //if (user.Age < 18)
            //    return false;
            //if (user.Name.Length <= 1)
            //    return false;
            //if (user.RegionInfo.TwoLetterISORegionName == "NO") return false;

            var handler = new IdNbrValidator();
            handler.SetNext(new AgeValidationHandler()).SetNext(new NameValidationHandler()).SetNext(new RegionValidationHandler());

            try
            {
                handler.Handle(user);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}