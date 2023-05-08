using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Configuration;
using System.Web.Security;

public partial class private_ms__Starter : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES 
    // ---------------------------------------
    //    SqlConnection sqlConn;
    //    SqlCommand sqlCmd;
    //    SqlDataReader sqlReader;

    //    OdbcConnection odbcConn;
    //    OdbcCommand odbcCmd;
    //    OdbcDataReader odbcReader;

    ErrorHandler eh;
    //    NumberFormatter nf;
    char[] cSplitter = { '|' };
    string sConnectionString = "";
    string sSql = "";
    // ========================================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        // sConnectionString = ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString;
        // odbcConn = new OdbcConnection(sConnectionString);

        // sConnectionString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;
        // sqlConn = new SqlConnection(sConnectionString);

        if (!IsPostBack)
        {
            // if (Request.QueryString["courseName"] != null && Request.QueryString["courseName"].ToString() != "")
            // hfCourseName.Value = Request.QueryString["courseName"].ToString().Trim();

            //if (Session["ReviewSearchCs1"] != null)
            //    txSearchCs1.Text = Session["ReviewSearchCs1"].ToString().Trim();

            try
            {
                GetUser();

                //odbcConn.Open();
                //sqlConn.Open();

                //ddTest.DataSource = GetData();
                //ddTest.DataTextField = "Text";
                //ddTest.DataValueField = "Value";
                //ddTest.DataBind();
                //ddTest.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", ""));

            }
            catch (Exception ex)
            {
                eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
                //odbcConn.Close();
                //sqlConn.Close();
            }

        }
    }
    // ========================================================================
    #region mySqls
    // ========================================================================
    protected DataTable GetDetail(int headerKey)
    {
        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        try
        {
            sSql = "Select" +
                " cKey" +
                ", cSeq" +
                ", IFNULL((select PEMLIF from " + sLibrary + ".PRODEQP where PEPART = c.cPartName and PEPART <> ''),0) as cartridgePages" +
                //", ISNULL((select PEMLIF from PRODEQP where PEPART = c.cPartName and PEPART <> ''),0) as cartridgePages" +
                " from " + sLibrary + ".MPUCAR c, " + sLibrary + ".MPUHD m " +
                " where hKey = cKey" +
                " and cKey = ?" +
                " order by cSeq";

            //odbcCmd = new OdbcCommand(sSql, odbcConn);
            //odbcCmd.Parameters.AddWithValue("@Key", headerKey);
            //odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            //dt.Load(odbcReader);

            //sqlCmd = new SqlCommand(sSql, sqlConn);
            //sqlCmd.Parameters.AddWithValue("@Key", headerKey);
            //sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default);
            //dt.Load(sqlReader);

            // dt.Columns.Add(MakeColumn("New"));

            //string sTemp = "";
            //int iRowIdx = 0;
            //foreach (DataRow row in dt.Rows)
            //{
            //    sTemp = dt.Rows[iRowIdx]["Temp"].ToString().Trim();
            //    iRowIdx++;
            //}

            //dt.Columns["CKEY"].ColumnName = "Key";
            // Remove unnecessary columns
            //dt.Columns.Remove("WEERR");
            // dt.AcceptChanges();

        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            //odbcCmd.Dispose();
            //sqlCmd.Dispose();
        }
        return dt;
    }
    // ========================================================================
    protected void DoSqls()
    {
        try
        {
            //odbcConn.Open();
            //sqlConn.Open();
        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            //odbcConn.Close();
            //sqlConn.Close();
        }
    }
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    #region misc
    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { eh = new ErrorHandler(); }
    // ========================================================================
    protected void Page_Unload(object sender, EventArgs e) { eh = null; }
    // ========================================================================
    protected void GetUser()
    {
        int iHtsNum = 0;
        if (User.Identity.IsAuthenticated)
        {
            if (int.TryParse(Profile.LoginEmpNum.ToString(), out iHtsNum) == false)
            {
                iHtsNum = 0;
            }
            hfHtsNum.Value = iHtsNum.ToString();
            MembershipUser mu = Membership.GetUser();
            hfUserName.Value = mu.UserName.ToString().ToLower();
        }
    }
    // ========================================================================
    #endregion // end misc
    // ========================================================================
    #region tableChanges
    // =========================================================
    /*
    protected DataTable getStarter_gv()
    {
        HiddenField hfTemp = new HiddenField();
        Label lbTemp = new Label();
        TextBox txTemp = new TextBox();
        HyperLink hlTemp = new HyperLink();
        DataControlFieldCell dcfcTemp;

        string sType = "";
        string sQty = "";
        string sPart = "";
        string sDescription = "";
        string sComment = "";
        int iSelected = 0;

        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        dt.Columns.Add(MakeColumn("Qty"));
        dt.Columns.Add(MakeColumn("Description"));
        dt.Columns.Add(MakeColumn("Part"));
        dt.Columns.Add(MakeColumn("Comment"));

        int iRowIdx = 0;

        try
        {
            foreach (Control c1 in gvStarter.Controls)
            {
                sType = c1.GetType().ToString();
                if (c1.GetType().ToString().Equals("System.Web.UI.WebControls.ChildTable"))
                {
                    foreach (Control c2 in c1.Controls)
                    {
                        sType = c2.GetType().ToString();
                        if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.GridViewRow"))
                        {
                            foreach (Control c3 in c2.Controls)
                            {
                                sType = c3.GetType().ToString();
                                if (c3.GetType().ToString().Equals("System.Web.UI.WebControls.DataControlFieldCell"))
                                {
                                    foreach (Control c4 in c3.Controls)
                                    {
                                        sType = c4.GetType().ToString();
                                        // -------------------------------------
                                        if (c4.GetType().ToString().Equals("System.Web.UI.WebControls.HyperLink"))
                                        {
                                            hlTemp = (HyperLink)c4;
                                            if (hlTemp.ID == "hlDescription" && iSelected == 1)
                                            {
                                                sDescription = hlTemp.Text;
                                            }
                                        }
                                        // -------------------------------------
                                        if (c4.GetType().ToString().Equals("System.Web.UI.WebControls.Label"))
                                        {
                                            lbTemp = (Label)c4;
                                            if (lbTemp.ID == "lbPart" && iSelected == 1)
                                            {
                                                sPart = lbTemp.Text.Trim();
                                            }
                                        }
                                        // -------------------------------------
                                        if (c4.GetType().ToString().Equals("System.Web.UI.WebControls.TextBox"))
                                        {
                                            txTemp = (TextBox)c4;
                                            if (txTemp.ID == "txQty")
                                            {
                                                iSelected = 0;
                                                sQty = txTemp.Text;
                                                if (sQty != "")
                                                {
                                                    iSelected = 1;
                                                }
                                            }
                                            if (txTemp.ID == "txComment" && iSelected == 1)
                                            {
                                                sComment = txTemp.Text;
                                                DataRow dr = dt.NewRow();
                                                dt.Rows.Add(dr);
                                                dt.Rows[iRowIdx]["Qty"] = sQty;
                                                dt.Rows[iRowIdx]["Description"] = sDescription;
                                                dt.Rows[iRowIdx]["Part"] = sPart;
                                                dt.Rows[iRowIdx]["Comment"] = sComment;
                                                dt.AcceptChanges();
                                                iRowIdx++;
                                            }
                                        }
                                        // -------------------------------------
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
        }
        return dt;
    }
     */
    // =========================================================
    protected void UpdDisplay_rp()
    {
        /*
        // ((RepeaterItem)rpSummary.Controls[i]).NamingContainer.ToString();
        HiddenField hfTemp = new HiddenField();
        HyperLink hlTemp = new HyperLink();
        DateTime datTemp = new DateTime();
        TimeSpan ts = new TimeSpan();
        string sLastApproval = "";
        string sType = "";

        foreach (Control c1 in rpSummary.Controls)
        {
            sType = c1.GetType().ToString();
            if (c1.GetType().ToString().Equals("System.Web.UI.WebControls.RepeaterItem"))
            {
                foreach (Control c2 in c1.Controls)
                {
                    sType = c2.GetType().ToString();
                    //-------------------------------------------------------------------------
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.HiddenField"))
                    {
                        hfTemp = (HiddenField)c2;
                        if (hfTemp.ID == "hfLastApproval")
                            sLastApproval = hfTemp.Value;
                    }
                    //-------------------------------------------------------------------------
                    if (c2.GetType().ToString().Equals("System.Web.UI.WebControls.HyperLink"))
                    {
                        hlTemp = (HyperLink)c2;
                        if (hlTemp.ID == "hlTitle")
                        {
                            if (hlTemp.Text.Trim() == "Available Services" || hlTemp.Text.Trim() == "Reviewer's Notes")
                                hlTemp.ForeColor = myGreen;
                            else if (DateTime.TryParse(sLastApproval, out datTemp) == true)
                            {
                                ts = DateTime.Now - datTemp;
                                if (ts.Days <= iMaxReviewDays)
                                {
                                    hlTemp.ForeColor = myGreen;
                                }
                            }
                            sLastApproval = "";
                        }
                    }
                    //-------------------------------------------------------------------------
                    //-------------------------------------------------------------------------
                }
            }
        }
        */
    }
    // =========================================================
    #endregion // end tableChanges
    // ========================================================================
    // ========================================================================
    #region actionEvents
    // ========================================================================
    protected void btAction_Click(object sender, EventArgs e)
    {
        Button myControl = (Button)sender;
        string sParms = myControl.CommandArgument.ToString().Trim();
        char[] cSplitter = { '|' };
        string[] saParms = new string[1];
        saParms = sParms.Split(cSplitter);

        //HyperLink hlUnit = myControl.NamingContainer.FindControl("hlUnit") as HyperLink;
        //TextBox txEndMeter = myControl.NamingContainer.FindControl("txEndMeter") as TextBox;
        //TextBox txPages = myControl.NamingContainer.FindControl("txPages") as TextBox;

        string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values";
        DataTable dt = new DataTable(sMethodName);

        try
        {
            //odbcConn.Open();
            //sqlConn.Open();
        }
        catch (Exception ex)
        {
            eh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            //odbcConn.Close();
            //sqlConn.Close();
        }
    }
    // ========================================================================
    #endregion // end actionEvents
    // ========================================================================
    // ========================================================================
}