using System.Data;

namespace Dal
{
    public class ItemPrice
    {
        public DataTable GetItemPrice()
        {
            DataTable dtPrice = new DataTable();

            dtPrice.Columns.Add("Item");
            dtPrice.Columns.Add("Price");

            DataRow dr = dtPrice.NewRow();
            dr["Item"] = "A";
            dr["Price"] = 50.0;
            dtPrice.Rows.Add(dr);
            dr = dtPrice.NewRow();
            dr["Item"] = "B";
            dr["Price"] = 30.0;
            dtPrice.Rows.Add(dr);
            dr = dtPrice.NewRow();
            dr["Item"] = "C";
            dr["Price"] = 20.0;
            dtPrice.Rows.Add(dr);
            dr = dtPrice.NewRow();
            dr["Item"] = "D";
            dr["Price"] = 15.0;
            dtPrice.Rows.Add(dr);

            return dtPrice;
        }
    }
}
