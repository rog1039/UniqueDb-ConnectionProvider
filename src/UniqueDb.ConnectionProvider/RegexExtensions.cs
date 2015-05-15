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

        private static Serializer serializer = new Serializer();
        public static IEnumerable<T> MatchRegex<T>(this IEnumerable<string> inputStrings, string regexPattern,
            T groupStructure)
        {
            var anonymousObjectProperties = groupStructure.GetType().GetProperties();
            anonymousObjectProperties.PrintStringTable(x => x.Name);
            var regex = new Regex(regexPattern);
            var results = inputStrings
                .Select(x => regex.Match(x))
                .Where(m => m.Success)
                .Select(x => PopulateNewAnonymousObjectWithMatchGroups<T>(x, anonymousObjectProperties))
                .ToList();
            
            return results;
        }

        private static T PopulateNewAnonymousObjectWithMatchGroups<T>(Match match, PropertyInfo[] anonymousObjectProperties)
        {
            var matchGroupValues = new string[anonymousObjectProperties.Length];
            for (int index = 0; index < anonymousObjectProperties.Length; index++)
            {
                var anonymousObjectProperty = anonymousObjectProperties[index];
                var value = match.Groups[anonymousObjectProperty.Name].Value;
                matchGroupValues[index] = value;
            }

            try
            {
                var newTObject = (T)Activator.CreateInstance(typeof (T), matchGroupValues);
                return newTObject;
            }
            catch (Exception e)
            {
                var newTObject = (T) Activator.CreateInstance(typeof (T));
                for (int index = 0; index < anonymousObjectProperties.Length; index++)
                {
                    var anonymousObjectProperty = anonymousObjectProperties[index];
                    anonymousObjectProperty.SetValue(newTObject, matchGroupValues[index], null);
                }
                return newTObject;
            }
        }
    }

    public static class YamlExtensionMethods
    {
        public static void PrintYamlList<T>(this IList<T> list)
        {
            foreach (var item in list)
            {
                item.PrintYaml();
            }
        }
        public static void PrintYaml<T>(this T o)
        {
            var stringWriter = new StringWriter();
            new Serializer().Serialize(stringWriter, o);
            var output = stringWriter.ToString();
            Console.WriteLine(output);
        }
    }
}
