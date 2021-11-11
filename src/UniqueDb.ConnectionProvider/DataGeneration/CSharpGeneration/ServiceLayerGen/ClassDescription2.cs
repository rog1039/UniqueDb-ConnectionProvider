using System.Reflection;

namespace UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration.ServiceLayerGen;

public class ConfigRealizer
{
    public IList<FileResult> RealizeConfig(ConfigBuilder builder)
    {
        var fileResults = RealizeFileResults(builder).ToList();
        return fileResults;
    }

    public static void WriteFileResultsToDisk(IEnumerable<FileResult> fileResults)
    {
        foreach (var fileResult in fileResults)
        {
            var a = Path.GetDirectoryName(fileResult.Path);
            Directory.CreateDirectory(a);
            File.WriteAllText(fileResult.Path, fileResult.Contents);
        }
    }

    public IEnumerable<FileResult> RealizeFileResults(ConfigBuilder builder)
    {
        foreach (var build in builder.Builders)
        {
            switch (build)
            {
                case WebApiBuilder webApiBuilder:
                    yield return Build(webApiBuilder);
                    break;
                case ClassBuilder classBuilder:
                    yield return Build(classBuilder);
                    break;
                case InterfaceBuilder interfaceBuilder:
                    yield return Build(interfaceBuilder);
                    break;
                case ApiDefBuilder apiDefBuilder:
                    yield return Build(apiDefBuilder);
                    break;
                default:
                    break;
            }
        }
    }

    private FileResult Build(ApiDefBuilder builder)
    {
        var result = new FileResult()
        {
            Path     = GetFilePath(builder),
            Contents = GetClassContents(builder)
        };
        return result;
    }

    private string GetClassContents(ApiDefBuilder builder)
    {
        var Namespace = builder.GetNamespace();
        var inheritsText = string.IsNullOrWhiteSpace(builder.BaseTypeName)
            ? string.Empty
            : $" : {builder.BaseTypeName}";
        var methodsCode      = GetMethodsContent(builder);
        var fieldsText       = GetFieldsText(builder);
        var additionalUsings = GetAdditionalUsings(builder);

        var classContents = $@"
//ApiDefBuilder
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
{additionalUsings}

namespace {Namespace}
{{
    public interface {builder.Name}{inheritsText}
    {{
        {methodsCode}
    }}
}}";
        return CustomCodeFormattingEngine.Format(classContents);
    }

    private List<string> GetAdditionalUsings(HasMembersBase builder)
    {
        return builder
            .AdditionalUsingStatements
            .Select(z => $"using {z};")
            .OrderBy(z => z)
            .Distinct()
            .ToList();
    }

    private FileResult Build(InterfaceBuilder builder)
    {
        var result = new FileResult()
        {
            Path     = GetFilePath(builder),
            Contents = GetClassContents(builder)
        };
        return result;
    }

    private string GetClassContents(InterfaceBuilder builder)
    {
        var Namespace = builder.GetNamespace();
        var inheritsText = string.IsNullOrWhiteSpace(builder.BaseTypeName)
            ? string.Empty
            : $" : {builder.BaseTypeName}";
        var methodsCode      = GetMethodSignatures(builder);
        var fieldsText       = GetFieldsText(builder);
        var additionalUsings = GetAdditionalUsings(builder);

        var classContents = $@"
//InterfaceBuilder
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
{additionalUsings}

namespace {Namespace}
{{
    public interface {builder.Name}{inheritsText}
    {{
        {methodsCode}
    }}
}}";
        return CustomCodeFormattingEngine.Format(classContents);
    }

    private string GetMethodSignatures(InterfaceBuilder builder)
    {
        var methods = builder
            .MethodDescriptions
            .Where(z => z.IsPublic)
            .Where(z => !BaseObjectMethods.Contains((string) z.Name))
            .Select(z =>
            {
                var parametersString = GetParameterTextWithTypes(z.Parameters);
                return $"{z.ReturnType.GetTypeName()} {z.Name}({parametersString});";
            })
            .ToList()
            .StringJoin(Environment.NewLine);
        return methods;
    }


    private FileResult Build(WebApiBuilder builder)
    {
        var result = new FileResult()
        {
            Path     = GetFilePath(builder),
            Contents = GetClassContents(builder)
        };
        return result;
    }

