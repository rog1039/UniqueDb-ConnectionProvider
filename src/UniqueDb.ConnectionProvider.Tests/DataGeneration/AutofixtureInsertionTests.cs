using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Microsoft.SqlServer.Types;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using UniqueDb.ConnectionProvider.DataGeneration;
using UniqueDb.ConnectionProvider.DataGeneration.Crud;
using UniqueDb.ConnectionProvider.Tests.DataGeneration.AdventureWorks;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public class AutofixtureInsertionTests
    {
        [Fact()]
        [Trait("Category", "Integration")]
        public void InsertManyRandomRowsIntoManyRandomTables_UsingAutoFixture()
        {
            var testCaseResults = new List<TestInsertDeleteCaseResult>();
            var typesToInsert = new List<Type>();
            var randomSqlTableReferences = new List<SqlTableReference>();

            var db = SqlConnectionProviders.AdventureWorksDb;

            "Given a bunch of DB tables and CLR types that represent those tables"
                ._(() =>
                {
                    randomSqlTableReferences = RandomTableSelector
                        .GetRandomSqlTableReferences(db, 400)
                        .OrderBy(x => x.SchemaName)
                        .ThenBy(x => x.TableName)
                        .ToList();

                    typesToInsert = Assembly
                        .GetAssembly(typeof(BusinessEntityContact))
                        .GetTypes()
                        .Where(x => !x.IsAnonymousType())
                        .Where(x => x.GetProperties().All(p => p.PropertyType != typeof(XElement)))
                        .Where(x => x.GetProperties().All(p => p.PropertyType != typeof(SqlHierarchyId)))
                        .Where(x => !x.Name.StartsWith("v"))
                        .Where(x => x.Namespace.Equals("UniqueDb.ConnectionProvider.Tests.DataGeneration.AdventureWorks"))
                        .ToList();
                });

            var fixture = new Fixture();
            fixture.Inject(new SqlGeography());

            "For each table in the DB"
                ._(() =>
                {
                    foreach (var sqlTableReference in randomSqlTableReferences)
                    {
                        TestInsertDeleteCaseResult testCase = null;
                        var clrTypeForTable = typesToInsert.SingleOrDefault(t => t.Name.Equals(sqlTableReference.TableName));

                        "Create a test case result"
                            ._(() =>
                            {
                                testCase = new TestInsertDeleteCaseResult() { SqlTableReference = sqlTableReference, ClrType = clrTypeForTable };
                                testCaseResults.Add(testCase);
                            });

                        if (clrTypeForTable == null) continue;
                        object clrObjectForSqlTable = null;

                        "Given a new, random instance of the CLR type representing the SqlTableReference"
                            ._(() =>
                            {
                                var context = new SpecimenContext(fixture);
                                clrObjectForSqlTable = context.Resolve(new SeededRequest(clrTypeForTable, null));
                            });

                        "Try inserting into database"
                            ._(() =>
                            {
                                try
                                {
                                    db.Insert(clrObjectForSqlTable, sqlTableReference.TableName, sqlTableReference.SchemaName);
                                }
                                catch (Exception e)
                                {
                                    testCase.AddInsertException(e);
                                }
                            });


                        "Try updating the database"
                            ._(() =>
                            {
                                try
                                {
                                    db.Update(clrObjectForSqlTable, null, sqlTableReference.TableName, sqlTableReference.SchemaName);
                                }
                                catch (Exception e)
                                {
                                    testCase.AddUpdateException(e);
                                }
                            });

                        "Try deleting from database"
                            ._(() =>
                            {
                                try
                                {
                                    db.Delete(clrObjectForSqlTable, null, sqlTableReference.TableName, sqlTableReference.SchemaName);
                                }
                                catch (Exception e)
                                {
                                    testCase.AddDeleteException(e);
                                }
                            });

                        testCase.WrapUp();
                    }
                });

            "Print out test results"
                ._(() =>
                {
                    PrintSummary(testCaseResults);
                    PrintDetails(testCaseResults);
                });
        }

        private void PrintSummary(List<TestInsertDeleteCaseResult> testCaseResults)
        {
            foreach (var testInsertDeleteCaseResult in testCaseResults.OrderBy(tc => tc.Status).ToList())
            {
                Console.WriteLine(testInsertDeleteCaseResult.ToSummary());
            }
        }

        private static void PrintDetails(List<TestInsertDeleteCaseResult> testCaseResults)
        {
            foreach (var testInsertDeleteCaseResult in testCaseResults.OrderBy(tc => tc.Status).ToList())
            {
                Console.WriteLine(testInsertDeleteCaseResult);
            }
        }
    }

    public class TestInsertDeleteCaseResult
    {
        public SqlTableReference SqlTableReference { get; set; }
        public Type ClrType { get; set; }

        public void AddInsertException(Exception e) => InsertExceptionList.Add(e);
        public void AddDeleteException(Exception e) => DeleteExceptionList.Add(e);
        public void AddUpdateException(Exception e) => UpdateExceptionList.Add(e);

        public IList<Exception> InsertExceptionList { get; } = new List<Exception>();
        public IList<Exception> DeleteExceptionList { get; } = new List<Exception>();
        public IList<Exception> UpdateExceptionList { get; } = new List<Exception>();


        public void WriteLine(string input) => _stringBuilder.AppendLine(input);
        private StringBuilder _stringBuilder = new StringBuilder();


        public TestInsertDeleteCaseResultStatus Status { get; private set; } = TestInsertDeleteCaseResultStatus.Unknown;


        public override string ToString()
        {
            CreateLog();
            return _stringBuilder.ToString();
        }

        private void CreateLog()
        {
            _stringBuilder.Clear();

            WriteLine($"\r\n\r\n-------Table: {SqlTableReference.SchemaName}.{SqlTableReference.TableName}-------------------------------------------------");
            WriteLine($" *Status: {Status,-9} | Insert/Update/Delete Exception Count: [{InsertExceptionList.Count}/{UpdateExceptionList.Count}/{DeleteExceptionList.Count}]");

            foreach (var exception in InsertExceptionList)
            {
                _stringBuilder.AppendLine($" Exception [{exception.GetType().Name}] Failed insert on type: {ClrType.Name} with exception:\r\n {exception.Message}");
                PrintProperties();
            }
            WriteLine("");
            foreach (var exception in UpdateExceptionList)
            {
                _stringBuilder.AppendLine($" Exception [{exception.GetType().Name}] Failed update on type: {ClrType.Name} with exception:\r\n {exception.Message}");
                PrintProperties();
            }
            WriteLine("");
            foreach (var exception in DeleteExceptionList)
            {
                _stringBuilder.AppendLine($" Exception [{exception.GetType().Name}] Failed delete on type: {ClrType.Name} with exception:\r\n {exception.Message}");
                PrintProperties();
            }
        }

        private void PrintProperties()
        {
            var propertyList = ClrType.GetProperties().Select(z => $"  {z.PropertyType.Name,14} {z.Name}");
            _stringBuilder.AppendLine($" Properties are:\r\n{string.Join(Environment.NewLine, propertyList)}\r\n");
        }

        public string ToSummary()
        {
            return $"Status: {Status,-10} Table: {SqlTableReference.SchemaName + "." + SqlTableReference.TableName,-45} Exceptions: [{InsertExceptionList.Count}/{UpdateExceptionList.Count}/{DeleteExceptionList.Count}] [Inserts/Updates/Deletes]";
        }

        public void WrapUp()
        {
            SetTestCaseStatus();
        }

        private void SetTestCaseStatus()
        {
            var onlyBenignExceptions = InsertExceptionList.All(SkipException) && DeleteExceptionList.All(SkipException);
            Status = onlyBenignExceptions
                ? TestInsertDeleteCaseResultStatus.Success
                : TestInsertDeleteCaseResultStatus.Failure;
        }
        private static bool SkipException(Exception e)
        {
            return e.Message.InsensitiveContains("constraint") || e.Message.InsensitiveContains("updatable");
        }
    }

    public enum TestInsertDeleteCaseResultStatus
    {
        Unknown,
        Failure,
        Skipped,
        Success
    }
}
