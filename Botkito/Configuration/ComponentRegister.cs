using Autofac;
using Botkito.Commands;

namespace Botkito.Configuration
{
    public static class ComponentRegister
    {
        public static IContainer BuildContainer()
        {
            var assembly = typeof(ComponentRegister).Assembly;

            var builder = new ContainerBuilder();
            builder.RegisterType<CommandController>();

            builder.RegisterAssemblyTypes(assembly)
                   .AssignableTo<ICommand>().As<ICommand>();


            return builder.Build();
        }
    }
}
