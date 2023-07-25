using System.Runtime.CompilerServices;

namespace UniqueDb.ConnectionProvider.Infrastructure.Extensions;

public static class TypeExtension
{
   public static Boolean IsAnonymousType(this Type type)
   {
      Boolean hasCompilerGeneratedAttribute = type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Any();
      Boolean nameContainsAnonymousType     = type.FullName.Contains("AnonymousType");
      Boolean isAnonymousType               = hasCompilerGeneratedAttribute && nameContainsAnonymousType;

      return isAnonymousType;
   }
}