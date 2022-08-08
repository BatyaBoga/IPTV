using System;
using System.Collections.Generic;

namespace IPTV.Services
{
    public static class DependencyContainer
    {
        private static Dictionary<Type, Type> dependency = new Dictionary<Type, Type>();

        public static void RegisterDependecy<TViewModel, TView>()
        {
            dependency.Add(typeof(TView), typeof(TViewModel));
        }

        public static Type GetDependecyType(Type TViewModel)
        {
            try
            {
                return dependency[TViewModel];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }
    }
}
