using System;
using System.Linq;

namespace Buform
{
    public static class FormComponentRegistry
    {
        public static void Register()
        {
            var assembly = typeof(FormComponentRegistry).Assembly;
            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            var name = assembly.GetName().Name;

            var assemblies = allAssemblies
                .Where(item => item.GetReferencedAssemblies().Any(i => i.Name == name))
                .ToList();

            assemblies.Add(assembly);

            var components = assemblies
                .SelectMany(item => item.GetTypes())
                .Where(item => item.GetCustomAttributes(typeof(FormComponentAttribute), false).Any())
                .Select(item => Activator.CreateInstance(item) as IFormComponent)
                .ToArray();

            foreach (var component in components)
            {
                component?.Register();
            }
        }
    }
}