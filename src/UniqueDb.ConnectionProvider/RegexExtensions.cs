using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UniqueDb.ConnectionProvider.Infrastructure;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;

namespace UniqueDb.ConnectionProvider
{
    public static class RegexExtensions
    {
        internal static GroupCollection GetMatches(this string input, string regexPattern)
        {
            var regex = new Regex(regexPattern);
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
            T regexResultDataStructure)
        {
            return inputString.MakeList().MatchRegex(regexPattern, regexResultDataStructure);
        }

        public static IEnumerable<T> MatchRegex<T>(this IEnumerable<string> inputStrings, string regexPattern,
            T regexResultDataStructure)
        {
            var resultProperties = regexResultDataStructure.GetType().GetProperties();
            var regex = new Regex(regexPattern);
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
                    var newTObjectAnonymous = (T)Activator.CreateInstance(typeof(T), regexGroupValues);
                    return newTObjectAnonymous;
                }
                catch (Exception)
                {

                }
            }

            //If we get here, then the type is not anonymous so we wil use PropertyInfo's
            //to set the values of the object's properties.
            var newTObject = (T)Activator.CreateInstance(typeof(T));
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
                var value = match.Groups[anonymousObjectProperty.Name].Value;
                matchGroupValues[index] = value;
            }
            return matchGroupValues;
        }
    }
    public static class TypeExtension
    {
        public static Boolean IsAnonymousType(this Type type)
        {
            Boolean hasCompilerGeneratedAttribute = type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Any();
            Boolean nameContainsAnonymousType = type.FullName.Contains("AnonymousType");
            Boolean isAnonymousType = hasCompilerGeneratedAttribute && nameContainsAnonymousType;

            return isAnonymousType;
        }
    }
}
