using System;
using System.Collections.Generic;

namespace IPTV.Services
{
    public static class DependencyTypeContainer
    {
        private static Dictionary<Type, Type> dependency = new Dictionary<Type, Type>();

        public static void RegisterDependecy(Dictionary<Type, Type> dependencyContainer)
        {
            dependency = dependencyContainer;   
        }

        public static Dictionary<Type,Type> AddDependency<View, ViewModel>(this Dictionary<Type, Type> dependencyContainer)
        {
            dependencyContainer.Add(typeof(ViewModel), typeof(View));

            return dependencyContainer;
        }

        public static Type GetDependecyType(Type TViewModel)
        {
            Type dependecyType;

            dependency.TryGetValue(TViewModel, out dependecyType);

            return dependecyType;
        }
    }
}
