using System;
using System.Collections.Generic;

namespace IPTV.Services
{
    public static class DependencyTypeContainer
    {
        private static Dictionary<Type, Type> dependency = new Dictionary<Type, Type>();

        public static void RegisterDependecy<TViewModel, TView>()
        {
            dependency.Add(typeof(TView), typeof(TViewModel));
        }

        public static Type GetDependecyType(Type TViewModel)
        {
            Type dependecyType;

            dependency.TryGetValue(TViewModel, out dependecyType);

            return dependecyType;
        }
    }
}
