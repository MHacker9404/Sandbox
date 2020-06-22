using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COR.FirstLook
{
    public class User
    {
        public string Name { get; }
        public string Id { get; }
        public RegionInfo RegionInfo { get; }
        public DateTimeOffset DOB { get; }
        public int Age { get; }

        public User(string name, string id, RegionInfo regionInfo, DateTimeOffset dob)
        {
            Name = name;
            Id = id;
            RegionInfo = regionInfo;
            DOB = dob;
            Age = (DateTime.Now - DOB).Days / 365;
        }
    }
}
