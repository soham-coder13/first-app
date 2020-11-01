using System.Data;

namespace Dal
{
    public class Promotion
    {
        public DataTable GetDiscounts()
        {
            DataTable dtDiscount = new DataTable();

            dtDiscount.Columns.Add("Item");
            dtDiscount.Columns.Add("Qty");
            dtDiscount.Columns.Add("Discount");

            DataRow dr = dtDiscount.NewRow();
            dr["Item"] = "A";
            dr["Qty"] = 3;
            dr["Discount"] = 130.0;
            dtDiscount.Rows.Add(dr);
            dr = dtDiscount.NewRow();
            dr["Item"] = "B";
            dr["Qty"] = 2;
            dr["Discount"] = 45.0;
            dtDiscount.Rows.Add(dr);
            dr = dtDiscount.NewRow();
            dr["Item"] = "C;D";
            dr["Qty"] = 1;
            dr["Discount"] = 30.0;
            dtDiscount.Rows.Add(dr);
            dr = dtDiscount.NewRow();

            return dtDiscount;
        }
    }
}