    private string GetClassContents(WebApiBuilder builder)
    {
        var Namespace = builder.GetNamespace();
        var inheritsText = string.IsNullOrWhiteSpace(builder.BaseTypeName)
            ? " : MyControllerBase"
            : $" : {builder.BaseTypeName}";
        var methodsCode      = GetMethodsContent(builder);
        var fieldsText       = GetFieldsText(builder);
        var constructorText  = GetConstructorsText(builder);
        var usingStatements  = GetUsingStatements(builder);
        var additionalUsings = GetAdditionalUsings(builder);
        var allUsingStatements = usingStatements.Concat(additionalUsings)
            .Concat(new []{"using System;"})
            .Distinct()
            .OrderBy(z => z)
            .ToList();
        var usingText = string.Join(Environment.NewLine, allUsingStatements);
                
        var overrideBasePath = string.IsNullOrWhiteSpace(builder.BasePath)
            ? string.Empty
            : $"[Route(\"{builder.BasePath})]";

        var classContents = $@"
//WebApiBuilder
{usingText}


namespace {Namespace}
{{
    {overrideBasePath}
    public class {builder.Name}{inheritsText}
    {{
        {fieldsText}

        {constructorText}


        {methodsCode}
    }}
}}";
        return CustomCodeFormattingEngine.Format(classContents);
    }

    public FileResult Build(ClassBuilder builder)
    {
        var result = new FileResult()
        {
            Path     = GetFilePath(builder),
            Contents = GetClassContents(builder)
        };
        return result;
    }

    private string GetClassContents(ClassBuilder builder)
    {
        var Namespace        = builder.GetNamespace();
        var MethodsCode      = GetMethodsContent(builder);
        var inheritsText     = GetInheritsText(builder);
        var fieldsText       = GetFieldsText(builder);
        var constructorsText = GetConstructorsText(builder);
        var usingStatements  = GetUsingStatements(builder);
        var additionalUsings = GetAdditionalUsings(builder);

        var classContents = $@"
//ClassBuilder
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
{usingStatements}
{additionalUsings}

namespace {Namespace}
{{
    public class {builder.Name}{inheritsText}
    {{
        {fieldsText}

        {constructorsText}

        {MethodsCode}
    }}  
}}
";
        return CustomCodeFormattingEngine.Format(classContents);
    }

    private IList<string> GetUsingStatements(ClassBuilder builder)
    {
        var namespaces = builder.ImplementList
            .Select(z => z.FullName.GetNamespaceFromFullName())
            .Concat(builder.DelegatesImplTo
                        .Select(z => z.FullTypeInfo.FullName.GetNamespaceFromFullName()))
            .Distinct();

        var usingStatements = namespaces
            .Select(@namespace => $"using {@namespace};")
            .ToList();

        return usingStatements;
    }

    private string GetConstructorsText(ClassBuilder builder)
    {
        var args = builder.DelegatesImplTo
            .Select(z => z.FullTypeInfo)
            .Select(z => $"{z.Name} {z.Name.ToCamelCase()}")
            .StringJoin(", ");
        var assignments = builder.DelegatesImplTo
            .Select(z => z.FullTypeInfo)
            .Select(z => $"{z.Name.ToUnderscoreCamelCase()} = {z.Name.ToCamelCase()};")
            .StringJoin(Environment.NewLine);

        foreach (var delegatesToOptionse in builder.DelegatesImplTo) { }

        return $@"
public {builder.Name}({args})
{{
    {assignments}
}}";
    }

    private string GetFieldsText(HasMembersBase builder)
    {
        var fields = new List<string>();
        foreach (var delegateTo in builder.DelegatesImplTo)
        {
            fields.Add(
                $"private readonly {delegateTo.FullTypeInfo.Name} {delegateTo.FullTypeInfo.Name.GetFieldName()};");
        }

        return string.Join(Environment.NewLine, fields);
    }

    private string GetInheritsText(HasMembersBase builder)
    {
        if (builder.ImplementList.Count == 0) return String.Empty;
        var implementsString = builder
            .ImplementList
            .Select(z => z.Name)
            .StringJoin(", ");
        return $" : {implementsString}";
    }

    public static readonly IReadOnlyList<string> BaseObjectMethods = new List<string>()
        {"ToString", "GetType", "GetHashCode", "Equals"};

