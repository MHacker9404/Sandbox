﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Marketplace.WebApi.Infrastructure
{
    public class PrivateResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty( MemberInfo member, MemberSerialization memberSerialization )
        {
            var prop = base.CreateProperty( member, memberSerialization );
            if ( !prop.Writable )
            {
                var property = member as PropertyInfo;
                var hasPrivateSetter = property?.GetSetMethod( true ) != null;
                prop.Writable = hasPrivateSetter;
            }
            return prop;
        }
    }
}
