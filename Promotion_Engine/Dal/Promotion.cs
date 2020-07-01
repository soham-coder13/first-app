using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    class Promotion
    {
        public DataTable GetDiscounts()
        {
            DataTable dtDiscount = new DataTable();

            dtDiscount.Columns.Add("Item");
            dtDiscount.Columns.Add("Quantity");
            dtDiscount.Columns.Add("DiscountedPrice");

            DataRow dr = dtDiscount.NewRow();
            dr["Item"] = "A";
            dr["Item"] = 3;
            dr["Discount"] = 130.0;
            dtDiscount.Rows.Add(dr);
            dr = dtDiscount.NewRow();
            dr["Item"] = "B";
            dr["Item"] = 2;
            dr["Discount"] = 45.0;
            dtDiscount.Rows.Add(dr);
            dr = dtDiscount.NewRow();
            dr["Item"] = "C;D";
            dr["Item"] = 1;
            dr["Discount"] = 30.0;
            dtDiscount.Rows.Add(dr);
            dr = dtDiscount.NewRow();

            return dtDiscount;
        }
    }
}
