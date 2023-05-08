<%@ Page Language="C#" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["adminCs1"] != null)
            Session["adminCs1"] = null;
        if (Session["processing"] != null)
            Session["processing"] = null;
        if (Session["reqKey"] != null)
            Session["reqKey"] = null;
        if (Session["ssTechNum"] != null)
            Session["ssTechNum"] = null;
        if (Session["ssStockLoc"] != null)
            Session["ssStockLoc"] = null;
        FormsAuthentication.SignOut();
        //Response.Redirect("~/Login.aspx", false);
        Response.Redirect("~/Login.aspx?ReturnUrl=/private/shared/menu.aspx", false);
        //Response.End();
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
