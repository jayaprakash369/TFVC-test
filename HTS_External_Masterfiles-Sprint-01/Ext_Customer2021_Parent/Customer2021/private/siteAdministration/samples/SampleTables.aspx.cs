using System;
using System.Data;
//using System.Collections.Generic;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

public partial class private_siteAdministration_samples_SampleTables : MyPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        rp_Small.DataSource = LoadDataTable();
        rp_Small.DataBind();

        rp_Large.DataSource = LoadDataTable();
        rp_Large.DataBind();

    }
    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e)
    {
        this.RequireSSL = true;
    }
    // ========================================================================
    #endregion // end misc
    // ========================================================================
    // -------------------------------------------------
    protected DataTable LoadDataTable()
    {

        string[] saAA = { "A1 Here is some text", "A2", "A3", "A4", "A5" }; // Column A
        string[] saBB = { "B1 More text on the B Level", "B2", "B3", "B4", "B5" }; // Column B
        string[] saCC = { "C1 Will have a shorter entry", "C2", "C3", "C4", "C5" }; // Column C
        string[] saDD = { "D1 Will have the longest of all of these", "D2", "D3", "D4", "D5" }; // Column D

        DataTable dt = new DataTable("Sample");

        dt.Columns.Add(MakeColumn("AA"));
        dt.Columns.Add(MakeColumn("BB"));
        dt.Columns.Add(MakeColumn("CC"));
        dt.Columns.Add(MakeColumn("DD"));

        DataRow dr;
        int iSeq = 0;
        for (int i = 0; i < saAA.Length; i++)
        {
            dr = dt.NewRow();
            dt.Rows.Add(dr);

            dt.Rows[iSeq]["AA"] = saAA[i];
            dt.Rows[iSeq]["BB"] = saBB[i];
            dt.Rows[iSeq]["CC"] = saCC[i];
            dt.Rows[iSeq]["DD"] = saDD[i];

            iSeq++;
        }
        dt.AcceptChanges();


        return dt;
    }

    // -------------------------------------------------
    // -------------------------------------------------

}
