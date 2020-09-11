using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Dapper;

namespace UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration.ServiceLayerGen
{
    public class ConfigRealizer
    {
        public IList<FileResult> RealizeConfig(ConfigBuilder builder)
        {
            var fileResults = RealizeFileResults(builder).ToList();
            return fileResults;
        }

        public IEnumerable<FileResult> RealizeFileResults(ConfigBuilder builder)
        {
            foreach (var build in builder.Builders)
            {
                switch (build)
                {
                    case ClassBuilder classBuilder:
                        yield return Build(classBuilder);
                        break;
                        case WebApiBuilder webApiBuilder:
                            yield return Build(webApiBuilder);
                            break;
                    default:
                            break;

                }
            }
        }

        private FileResult Build(WebApiBuilder builder)
        {
            var result = new FileResult()
            {
                Path = GetFilePath(builder),
                Contents = GetClassContents(builder)
            };
            return result;
        }

        private string GetClassContents(WebApiBuilder builder)
        {
            var Namespace    = builder.GetNamespace();
            var inheritsText = GetInheritsText(builder);
            var methodsCode  = GetMethodsContent(builder);

            var classContents = $@"
namespace {Namespace}
{{
    public class {builder.Name}{inheritsText}
    {{
        {methodsCode}
    }}
}}";
            return classContents;
        }

        public FileResult Build(ClassBuilder builder)
        {
            var result = new FileResult()
            {
                Path = GetFilePath(builder),
                Contents = GetClassContents(builder)
            };
            return result;
        }

        private string GetClassContents(ClassBuilder builder)
        {
            var Namespace    = builder.GetNamespace();
            var MethodsCode  = GetMethodsContent(builder);
            var inheritsText = GetInheritsText(builder);
            
            var classContents = $@"
namespace {Namespace}
{{
    public class {builder.Name}{inheritsText}
    {{
        {MethodsCode}
    }}  
}}
";
            return classContents;
        }

        private string GetInheritsText(BuilderPart builder)
        {
            if(builder.ImplementList.Count == 0) return String.Empty;
            var implementsString = builder
                .ImplementList
                .Select(z => z.Type.GetTypeName())
                .StringJoin(", ");
            return $" : {implementsString}";
        }

        private string GetMethodsContent(BuilderPart builder)
        {
            var methodsToImplement = new List<ReadObject>();
            throw new NotImplementedException("Figure out how to handle methods from DelegateTo and ImplementList");
            var methodDescriptions = builder
                .ImplementList
                .SelectMany(z => z.MethodDescriptions)
                .Select(z => GetMethodContent(builder, z))
                .ToList();

            var methodsText = string.Join(Environment.NewLine + Environment.NewLine, methodDescriptions);
            return methodsText;
        }

        private string GetMethodContent(BuilderPart builder, MethodDescription methodDescription)
        {
            var returnType    = methodDescription.ReturnType.GetTypeName();
            var methodName          = methodDescription.Name;
            var parameterText = GetParameterText(methodDescription.Parameters);

            return $@"public {returnType} {methodName}({parameterText})
{{
    throw new NotImplementedException();
}}";
        }

        private string GetParameterText(IList<ParameterDescription> methodDescriptionParameters)
        {
            var parameterTextParts = methodDescriptionParameters
                .Select(mdp => GetParameterText(mdp))
                .ToList();
            var parameterText = string.Join(", ", parameterTextParts);
            return parameterText;
        }

        private string GetParameterText(ParameterDescription parameter)
        {
            return $"{parameter.Type.Name} {parameter.Name}";
        }

        public string GetFilePath(BuilderPart builder)
        {
            var featurePath = GetFeaturePath(builder.ProjectBuilder, builder.FeatureBuilder);
            var fileName    = $"{builder.Name}.cs";

            var filePath = Path.Combine(featurePath, fileName);
            return filePath;
        }

        public string GetFeaturePath(ProjectBuilder project, FeatureBuilder feature)
        {
            var basePath       = project.ProjectRoot;
            var featureRoot    = project.DefaultFeaturePath;
            var featureSubPath = feature.Name.Split(new[]{"."}, StringSplitOptions.RemoveEmptyEntries);
            
            var pathParts = new List<string>(){basePath, featureRoot};
            pathParts.AddRange(featureSubPath);

            var path = Path.Combine(pathParts.ToArray());
            return path;
        }
    }

    public class MethodDescription
    {
        public Type    ReturnType { get; set; }
        public string  Name       { get; set; }
        public IList<ParameterDescription> Parameters       { get; set; }
    }

    public class ParameterDescription
    {
        public Type Type { get; set; }
        public string    Name    { get; set; }
    }

    public class FileResult
    {
        public string Path     { get; set; }
        public string Contents { get; set; }
    }

    public class ConfigBuilder
    {
        public IList<object> Builders { get; set; } = new List<object>();

        public ProjectBuilder CreateProject(string path, string projectName, string featureRoot)
        {
            var project = new ProjectBuilder(path, projectName, featureRoot);
            return project;
        }

        public ReadObject ReadInterface(Type type)
        {
            var readObject = new ReadObject(type);
            Builders.Add(readObject);
            return readObject;
        }

        public FeatureBuilder CreateFeature(string featureName)
        {
            var feature = new FeatureBuilder(featureName);
            Builders.Add(feature);
            return feature;
        }