    private string GetMethodsContent(HasMembersBase builder)
    {
        var methodsToImplement = new List<MethodDescription>();
        var content            = string.Empty;

        foreach (var delegateTo in builder.DelegatesImplTo)
        {
            if (builder.GetType() == typeof(WebApiBuilder))
            {
                foreach (var delegateMethodDesc in delegateTo
                             .Methods
                             .Where(z => z.IsPublic)
                             .ExceptObjectBaseMethods()
                        )
                {
                    if (methodsToImplement.Any(z => z.Name == delegateMethodDesc.Name))
                        continue;
                    methodsToImplement.Add(delegateMethodDesc);
                    content += $"[HttpPost(\"{delegateMethodDesc.Name.ToLower()}\")]";
                    content += Environment.NewLine +
                               GetmethodContentViaDelegation(builder, delegateTo, delegateMethodDesc);
                }
            }
            else
            {
                foreach (var delegateMethodDesc in delegateTo
                             .Methods
                             .Where(z => z.IsPublic)
                             .ExceptObjectBaseMethods()
                        )
                {
                    if (methodsToImplement.Any(z => z.Name == delegateMethodDesc.Name))
                        continue;
                    methodsToImplement.Add(delegateMethodDesc);
                    content += Environment.NewLine +
                               GetmethodContentViaDelegation(builder, delegateTo, delegateMethodDesc);
                }
            }
        }

        foreach (var implement in builder.ImplementList)
        {
            foreach (var implementMethodDesc in implement.MethodDescriptions)
            {
                if (methodsToImplement.Any(z => z.Name == implementMethodDesc.Name))
                    continue;
                methodsToImplement.Add(implementMethodDesc);
                content += Environment.NewLine + GetMethodContentViaNotImplemented(builder, implementMethodDesc);
            }
        }

        if (builder is ApiDefBuilder apiDefBuilder)
        {
            foreach (var methodDesc in apiDefBuilder
                         .TalksTo
                         .MethodDescriptions
                         .Where(z => z.IsPublic)
                         .ExceptObjectBaseMethods())
            {
                content += $"[Post(\"{apiDefBuilder.TalksTo.GetUrl()}{methodDesc.Name.ToCamelCase()}\")]";
                content += Environment.NewLine;
                content +=
                    $"{methodDesc.ReturnType.GetTypeName()} {methodDesc.Name}({GetParameterTextWithTypes(methodDesc.Parameters)});";
                content += Environment.NewLine;
            }
        }


        return content;
    }

    private object GetmethodContentViaDelegation(
        HasMembersBase     builder,
        DelegatesToOptions delegatesToOptions,
        MethodDescription  methodDescription)
    {
        var returnType    = methodDescription.ReturnType.GetTypeName();
        var methodName    = methodDescription.Name;
        var parameterText = GetParameterTextWithTypes(methodDescription.Parameters);

        return $@"public {returnType} {methodName}({parameterText})
{{
    return {delegatesToOptions.FieldName}.{methodDescription.Name}({GetParameterTextWithoutTypes(methodDescription.Parameters)});
}}";
    }

    private string GetMethodContentViaNotImplemented(HasMembersBase builder, MethodDescription methodDescription)
    {
        var returnType    = methodDescription.ReturnType.GetTypeName();
        var methodName    = methodDescription.Name;
        var parameterText = GetParameterTextWithTypes(methodDescription.Parameters);

        return $@"public {returnType} {methodName}({parameterText})
{{
    throw new NotImplementedException();
}}";
    }

    private string GetParameterTextWithTypes(IList<ParameterDescription> methodDescriptionParameters)
    {
        var parameterTextParts = methodDescriptionParameters
            .Select(mdp => GetParameterTextWithTypes(mdp))
            .ToList();
        var parameterText = string.Join(", ", parameterTextParts);
        return parameterText;
    }

    private string GetParameterTextWithoutTypes(IList<ParameterDescription> methodDescriptionParameters)
    {
        var parameterTextParts = methodDescriptionParameters
            .Select(mdp => GetParameterTextWithoutTypes(mdp))
            .ToList();
        var parameterText = string.Join(", ", parameterTextParts);
        return parameterText;
    }

    private string GetParameterTextWithTypes(ParameterDescription parameter)
    {
        return $"{parameter.Type.Name} {parameter.Name}";
    }

