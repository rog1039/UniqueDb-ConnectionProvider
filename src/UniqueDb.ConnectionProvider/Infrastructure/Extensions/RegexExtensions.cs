using System.Collections.Concurrent;
using System.Reflection;
using System.Text.RegularExpressions;

namespace UniqueDb.ConnectionProvider.Infrastructure.Extensions;

public static class RegexExtensions
{
   private static readonly ConcurrentDictionary<RegexId, Regex> RegexCache = new();

   private record RegexId(string RegexPattern, RegexOptions RegexOptions);

   private static Regex GetRegex(string regexPattern, RegexOptions regexOptions = RegexOptions.None)
   {
      var regexId = new RegexId(regexPattern, regexOptions);
      if (RegexCache.TryGetValue(regexId, out var regex))
      {
         return regex;
      }

      var newRegex = new Regex(regexPattern, regexOptions);
      if (RegexCache.TryAdd(regexId, newRegex))
      {
         //Here and in the return below, we don't care if the TryAdd succeeds.
         //At the end of the day we just want a regex, so if the add fails because
         //it already exists, then we still just want a regex and it doesn't matter
         //if we return the newly made one or the version from the dictionary.
      }

      return newRegex;
   }

   public static GroupCollection GetMatches(this string  input,
                                            string       regexPattern,
                                            RegexOptions regexOptions = RegexOptions.None)
   {
      var regex   = GetRegex(regexPattern, regexOptions);
      var matches = regex.Match(input);
      return matches.Groups;
   }

   public static IEnumerable<string> MatchRegex(this IEnumerable<string> inputStrings, string regex, string groupName)
   {
      return inputStrings.Select(input => GetMatches(input, regex)[groupName]).WhereMatchSuccessful();
   }

   public static IEnumerable<string> WhereMatchSuccessful(this IEnumerable<Group> groups)
   {
      foreach (var group in groups)
      {
         if (string.IsNullOrWhiteSpace(group.Value))
         {
            continue;
         }

         yield return group.Value;
      }
   }

   public static IEnumerable<T> MatchRegex<T>(this string inputString, string regexPattern,
                                              T           regexResultDataStructure)
   {
      return GenericExtensionMethods.MakeList(inputString).MatchRegex(regexPattern, regexResultDataStructure);
   }

   public static IEnumerable<T> MatchRegex<T>(this IEnumerable<string> inputStrings,
                                              string                   regexPattern,
                                              T                        regexResultDataStructure)
   {
      var resultProperties = regexResultDataStructure.GetType().GetProperties();
      var regex            = new Regex(regexPattern);
      var results = inputStrings
         .Select(input => regex.Match(input))
         .Where(regexMatch => regexMatch.Success)
         .Select(regexMatch => CreateResultDataStructure<T>(regexMatch, resultProperties))
         .ToList();

      return results;
   }

   private static T CreateResultDataStructure<T>(Match match, PropertyInfo[] anonymousObjectProperties)
   {
      var regexGroupValues = ExtractValuesFromRegexMatch<T>(match, anonymousObjectProperties);

      if (typeof(T).IsAnonymousType())
      {
         try
         {
            //If the type parameter, T, is an anonymous type, then it will have a constructor
            //which will accept all the values so the below code will succeed.
            var newTObjectAnonymous = (T) Activator.CreateInstance(typeof(T), regexGroupValues);
            return newTObjectAnonymous;
         }
         catch (Exception) { }
      }

      //If we get here, then the type is not anonymous so we wil use PropertyInfo's
      //to set the values of the object's properties.
      var newTObject = (T) Activator.CreateInstance(typeof(T));
      for (int index = 0; index < anonymousObjectProperties.Length; index++)
      {
         var anonymousObjectProperty = anonymousObjectProperties[index];
         anonymousObjectProperty.SetValue(newTObject, regexGroupValues[index], null);
      }

      return newTObject;
   }

   private static string[] ExtractValuesFromRegexMatch<T>(Match match, PropertyInfo[] anonymousObjectProperties)
   {
      var matchGroupValues = new string[anonymousObjectProperties.Length];
      for (int index = 0; index < anonymousObjectProperties.Length; index++)
      {
         var anonymousObjectProperty = anonymousObjectProperties[index];
         var value                   = match.Groups[anonymousObjectProperty.Name].Value;
         matchGroupValues[index] = value;
      }

      return matchGroupValues;
   }

   public static IEnumerable<string> MatchRegex2(this IEnumerable<string> inputStrings, string regex, string groupName)
   {
      return inputStrings.Select(input => GetMatches(input, regex)[groupName]).WhereMatchSuccessful();
   }

   public static IEnumerable<string> MatchRegex2(this IEnumerable<string> inputStrings, Regex regex, string groupName)
   {
      return inputStrings.Select(input => regex.Match(input).Groups[groupName]).WhereMatchSuccessful();
   }
}