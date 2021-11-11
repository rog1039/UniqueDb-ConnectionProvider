using System;

namespace UniqueDb.ConnectionProvider;

public static class AttributeTesting
{
    // @formatter:csharp_max_line_length 200 

    /*
     * The attribute checking code below is generated in the UnitTest project. See GenerateAttributeCheckingCode unit test.
     */
    public const string AssociationAttributeFullName       = @"System.ComponentModel.DataAnnotations.AssociationAttribute";
    public const string CompareAttributeFullName           = @"System.ComponentModel.DataAnnotations.CompareAttribute";
    public const string ConcurrencyCheckAttributeFullName  = @"System.ComponentModel.DataAnnotations.ConcurrencyCheckAttribute";
    public const string CreditCardAttributeFullName        = @"System.ComponentModel.DataAnnotations.CreditCardAttribute";
    public const string CustomValidationAttributeFullName  = @"System.ComponentModel.DataAnnotations.CustomValidationAttribute";
    public const string DataTypeAttributeFullName          = @"System.ComponentModel.DataAnnotations.DataTypeAttribute";
    public const string DisplayAttributeFullName           = @"System.ComponentModel.DataAnnotations.DisplayAttribute";
    public const string DisplayColumnAttributeFullName     = @"System.ComponentModel.DataAnnotations.DisplayColumnAttribute";
    public const string DisplayFormatAttributeFullName     = @"System.ComponentModel.DataAnnotations.DisplayFormatAttribute";
    public const string EditableAttributeFullName          = @"System.ComponentModel.DataAnnotations.EditableAttribute";
    public const string EmailAddressAttributeFullName      = @"System.ComponentModel.DataAnnotations.EmailAddressAttribute";
    public const string EnumDataTypeAttributeFullName      = @"System.ComponentModel.DataAnnotations.EnumDataTypeAttribute";
    public const string FileExtensionsAttributeFullName    = @"System.ComponentModel.DataAnnotations.FileExtensionsAttribute";
    public const string FilterUIHintAttributeFullName      = @"System.ComponentModel.DataAnnotations.FilterUIHintAttribute";
    public const string KeyAttributeFullName               = @"System.ComponentModel.DataAnnotations.KeyAttribute";
    public const string MaxLengthAttributeFullName         = @"System.ComponentModel.DataAnnotations.MaxLengthAttribute";
    public const string MetadataTypeAttributeFullName      = @"System.ComponentModel.DataAnnotations.MetadataTypeAttribute";
    public const string MinLengthAttributeFullName         = @"System.ComponentModel.DataAnnotations.MinLengthAttribute";
    public const string PhoneAttributeFullName             = @"System.ComponentModel.DataAnnotations.PhoneAttribute";
    public const string RangeAttributeFullName             = @"System.ComponentModel.DataAnnotations.RangeAttribute";
    public const string RegularExpressionAttributeFullName = @"System.ComponentModel.DataAnnotations.RegularExpressionAttribute";
    public const string RequiredAttributeFullName          = @"System.ComponentModel.DataAnnotations.RequiredAttribute";
    public const string ScaffoldColumnAttributeFullName    = @"System.ComponentModel.DataAnnotations.ScaffoldColumnAttribute";
    public const string StringLengthAttributeFullName      = @"System.ComponentModel.DataAnnotations.StringLengthAttribute";
    public const string TimestampAttributeFullName         = @"System.ComponentModel.DataAnnotations.TimestampAttribute";
    public const string UIHintAttributeFullName            = @"System.ComponentModel.DataAnnotations.UIHintAttribute";
    public const string UrlAttributeFullName               = @"System.ComponentModel.DataAnnotations.UrlAttribute";
    public const string ValidationAttributeFullName        = @"System.ComponentModel.DataAnnotations.ValidationAttribute";
    public const string ColumnAttributeFullName            = @"System.ComponentModel.DataAnnotations.Schema.ColumnAttribute";
    public const string ComplexTypeAttributeFullName       = @"System.ComponentModel.DataAnnotations.Schema.ComplexTypeAttribute";
    public const string DatabaseGeneratedAttributeFullName = @"System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedAttribute";
    public const string ForeignKeyAttributeFullName        = @"System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute";
    public const string InversePropertyAttributeFullName   = @"System.ComponentModel.DataAnnotations.Schema.InversePropertyAttribute";
    public const string NotMappedAttributeFullName         = @"System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute";
    public const string TableAttributeFullName             = @"System.ComponentModel.DataAnnotations.Schema.TableAttribute";