    private string GetParameterTextWithoutTypes(ParameterDescription parameter)
    {
        return $"{parameter.Name}";
    }

    public string GetFilePath(HasMembersBase builder)
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
        var featureSubPath = feature.Name.Split(new[] {"."}, StringSplitOptions.RemoveEmptyEntries);

        var pathParts = new List<string>() {basePath, featureRoot};
        pathParts.AddRange(featureSubPath);

        var path = Path.Combine(pathParts.ToArray());
        return path;
    }
}

public class MethodDescription
{
    public Type                        ReturnType { get; set; }
    public string                      Name       { get; set; }
    public IList<ParameterDescription> Parameters { get; set; }
    public bool                        IsPublic   { get; set; }
}

public class ParameterDescription
{
    public Type   Type { get; set; }
    public string Name { get; set; }
}

public class FileResult
{
    public string Path     { get; set; }
    public string Contents { get; set; }

    public FileResult()
    {
            
    }

    public FileResult(string path, string contents)
    {
        Path     = path;
        Contents = contents;
    }

    public void SaveToDisk()
    {
        var directoryName = System.IO.Path.GetDirectoryName(Path);
        try
        {
            Directory.CreateDirectory(directoryName);
            File.WriteAllText(Path, Contents);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unable to write to {Path} with directory: {directoryName}");
            Console.WriteLine(e);
            throw;
        }
    }
}

public static class FileResultExtensions
{
    public static void WriteAll(this IEnumerable<FileResult> fileResults)
    {
        foreach (var fileResult in fileResults)
        {
            fileResult.SaveToDisk();
        }
    }
}

public class ConfigBuilder
{
    public IList<object> Builders { get; set; } = new List<object>();

    public ProjectBuilder CreateProject(string path, string projectName, string featureRoot)
    {
        var project = new ProjectBuilder(path, projectName, featureRoot);
        return project;
    }

    public ReflectedType ReadType(Type type)
    {
        var readObject = new ReflectedType(type);
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

    public ApiDefBuilder CreateApiDef(ProjectBuilder proj, FeatureBuilder feature)
    {
        var builder = new ApiDefBuilder(proj, feature);
        Builders.Add(builder);
        return builder;
    }

    public InterfaceBuilder CreateInterface(ProjectBuilder proj, FeatureBuilder feature)
    {
        var builder = new InterfaceBuilder(proj, feature);
        Builders.Add(builder);
        return builder;
    }
}

public class InterfaceBuilder : HasMembersBase
{
    public string BaseTypeName { get; set; }

    public IList<ITypeNameAndType> TypeDescriptions { get; set; } = new List<ITypeNameAndType>();

    public InterfaceBuilder(ProjectBuilder projectBuilder, FeatureBuilder feature)
    {
        ProjectBuilder = projectBuilder;
        FeatureBuilder = feature;
        TypeType       = TypeType.Interface;
    }

    public InterfaceBuilder HasBaseInterface(ITypeNameAndType typeNameAndType)
    {
        if (typeNameAndType.TypeType != TypeType.Interface)
            throw new InvalidOperationException("Interface can only have other interfaces as base types.");

        TypeDescriptions.Add(typeNameAndType);
        return this;
    }

    public override void NamedBy(ITypeNameAndType reflectedType, string suffix)
    {
        base.NamedBy(reflectedType, suffix);
        if (reflectedType.TypeType != TypeType.Interface && reflectedType.Name.StartsWith("I"))
            BaseTypeName = "I" + BaseTypeName;
    }

    public void ExtractPublicNonObjectMethodsFrom(ReflectedType partTranSearchFromDb)
    {
        var methodDescriptions = partTranSearchFromDb.MethodDescriptions
            .Where(z => z.IsPublic)
            .Where(z => !ConfigRealizer.BaseObjectMethods.Contains(z.Name))
            .ToList();
        MethodDescriptions.AddRange(methodDescriptions);
    }
}

public class ApiDefBuilder : HasMembersBase
{
    public string BaseTypeName { get; set; }

    public WebApiBuilder TalksTo { get; set; }

    public ApiDefBuilder(ProjectBuilder projectBuilder, FeatureBuilder feature)
    {
        ProjectBuilder = projectBuilder;
        FeatureBuilder = feature;
        TypeType       = TypeType.Interface;
    }

