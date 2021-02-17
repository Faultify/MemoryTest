using System;
using System.Reflection;
using System.Runtime.Loader;

namespace Faultify.MemoryTest
{
    public class CustomAssemblyLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver _resolver;

        public CustomAssemblyLoadContext(string mainAssemblyToLoadPath) : base(true)
        {
            _resolver = new AssemblyDependencyResolver(mainAssemblyToLoadPath);
        }

        protected override Assembly Load(AssemblyName name)
        {
            var assemblyPath = _resolver.ResolveAssemblyToPath(name);
            return assemblyPath != null ? LoadFromAssemblyPath(assemblyPath) : null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            var assemblyPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            return LoadUnmanagedDllFromPath(assemblyPath);
        }
    }
}