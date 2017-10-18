using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DesignTimeData
{
    public static class ItemDesignTimeData
    {
        private const string ItemDataCustomJson = @"[{`~`ITEM_R1`~`:6195,`~`ITEM_STAT`~`:1,`~`ZERO`~`:0,`~`ITEM`~`:`~` `~`,`~`PRODUCT_LINE`~`:4,`~`BITS`~`:0,`~`DESCRIPTION_1`~`:`~` `~`,`~`PRICE_1`~`:0.00000000,`~`PRICE_2`~`:0.00000000,`~`PRICE_3`~`:0.00000000,`~`PRICE_4`~`:0.00000000,`~`PRICE_5`~`:0.00000000,`~`PRICE_6`~`:0.00000000,`~`STOCK_WEIGHT`~`:0.00000000,`~`PURCHASE_FACTOR`~`:1.00000000,`~`PURCH_UNIT_MEAS`~`:`~`CS`~`,`~`STOCK_UNIT_MEAS`~`:`~`1`~`,`~`SCRAP_FACTOR`~`:0,`~`UNITS_PER_CASE`~`:1,`~`FREIGHT_CLASS`~`:0,`~`ITEM_CLASS`~`:`~`F`~`,`~`TAXABLE_FLAG`~`:`~`N`~`,`~`AVG_MATL_COST`~`:0.00000000,`~`STD_MATL_COST`~`:0.00000000,`~`INCR_LABOR_COST`~`:0.00000000,`~`INCR_BURDEN_COST`~`:0.00000000,`~`INCR_OUTSID_COST`~`:0.00000000,`~`INCR_LABOR_HOUR`~`:0.00000000,`~`CUM_LABOR_COST`~`:0.00000000,`~`CUM_BURDEN_COST`~`:0.00000000,`~`CUM_OUTSIDE_COST`~`:0.00000000,`~`CUM_LABOR_HOUR`~`:0.00000000,`~`MFG_DEPARTMANT`~`:`~` `~`,`~`VENDOR_PRODLN`~`:0,`~`VENDOR_ITEM`~`:`~` `~`,`~`LEAD_TIME_DAYS`~`:10,`~`REVISION_NUMBER`~`:0,`~`REVISION_DATE`~`:`~`2006-08-21T00:00:00`~`,`~`VENDOR`~`:0.00000000,`~`INVENTORY_CODE`~`:`~` `~`,`~`INVENTORIED_ITEM`~`:`~` `~`,`~`BOM_PARENT_R1`~`:0,`~`LOW_LEVEL_CODE`~`:0,`~`TEMP_ACUM_BUCKET`~`:0.00000000,`~`ENCOUNTER_CODE`~`:0.00000000,`~`PRICE_DIVISOR`~`:1,`~`COST_DIVISOR`~`:1,`~`ITEM_CUST_HIST`~`:`~`Y`~`,`~`DESCRIPTION_2`~`:`~` `~`,`~`DESCRIPTION_3`~`:`~` `~`,`~`EXTRA_COST_1`~`:0.00000000,`~`EXTRA_COST_2`~`:0.00000000,`~`EXTRA_COST_3`~`:0.00000000,`~`ITEM_COMMNT_R1`~`:0,`~`COST_PER_LB`~`:0.00000000,`~`SERIAL_ITEM`~`:`~`N`~`,`~`PRINT_DESC2_QUO`~`:`~` `~`,`~`PRINT_DESC2_SO`~`:`~` `~`,`~`PRINT_DESC2_INV`~`:`~` `~`,`~`PRINT_DESC2_PO`~`:`~` `~`,`~`PRINT_DESC3_QUO`~`:`~` `~`,`~`PRINT_DESC3_SO`~`:`~` `~`,`~`PRINT_DESC3_INV`~`:`~` `~`,`~`PRINT_DESC3_PO`~`:`~` `~`,`~`CREATION_DATE`~`:`~`2006-08-21T00:00:00`~`,`~`CREATION_COST`~`:0.00000000,`~`HAZARDOUS_MATL`~`:`~`N`~`,`~`HAZARDOUS_CODE`~`:`~` `~`,`~`REPORTABLE_QTY`~`:0.00000000,`~`PLACARD_CODE`~`:`~` `~`,`~`DATE_PRICE_1_CHG`~`:`~`1967-12-31T00:00:00`~`,`~`DATE_PRICE_2_CHG`~`:`~`1967-12-31T00:00:00`~`,`~`DATE_PRICE_3_CHG`~`:`~`1967-12-31T00:00:00`~`,`~`DATE_PRICE_4_CHG`~`:`~`1967-12-31T00:00:00`~`,`~`DATE_PRICE_5_CHG`~`:`~`1967-12-31T00:00:00`~`,`~`DATE_PRICE_6_CHG`~`:`~`1967-12-31T00:00:00`~`,`~`ORDER_QTY_FACTOR`~`:0.00000000,`~`PRICE_1_UPD_TYPE`~`:`~` `~`,`~`PRICE_1_UPD_SRC`~`:`~` `~`,`~`PRICE_1_UPD_PCNT`~`:0.00000000,`~`PRICE_2_UPD_TYPE`~`:`~` `~`,`~`PRICE_2_UPD_SRC`~`:`~` `~`,`~`PRICE_2_UPD_PCNT`~`:0.00000000,`~`PRICE_3_UPD_TYPE`~`:`~` `~`,`~`PRICE_3_UPD_SRC`~`:`~` `~`,`~`PRICE_3_UPD_PCNT`~`:0.00000000,`~`PRICE_4_UPD_TYPE`~`:`~` `~`,`~`PRICE_4_UPD_SRC`~`:`~` `~`,`~`PRICE_4_UPD_PCNT`~`:0.00000000,`~`PRICE_5_UPD_TYPE`~`:`~` `~`,`~`PRICE_5_UPD_SRC`~`:`~` `~`,`~`PRICE_5_UPD_PCNT`~`:0.00000000,`~`PRICE_6_UPD_TYPE`~`:`~` `~`,`~`PRICE_6_UPD_SRC`~`:`~` `~`,`~`PRICE_6_UPD_PCNT`~`:0.00000000,`~`USER_DEFINED_FLD`~`:`~` `~`,`~`CUR_MATL_COST`~`:0.00000000,`~`CUR_EXTRA_COST_1`~`:0.00000000,`~`CUR_EXTRA_COST_2`~`:0.00000000,`~`CUR_EXTRA_COST_3`~`:0.00000000,`~`CUR_INCR_LAB_CST`~`:0.00000000,`~`CUR_INCR_BUR_CST`~`:0.00000000,`~`CUR_INCR_OUT_CST`~`:0.00000000,`~`CUR_INCR_LAB_HRS`~`:0.00000000,`~`CUR_CUM_LAB_CST`~`:0.00000000,`~`CUR_CUM_BUR_CST`~`:0.00000000,`~`CUR_CUM_OUT_CST`~`:0.00000000,`~`CUR_CUM_LAB_HRS`~`:0.00000000,`~`COST_ROLLUP_FLAG`~`:`~` `~`,`~`FO_APPEND_PRICE`~`:0.00000000,`~`RECV_TO_INSPECT`~`:`~`N`~`,`~`CERT_REQUIRED`~`:`~`N`~`,`~`CERT_FORM_NAME`~`:`~` `~`,`~`SHAPE_CODE`~`:`~` `~`,`~`DEVALUE_COST`~`:0.00000000,`~`DEF_TAG_LABEL`~`:`~` `~`,`~`STD_WEIGHT`~`:0.00000000,`~`DEVALUE_SIZE`~`:0.00000000,`~`PACKAGE_PRICE`~`:`~`N`~`,`~`CARTON_BASED_PRICE`~`:`~`N`~`,`~`MINIMUM_PRICE`~`:0.00000000,`~`SETUP_PRICE`~`:0.00000000,`~`CONFIG_AT_ORD_TIME`~`:78,`~`PRODUCT_LABEL`~`:`~`N`~`,`~`OBSOLETE_ITEM`~`:`~`N`~`,`~`FO_APPEND_ITEM`~`:`~` `~`,`~`FO_APPEND_DESC`~`:`~` `~`,`~`AVERAGE_UPD_SRC`~`:`~` `~`,`~`AVERAGE_UPD_PCNT`~`:0.00000000,`~`STANDARD_UPD_SRC`~`:`~` `~`,`~`STANDARD_UPD_PCNT`~`:0.00000000,`~`CURRENT_UPD_SRC`~`:`~` `~`,`~`CURRENT_UPD_PCNT`~`:0.00000000,`~`EXTRA_1_UPD_SRC`~`:`~` `~`,`~`EXTRA_1_UPD_PCNT`~`:0.00000000,`~`EXTRA_2_UPD_SRC`~`:`~` `~`,`~`EXTRA_2_UPD_PCNT`~`:0.00000000,`~`EXTRA_3_UPD_SRC`~`:`~` `~`,`~`EXTRA_3_UPD_PCNT`~`:0.00000000,`~`LABOR_UPD_SRC`~`:`~` `~`,`~`LABOR_UPD_PCNT`~`:0.00000000,`~`BURDEN_UPD_SRC`~`:`~` `~`,`~`BURDEN_UPD_PCNT`~`:0.00000000,`~`OUTSIDE_UPD_SRC`~`:`~` `~`,`~`OUTSIDE_UPD_PCNT`~`:0.00000000,`~`STD_AVG_EXT1_UPDTE`~`:`~` `~`,`~`CURRENT_EXT1_UPDTE`~`:`~` `~`,`~`STD_AVG_EXT2_UPDTE`~`:`~` `~`,`~`CURRENT_EXT2_UPDTE`~`:`~` `~`,`~`STD_AVG_EXT3_UPDTE`~`:`~` `~`,`~`CURRENT_EXT3_UPDTE`~`:`~` `~`,`~`STD_AVG_LABOR_UPD`~`:`~` `~`,`~`CURRENT_LABOR_UPD`~`:`~` `~`,`~`STD_AVG_BURDEN_UPD`~`:`~` `~`,`~`CURRENT_BURDEN_UPD`~`:`~` `~`,`~`STD_AVG_OUTSD_UPD`~`:`~` `~`,`~`CURRENT_OUTSD_UPD`~`:`~` `~`,`~`USER_DEFINED_FLD2`~`:`~` `~`},{`~`ITEM_R1`~`:12791,`~`ITEM_STAT`~`:1,`~`ZERO`~`:0,`~`ITEM`~`:`~`..`~`,`~`PRODUCT_LINE`~`:97,`~`BITS`~`:0,`~`DESCRIPTION_1`~`:`~`PALLETS`~`,`~`PRICE_1`~`:0.00000000,`~`PRICE_2`~`:0.00000000,`~`PRICE_3`~`:0.00000000,`~`PRICE_4`~`:0.00000000,`~`PRICE_5`~`:0.00000000,`~`PRICE_6`~`:0.00000000,`~`STOCK_WEIGHT`~`:50.00000000,`~`PURCHASE_FACTOR`~`:1.00000000,`~`PURCH_UNIT_MEAS`~`:`~`EA`~`,`~`STOCK_UNIT_MEAS`~`:`~`EA`~`,`~`SCRAP_FACTOR`~`:0,`~`UNITS_PER_CASE`~`:1,`~`FREIGHT_CLASS`~`:0,`~`ITEM_CLASS`~`:`~`F`~`,`~`TAXABLE_FLAG`~`:`~`N`~`,`~`AVG_MATL_COST`~`:0.00000000,`~`STD_MATL_COST`~`:0.00000000,`~`INCR_LABOR_COST`~`:0.00000000,`~`INCR_BURDEN_COST`~`:0.00000000,`~`INCR_OUTSID_COST`~`:0.00000000,`~`INCR_LABOR_HOUR`~`:0.00000000,`~`CUM_LABOR_COST`~`:0.00000000,`~`CUM_BURDEN_COST`~`:0.00000000,`~`CUM_OUTSIDE_COST`~`:0.00000000,`~`CUM_LABOR_HOUR`~`:0.00000000,`~`MFG_DEPARTMANT`~`:`~` `~`,`~`VENDOR_PRODLN`~`:0,`~`VENDOR_ITEM`~`:`~` `~`,`~`LEAD_TIME_DAYS`~`:10,`~`REVISION_NUMBER`~`:0,`~`REVISION_DATE`~`:`~`2012-09-04T00:00:00`~`,`~`VENDOR`~`:0.00000000,`~`INVENTORY_CODE`~`:`~` `~`,`~`INVENTORIED_ITEM`~`:`~` `~`,`~`BOM_PARENT_R1`~`:0,`~`LOW_LEVEL_CODE`~`:0,`~`TEMP_ACUM_BUCKET`~`:0.00000000,`~`ENCOUNTER_CODE`~`:0.00000000,`~`PRICE_DIVISOR`~`:1,`~`COST_DIVISOR`~`:1,`~`ITEM_CUST_HIST`~`:`~`Y`~`,`~`DESCRIPTION_2`~`:`~` `~`,`~`DESCRIPTION_3`~`:`~` `~`,`~`EXTRA_COST_1`~`:0.00000000,`~`EXTRA_COST_2`~`:0.00000000,`~`EXTRA_COST_3`~`:0.00000000,`~`ITEM_COMMNT_R1`~`:0,`~`COST_PER_LB`~`:0.00000000,`~`SERIAL_ITEM`~`:`~`N`~`,`~`PRINT_DESC2_QUO`~`:`~` `~`,`~`PRINT_DESC2_SO`~`:`~` `~`,`~`PRINT_DESC2_INV`~`:`~` `~`,`~`PRINT_DESC2_PO`~`:`~` `~`,`~`PRINT_DESC3_QUO`~`:`~` `~`,`~`PRINT_DESC3_SO`~`:`~` `~`,`~`PRINT_DESC3_INV`~`:`~` `~`,`~`PRINT_DESC3_PO`~`:`~` `~`,`~`CREATION_DATE`~`:`~`2012-09-04T00:00:00`~`,`~`CREATION_COST`~`:0.00000000,`~`HAZARDOUS_MATL`~`:`~`N`~`,`~`HAZARDOUS_CODE`~`:`~` `~`,`~`REPORTABLE_QTY`~`:0.00000000,`~`PLACARD_CODE`~`:`~` `~`,`~`DATE_PRICE_1_CHG`~`:`~`1967-12-31T00:00:00`~`,`~`DATE_PRICE_2_CHG`~`:`~`1967-12-31T00:00:00`~`,`~`DATE_PRICE_3_CHG`~`:`~`1967-12-31T00:00:00`~`,`~`DATE_PRICE_4_CHG`~`:`~`1967-12-31T00:00:00`~`,`~`DATE_PRICE_5_CHG`~`:`~`1967-12-31T00:00:00`~`,`~`DATE_PRICE_6_CHG`~`:`~`1967-12-31T00:00:00`~`,`~`ORDER_QTY_FACTOR`~`:0.00000000,`~`PRICE_1_UPD_TYPE`~`:`~` `~`,`~`PRICE_1_UPD_SRC`~`:`~` `~`,`~`PRICE_1_UPD_PCNT`~`:0.00000000,`~`PRICE_2_UPD_TYPE`~`:`~` `~`,`~`PRICE_2_UPD_SRC`~`:`~` `~`,`~`PRICE_2_UPD_PCNT`~`:0.00000000,`~`PRICE_3_UPD_TYPE`~`:`~` `~`,`~`PRICE_3_UPD_SRC`~`:`~` `~`,`~`PRICE_3_UPD_PCNT`~`:0.00000000,`~`PRICE_4_UPD_TYPE`~`:`~` `~`,`~`PRICE_4_UPD_SRC`~`:`~` `~`,`~`PRICE_4_UPD_PCNT`~`:0.00000000,`~`PRICE_5_UPD_TYPE`~`:`~` `~`,`~`PRICE_5_UPD_SRC`~`:`~` `~`,`~`PRICE_5_UPD_PCNT`~`:0.00000000,`~`PRICE_6_UPD_TYPE`~`:`~` `~`,`~`PRICE_6_UPD_SRC`~`:`~` `~`,`~`PRICE_6_UPD_PCNT`~`:0.00000000,`~`USER_DEFINED_FLD`~`:`~` `~`,`~`CUR_MATL_COST`~`:0.00000000,`~`CUR_EXTRA_COST_1`~`:0.00000000,`~`CUR_EXTRA_COST_2`~`:0.00000000,`~`CUR_EXTRA_COST_3`~`:0.00000000,`~`CUR_INCR_LAB_CST`~`:0.00000000,`~`CUR_INCR_BUR_CST`~`:0.00000000,`~`CUR_INCR_OUT_CST`~`:0.00000000,`~`CUR_INCR_LAB_HRS`~`:0.00000000,`~`CUR_CUM_LAB_CST`~`:0.00000000,`~`CUR_CUM_BUR_CST`~`:0.00000000,`~`CUR_CUM_OUT_CST`~`:0.00000000,`~`CUR_CUM_LAB_HRS`~`:0.00000000,`~`COST_ROLLUP_FLAG`~`:`~` `~`,`~`FO_APPEND_PRICE`~`:0.00000000,`~`RECV_TO_INSPECT`~`:`~`N`~`,`~`CERT_REQUIRED`~`:`~`N`~`,`~`CERT_FORM_NAME`~`:`~` `~`,`~`SHAPE_CODE`~`:`~` `~`,`~`DEVALUE_COST`~`:0.00000000,`~`DEF_TAG_LABEL`~`:`~` `~`,`~`STD_WEIGHT`~`:0.00000000,`~`DEVALUE_SIZE`~`:0.00000000,`~`PACKAGE_PRICE`~`:`~`N`~`,`~`CARTON_BASED_PRICE`~`:`~`N`~`,`~`MINIMUM_PRICE`~`:0.00000000,`~`SETUP_PRICE`~`:0.00000000,`~`CONFIG_AT_ORD_TIME`~`:78,`~`PRODUCT_LABEL`~`:`~`N`~`,`~`OBSOLETE_ITEM`~`:`~`N`~`,`~`FO_APPEND_ITEM`~`:`~` `~`,`~`FO_APPEND_DESC`~`:`~` `~`,`~`AVERAGE_UPD_SRC`~`:`~` `~`,`~`AVERAGE_UPD_PCNT`~`:0.00000000,`~`STANDARD_UPD_SRC`~`:`~` `~`,`~`STANDARD_UPD_PCNT`~`:0.00000000,`~`CURRENT_UPD_SRC`~`:`~` `~`,`~`CURRENT_UPD_PCNT`~`:0.00000000,`~`EXTRA_1_UPD_SRC`~`:`~` `~`,`~`EXTRA_1_UPD_PCNT`~`:0.00000000,`~`EXTRA_2_UPD_SRC`~`:`~` `~`,`~`EXTRA_2_UPD_PCNT`~`:0.00000000,`~`EXTRA_3_UPD_SRC`~`:`~` `~`,`~`EXTRA_3_UPD_PCNT`~`:0.00000000,`~`LABOR_UPD_SRC`~`:`~` `~`,`~`LABOR_UPD_PCNT`~`:0.00000000,`~`BURDEN_UPD_SRC`~`:`~` `~`,`~`BURDEN_UPD_PCNT`~`:0.00000000,`~`OUTSIDE_UPD_SRC`~`:`~` `~`,`~`OUTSIDE_UPD_PCNT`~`:0.00000000,`~`STD_AVG_EXT1_UPDTE`~`:`~`N`~`,`~`CURRENT_EXT1_UPDTE`~`:`~`N`~`,`~`STD_AVG_EXT2_UPDTE`~`:`~`N`~`,`~`CURRENT_EXT2_UPDTE`~`:`~`N`~`,`~`STD_AVG_EXT3_UPDTE`~`:`~`N`~`,`~`CURRENT_EXT3_UPDTE`~`:`~`N`~`,`~`STD_AVG_LABOR_UPD`~`:`~`N`~`,`~`CURRENT_LABOR_UPD`~`:`~`N`~`,`~`STD_AVG_BURDEN_UPD`~`:`~`N`~`,`~`CURRENT_BURDEN_UPD`~`:`~`N`~`,`~`STD_AVG_OUTSD_UPD`~`:`~`N`~`,`~`CURRENT_OUTSD_UPD`~`:`~`N`~`,`~`USER_DEFINED_FLD2`~`:`~` `~`}]";

        public static Item Get1()
        {
            return Get().Take(1).First();
        }

        public static IList<Item> Get()
        {
            var normalJson = ItemDataCustomJson.Replace("`~`", "\"");
            var result = JsonConvert.DeserializeObject<List<Item>>(normalJson);
            return result;
        }
    }

    public class Item
    {
        public int? ITEM_R1 { get; set; }
        public int? ITEM_STAT { get; set; }
        public int? ZERO { get; set; }
        [StringLength(300)]
        public string ITEM { get; set; }
        public short? PRODUCT_LINE { get; set; }
        public short? BITS { get; set; }
        [StringLength(300)]
        public string DESCRIPTION_1 { get; set; }
        public decimal? PRICE_1 { get; set; }
        public decimal? PRICE_2 { get; set; }
        public decimal? PRICE_3 { get; set; }
        public decimal? PRICE_4 { get; set; }
        public decimal? PRICE_5 { get; set; }
        public decimal? PRICE_6 { get; set; }
        public decimal? STOCK_WEIGHT { get; set; }
        public decimal? PURCHASE_FACTOR { get; set; }
        [StringLength(300)]
        public string PURCH_UNIT_MEAS { get; set; }
        [StringLength(300)]
        public string STOCK_UNIT_MEAS { get; set; }
        public short? SCRAP_FACTOR { get; set; }
        public int? UNITS_PER_CASE { get; set; }
        public short? FREIGHT_CLASS { get; set; }
        [StringLength(300)]
        public string ITEM_CLASS { get; set; }
        [StringLength(300)]
        public string TAXABLE_FLAG { get; set; }
        public decimal? AVG_MATL_COST { get; set; }
        public decimal? STD_MATL_COST { get; set; }
        public decimal? INCR_LABOR_COST { get; set; }
        public decimal? INCR_BURDEN_COST { get; set; }
        public decimal? INCR_OUTSID_COST { get; set; }
        public decimal? INCR_LABOR_HOUR { get; set; }
        public decimal? CUM_LABOR_COST { get; set; }
        public decimal? CUM_BURDEN_COST { get; set; }
        public decimal? CUM_OUTSIDE_COST { get; set; }
        public decimal? CUM_LABOR_HOUR { get; set; }
        [StringLength(300)]
        public string MFG_DEPARTMANT { get; set; }
        public short? VENDOR_PRODLN { get; set; }
        [StringLength(300)]
        public string VENDOR_ITEM { get; set; }
        public int? LEAD_TIME_DAYS { get; set; }
        public short? REVISION_NUMBER { get; set; }
        public DateTime? REVISION_DATE { get; set; }
        public decimal? VENDOR { get; set; }
        [StringLength(300)]
        public string INVENTORY_CODE { get; set; }
        [StringLength(300)]
        public string INVENTORIED_ITEM { get; set; }
        public int? BOM_PARENT_R1 { get; set; }
        public short? LOW_LEVEL_CODE { get; set; }
        public decimal? TEMP_ACUM_BUCKET { get; set; }
        public decimal? ENCOUNTER_CODE { get; set; }
        public int? PRICE_DIVISOR { get; set; }
        public int? COST_DIVISOR { get; set; }
        [StringLength(300)]
        public string ITEM_CUST_HIST { get; set; }
        [StringLength(300)]
        public string DESCRIPTION_2 { get; set; }
        [StringLength(300)]
        public string DESCRIPTION_3 { get; set; }
        public decimal? EXTRA_COST_1 { get; set; }
        public decimal? EXTRA_COST_2 { get; set; }
        public decimal? EXTRA_COST_3 { get; set; }
        public int? ITEM_COMMNT_R1 { get; set; }
        public decimal? COST_PER_LB { get; set; }
        [StringLength(300)]
        public string SERIAL_ITEM { get; set; }
        [StringLength(300)]
        public string PRINT_DESC2_QUO { get; set; }
        [StringLength(300)]
        public string PRINT_DESC2_SO { get; set; }
        [StringLength(300)]
        public string PRINT_DESC2_INV { get; set; }
        [StringLength(300)]
        public string PRINT_DESC2_PO { get; set; }
        [StringLength(300)]
        public string PRINT_DESC3_QUO { get; set; }
        [StringLength(300)]
        public string PRINT_DESC3_SO { get; set; }
        [StringLength(300)]
        public string PRINT_DESC3_INV { get; set; }
        [StringLength(300)]
        public string PRINT_DESC3_PO { get; set; }
        public DateTime? CREATION_DATE { get; set; }
        public decimal? CREATION_COST { get; set; }
        [StringLength(300)]
        public string HAZARDOUS_MATL { get; set; }
        [StringLength(300)]
        public string HAZARDOUS_CODE { get; set; }
        public decimal? REPORTABLE_QTY { get; set; }
        [StringLength(300)]
        public string PLACARD_CODE { get; set; }
        public DateTime? DATE_PRICE_1_CHG { get; set; }
        public DateTime? DATE_PRICE_2_CHG { get; set; }
        public DateTime? DATE_PRICE_3_CHG { get; set; }
        public DateTime? DATE_PRICE_4_CHG { get; set; }
        public DateTime? DATE_PRICE_5_CHG { get; set; }
        public DateTime? DATE_PRICE_6_CHG { get; set; }
        public decimal? ORDER_QTY_FACTOR { get; set; }
        [StringLength(300)]
        public string PRICE_1_UPD_TYPE { get; set; }
        [StringLength(300)]
        public string PRICE_1_UPD_SRC { get; set; }
        public decimal? PRICE_1_UPD_PCNT { get; set; }
        [StringLength(300)]
        public string PRICE_2_UPD_TYPE { get; set; }
        [StringLength(300)]
        public string PRICE_2_UPD_SRC { get; set; }
        public decimal? PRICE_2_UPD_PCNT { get; set; }
        [StringLength(300)]
        public string PRICE_3_UPD_TYPE { get; set; }
        [StringLength(300)]
        public string PRICE_3_UPD_SRC { get; set; }
        public decimal? PRICE_3_UPD_PCNT { get; set; }
        [StringLength(300)]
        public string PRICE_4_UPD_TYPE { get; set; }
        [StringLength(300)]
        public string PRICE_4_UPD_SRC { get; set; }
        public decimal? PRICE_4_UPD_PCNT { get; set; }
        [StringLength(300)]
        public string PRICE_5_UPD_TYPE { get; set; }
        [StringLength(300)]
        public string PRICE_5_UPD_SRC { get; set; }
        public decimal? PRICE_5_UPD_PCNT { get; set; }
        [StringLength(300)]
        public string PRICE_6_UPD_TYPE { get; set; }
        [StringLength(300)]
        public string PRICE_6_UPD_SRC { get; set; }
        public decimal? PRICE_6_UPD_PCNT { get; set; }
        [StringLength(300)]
        public string USER_DEFINED_FLD { get; set; }
        public decimal? CUR_MATL_COST { get; set; }
        public decimal? CUR_EXTRA_COST_1 { get; set; }
        public decimal? CUR_EXTRA_COST_2 { get; set; }
        public decimal? CUR_EXTRA_COST_3 { get; set; }
        public decimal? CUR_INCR_LAB_CST { get; set; }
        public decimal? CUR_INCR_BUR_CST { get; set; }
        public decimal? CUR_INCR_OUT_CST { get; set; }
        public decimal? CUR_INCR_LAB_HRS { get; set; }
        public decimal? CUR_CUM_LAB_CST { get; set; }
        public decimal? CUR_CUM_BUR_CST { get; set; }
        public decimal? CUR_CUM_OUT_CST { get; set; }
        public decimal? CUR_CUM_LAB_HRS { get; set; }
        [StringLength(300)]
        public string COST_ROLLUP_FLAG { get; set; }
        public decimal? FO_APPEND_PRICE { get; set; }
        [StringLength(300)]
        public string RECV_TO_INSPECT { get; set; }
        [StringLength(300)]
        public string CERT_REQUIRED { get; set; }
        [StringLength(300)]
        public string CERT_FORM_NAME { get; set; }
        [StringLength(300)]
        public string SHAPE_CODE { get; set; }
        public decimal? DEVALUE_COST { get; set; }
        [StringLength(300)]
        public string DEF_TAG_LABEL { get; set; }
        public decimal? STD_WEIGHT { get; set; }
        public decimal? DEVALUE_SIZE { get; set; }
        [StringLength(300)]
        public string PACKAGE_PRICE { get; set; }
        [StringLength(300)]
        public string CARTON_BASED_PRICE { get; set; }
        public decimal? MINIMUM_PRICE { get; set; }
        public decimal? SETUP_PRICE { get; set; }
        public short? CONFIG_AT_ORD_TIME { get; set; }
        [StringLength(300)]
        public string PRODUCT_LABEL { get; set; }
        [StringLength(300)]
        public string OBSOLETE_ITEM { get; set; }
        [StringLength(300)]
        public string FO_APPEND_ITEM { get; set; }
        [StringLength(300)]
        public string FO_APPEND_DESC { get; set; }
        [StringLength(300)]
        public string AVERAGE_UPD_SRC { get; set; }
        public decimal? AVERAGE_UPD_PCNT { get; set; }
        [StringLength(300)]
        public string STANDARD_UPD_SRC { get; set; }
        public decimal? STANDARD_UPD_PCNT { get; set; }
        [StringLength(300)]
        public string CURRENT_UPD_SRC { get; set; }
        public decimal? CURRENT_UPD_PCNT { get; set; }
        [StringLength(300)]
        public string EXTRA_1_UPD_SRC { get; set; }
        public decimal? EXTRA_1_UPD_PCNT { get; set; }
        [StringLength(300)]
        public string EXTRA_2_UPD_SRC { get; set; }
        public decimal? EXTRA_2_UPD_PCNT { get; set; }
        [StringLength(300)]
        public string EXTRA_3_UPD_SRC { get; set; }
        public decimal? EXTRA_3_UPD_PCNT { get; set; }
        [StringLength(300)]
        public string LABOR_UPD_SRC { get; set; }
        public decimal? LABOR_UPD_PCNT { get; set; }
        [StringLength(300)]
        public string BURDEN_UPD_SRC { get; set; }
        public decimal? BURDEN_UPD_PCNT { get; set; }
        [StringLength(300)]
        public string OUTSIDE_UPD_SRC { get; set; }
        public decimal? OUTSIDE_UPD_PCNT { get; set; }
        [StringLength(300)]
        public string STD_AVG_EXT1_UPDTE { get; set; }
        [StringLength(300)]
        public string CURRENT_EXT1_UPDTE { get; set; }
        [StringLength(300)]
        public string STD_AVG_EXT2_UPDTE { get; set; }
        [StringLength(300)]
        public string CURRENT_EXT2_UPDTE { get; set; }
        [StringLength(300)]
        public string STD_AVG_EXT3_UPDTE { get; set; }
        [StringLength(300)]
        public string CURRENT_EXT3_UPDTE { get; set; }
        [StringLength(300)]
        public string STD_AVG_LABOR_UPD { get; set; }
        [StringLength(300)]
        public string CURRENT_LABOR_UPD { get; set; }
        [StringLength(300)]
        public string STD_AVG_BURDEN_UPD { get; set; }
        [StringLength(300)]
        public string CURRENT_BURDEN_UPD { get; set; }
        [StringLength(300)]
        public string STD_AVG_OUTSD_UPD { get; set; }
        [StringLength(300)]
        public string CURRENT_OUTSD_UPD { get; set; }
        [StringLength(300)]
        public string USER_DEFINED_FLD2 { get; set; }
    }

}
