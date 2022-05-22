using System;
using System.Linq;

namespace Buform
{
    public static class FormComponentRegistry
    {
        public static void Register()
        {
            var name = typeof(FormComponentRegistry).Assembly.GetName().Name;

            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            var assemblies = allAssemblies.Where(item => item.GetReferencedAssemblies().Any(i => i.Name == name)).ToArray();

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    if (type.GetCustomAttributes(typeof(ComponentAttribute), false).Length <= 0)
                    {
                        continue;
                    }

                    var component = Activator.CreateInstance(type) as IFormComponent;

                    component?.Register();
                }
            }
        }
    }
}