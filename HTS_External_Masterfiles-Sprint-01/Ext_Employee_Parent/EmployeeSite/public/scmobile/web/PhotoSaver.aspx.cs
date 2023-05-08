using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.Odbc;
using System.Configuration;
using System.Web.Security;
using System.Drawing;

public partial class public_scmobile_web_PhotoSaver : MyPage
{
    // ---------------------------------------
    // GLOBAL VARIABLES 
    // ---------------------------------------
    // string sLibrary = "OMDTALIB";
    OdbcConnection odbcConn;
    OdbcCommand odbcCmd;
    OdbcDataReader odbcReader;

    private string sCenter = "";
    private string sTicket = "";
    private string sPhotoName = "";
    private string sPhotoText = "";

    ErrorHandler erh;

    // ========================================================================
    protected void Page_Init(object sender, EventArgs e) { erh = new ErrorHandler(); this.RequireSSL = false; }
    // ========================================================================
    protected void Page_Unload(object sender, EventArgs e) { erh = null; }
    // ========================================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        string sResponseToDevice = "REACHED STARTUP";

        odbcConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString);

        if (!IsPostBack)
        {
            //DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

            if (Request.Form["center"] != null && Request.Form["center"].ToString() != "")
                sCenter = Request.Form["center"].ToString().Trim();

            if (Request.Form["ticket"] != null && Request.Form["ticket"].ToString() != "")
                sTicket = Request.Form["ticket"].ToString().Trim();

            if (Request.Form["photoName"] != null && Request.Form["photoName"].ToString() != "")
                sPhotoName = Request.Form["photoName"].ToString().Trim();

            if (Request.Form["photoText"] != null && Request.Form["photoText"].ToString() != "")
                sPhotoText = Request.Form["photoText"].ToString().Trim();

            try
            {
                odbcConn.Open();
                using (Stream receiveStream = Request.InputStream)
                {
                    sResponseToDevice = Upload(receiveStream);
                }
            }
            catch (Exception ex)
            {
                erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
            }
            finally
            {
                odbcConn.Close();
            }
            Response.Write(sResponseToDevice);
            Response.End();
        }
    }
    // ----------------------------------------------------------------------
    public string Upload(Stream stream)
    {
        string sResponseToDevice = "REACHED UPLOAD";

        MultipartParser parser = new MultipartParser(stream);

        if (parser.Success)
        {
            // Save the file
            sResponseToDevice = SaveFile(parser.Filename, parser.ContentType, parser.FileContents); // string, string byte[]
        }
        else
        {
            //throw new System.Net.WebException(System.Net.HttpStatusCode.UnsupportedMediaType, "The posted file was not recognized");
            throw new System.Net.WebException("The binary stream was not successfully converted into an image");
        }
        return sResponseToDevice;
    }
    // ----------------------------------------------------------------------
    public string SaveFile(string fileName, string fileType, byte[] imageArray)
    {
        string sResponseToDevice = "REACHED SAVEFILE";

        try
        {
            // Save the binary data as a .jpg image
            string sRootDir = Server.MapPath("~");
            //string sFilename = "Photo1.jpg";
            string sFilename = fileName;
            //string sFullPath = sRootDir + @"public\scmobile\web\" + sFilename;
            string sCallFolder = sCenter + "-" + sTicket;
            string sSubDir = sRootDir + @"media\images\call\" + sCallFolder;

            bool exists = System.IO.Directory.Exists(sSubDir);
            if (!exists)
                System.IO.Directory.CreateDirectory(sSubDir);

            string sFullPath = sSubDir + @"\" + sFilename;

            File.WriteAllBytes(sFullPath, imageArray);

            // Flip the image 90 degrees
            using (Image image = Image.FromFile(sFullPath))
            {
                image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                image.Save(sFullPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                image.Dispose();
            }

            // You also need to save the fileText to a database somewhere...
            // sPhotoText
            int iCtr = 0;
            int iTck = 0;
            int iSeq = 0;
            int iRowsAffected = 0;

            if (int.TryParse(sCenter, out iCtr) == false)
                iCtr = -1;
            if (int.TryParse(sTicket, out iTck) == false)
                iTck = -1;

            if (fileName.EndsWith("1.jpg"))
                iSeq = 1;
            else if (fileName.EndsWith("2.jpg"))
                iSeq = 2;
            else if (fileName.EndsWith("3.jpg"))
                iSeq = 3;

            if (iCtr > 0 && iTck > 0 && iSeq > 0)
            {
                DataTable dt = Select_ImageText(iCtr, iTck);
                if (dt.Rows.Count > 0)
                {
                    iRowsAffected = Update_ImageText(iCtr, iTck, iSeq, sPhotoText);
                }
                else
                {
                    string sLd1 = "";
                    string sTx1 = "";
                    string sLd2 = "";
                    string sTx2 = "";
                    string sLd3 = "";
                    string sTx3 = "";
                    if (iSeq == 1) {
                        sLd1 = "Y";
                        sTx1 = sPhotoText;
                    }
                    if (iSeq == 2)
                    {
                        sLd2 = "Y";
                        sTx2 = sPhotoText;
                    }
                    if (iSeq == 3)
                    {
                        sLd3 = "Y";
                        sTx3 = sPhotoText;
                    }

                    iRowsAffected = Insert_ImageText(iCtr, iTck, sLd1, sTx1, sLd2, sTx2, sLd3, sTx3);
                }
            }
            sResponseToDevice = "SUCCESS";
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }

        return sResponseToDevice;

    }
    // ========================================================================
    #region mySqls
    // ========================================================================
    protected DataTable Select_ImageText(int center, int ticket)
    {
        string[] saImgTxt = { "" };

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            string sSql = "Select" +
                 " CILD1" +
                ", CITX1" +
                ", CILD2" +
                ", CITX2" +
                ", CILD3" +
                ", CITX3" +
                " from " + sLibrary + ".CALIMGTXT" +
                " where CICTR = ?" +
                " and CITCK = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Ctr", center);
            odbcCmd.Parameters.AddWithValue("@Tck", ticket);
            odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default);
            dt.Load(odbcReader);
            string sTextList = "";
            if (dt.Rows.Count > 0)
            {
                sTextList += dt.Rows[0]["CILD1"].ToString() + "|";
                sTextList += dt.Rows[0]["CITX1"].ToString() + "|";
                sTextList += dt.Rows[0]["CILD2"].ToString() + "|";
                sTextList += dt.Rows[0]["CITX2"].ToString() + "|";
                sTextList += dt.Rows[0]["CILD3"].ToString() + "|";
                sTextList += dt.Rows[0]["CITX3"].ToString() + "|X";  // final end value to protect array size if blanks exist
            }
            saImgTxt = sTextList.Split('|');
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return dt;
    }
    // ========================================================================
    protected int Insert_ImageText(int center, int ticket, string image1Loaded, string image1Text, string image2Loaded, string image2Text, string image3Loaded, string image3Text)
    {
        int iRowsAffected = 0;
        try
        {
            if (image1Text.Length > 300)
                image1Text = image1Text.Substring(0, 300);
            if (image2Text.Length > 300)
                image2Text = image2Text.Substring(0, 300);
            if (image3Text.Length > 300)
                image3Text = image3Text.Substring(0, 300);

            string sSql = "insert into " + sLibrary + ".CALIMGTXT" +
                " (CICTR, CITCK, CILD1, CITX1, CILD2, CITX2, CILD3, CITX3)" +
                " values(?, ?, ?, ?, ?, ?, ?, ?)";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Ctr", center);
            odbcCmd.Parameters.AddWithValue("@Tck", ticket);
            odbcCmd.Parameters.AddWithValue("@Ld1", image1Loaded);
            odbcCmd.Parameters.AddWithValue("@Tx1", image1Text);
            odbcCmd.Parameters.AddWithValue("@Ld2", image2Loaded);
            odbcCmd.Parameters.AddWithValue("@Tx2", image2Text);
            odbcCmd.Parameters.AddWithValue("@Ld3", image3Loaded);
            odbcCmd.Parameters.AddWithValue("@Tx3", image3Text);

            iRowsAffected = odbcCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return iRowsAffected;
    }
    // ========================================================================
    protected int Update_ImageText(int center, int ticket, int imageSequence, string imageText)
    {
        int iRowsAffected = 0;
        try
        {
            string sSql = "Update " + sLibrary + ".CALIMGTXT set";
            if (imageSequence == 1)
                sSql += " CILD1 = ?, CITX1 = ?";
            else if (imageSequence == 2)
                sSql += " CILD2 = ?, CITX2 = ?";
            else if (imageSequence == 3)
                sSql += " CILD3 = ?, CITX3 = ?";
            sSql += " where CICTR = ?" +
                " and CITCK = ?";

            odbcCmd = new OdbcCommand(sSql, odbcConn);
            odbcCmd.Parameters.AddWithValue("@Loaded", "Y");
            odbcCmd.Parameters.AddWithValue("@Text", imageText);
            odbcCmd.Parameters.AddWithValue("@Ctr", center);
            odbcCmd.Parameters.AddWithValue("@Tck", ticket);

            iRowsAffected = odbcCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "");
        }
        finally
        {
            odbcCmd.Dispose();
        }
        return iRowsAffected;
    }
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================
    #region misc
    // ========================================================================

    // ========================================================================
    #endregion // end misc
    // ========================================================================
    public class MultipartParser
    {
        public MultipartParser(Stream stream)
        {
            this.Parse(stream, Encoding.UTF8);
        }

        public MultipartParser(Stream stream, Encoding encoding)
        {
            this.Parse(stream, encoding);
        }

        private void Parse(Stream stream, Encoding encoding)
        {
            this.Success = false;

            // Read the stream into a byte array
            byte[] data = ToByteArray(stream);

            // Copy to a string for header parsing
            string content = encoding.GetString(data);

            // The first line should contain the delimiter
            int delimiterEndIndex = content.IndexOf("\r\n");

            if (delimiterEndIndex > -1)
            {
                string delimiter = content.Substring(0, content.IndexOf("\r\n"));

                // Look for Content-Type
                Regex re = new Regex(@"(?<=Content\-Type:)(.*?)(?=\r\n\r\n)");
                Match contentTypeMatch = re.Match(content);

                // Look for filename
                re = new Regex(@"(?<=filename\=\"")(.*?)(?=\"")");
                Match filenameMatch = re.Match(content);

                // Did we find the required values?
                if (contentTypeMatch.Success && filenameMatch.Success)
                {
                    // Set properties
                    this.ContentType = contentTypeMatch.Value.Trim();
                    this.Filename = filenameMatch.Value.Trim();

                    // Get the start & end indexes of the file contents
                    int startIndex = contentTypeMatch.Index + contentTypeMatch.Length + "\r\n\r\n".Length;

                    byte[] delimiterBytes = encoding.GetBytes("\r\n" + delimiter);
                    int endIndex = IndexOf(data, delimiterBytes, startIndex);

                    int contentLength = endIndex - startIndex;
                    try 
                    {
                        // Extract the file contents from the byte array
                        byte[] fileData = new byte[contentLength];
                        System.Buffer.BlockCopy(data, startIndex, fileData, 0, contentLength);
                        this.FileContents = fileData;
                        this.Success = true;
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler erh = new ErrorHandler();
                        erh.SaveErrorText(ex.Message.ToString(), ex.ToString(), "Byte array loading crash.  Picture content length: " + content.Length.ToString(""));
                        erh = null;
                    }
                }
            }
        }

        private int IndexOf(byte[] searchWithin, byte[] serachFor, int startIndex)
        {
            int index = 0;
            int startPos = Array.IndexOf(searchWithin, serachFor[0], startIndex);

            if (startPos != -1)
            {
                while ((startPos + index) < searchWithin.Length)
                {
                    if (searchWithin[startPos + index] == serachFor[index])
                    {
                        index++;
                        if (index == serachFor.Length)
                        {
                            return startPos;
                        }
                    }
                    else
                    {
                        startPos = Array.IndexOf<byte>(searchWithin, serachFor[0], startPos + index);
                        if (startPos == -1)
                        {
                            return -1;
                        }
                        index = 0;
                    }
                }
            }

            return -1;
        }

        private byte[] ToByteArray(Stream stream)
        {
            //byte[] buffer = new byte[32768];
            byte[] buffer = new byte[60000]; // 65536 is 2x 32768
            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }

        public bool Success
        {
            get;
            private set;
        }

        public string ContentType
        {
            get;
            private set;
        }

        public string Filename
        {
            get;
            private set;
        }

        public byte[] FileContents
        {
            get;
            private set;
        }
    }
    // ========================================================================
}