    public override List<MethodDescription> MethodDescriptions => TalksTo.MethodDescriptions;
}

public class WebApiBuilder : ClassBuilder
{
    public string BaseTypeName { get; set; }

    public string GetUrl()
    {
        return $"/api/{GetNameWithoutControllerSuffix()}/";
    }
        
    private string GetNameWithoutControllerSuffix()
    {
        if (!Name.EndsWith("Controller"))
            throw new Exception($"Expected controller name to end with Controller, instead had name of {Name}");

        return Name.Remove(Name.Length - 10);

    }
        
    public WebApiBuilder(ProjectBuilder projectBuilder, FeatureBuilder feature) : base(projectBuilder, feature)
    {
        ProjectBuilder = projectBuilder;
        FeatureBuilder = feature;
    }

    public override List<MethodDescription> MethodDescriptions
    {
        get
        {
            return DelegatesImplTo
                .SelectMany(z => z.Methods)
                .ToList();
        }
    }

    public string BasePath { get; set; } = String.Empty;
}

public interface IAttributeBuilder
{
    string GetCode();
}

public class AttributeBuilderFromString : IAttributeBuilder
{
    public string AttributeText { get; set; }

    public AttributeBuilderFromString(string attributeText)
    {
        AttributeText = attributeText;
    }

    public string GetCode()
    {
        return AttributeText;
    }
}

public enum TypeType
{
    Unknown,
    Class,
    Interface
}

public interface ITypeNameAndType
{
    public string   FullName { get; set; }
    public string   Name     { get; set; }
    public TypeType TypeType { get; set; }
}

public class TypeNameAndTypeFromType : ITypeNameAndType
{
    public string   FullName { get; set; }
    public string   Name     { get; set; }
    public TypeType TypeType { get; set; }
}

public class TypeNameAndTypeFromString : ITypeNameAndType
{
    public string   FullName { get; set; }
    public string   Name     { get; set; }
    public TypeType TypeType { get; set; }
}

public class TypeNameAndTypeFromHasMembers : ITypeNameAndType
{
    public string   FullName { get; set; }
    public string   Name     { get; set; }
    public TypeType TypeType { get; set; }
}

public class DelegatesToOptions
{
    public string                   FieldName       { get; set; }
    public bool                     IsContructorArg { get; set; }
    public IList<MethodDescription> Methods         { get; set; }
    public IFullTypeInfo            FullTypeInfo    { get; set; }

    public DelegatesToOptions(IFullTypeInfo fullTypeInfo)
    {
        FullTypeInfo    = fullTypeInfo;
        FieldName       = fullTypeInfo.Name.ToUnderscoreCamelCase();
        IsContructorArg = true;
        Methods         = fullTypeInfo.MethodDescriptions.ToList();
    }
}

public interface IFullTypeInfo : IHasMembers, ITypeNameAndType { }

public class HasMembersBase : IFullTypeInfo
{
    private string        _name;
    public  ConfigBuilder ConfigBuilder { get; set; }

    public ProjectBuilder ProjectBuilder { get; set; }
    public FeatureBuilder FeatureBuilder { get; set; }

    public IList<string> AdditionalUsingStatements { get; set; } = new List<string>();

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            SetFullName();
        }
    }

    private void SetFullName()
    {
        FullName = GetNamespace() + "." + Name;
    }

    public string                    FullName           { get; set; }
    public TypeType                  TypeType           { get; set; }
    public IList<IFullTypeInfo>      ImplementList      { get; }      = new List<IFullTypeInfo>();
    public IList<DelegatesToOptions> DelegatesImplTo    { get; set; } = new List<DelegatesToOptions>();
    public IList<IHasMembers>        DelegatesImplToOld { get; set; } = new List<IHasMembers>();

    public virtual List<MethodDescription> MethodDescriptions
    {
        get
        {
            var readObjectsMethods = ImplementList
                .Where(z => z is ReflectedType)
                .SelectMany(z => z.MethodDescriptions)
                .Where(z =>
                {
                    return TypeType switch
                    {
                        TypeType.Interface => !ConfigRealizer.BaseObjectMethods.Contains(z.Name),
                        _                  => true
                    };
                })
                .ToList();
            return readObjectsMethods;
        }
    }

    public void SetNames(string fullName)
    {
        Name     = fullName.Split(new[] {'.'}).Last();
        FullName = fullName;
    }


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

    public void Implements(IFullTypeInfo reflectedType)
    {
        ImplementList.Add(reflectedType);
    }

    public void DelegatesTo(IFullTypeInfo builder)
    {
        DelegatesImplTo.Add(new DelegatesToOptions(builder));
    }


    public void DelegatesToOld(IHasMembers builder)
    {
        DelegatesImplToOld.Add(builder);
    }

    public virtual void NamedBy(ITypeNameAndType reflectedType, string suffix)
    {
        var baseName = reflectedType.TypeType == TypeType.Interface && reflectedType.Name.StartsWith("I")
            ? reflectedType.Name.Substring(1)
            : reflectedType.Name;
        var IOrNot = (TypeType == TypeType.Interface) ? "I" : String.Empty;
        Name = $"{IOrNot}{baseName}{suffix}";
    }
}

