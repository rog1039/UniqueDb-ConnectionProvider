using Dapper;
using NUnit.Framework;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;

namespace UniqueDb.ConnectionProvider.Tests;

[TestFixture]
public class SimpleDataQueryTests
{
   [Test]
   public async Task SimpleQuery()
   {
      var scp     = new StaticSqlConnectionProvider("WS2016Sql", "Pbsi");
      var conn    = scp.GetSqlConnection();
      var results = await conn.QueryAsync<OrderHead>("SELECT TOP 10 * FROM PBSIWM.ORDER_HEAD");
      results.ToList().PrintStringTable();
   }
}

internal class OrderHead
{
   public int?      ORDER_HEAD_R1    { get; set; }
   public int?      ORDER_HEAD_STAT  { get; set; }
   public int?      ORDER            { get; set; }
   public string?   ORDER_TYPE       { get; set; }
   public string?   ORDER_STATUS     { get; set; }
   public decimal?  SOLDTO           { get; set; }
   public decimal?  SHIPTO           { get; set; }
   public string?   CUSTOMER_TYPE    { get; set; }
   public int?      CLASS            { get; set; }
   public short?    REGION           { get; set; }
   public int?      AREA             { get; set; }
   public int?      SOLDTO_SLSPERSON { get; set; }
   public int?      TAX_AUTH         { get; set; }
   public int?      SHIPTO_SLSPERSON { get; set; }
   public decimal?  TAX_PERCENT      { get; set; }
   public string?   COMMISSION_TYPE  { get; set; }
   public DateTime? ENTRY_DATE       { get; set; }
   public DateTime? PROMISE_DATE     { get; set; }
   public DateTime? INVOICE_DATE     { get; set; }
   public DateTime? SHIP_DATE        { get; set; }
   public int?      TERMS            { get; set; }
   public int?      DISCOUNT         { get; set; }
   public short?    PRICE_LIST       { get; set; }
   public int?      WAREHOUSE        { get; set; }
   public short?    BACKORDER        { get; set; }
   public int?      NUMBER_LINES     { get; set; }
   public decimal?  VALUE_LINES      { get; set; }
   public int?      AUTHORIZATION    { get; set; }
   public string?   CUSTOMER_PO      { get; set; }
   public string?   CARRIER          { get; set; }
   public int?      FREIGHT_CODE     { get; set; }
   public decimal?  FREIGHT_AMOUNT   { get; set; }
   public string?   SPL_CHRG_DESC    { get; set; }
   public decimal?  SPL_CHRG_AMOUNT  { get; set; }
   public short?    SPL_CHRG_CODE    { get; set; }
   public DateTime? INVOICE_DUE_DATE { get; set; }
   public int?      SPL_PRICE_BREAK  { get; set; }
   public decimal?  BILLED_TO_DATE   { get; set; }
   public decimal?  LAST_BILL_AMT    { get; set; }
   public string?   PROGRESS_BILL    { get; set; }
   public decimal?  PERCENT_TODAY    { get; set; }
   public string?   USER_ACCOUNT     { get; set; }
   public string?   STATUS_FLAG      { get; set; }
   public string?   FREIGHT_OVERRIDE { get; set; }
   public string?   INVOICE_HOLD     { get; set; }
   public string?   TAX_COUNTRY      { get; set; }
   public string?   TAX_SUBDIVISION  { get; set; }
   public string?   ROUND_TAX        { get; set; }
   public string?   FREIGHT_TAXABLE  { get; set; }
}