    public static bool IsAssociationAttribute(this       Type type) => type.IsAttributeType(AssociationAttributeFullName);
    public static bool IsCompareAttribute(this           Type type) => type.IsAttributeType(CompareAttributeFullName);
    public static bool IsConcurrencyCheckAttribute(this  Type type) => type.IsAttributeType(ConcurrencyCheckAttributeFullName);
    public static bool IsCreditCardAttribute(this        Type type) => type.IsAttributeType(CreditCardAttributeFullName);
    public static bool IsCustomValidationAttribute(this  Type type) => type.IsAttributeType(CustomValidationAttributeFullName);
    public static bool IsDataTypeAttribute(this          Type type) => type.IsAttributeType(DataTypeAttributeFullName);
    public static bool IsDisplayAttribute(this           Type type) => type.IsAttributeType(DisplayAttributeFullName);
    public static bool IsDisplayColumnAttribute(this     Type type) => type.IsAttributeType(DisplayColumnAttributeFullName);
    public static bool IsDisplayFormatAttribute(this     Type type) => type.IsAttributeType(DisplayFormatAttributeFullName);
    public static bool IsEditableAttribute(this          Type type) => type.IsAttributeType(EditableAttributeFullName);
    public static bool IsEmailAddressAttribute(this      Type type) => type.IsAttributeType(EmailAddressAttributeFullName);
    public static bool IsEnumDataTypeAttribute(this      Type type) => type.IsAttributeType(EnumDataTypeAttributeFullName);
    public static bool IsFileExtensionsAttribute(this    Type type) => type.IsAttributeType(FileExtensionsAttributeFullName);
    public static bool IsFilterUIHintAttribute(this      Type type) => type.IsAttributeType(FilterUIHintAttributeFullName);
    public static bool IsKeyAttribute(this               Type type) => type.IsAttributeType(KeyAttributeFullName);
    public static bool IsMaxLengthAttribute(this         Type type) => type.IsAttributeType(MaxLengthAttributeFullName);
    public static bool IsMetadataTypeAttribute(this      Type type) => type.IsAttributeType(MetadataTypeAttributeFullName);
    public static bool IsMinLengthAttribute(this         Type type) => type.IsAttributeType(MinLengthAttributeFullName);
    public static bool IsPhoneAttribute(this             Type type) => type.IsAttributeType(PhoneAttributeFullName);
    public static bool IsRangeAttribute(this             Type type) => type.IsAttributeType(RangeAttributeFullName);
    public static bool IsRegularExpressionAttribute(this Type type) => type.IsAttributeType(RegularExpressionAttributeFullName);
    public static bool IsRequiredAttribute(this          Type type) => type.IsAttributeType(RequiredAttributeFullName);
    public static bool IsScaffoldColumnAttribute(this    Type type) => type.IsAttributeType(ScaffoldColumnAttributeFullName);
    public static bool IsStringLengthAttribute(this      Type type) => type.IsAttributeType(StringLengthAttributeFullName);
    public static bool IsTimestampAttribute(this         Type type) => type.IsAttributeType(TimestampAttributeFullName);
    public static bool IsUIHintAttribute(this            Type type) => type.IsAttributeType(UIHintAttributeFullName);
    public static bool IsUrlAttribute(this               Type type) => type.IsAttributeType(UrlAttributeFullName);
    public static bool IsValidationAttribute(this        Type type) => type.IsAttributeType(ValidationAttributeFullName);
    public static bool IsColumnAttribute(this            Type type) => type.IsAttributeType(ColumnAttributeFullName);
    public static bool IsComplexTypeAttribute(this       Type type) => type.IsAttributeType(ComplexTypeAttributeFullName);
    public static bool IsDatabaseGeneratedAttribute(this Type type) => type.IsAttributeType(DatabaseGeneratedAttributeFullName);
    public static bool IsForeignKeyAttribute(this        Type type) => type.IsAttributeType(ForeignKeyAttributeFullName);
    public static bool IsInversePropertyAttribute(this   Type type) => type.IsAttributeType(InversePropertyAttributeFullName);
    public static bool IsNotMappedAttribute(this         Type type) => type.IsAttributeType(NotMappedAttributeFullName);
    public static bool IsTableAttribute(this             Type type) => type.IsAttributeType(TableAttributeFullName);

    /// <summary>
    /// Is the Type an attribute with the provided full name?
    /// </summary>
    /// <param name="type"></param>
    /// <param name="fullTypeName"></param>
    /// <returns></returns>
    public static bool IsAttributeType(this Type type, string fullTypeName)
    {
        var inheritsFromAttribute       = type.InheritsFromAttribute();
        var matchesExpectedFullTypeName = type.FullName == fullTypeName;

        return inheritsFromAttribute && matchesExpectedFullTypeName;
    }

    /// <summary>
    /// Does the type inherit from Attribute?
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static bool InheritsFromAttribute(this Type type)
    {
        return type.InheritsFrom(typeof(Attribute));
    }

    /// <summary>
    /// Does the type inherit from an attribute with the provided name?
    /// </summary>
    /// <param name="type"></param>
    /// <param name="baseTypeFullName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static bool InheritsFrom(this Type type, string baseTypeFullName)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (string.IsNullOrWhiteSpace(baseTypeFullName))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(baseTypeFullName));

        while (true)
        {
            /*
             * /What if we are passed the type "object" and thus it has no base type.
             * Object inherits from nothing so we can safely return false.
             */
            if (type.BaseType == null) return false;

            if (type.BaseType.FullName == baseTypeFullName) return true;

            type = type.BaseType;
        }
    }

    /// <summary>
    /// Does the type inherit from the provided Attribute type.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="baseType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static bool InheritsFrom(this Type type, Type baseType)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (baseType == null) throw new ArgumentNullException(nameof(baseType));

        while (true)
        {
            /*
             * /What if we are passed the type "object" and thus it has no base type.
             * Object inherits from nothing so we can safely return false.
             */
            if (type.BaseType == null) return false;

            if (type.BaseType == baseType) return true;

            type = type.BaseType;
        }
    }
}