public class ClassBuilder : HasMembersBase
{
    public ClassBuilder(ConfigBuilder configBuilder, ProjectBuilder projectBuilder, FeatureBuilder featureBuilder,
                        string        name)
    {
        ConfigBuilder  = configBuilder;
        ProjectBuilder = projectBuilder;
        FeatureBuilder = featureBuilder;
        SetNames(name);
        TypeType = TypeType.Class;
    }

    public ClassBuilder(ProjectBuilder projectBuilder, FeatureBuilder featureBuilder)
    {
        ProjectBuilder = projectBuilder;
        FeatureBuilder = featureBuilder;
    }

    public HasMembersBase SetBaseType(IHasMembers hasMembers)
    {
        throw new NotImplementedException();
    }

    public void InheritsInterface(InterfaceBuilder stub)
    {
        throw new NotImplementedException();
    }
}

public class ImplementsBuilder
{
    public ReflectedType ObjectToImplement                    { get; set; }
    public bool          ImplementWithNotImplementedException { get; set; }
}

public class FeatureBuilder
{
    public string Name { get; set; }

    public FeatureBuilder(string name)
    {
        Name = name;
    }
}

public interface IHasMembers
{
    List<MethodDescription> MethodDescriptions { get; }
}

public class ReflectedType : IFullTypeInfo
{
    public Type                    Type               { get; set; }
    public string                  Name               { get; set; }
    public string                  FullName           { get; set; }
    public List<MethodDescription> MethodDescriptions { get; set; } = new List<MethodDescription>();
    public TypeType                TypeType           { get; set; } = TypeType.Unknown;

    public ReflectedType(Type type)
    {
        Type = type;
        GetMethods();

        Name     = type.GetTypeName();
        FullName = type.Namespace + "." + Name;

        if (type.IsClass) TypeType     = TypeType.Class;
        if (type.IsInterface) TypeType = TypeType.Interface;
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
                Type = parameter.ParameterType,
            })
            .ToList();

        var description = new MethodDescription()
        {
            Name       = method.Name,
            ReturnType = method.ReturnType,
            Parameters = parameters,
            IsPublic   = method.IsPublic
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
    public string  ProjectRoot        { get; set; }
    public string  ProjectName        { get; set; }
    public string? DefaultFeaturePath { get; set; }

    public ProjectBuilder(string projectRoot, string projectName, string? featurePath = default)
    {
        ProjectRoot        = projectRoot;
        ProjectName        = projectName;
        DefaultFeaturePath = featurePath;
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

public static class StringExtensionMethods
{
    public static string GetFieldName(this string name)
    {
        name = name[0].ToString().ToLower() + name.Substring(1);
        return $"_{name}";
    }

    public static string GetNamespaceFromFullName(this string fullName)
    {
        var lastDotIndex = fullName.LastIndexOf(".");
        if (lastDotIndex <= 0)
            throw new InvalidOperationException(
                $"Needs a last dot in the name {fullName}. Last index of '.' was {lastDotIndex}");

        return fullName.Substring(0, lastDotIndex);
    }
}

public static class BuilderExtensionMethods
{
    public static IEnumerable<MethodDescription> ExceptObjectBaseMethods(
        this IEnumerable<MethodDescription> methodDescriptions)
    {
        return methodDescriptions
            .Where(methodDescription => !ConfigRealizer.BaseObjectMethods.Contains(methodDescription.Name));
    }
}