using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using Microsoft.SqlServer.Types;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration.AdventureWorks
{

    public class AWBuildVersion
    {
        [Range(0, 255)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte SystemInformationID { get; set; }
        [StringLength(25)]
        public string Database_Version { get; set; }
        public DateTime VersionDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class DatabaseLog
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DatabaseLogID { get; set; }
        public DateTime PostTime { get; set; }
        public string DatabaseUser { get; set; }
        public string Event { get; set; }
        public string Schema { get; set; }
        public string Object { get; set; }
        public string TSQL { get; set; }
        public XElement XmlEvent { get; set; }
    }
    public class ErrorLog
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ErrorLogID { get; set; }
        public DateTime ErrorTime { get; set; }
        public string UserName { get; set; }
        [Range(-2147483648, 2147483647)]
        public int ErrorNumber { get; set; }
        [Range(-2147483648, 2147483647)]
        public int? ErrorSeverity { get; set; }
        [Range(-2147483648, 2147483647)]
        public int? ErrorState { get; set; }
        [StringLength(126)]
        public string ErrorProcedure { get; set; }
        [Range(-2147483648, 2147483647)]
        public int? ErrorLine { get; set; }
        [StringLength(4000)]
        public string ErrorMessage { get; set; }
    }
    public class Department
    {
        [Range(-32768, 32767)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int16 DepartmentID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string GroupName { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class Employee
    {
        [Range(-2147483648, 2147483647)]
        public int BusinessEntityID { get; set; }
        [StringLength(15)]
        public string NationalIDNumber { get; set; }
        [StringLength(256)]
        public string LoginID { get; set; }
        public SqlHierarchyId OrganizationNode { get; set; }
        [Range(-32768, 32767)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Int16? OrganizationLevel { get; set; }
        [StringLength(50)]
        public string JobTitle { get; set; }
        public DateTime BirthDate { get; set; }
        [StringLength(1)]
        public string MaritalStatus { get; set; }
        [StringLength(1)]
        public string Gender { get; set; }
        public DateTime HireDate { get; set; }
        public bool SalariedFlag { get; set; }
        [Range(-32768, 32767)]
        public Int16 VacationHours { get; set; }
        [Range(-32768, 32767)]
        public Int16 SickLeaveHours { get; set; }
        public bool CurrentFlag { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class EmployeeDepartmentHistory
    {
        [Range(-2147483648, 2147483647)]
        public int BusinessEntityID { get; set; }
        [Range(-32768, 32767)]
        public Int16 DepartmentID { get; set; }
        [Range(0, 255)]
        public byte ShiftID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class EmployeePayHistory
    {
        [Range(-2147483648, 2147483647)]
        public int BusinessEntityID { get; set; }
        public DateTime RateChangeDate { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal Rate { get; set; }
        [Range(0, 255)]
        public byte PayFrequency { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class JobCandidate
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JobCandidateID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int? BusinessEntityID { get; set; }
        public XElement Resume { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class Shift
    {
        [Range(0, 255)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte ShiftID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class vEmployee
    {
        [Range(-2147483648, 2147483647)]
        public int BusinessEntityID { get; set; }
        [StringLength(8)]
        public string Title { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string MiddleName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(10)]
        public string Suffix { get; set; }
        [StringLength(50)]
        public string JobTitle { get; set; }
        [StringLength(25)]
        public string PhoneNumber { get; set; }
        [StringLength(50)]
        public string PhoneNumberType { get; set; }
        [StringLength(50)]
        public string EmailAddress { get; set; }
        [Range(-2147483648, 2147483647)]
        public int EmailPromotion { get; set; }
        [StringLength(60)]
        public string AddressLine1 { get; set; }
        [StringLength(60)]
        public string AddressLine2 { get; set; }
        [StringLength(30)]
        public string City { get; set; }
        [StringLength(50)]
        public string StateProvinceName { get; set; }
        [StringLength(15)]
        public string PostalCode { get; set; }
        [StringLength(50)]
        public string CountryRegionName { get; set; }
        public XElement AdditionalContactInfo { get; set; }
    }
    public class vEmployeeDepartment
    {
        [Range(-2147483648, 2147483647)]
        public int BusinessEntityID { get; set; }
        [StringLength(8)]
        public string Title { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string MiddleName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(10)]
        public string Suffix { get; set; }
        [StringLength(50)]
        public string JobTitle { get; set; }
        [StringLength(50)]
        public string Department { get; set; }
        [StringLength(50)]
        public string GroupName { get; set; }
        public DateTime StartDate { get; set; }
    }
    public class vEmployeeDepartmentHistory
    {
        [Range(-2147483648, 2147483647)]
        public int BusinessEntityID { get; set; }
        [StringLength(8)]
        public string Title { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string MiddleName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(10)]
        public string Suffix { get; set; }
        [StringLength(50)]
        public string Shift { get; set; }
        [StringLength(50)]
        public string Department { get; set; }
        [StringLength(50)]
        public string GroupName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class vJobCandidate
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JobCandidateID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int? BusinessEntityID { get; set; }
        [StringLength(30)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Name_Prefix { get; set; }
        [StringLength(30)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Name_First { get; set; }
        [StringLength(30)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Name_Middle { get; set; }
        [StringLength(30)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Name_Last { get; set; }
        [StringLength(30)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Name_Suffix { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Skills { get; set; }
        [StringLength(30)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Addr_Type { get; set; }
        [StringLength(100)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Addr_Loc_CountryRegion { get; set; }
        [StringLength(100)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Addr_Loc_State { get; set; }
        [StringLength(100)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Addr_Loc_City { get; set; }
        [StringLength(20)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Addr_PostalCode { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string EMail { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string WebSite { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class vJobCandidateEducation
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JobCandidateID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Edu_Level { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Edu_StartDate { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Edu_EndDate { get; set; }
        [StringLength(50)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Edu_Degree { get; set; }
        [StringLength(50)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Edu_Major { get; set; }
        [StringLength(50)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Edu_Minor { get; set; }
        [StringLength(5)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Edu_GPA { get; set; }
        [StringLength(5)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Edu_GPAScale { get; set; }
        [StringLength(100)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Edu_School { get; set; }
        [StringLength(100)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Edu_Loc_CountryRegion { get; set; }
        [StringLength(100)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Edu_Loc_State { get; set; }
        [StringLength(100)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Edu_Loc_City { get; set; }
    }
    public class vJobCandidateEmployment
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JobCandidateID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Emp_StartDate { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Emp_EndDate { get; set; }
        [StringLength(100)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Emp_OrgName { get; set; }
        [StringLength(100)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Emp_JobTitle { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Emp_Responsibility { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Emp_FunctionCategory { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Emp_IndustryCategory { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Emp_Loc_CountryRegion { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Emp_Loc_State { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Emp_Loc_City { get; set; }
    }
    public class Address
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressID { get; set; }
        [StringLength(60)]
        public string AddressLine1 { get; set; }
        [StringLength(60)]
        public string AddressLine2 { get; set; }
        [StringLength(30)]
        public string City { get; set; }
        [Range(-2147483648, 2147483647)]
        public int StateProvinceID { get; set; }
        [StringLength(15)]
        public string PostalCode { get; set; }
        public SqlGeography SpatialLocation { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class AddressType
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressTypeID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class BusinessEntity
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BusinessEntityID { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class BusinessEntityAddress
    {
        [Range(-2147483648, 2147483647)]
        public int BusinessEntityID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int AddressID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int AddressTypeID { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class BusinessEntityContact
    {
        [Range(-2147483648, 2147483647)]
        public int BusinessEntityID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int PersonID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int ContactTypeID { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class ContactType
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContactTypeID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class CountryRegion
    {
        [StringLength(3)]
        public string CountryRegionCode { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class EmailAddress
    {
        [Range(-2147483648, 2147483647)]
        public int BusinessEntityID { get; set; }
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmailAddressID { get; set; }
        [StringLength(50)]
        public string EmailAddressProperty { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class Password
    {
        [Range(-2147483648, 2147483647)]
        public int BusinessEntityID { get; set; }
        [StringLength(128)]
        public string PasswordHash { get; set; }
        [StringLength(10)]
        public string PasswordSalt { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class Person
    {
        [Range(-2147483648, 2147483647)]
        public int BusinessEntityID { get; set; }
        [StringLength(2)]
        public string PersonType { get; set; }
        public bool NameStyle { get; set; }
        [StringLength(8)]
        public string Title { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string MiddleName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(10)]
        public string Suffix { get; set; }
        [Range(-2147483648, 2147483647)]
        public int EmailPromotion { get; set; }
        public XElement AdditionalContactInfo { get; set; }
        public XElement Demographics { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class PersonPhone
    {
        [Range(-2147483648, 2147483647)]
        public int BusinessEntityID { get; set; }
        [StringLength(25)]
        public string PhoneNumber { get; set; }
        [Range(-2147483648, 2147483647)]
        public int PhoneNumberTypeID { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class PhoneNumberType
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PhoneNumberTypeID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class StateProvince
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StateProvinceID { get; set; }
        [StringLength(3)]
        public string StateProvinceCode { get; set; }
        [StringLength(3)]
        public string CountryRegionCode { get; set; }
        public bool IsOnlyStateProvinceFlag { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [Range(-2147483648, 2147483647)]
        public int TerritoryID { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class vAdditionalContactInfo
    {
        [Range(-2147483648, 2147483647)]
        public int BusinessEntityID { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string MiddleName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(50)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string TelephoneNumber { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string TelephoneSpecialInstructions { get; set; }
        [StringLength(50)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Street { get; set; }
        [StringLength(50)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string City { get; set; }
        [StringLength(50)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string StateProvince { get; set; }
        [StringLength(50)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string PostalCode { get; set; }
        [StringLength(50)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string CountryRegion { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string HomeAddressSpecialInstructions { get; set; }
        [StringLength(128)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string EMailAddress { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string EMailSpecialInstructions { get; set; }
        [StringLength(50)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string EMailTelephoneNumber { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class vStateProvinceCountryRegion
    {
        [Range(-2147483648, 2147483647)]
        public int StateProvinceID { get; set; }
        [StringLength(3)]
        public string StateProvinceCode { get; set; }
        public bool IsOnlyStateProvinceFlag { get; set; }
        [StringLength(50)]
        public string StateProvinceName { get; set; }
        [Range(-2147483648, 2147483647)]
        public int TerritoryID { get; set; }
        [StringLength(3)]
        public string CountryRegionCode { get; set; }
        [StringLength(50)]
        public string CountryRegionName { get; set; }
    }
    public class BillOfMaterials
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BillOfMaterialsID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int? ProductAssemblyID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int ComponentID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [StringLength(3)]
        public string UnitMeasureCode { get; set; }
        [Range(-32768, 32767)]
        public Int16 BOMLevel { get; set; }
        [Range(-999999.99, 999999.99)]
        public decimal PerAssemblyQty { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class Culture
    {
        [StringLength(6)]
        public string CultureID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class Document
    {
        public SqlHierarchyId DocumentNode { get; set; }
        [Range(-32768, 32767)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Int16? DocumentLevel { get; set; }
        [StringLength(50)]
        public string Title { get; set; }
        [Range(-2147483648, 2147483647)]
        public int Owner { get; set; }
        public bool FolderFlag { get; set; }
        [StringLength(400)]
        public string FileName { get; set; }
        [StringLength(8)]
        public string FileExtension { get; set; }
        [StringLength(5)]
        public string Revision { get; set; }
        [Range(-2147483648, 2147483647)]
        public int ChangeNumber { get; set; }
        [Range(0, 255)]
        public byte Status { get; set; }
        public string DocumentSummary { get; set; }
        public byte[] DocumentProperty { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class Illustration
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IllustrationID { get; set; }
        public XElement Diagram { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class Location
    {
        [Range(-32768, 32767)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int16 LocationID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [Range(-214748.3648, 214748.3647)]
        public decimal CostRate { get; set; }
        [Range(-999999.99, 999999.99)]
        public decimal Availability { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class Product
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(25)]
        public string ProductNumber { get; set; }
        public bool MakeFlag { get; set; }
        public bool FinishedGoodsFlag { get; set; }
        [StringLength(15)]
        public string Color { get; set; }
        [Range(-32768, 32767)]
        public Int16 SafetyStockLevel { get; set; }
        [Range(-32768, 32767)]
        public Int16 ReorderPoint { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal StandardCost { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal ListPrice { get; set; }
        [StringLength(5)]
        public string Size { get; set; }
        [StringLength(3)]
        public string SizeUnitMeasureCode { get; set; }
        [StringLength(3)]
        public string WeightUnitMeasureCode { get; set; }
        [Range(-999999.99, 999999.99)]
        public decimal? Weight { get; set; }
        [Range(-2147483648, 2147483647)]
        public int DaysToManufacture { get; set; }
        [StringLength(2)]
        public string ProductLine { get; set; }
        [StringLength(2)]
        public string Class { get; set; }
        [StringLength(2)]
        public string Style { get; set; }
        [Range(-2147483648, 2147483647)]
        public int? ProductSubcategoryID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int? ProductModelID { get; set; }
        public DateTime SellStartDate { get; set; }
        public DateTime SellEndDate { get; set; }
        public DateTime DiscontinuedDate { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class ProductCategory
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductCategoryID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class ProductCostHistory
    {
        [Range(-2147483648, 2147483647)]
        public int ProductID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal StandardCost { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class ProductDescription
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductDescriptionID { get; set; }
        [StringLength(400)]
        public string Description { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class ProductDocument
    {
        [Range(-2147483648, 2147483647)]
        public int ProductID { get; set; }
        public SqlHierarchyId DocumentNode { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class ProductInventory
    {
        [Range(-2147483648, 2147483647)]
        public int ProductID { get; set; }
        [Range(-32768, 32767)]
        public Int16 LocationID { get; set; }
        [StringLength(10)]
        public string Shelf { get; set; }
        [Range(0, 255)]
        public byte Bin { get; set; }
        [Range(-32768, 32767)]
        public Int16 Quantity { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class ProductListPriceHistory
    {
        [Range(-2147483648, 2147483647)]
        public int ProductID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal ListPrice { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class ProductModel
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductModelID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public XElement CatalogDescription { get; set; }
        public XElement Instructions { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class ProductModelIllustration
    {
        [Range(-2147483648, 2147483647)]
        public int ProductModelID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int IllustrationID { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class ProductModelProductDescriptionCulture
    {
        [Range(-2147483648, 2147483647)]
        public int ProductModelID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int ProductDescriptionID { get; set; }
        [StringLength(6)]
        public string CultureID { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class ProductPhoto
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductPhotoID { get; set; }
        public byte[] ThumbNailPhoto { get; set; }
        [StringLength(50)]
        public string ThumbnailPhotoFileName { get; set; }
        public byte[] LargePhoto { get; set; }
        [StringLength(50)]
        public string LargePhotoFileName { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class ProductProductPhoto
    {
        [Range(-2147483648, 2147483647)]
        public int ProductID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int ProductPhotoID { get; set; }
        public bool Primary { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class ProductReview
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductReviewID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int ProductID { get; set; }
        [StringLength(50)]
        public string ReviewerName { get; set; }
        public DateTime ReviewDate { get; set; }
        [StringLength(50)]
        public string EmailAddress { get; set; }
        [Range(-2147483648, 2147483647)]
        public int Rating { get; set; }
        [StringLength(3850)]
        public string Comments { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class ProductSubcategory
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductSubcategoryID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int ProductCategoryID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class ScrapReason
    {
        [Range(-32768, 32767)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int16 ScrapReasonID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class TransactionHistory
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int ProductID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int ReferenceOrderID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int ReferenceOrderLineID { get; set; }
        public DateTime TransactionDate { get; set; }
        [StringLength(1)]
        public string TransactionType { get; set; }
        [Range(-2147483648, 2147483647)]
        public int Quantity { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal ActualCost { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class TransactionHistoryArchive
    {
        [Range(-2147483648, 2147483647)]
        public int TransactionID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int ProductID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int ReferenceOrderID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int ReferenceOrderLineID { get; set; }
        public DateTime TransactionDate { get; set; }
        [StringLength(1)]
        public string TransactionType { get; set; }
        [Range(-2147483648, 2147483647)]
        public int Quantity { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal ActualCost { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class UnitMeasure
    {
        [StringLength(3)]
        public string UnitMeasureCode { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class vProductAndDescription
    {
        [Range(-2147483648, 2147483647)]
        public int ProductID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string ProductModel { get; set; }
        [StringLength(6)]
        public string CultureID { get; set; }
        [StringLength(400)]
        public string Description { get; set; }
    }
    public class vProductModelCatalogDescription
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductModelID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Summary { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Manufacturer { get; set; }
        [StringLength(30)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Copyright { get; set; }
        [StringLength(256)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string ProductURL { get; set; }
        [StringLength(256)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string WarrantyPeriod { get; set; }
        [StringLength(256)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string WarrantyDescription { get; set; }
        [StringLength(256)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string NoOfYears { get; set; }
        [StringLength(256)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string MaintenanceDescription { get; set; }
        [StringLength(256)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Wheel { get; set; }
        [StringLength(256)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Saddle { get; set; }
        [StringLength(256)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Pedal { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string BikeFrame { get; set; }
        [StringLength(256)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Crankset { get; set; }
        [StringLength(256)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string PictureAngle { get; set; }
        [StringLength(256)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string PictureSize { get; set; }
        [StringLength(256)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string ProductPhotoID { get; set; }
        [StringLength(256)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Material { get; set; }
        [StringLength(256)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Color { get; set; }
        [StringLength(256)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string ProductLine { get; set; }
        [StringLength(256)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Style { get; set; }
        [StringLength(1024)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string RiderExperience { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class vProductModelInstructions
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductModelID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Instructions { get; set; }
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? LocationID { get; set; }
        [Range(-99999.9999, 99999.9999)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? SetupHours { get; set; }
        [Range(-99999.9999, 99999.9999)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? MachineHours { get; set; }
        [Range(-99999.9999, 99999.9999)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? LaborHours { get; set; }
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? LotSize { get; set; }
        [StringLength(1024)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Step { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class WorkOrder
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WorkOrderID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int ProductID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int OrderQty { get; set; }
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int StockedQty { get; set; }
        [Range(-32768, 32767)]
        public Int16 ScrappedQty { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DueDate { get; set; }
        [Range(-32768, 32767)]
        public Int16? ScrapReasonID { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class WorkOrderRouting
    {
        [Range(-2147483648, 2147483647)]
        public int WorkOrderID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int ProductID { get; set; }
        [Range(-32768, 32767)]
        public Int16 OperationSequence { get; set; }
        [Range(-32768, 32767)]
        public Int16 LocationID { get; set; }
        public DateTime ScheduledStartDate { get; set; }
        public DateTime ScheduledEndDate { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime ActualEndDate { get; set; }
        [Range(-99999.9999, 99999.9999)]
        public decimal? ActualResourceHrs { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal PlannedCost { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal? ActualCost { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class ProductVendor
    {
        [Range(-2147483648, 2147483647)]
        public int ProductID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int BusinessEntityID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int AverageLeadTime { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal StandardPrice { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal? LastReceiptCost { get; set; }
        public DateTime LastReceiptDate { get; set; }
        [Range(-2147483648, 2147483647)]
        public int MinOrderQty { get; set; }
        [Range(-2147483648, 2147483647)]
        public int MaxOrderQty { get; set; }
        [Range(-2147483648, 2147483647)]
        public int? OnOrderQty { get; set; }
        [StringLength(3)]
        public string UnitMeasureCode { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class PurchaseOrderDetail
    {
        [Range(-2147483648, 2147483647)]
        public int PurchaseOrderID { get; set; }
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseOrderDetailID { get; set; }
        public DateTime DueDate { get; set; }
        [Range(-32768, 32767)]
        public Int16 OrderQty { get; set; }
        [Range(-2147483648, 2147483647)]
        public int ProductID { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal UnitPrice { get; set; }
        [Range(-922337203685478, 922337203685478)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal LineTotal { get; set; }
        [Range(-999999.99, 999999.99)]
        public decimal ReceivedQty { get; set; }
        [Range(-999999.99, 999999.99)]
        public decimal RejectedQty { get; set; }
        [Range(-9999999.99, 9999999.99)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal StockedQty { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class PurchaseOrderHeader
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseOrderID { get; set; }
        [Range(0, 255)]
        public byte RevisionNumber { get; set; }
        [Range(0, 255)]
        public byte Status { get; set; }
        [Range(-2147483648, 2147483647)]
        public int EmployeeID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int VendorID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int ShipMethodID { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShipDate { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal SubTotal { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal TaxAmt { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal Freight { get; set; }
        [Range(-922337203685478, 922337203685478)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal TotalDue { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class ShipMethod
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShipMethodID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal ShipBase { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal ShipRate { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class Vendor
    {
        [Range(-2147483648, 2147483647)]
        public int BusinessEntityID { get; set; }
        [StringLength(15)]
        public string AccountNumber { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [Range(0, 255)]
        public byte CreditRating { get; set; }
        public bool PreferredVendorStatus { get; set; }
        public bool ActiveFlag { get; set; }
        [StringLength(1024)]
        public string PurchasingWebServiceURL { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class vVendorWithAddresses
    {
        [Range(-2147483648, 2147483647)]
        public int BusinessEntityID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string AddressType { get; set; }
        [StringLength(60)]
        public string AddressLine1 { get; set; }
        [StringLength(60)]
        public string AddressLine2 { get; set; }
        [StringLength(30)]
        public string City { get; set; }
        [StringLength(50)]
        public string StateProvinceName { get; set; }
        [StringLength(15)]
        public string PostalCode { get; set; }
        [StringLength(50)]
        public string CountryRegionName { get; set; }
    }
    public class vVendorWithContacts
    {
        [Range(-2147483648, 2147483647)]
        public int BusinessEntityID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string ContactType { get; set; }
        [StringLength(8)]
        public string Title { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string MiddleName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(10)]
        public string Suffix { get; set; }
        [StringLength(25)]
        public string PhoneNumber { get; set; }
        [StringLength(50)]
        public string PhoneNumberType { get; set; }
        [StringLength(50)]
        public string EmailAddress { get; set; }
        [Range(-2147483648, 2147483647)]
        public int EmailPromotion { get; set; }
    }
    public class CountryRegionCurrency
    {
        [StringLength(3)]
        public string CountryRegionCode { get; set; }
        [StringLength(3)]
        public string CurrencyCode { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class CreditCard
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CreditCardID { get; set; }
        [StringLength(50)]
        public string CardType { get; set; }
        [StringLength(25)]
        public string CardNumber { get; set; }
        [Range(0, 255)]
        public byte ExpMonth { get; set; }
        [Range(-32768, 32767)]
        public Int16 ExpYear { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class Currency
    {
        [StringLength(3)]
        public string CurrencyCode { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class CurrencyRate
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CurrencyRateID { get; set; }
        public DateTime CurrencyRateDate { get; set; }
        [StringLength(3)]
        public string FromCurrencyCode { get; set; }
        [StringLength(3)]
        public string ToCurrencyCode { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal AverageRate { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal EndOfDayRate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class Customer
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int? PersonID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int? StoreID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int? TerritoryID { get; set; }
        [StringLength(10)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string AccountNumber { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class PersonCreditCard
    {
        [Range(-2147483648, 2147483647)]
        public int BusinessEntityID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int CreditCardID { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class SalesOrderDetail
    {
        [Range(-2147483648, 2147483647)]
        public int SalesOrderID { get; set; }
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalesOrderDetailID { get; set; }
        [StringLength(25)]
        public string CarrierTrackingNumber { get; set; }
        [Range(-32768, 32767)]
        public Int16 OrderQty { get; set; }
        [Range(-2147483648, 2147483647)]
        public int ProductID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int SpecialOfferID { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal UnitPrice { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal UnitPriceDiscount { get; set; }
        [Range(-7.92281625142643E+28, 7.92281625142643E+28)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal LineTotal { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class SalesOrderHeader
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalesOrderID { get; set; }
        [Range(0, 255)]
        public byte RevisionNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ShipDate { get; set; }
        [Range(0, 255)]
        public byte Status { get; set; }
        public bool OnlineOrderFlag { get; set; }
        [StringLength(25)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string SalesOrderNumber { get; set; }
        [StringLength(25)]
        public string PurchaseOrderNumber { get; set; }
        [StringLength(15)]
        public string AccountNumber { get; set; }
        [Range(-2147483648, 2147483647)]
        public int CustomerID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int? SalesPersonID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int? TerritoryID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int BillToAddressID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int ShipToAddressID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int ShipMethodID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int? CreditCardID { get; set; }
        [StringLength(15)]
        public string CreditCardApprovalCode { get; set; }
        [Range(-2147483648, 2147483647)]
        public int? CurrencyRateID { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal SubTotal { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal TaxAmt { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal Freight { get; set; }
        [Range(-922337203685478, 922337203685478)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal TotalDue { get; set; }
        [StringLength(128)]
        public string Comment { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class SalesOrderHeaderSalesReason
    {
        [Range(-2147483648, 2147483647)]
        public int SalesOrderID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int SalesReasonID { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class SalesPerson
    {
        [Range(-2147483648, 2147483647)]
        public int BusinessEntityID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int? TerritoryID { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal? SalesQuota { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal Bonus { get; set; }
        [Range(-214748.3648, 214748.3647)]
        public decimal CommissionPct { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal SalesYTD { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal SalesLastYear { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class SalesPersonQuotaHistory
    {
        [Range(-2147483648, 2147483647)]
        public int BusinessEntityID { get; set; }
        public DateTime QuotaDate { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal SalesQuota { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class SalesReason
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalesReasonID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string ReasonType { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class SalesTaxRate
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalesTaxRateID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int StateProvinceID { get; set; }
        [Range(0, 255)]
        public byte TaxType { get; set; }
        [Range(-214748.3648, 214748.3647)]
        public decimal TaxRate { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class SalesTerritory
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TerritoryID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(3)]
        public string CountryRegionCode { get; set; }
        [StringLength(50)]
        public string Group { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal SalesYTD { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal SalesLastYear { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal CostYTD { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal CostLastYear { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class SalesTerritoryHistory
    {
        [Range(-2147483648, 2147483647)]
        public int BusinessEntityID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int TerritoryID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class ShoppingCartItem
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShoppingCartItemID { get; set; }
        [StringLength(50)]
        public string ShoppingCartID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int Quantity { get; set; }
        [Range(-2147483648, 2147483647)]
        public int ProductID { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class SpecialOffer
    {
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SpecialOfferID { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        [Range(-214748.3648, 214748.3647)]
        public decimal DiscountPct { get; set; }
        [StringLength(50)]
        public string Type { get; set; }
        [StringLength(50)]
        public string Category { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Range(-2147483648, 2147483647)]
        public int MinQty { get; set; }
        [Range(-2147483648, 2147483647)]
        public int? MaxQty { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class SpecialOfferProduct
    {
        [Range(-2147483648, 2147483647)]
        public int SpecialOfferID { get; set; }
        [Range(-2147483648, 2147483647)]
        public int ProductID { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class Store
    {
        [Range(-2147483648, 2147483647)]
        public int BusinessEntityID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [Range(-2147483648, 2147483647)]
        public int? SalesPersonID { get; set; }
        public XElement Demographics { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class vIndividualCustomer
    {
        [Range(-2147483648, 2147483647)]
        public int BusinessEntityID { get; set; }
        [StringLength(8)]
        public string Title { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string MiddleName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(10)]
        public string Suffix { get; set; }
        [StringLength(25)]
        public string PhoneNumber { get; set; }
        [StringLength(50)]
        public string PhoneNumberType { get; set; }
        [StringLength(50)]
        public string EmailAddress { get; set; }
        [Range(-2147483648, 2147483647)]
        public int EmailPromotion { get; set; }
        [StringLength(50)]
        public string AddressType { get; set; }
        [StringLength(60)]
        public string AddressLine1 { get; set; }
        [StringLength(60)]
        public string AddressLine2 { get; set; }
        [StringLength(30)]
        public string City { get; set; }
        [StringLength(50)]
        public string StateProvinceName { get; set; }
        [StringLength(15)]
        public string PostalCode { get; set; }
        [StringLength(50)]
        public string CountryRegionName { get; set; }
        public XElement Demographics { get; set; }
    }
    public class vPersonDemographics
    {
        [Range(-2147483648, 2147483647)]
        public int BusinessEntityID { get; set; }
        [Range(-922337203685478, 922337203685478)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? TotalPurchaseYTD { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateFirstPurchase { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime BirthDate { get; set; }
        [StringLength(1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string MaritalStatus { get; set; }
        [StringLength(30)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string YearlyIncome { get; set; }
        [StringLength(1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Gender { get; set; }
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? TotalChildren { get; set; }
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? NumberChildrenAtHome { get; set; }
        [StringLength(30)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Education { get; set; }
        [StringLength(30)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Occupation { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public bool? HomeOwnerFlag { get; set; }
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? NumberCarsOwned { get; set; }
    }
    public class vSalesPerson
    {
        [Range(-2147483648, 2147483647)]
        public int BusinessEntityID { get; set; }
        [StringLength(8)]
        public string Title { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string MiddleName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(10)]
        public string Suffix { get; set; }
        [StringLength(50)]
        public string JobTitle { get; set; }
        [StringLength(25)]
        public string PhoneNumber { get; set; }
        [StringLength(50)]
        public string PhoneNumberType { get; set; }
        [StringLength(50)]
        public string EmailAddress { get; set; }
        [Range(-2147483648, 2147483647)]
        public int EmailPromotion { get; set; }
        [StringLength(60)]
        public string AddressLine1 { get; set; }
        [StringLength(60)]
        public string AddressLine2 { get; set; }
        [StringLength(30)]
        public string City { get; set; }
        [StringLength(50)]
        public string StateProvinceName { get; set; }
        [StringLength(15)]
        public string PostalCode { get; set; }
        [StringLength(50)]
        public string CountryRegionName { get; set; }
        [StringLength(50)]
        public string TerritoryName { get; set; }
        [StringLength(50)]
        public string TerritoryGroup { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal? SalesQuota { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal SalesYTD { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal SalesLastYear { get; set; }
    }
    public class vSalesPersonSalesByFiscalYears
    {
        [Range(-2147483648, 2147483647)]
        public int? SalesPersonID { get; set; }
        [StringLength(152)]
        public string FullName { get; set; }
        [StringLength(50)]
        public string JobTitle { get; set; }
        [StringLength(50)]
        public string SalesTerritory { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal? _2002 { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal? _2003 { get; set; }
        [Range(-922337203685478, 922337203685478)]
        public decimal? _2004 { get; set; }
    }
    public class vStoreWithAddresses
    {
        [Range(-2147483648, 2147483647)]
        public int BusinessEntityID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string AddressType { get; set; }
        [StringLength(60)]
        public string AddressLine1 { get; set; }
        [StringLength(60)]
        public string AddressLine2 { get; set; }
        [StringLength(30)]
        public string City { get; set; }
        [StringLength(50)]
        public string StateProvinceName { get; set; }
        [StringLength(15)]
        public string PostalCode { get; set; }
        [StringLength(50)]
        public string CountryRegionName { get; set; }
    }
    public class vStoreWithContacts
    {
        [Range(-2147483648, 2147483647)]
        public int BusinessEntityID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string ContactType { get; set; }
        [StringLength(8)]
        public string Title { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string MiddleName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(10)]
        public string Suffix { get; set; }
        [StringLength(25)]
        public string PhoneNumber { get; set; }
        [StringLength(50)]
        public string PhoneNumberType { get; set; }
        [StringLength(50)]
        public string EmailAddress { get; set; }
        [Range(-2147483648, 2147483647)]
        public int EmailPromotion { get; set; }
    }
    public class vStoreWithDemographics
    {
        [Range(-2147483648, 2147483647)]
        public int BusinessEntityID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [Range(-922337203685478, 922337203685478)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? AnnualSales { get; set; }
        [Range(-922337203685478, 922337203685478)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? AnnualRevenue { get; set; }
        [StringLength(50)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string BankName { get; set; }
        [StringLength(5)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string BusinessType { get; set; }
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? YearOpened { get; set; }
        [StringLength(50)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Specialty { get; set; }
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? SquareFeet { get; set; }
        [StringLength(30)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Brands { get; set; }
        [StringLength(30)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Internet { get; set; }
        [Range(-2147483648, 2147483647)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? NumberEmployees { get; set; }
    }




}