using System.Reflection;

namespace Modules.Authentication;

public static class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}
