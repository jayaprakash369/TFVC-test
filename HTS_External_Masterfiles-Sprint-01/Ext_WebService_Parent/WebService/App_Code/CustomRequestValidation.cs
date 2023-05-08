using System;
using System.Web;
using System.Web.Util;
// the original source for this class was found at https://msdn.microsoft.com/en-us/library/system.web.util.requestvalidator.aspx
public class CustomRequestValidation : RequestValidator
{
    public CustomRequestValidation() { }

    protected override bool IsValidRequestString(
        HttpContext context, string value,
        RequestValidationSource requestValidationSource, string collectionKey,
        out int validationFailureIndex)
    {
        validationFailureIndex = -1;  //Set a default value for the out parameter.

        //This application does not use RawUrl directly so you can ignore the check.
        if (requestValidationSource == RequestValidationSource.RawUrl)
            return true;

        string sIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        //if (sIpAddress == "156.77.79.21" || sIpAddress == "10.41.30.9" || sIpAddress == "10.41.28.179" || sIpAddress == "::1")

        if ((requestValidationSource == RequestValidationSource.Form) &&
            (collectionKey == "request"))
        {
            //The form parameter "request" value xml is allowed.
            //if (value.StartsWith("<?xml"))
            //if (sIpAddress == "10.41.28.179")
            //if (sIpAddress == "10.41.28.179" && value.StartsWith("<?xml"))
            //if (value.StartsWith("<?xml"))
            // endswith </serviceDocument>
            // if(value.ToLower().StartsWith("<?xml version=\"1.0\" encoding=\"utf-8\"?>"))
            //if ((sIpAddress == "156.77.79.21" || sIpAddress == "10.41.30.9" || sIpAddress == "10.41.28.179" || sIpAddress == "::1") 
            //    && (value.ToLower().StartsWith("<?xml version=\"1.0\" encoding=\"utf-8\"?>")))
            //if(value.ToLower().StartsWith("<?xml version=\"1.0\" encoding=\"utf-8\"?>"))
            if(value.ToLower().StartsWith("<?xml"))
            {
                validationFailureIndex = -1;
                return true;
            }
            else
                //Leave any further checks to ASP.NET.
                return base.IsValidRequestString(context, value,
                requestValidationSource,
                collectionKey, out validationFailureIndex);
        }
        //All other HTTP input checks are left to the base ASP.NET implementation.
        else
        {
            return base.IsValidRequestString(context, value, requestValidationSource,
                                             collectionKey, out validationFailureIndex);
        }
    }
}