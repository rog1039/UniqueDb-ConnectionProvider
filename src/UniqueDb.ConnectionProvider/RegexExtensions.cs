using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UniqueDb.ConnectionProvider.DataGeneration;
using YamlDotNet.Serialization;

namespace UniqueDb.ConnectionProvider
{
    public static class RegexExtensions
    {
        public static GroupCollection GetMatches(this string input, string regexPattern)
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

            try
            {
                //If the type parameter, T, is an anonymous type, then it will have a constructor
                //which will accept all the values so the below code will succeed.
                //** Possible the passed in type is not anonymous and also accepts all the group 
                //values as parameters as well so this would succeed then too.  Pretty unlikely though.
                var newTObject = (T)Activator.CreateInstance(typeof (T), regexGroupValues);
                return newTObject;
            }
            catch (Exception e)
            {
                //If we get here, then we can assume the type is not anonymous or there is no constructor
                //which accepts all the match group results as parameters, so we must use PropertyInfo
                //to set the values of the object's properties.
                var newTObject = (T) Activator.CreateInstance(typeof (T));
                for (int index = 0; index < anonymousObjectProperties.Length; index++)
                {
                    var anonymousObjectProperty = anonymousObjectProperties[index];
                    anonymousObjectProperty.SetValue(newTObject, regexGroupValues[index], null);
                }
                return newTObject;
            }
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
}