        public ClassBuilder CreateClass(ProjectBuilder project, FeatureBuilder feature)
        {
            var classBuilder = new ClassBuilder(project, feature);
            Builders.Add(classBuilder);
            return classBuilder;
        }

        public WebApiBuilder CreateApiController(ProjectBuilder proj, FeatureBuilder feature)
        {
            var builder = new WebApiBuilder(proj, feature);
            Builders.Add(builder);
            return builder;
        }
    }

    public class WebApiBuilder : BuilderPart
    {
        public string BaseTypeName { get; set; }
        public ClassBuilder DelegatesImplTo { get; set; }

        public WebApiBuilder(ProjectBuilder projectBuilder, FeatureBuilder feature)
        {
            ProjectBuilder = projectBuilder;
            FeatureBuilder = feature;
        }

        public void DelegatesTo(ClassBuilder builder)
        {
            DelegatesImplTo = builder;
            //Is below a good idea?
            ImplementList.Add(ClassBuilder);
        }

    }

    public class BuilderPart
    {
        public ConfigBuilder ConfigBuilder { get; set; }

        public ProjectBuilder    ProjectBuilder { get; set; }
        public FeatureBuilder    FeatureBuilder { get; set; }
        public string            Name           { get; set; }
        public IList<ReadObject> ImplementList  { get; } = new List<ReadObject>();

        public string GetNamespace()
        {
            var projectNamespace     = ProjectBuilder.ProjectName;
            var featureNamespaceRoot = ProjectBuilder.DefaultFeaturePath;
            var featureNamespacePart = FeatureBuilder.Name;

            var namespaceParts = new[] {projectNamespace, featureNamespaceRoot, featureNamespacePart};

            var combinedNamespace = CreateNamespaceFromParts(namespaceParts);

            return combinedNamespace;
        }

        private string CreateNamespaceFromParts(string[] namespaceParts)
        {
            var nonEmptyParts = namespaceParts.Where(z => !string.IsNullOrWhiteSpace(z)).ToList();
            var nameSpace     = string.Join(".", nonEmptyParts);
            return nameSpace;
        }

        public void Implements(ReadObject readObject)
        {
            ImplementList.Add(readObject);
        }
    }

    public class ClassBuilder : BuilderPart
    {
        public ClassBuilder(ConfigBuilder configBuilder, ProjectBuilder projectBuilder, FeatureBuilder featureBuilder, string name)
        {
            ConfigBuilder = configBuilder;
            ProjectBuilder     = projectBuilder;
            FeatureBuilder     = featureBuilder;
            Name               = name;
        }

        public ClassBuilder(ProjectBuilder projectBuilder, FeatureBuilder featureBuilder)
        {
            ProjectBuilder = projectBuilder;
            FeatureBuilder = featureBuilder;
        }

        public void NamedBy(ReadObject readObject, string suffix)
        {
            var baseName = readObject.Type.IsInterface && readObject.Type.Name.StartsWith("I")
                ? readObject.Type.GetTypeName().Substring(1)
                : readObject.Type.GetTypeName();
            Name = $"{baseName}{suffix}";
        }
    }

    public class ImplementsBuilder
    {
        public ReadObject ObjectToImplement                    { get; set; }
        public bool       ImplementWithNotImplementedException { get; set; }
    }

    public class FeatureBuilder
    {
        public string Name { get; set; }

        public FeatureBuilder(string name)
        {
            Name = name;
        }
    }

    public class ReadObject
    {
        public Type   Type              { get; set; }
        public List<MethodDescription> MethodDescriptions { get; set; } = new List<MethodDescription>();

        public ReadObject(Type type)
        {
            Type = type;
            GetMethods();
        }

        private void GetMethods()
        {
            var methods = Type.GetMethods();
            var methodDescriptions = methods
                .Select(CreateMethodDescription)
                .ToList();
            MethodDescriptions.AddRange(methodDescriptions);
        }

        private MethodDescription CreateMethodDescription(MethodInfo method)
        {
            var parameters = method.GetParameters()
                .Select(parameter => new ParameterDescription()
                {
                    Name = parameter.Name,
                    Type = parameter.ParameterType
                })
                .ToList();

            var description = new MethodDescription()
            {
                Name       = method.Name,
                ReturnType = method.ReturnType,
                Parameters = parameters
            };

            return description;
        }
    }

    public class Directory2
    {
        public string Path { get; set; }

        public Directory2(string path)
        {
            Path = path;
        }
    }

    public class ProjectBuilder
    {
        public string ProjectRoot        { get; set; }
        public string ProjectName        { get; set; }
        public string DefaultFeaturePath { get; set; }

        public ProjectBuilder(string projectRoot, string projectName, string featurePath = default)
        {
            ProjectRoot        = projectRoot;
            ProjectName        = projectName;
            DefaultFeaturePath = featurePath ?? string.Empty;
        }
    }

    public static class TypeExtensionMethods
    {
        public static string GetTypeName(this Type type)
        {
            if (type.IsConstructedGenericType)
            {
                var genericType     = type.GetGenericTypeDefinition();
                var genericArgs     = type.GenericTypeArguments.ToList();
                var genericArgTexts = genericArgs.Select(z => z.GetTypeName()).ToList();
                var genericArgText  = string.Join(", ", genericArgTexts);

                var genericTypeName = type.Name.Split(new[] {'`'}).First();

                return $"{genericTypeName}<{genericArgText}>";
            }
            else
            {
                return type.Name;
            }
        }
    }
}