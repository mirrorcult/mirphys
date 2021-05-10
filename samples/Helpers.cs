using System;
using System.Collections.Generic;
using System.Linq;

namespace samples
{
    public static class Helpers
    {
        public static IEnumerable<Type> GetImplementationsOf<TInterface>()
        {
            var interfaceType = typeof(TInterface);
            return AppDomain.CurrentDomain.GetAssemblies()
                .Select(assembly =>
                    assembly.GetTypes().Where(type => !type.IsInterface && interfaceType.IsAssignableFrom(type)))
                .SelectMany(implementation => implementation);
        }
    }
}