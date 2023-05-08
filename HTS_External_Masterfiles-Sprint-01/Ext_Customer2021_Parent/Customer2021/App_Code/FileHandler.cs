using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web;
using System.Drawing.Imaging;
using System.ServiceModel.Dispatcher;


/// <summary>
/// Summary description for FileHandler
/// </summary>
public class FileHandler
{
    // ---------------------------------------
    // GLOBAL VARIABLES 
    // ---------------------------------------
    OdbcConnection odbcConn;
    OdbcCommand odbcCmd;
    OdbcDataReader odbcReader;

    SqlCommand sqlCmd;
    SqlConnection sqlConn;
    SqlDataReader sqlReader;

    SqlConnection sqlConn2;  // Use for nested queries inside another query where the connection is already open

    MyPage myPage;

    string sLibrary = "";
    public string sSqlDbToUse = "";

    // ========================================================================
    // Constructor
    // ========================================================================
    public FileHandler(string library)
    {
        sLibrary = library;

        if (sLibrary == "OMDTALIB")
            sSqlDbToUse = "CustomerLegacy_LIVE.dbo";
        else
            sSqlDbToUse = "CustomerLegacy_TEST.dbo";
    }
    // ========================================================================
    #region mySqls
    // ========================================================================
    // This must either look at only the library or connect the library 
    public DataTable Select_Files(
        string areaWhereUsed, // ticket, customer, procedure
        string areaItemId,   // 123-4567 or 99999-5
        string smallMediumLargeImage,
        string fileTypeList, // fileTypeList i.e. image|pdf|excel|word
        string fileName,
        string fileDescription, // 
        string oneOrManyUses, // 
        int creatorEmployeeNum, // 
        string userAccountType // (Either REG, LRG, SRC) 
        )
    {
        string sSql = "";

        // If you pass blank in the fileTypeList it will return ALL types (normal) 
        string[] saTyp = { "" };
        string sFileTypes = "";
        if (!String.IsNullOrEmpty(fileTypeList))
        {
            saTyp = fileTypeList.Split('|');
            for (int i = 0; i < saTyp.Length; i++)
            {
                if (!String.IsNullOrEmpty(sFileTypes))
                    sFileTypes += ", ";
                sFileTypes += "'" + saTyp[i] + "'";
            }
        }

        areaWhereUsed = areaWhereUsed.ToLower();

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            //odbcConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString);

            sqlConn.Open();

            using (sqlConn)
            {
                // First: get ALL fields (except Binary image) from the table
                sSql = "Select top 200" +
                     " flId" +
                    ", flFileName" +
                    ", flFileExtension" +
                    ", flFileType" +
                    ", flFileDescription" +
                    ", flAreaWhereUsed" +
                    ", flFileSizeInKb" +
                    ", flCreationStamp" +
                    ", flCreatorUsername" +
                    ", flCreatorEmployeeNum" +
                    ", flVisibleToServrightTech" +
                    ", flVisibleToCustomer";

                // -----------------------------
                if (!String.IsNullOrEmpty(areaItemId))
                    sSql += ", wuId" +
                            ", wuAreaItemId";

                sSql += " from " + sSqlDbToUse + ".FileLibrary";
                if (!String.IsNullOrEmpty(areaItemId))
                {
                    sSql += ", " + sSqlDbToUse + ".FilesWhereUsed";
                }
                sSql += " where flId > 0";
                if (!String.IsNullOrEmpty(areaItemId))
                {
                    sSql += " and flId = wuFileId";
                }
                if (!String.IsNullOrEmpty(sFileTypes))
                    sSql += " and flFileType in (" + sFileTypes + ")";
                if (!String.IsNullOrEmpty(fileName))
                    sSql += " and lower(flFileName) like @FileName";
                if (!String.IsNullOrEmpty(fileDescription))
                    sSql += " and lower(flFileDescription) like @FileDescription";
                if (creatorEmployeeNum > 0)
                    sSql += " and flCreatorEmployeeNum = @CreatorEmployeeNum";
                if (!String.IsNullOrEmpty(oneOrManyUses))
                    sSql += " and flExpectOneOrManyUses = @OneOrManyUses";
                //if (!String.IsNullOrEmpty(areaWhereUsed) && areaWhereUsed != "procedure")
                //if (!String.IsNullOrEmpty(areaItemId))
                if (!String.IsNullOrEmpty(areaWhereUsed))
                {
                    if (!String.IsNullOrEmpty(areaItemId))
                        sSql += " and wuAreaWhereUsed = @AreaWhereUsed";
                    else
                        sSql += " and flAreaWhereUsed = @AreaWhereUsed";
                }
                // If you are any type of servright user - you may only see files specifically flagged as to be shown to you.
                // But this also means that if you are NOT a servright tech, and you see the file section, you'll see ANY file
                if (userAccountType == "SRC" || userAccountType == "SRP" || userAccountType == "SRG")
                    sSql += " and flVisibleToServrightTech = 1";
                // -------------------------------------------------------
                if (!String.IsNullOrEmpty(areaItemId))
                    sSql += " and wuAreaItemId = @AreaItemId";

                sqlCmd = new SqlCommand(sSql, sqlConn);

                if (!String.IsNullOrEmpty(fileName))
                    sqlCmd.Parameters.AddWithValue("@FileName", "%" + fileName.ToLower() + "%");
                if (!String.IsNullOrEmpty(fileDescription))
                    sqlCmd.Parameters.AddWithValue("@FileDescription", "%" + fileDescription.ToLower() + "%");
                if (creatorEmployeeNum > 0)
                    sqlCmd.Parameters.AddWithValue("@CreatorEmployeeNum", creatorEmployeeNum);
                if (!String.IsNullOrEmpty(oneOrManyUses))
                    sqlCmd.Parameters.AddWithValue("@OneOrManyUses", oneOrManyUses);
                //if (!String.IsNullOrEmpty(areaWhereUsed) && areaWhereUsed != "procedure")
                if (!String.IsNullOrEmpty(areaWhereUsed))
                    sqlCmd.Parameters.AddWithValue("@AreaWhereUsed", areaWhereUsed);
                // -------------------------------------------------------
                if (!String.IsNullOrEmpty(areaItemId))
                    sqlCmd.Parameters.AddWithValue("@AreaItemId", areaItemId);

                byte[] bytes;
                string sBase64 = "";
                string sFileExtension = "";
                int iLibraryFileId = 0;
                int iTimesUsed = 0;

                using (sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default))
                {
                    dt.Load(sqlReader);

                    dt.Columns.Add(MakeColumn("Base64AsString"));
                    dt.Columns.Add(MakeColumn("Base64ForLink"));
                    dt.Columns.Add(MakeColumn("FileUseCount"));
                    dt.Columns.Add(MakeColumn("displayVisibilityToServrightTech"));
                    dt.Columns.Add(MakeColumn("displayVisibilityToCustomer"));

                    dt.AcceptChanges();

                    foreach (DataRow row in dt.Rows)
                    {

                        if (int.TryParse(row["flId"].ToString().Trim(), out iLibraryFileId) == false)
                            iLibraryFileId = -1;
                        if (iLibraryFileId > 0)
                        {

                            if (row["flVisibleToServrightTech"].ToString().Trim() == "1")
                                row["displayVisibilityToServrightTech"] = "Yes";
                            else
                                row["displayVisibilityToServrightTech"] = "N";

                            if (row["flVisibleToCustomer"].ToString().Trim() == "1")
                                row["displayVisibilityToCustomer"] = "Yes";
                            else
                                row["displayVisibilityToCustomer"] = "N";

                            sFileExtension = row["flFileExtension"].ToString().Trim().Replace(".", "");
                            // Second: get just the BINARY IMAGE with the ExecuteScalar option below
                            sSql = "Select";

                            if (smallMediumLargeImage.ToLower() == "large")
                                sSql += " flBinaryOriginal";
                            else if (smallMediumLargeImage.ToLower() == "medium")
                                sSql += " flBinaryWithSizeLimit";
                            else // not specified? (or small passed?) just send thumbnail
                                sSql += " flBinaryThumbnail";

                            sSql += " from " + sSqlDbToUse + ".FileLibrary where flId = @RecordId";

                            sqlCmd = new SqlCommand(sSql, sqlConn);

                            sqlCmd.Parameters.AddWithValue("@RecordId", iLibraryFileId);
                            try 
                            {
                                bytes = (byte[])sqlCmd.ExecuteScalar();
                                sBase64 = Convert.ToBase64String(bytes);
                            }
                            catch (Exception ex)
                            {
                                //myPage = new MyPage();
                                //myPage.SaveError(ex.Message.ToString(), ex.ToString(), "OK: inner catch is handling a null attachment");
                                //myPage = null;
                            }

                            row["Base64AsString"] = sBase64;
                            if (sFileExtension == "jpg"
                                || sFileExtension == "jpeg"
                                || sFileExtension == "png"
                                || sFileExtension == "gif"
                                || sFileExtension == "bmp"
                                )
                                row["Base64ForLink"] = "data:Image/" + sFileExtension + ";base64,";
                            else if (sFileExtension == "pdf")
                                row["Base64ForLink"] = "Application/pdf;base64,";
                            else if (sFileExtension == "xlsx" || sFileExtension == "xls")
                                row["Base64ForLink"] = "Application/x-msexcel;base64,";
                            else if (sFileExtension == "docx" || sFileExtension == "doc")
                                row["Base64ForLink"] = "Application/msword;base64,";
                            else if (sFileExtension == "txt")
                                row["Base64ForLink"] = "text/plain;base64,";
                            // or text/HTML
                            // or image/GIF, JPG, JPEG, GIF, BMP
                            // or text/plain
                            // or Application/msword (for Word files)
                            // or Applicaton/x-msexcel (for Excel files)
                            // or Application/pdf

                            row["FileUseCount"] = "";

                            //if (!String.IsNullOrEmpty(areaWhereUsed) || !String.IsNullOrEmpty(areaItemId)) 
                            //{
                            iTimesUsed = Get_LibraryFileUseCount(iLibraryFileId);
                            if (iTimesUsed > 0)
                                row["FileUseCount"] = iTimesUsed.ToString();
                            //}
                        }
                    }
                }
                dt.AcceptChanges();
            }

        }
        catch (Exception ex)
        {
            myPage = new MyPage();
            myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            myPage = null;
        }
        finally
        {
            sqlCmd.Dispose();
            if (sqlConn.State != ConnectionState.Closed)
                sqlConn.Close(); // It was closed by the using clause (but just in case of a crash) 
        }

        return dt;
    }
    // ========================================================================
    public string Save_BinaryToDiskReturnPath(string recordId)
    {
        string sFullPhysicalPath = "";
        string sSql = "";
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            sqlConn.Open();

            using (sqlConn)
            {
                // First: get ALL fields (except Binary image) from the table
                sSql = "Select" +
                     " flId" +
                    ", flFileName" +
                    ", flFileExtension" +
                    ", flFileType" +
                    ", flFileDescription" +
                    ", flFileSizeInKb" +
                    ", flCreationStamp" +
                    ", flCreatorUsername" +
                    ", flCreatorEmployeeNum" +
                    ", flVisibleToServrightTech" +
                    ", flVisibleToCustomer" +
                    " from " + sSqlDbToUse + ".FileLibrary" +
                " where flId = @RecordId";

                sqlCmd = new SqlCommand(sSql, sqlConn);

                sqlCmd.Parameters.AddWithValue("@RecordId", recordId);

                //byte[] bytes;
                byte[] byteArray = null;
                //string sBase64;
                //string sFileExtension = "";
                string sRootPath = HttpContext.Current.Server.MapPath(@"~\media\workfiles\");
                string sFileName = "";

                int iRecordId = 0;

                using (sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default))
                {
                    dt.Load(sqlReader);
                    dt.Columns.Add(MakeColumn("Base64AsString"));
                    //dt.Columns.Add(MakeColumn("Base64ForLink"));
                    dt.AcceptChanges();

                    if (dt.Rows.Count > 0)
                    {
                        sFileName = dt.Rows[0]["flFileName"].ToString().Trim();

                        if (int.TryParse(dt.Rows[0]["flId"].ToString().Trim(), out iRecordId) == false)
                            iRecordId = -1;
                        if (iRecordId > 0)
                        {
                            //sFileExtension = dt.Rows[0]["tfFileExtension"].ToString().Trim().Replace(".", "");

                            // Second: get just the BINARY IMAGE with the ExecuteScalar option below
                            //sSql = "Select flBinaryWithSizeLimit from " + sSqlDbToUse + ".FileLibrary where flId = @RecordId";
                            sSql = "Select flBinaryOriginal from " + sSqlDbToUse + ".FileLibrary where flId = @RecordId";
                            sqlCmd = new SqlCommand(sSql, sqlConn);
                            sqlCmd.Parameters.AddWithValue("@RecordId", iRecordId);
                            byteArray = (byte[])sqlCmd.ExecuteScalar();

                            //sBase64 = Convert.ToBase64String(byteArray);
                            //row["Base64AsString"] = sBase64;

                            sFullPhysicalPath = sRootPath + sFileName;

                            // Delete before you write it to the folder
                            if (File.Exists(sFullPhysicalPath))
                                File.Delete(sFullPhysicalPath);

                            System.IO.File.WriteAllBytes(sFullPhysicalPath, byteArray);
                        }

                        //byteArray = Convert.FromBase64String(dt.Rows[0]["Base64AsString"].ToString().Trim());

                        //sWebLink = "~/media/workfiles/TestPdfFile.pdf";

                        //Response.ClearContent();
                        //Response.ContentType = "application/ms-excel";
                        //Response.ContentType = "Application/pdf";
                        //Response.AddHeader("content-disposition", "attachment; filename= " + sFileName);
                        //Response.Write(sCsv);
                        //Response.WriteFile(sFullPath);
                        //Response.WriteFile(sWebLink);

                    }
                }
            }

        }
        catch (Exception ex)
        {
            myPage = new MyPage();
            myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            myPage = null;
        }
        finally
        {
            sqlCmd.Dispose();
            if (sqlConn.State != ConnectionState.Closed)
                sqlConn.Close(); // It was closed by the using clause (but just in case of a crash) 
        }

        return sFullPhysicalPath;
    }
    // ========================================================================
    //public string Get_TicketImage(string ticketId, int recordId) 
    //public string Get_ImageFile(string groupName, string groupId, int recordId) 
    public string Get_ImageFile(int recordId, string smallMediumLarge) 
    {
        string sImageUrl = "";

        string sSql = "";
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

            sqlConn.Open();

            using (sqlConn)
            {
                // First: get ALL fields (except Binary image) from the table
                sSql = "Select" +
                     " flId" +
                    ", flFileName" +
                    ", flFileExtension" +
                    ", flFileType" +
                    ", flFileDescription" +
                    ", flFileSizeInKb" +
                    ", flCreationStamp" +
                    ", flCreatorUsername" +
                    ", flCreatorEmployeeNum" +
                    ", flVisibleToServrightTech" +
                    ", flVisibleToCustomer" +
                    " from " + sSqlDbToUse + ".FileLibrary" +
                " where flId = @RecordId";

                sqlCmd = new SqlCommand(sSql, sqlConn);

                sqlCmd.Parameters.AddWithValue("@RecordId", recordId);

                byte[] bytes;
                string sBase64;
                string sFileExtension = "";
                int iId = 0;

                using (sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default))
                {
                    dt.Load(sqlReader);

                    foreach (DataRow row in dt.Rows)
                    {

                        if (int.TryParse(row["flId"].ToString().Trim(), out iId) == false)
                            iId = -1;
                        if (iId > 0)
                        {
                            sFileExtension = row["flFileExtension"].ToString().Trim().Replace(".", "");
                            // Second: get just the BINARY IMAGE with the ExecuteScalar option below
                            sSql = "Select";

                            if (smallMediumLarge.ToLower() == "small")
                                sSql += " flBinaryThumbnail";
                            else if (smallMediumLarge.ToLower() == "medium")
                                sSql += " flBinaryWithSizeLimit";
                            else // Large (full size image) 
                                sSql += " flBinaryOriginal";

                            sSql += " from " + sSqlDbToUse + ".FileLibrary where flId = @RecordId";

                            sqlCmd = new SqlCommand(sSql, sqlConn);

                            sqlCmd.Parameters.AddWithValue("@RecordId", iId);

                            bytes = (byte[])sqlCmd.ExecuteScalar();

                            sBase64 = Convert.ToBase64String(bytes);
                            if (sFileExtension == "jpg"
                                || sFileExtension == "jpeg"
                                || sFileExtension == "png"
                                || sFileExtension == "gif"
                                || sFileExtension == "bmp"
                                )
                                sImageUrl = "data:Image/" + sFileExtension + ";base64," + sBase64;
                        }
                    }
                }
                dt.AcceptChanges();
            }

        }
        catch (Exception ex)
        {
            myPage = new MyPage();
            myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            myPage = null;
        }
        finally
        {
            sqlCmd.Dispose();
            if (sqlConn.State != ConnectionState.Closed)
                sqlConn.Close(); // It was closed by the using clause (but just in case of a crash) 
        }

        return sImageUrl;
    }
    // -------------------------------------------------------------------------------------------------------
    public string Insert_FileForArea(
        HttpPostedFile postedFile,
        string scrubbedFileName,
        string description,
        string creatorUsername,
        string creatorEmployeeNum,
        string fileType,
        string areaWhereUsed,
        string areaItemId,
        string expectOneOrManyUses
        )
    {
        string sErrorOrNewId = "";
        string sSql = "";
        string sCreationStamp = "";

        areaWhereUsed = areaWhereUsed.ToLower();

        try
        {
            int iNewFileLibraryId = 0;
            int iCreatorEmployeeNum = 0;
            if (int.TryParse(creatorEmployeeNum, out iCreatorEmployeeNum) == false)
                iCreatorEmployeeNum = 0;

            // Using the scrubbed version rather than the one from the object which may have special characters
            //string sFileName = Path.GetFileName(postedFile.FileName);
            string sFileName = scrubbedFileName;
            string sFileExtension = Path.GetExtension(sFileName);
            int iFileSizeInKb = 0;
            if (postedFile.ContentLength > 0)
                iFileSizeInKb = postedFile.ContentLength / 1000;

            // int fileSize = postedFile.ContentLength; // size of the file in bytes (not KB -> x 1000)

            if (postedFile.ContentLength <= 0)
            {
                sErrorOrNewId = "Error: Please select a file to be uploaded.";
            }
            else if
                (
                    fileType == "image"
                    &&
                       (
                        sFileExtension.ToLower() != ".jpg"
                        && sFileExtension.ToLower() != ".jpeg"
                        && sFileExtension.ToLower() != ".bmp"
                        && sFileExtension.ToLower() != ".gif"
                        && sFileExtension.ToLower() != ".png"
                        )
                )
            {
                //sResult = "Error: Only images (.jpg, .jpeg, .png, .gif and .bmp) can be uploaded.";
                sErrorOrNewId = "Error: Only images of type (.jpg, .jpeg, .png, .gif and .bmp) can be uploaded.";
            }
            else if
    (
        fileType == "excel"
        &&
           (
            sFileExtension.ToLower() != ".xls"
            && sFileExtension.ToLower() != ".xlsx"
            )
        )
            {
                sErrorOrNewId = "Error: Only spreadsheets of type (.xls, .xlsx) can be uploaded.";
            }
            else if
    (
        fileType == "word"
        &&
           (
            sFileExtension.ToLower() != ".doc"
            && sFileExtension.ToLower() != ".docx"
            )
    )
            {
                sErrorOrNewId = "Error: Only documents of type (.doc, .docs) can be uploaded.";
            }
            else if (String.IsNullOrEmpty(description))
            {
                sErrorOrNewId = "A brief description is required.";
            }
            else if (!String.IsNullOrEmpty(description) && description.Length > 500)
            {
                sErrorOrNewId = "Description has exceeded the 500 char max (" + description.Length + ").";
            }
            else
            {
                Stream inputStream = postedFile.InputStream;
                BinaryReader binaryReader = new BinaryReader(inputStream);
                byte[] byteArrayForLargeCopy = binaryReader.ReadBytes((int)inputStream.Length);
                byte[] byteArrayForMediumCopy = null;
                byte[] byteArrayForThumbnailCopy = null;

                // You need to figure out how to not load these extra binary objects for files that are not images!
                if (fileType == "image" && (sFileExtension.ToLower() == ".jpg" || sFileExtension.ToLower() == ".bmp" || sFileExtension.ToLower() == ".gif" || sFileExtension.ToLower() == ".png"))
                {
                    byteArrayForMediumCopy = byteArrayForLargeCopy;
                    byteArrayForThumbnailCopy = byteArrayForLargeCopy;
                }


                // ---------------------------------------------------------------------------------------
                // RESIZE if needed
                // ---------------------------------------------------------------------------------------
                if (fileType == "image" && (sFileExtension.ToLower() == ".jpg" || sFileExtension.ToLower() == ".jpeg" || sFileExtension.ToLower() == ".bmp" || sFileExtension.ToLower() == ".gif" || sFileExtension.ToLower() == ".png"))
                {
                    Image fullsizeImage = System.Drawing.Image.FromStream(inputStream);

                    if (fullsizeImage != null)
                    {
                        int iFullsizeImageWidth = fullsizeImage.Width;
                        int iMaxWidthForLargeCopy = 3840;
                        int iMaxWidthForMediumCopy = 1000;
                        int iMaxWidthForThumbnailCopy = 100;

                        if (iFullsizeImageWidth > iMaxWidthForLargeCopy)
                        {
                            //resizedImage = ResizeImage(fullsizeImage, iMaxWidthForLargerCopy);
                            //byteArrayForLargerCopy = ConvertImageToByteArray(resizedImage, sFileExtension);
                            byteArrayForLargeCopy = ConvertImageToByteArray(ResizeImage(fullsizeImage, iMaxWidthForLargeCopy), sFileExtension);
                        }

                        if (iFullsizeImageWidth > iMaxWidthForMediumCopy)
                        {
                            byteArrayForMediumCopy = ConvertImageToByteArray(ResizeImage(fullsizeImage, iMaxWidthForMediumCopy), sFileExtension);
                        }
                        if (iFullsizeImageWidth > iMaxWidthForThumbnailCopy)
                        {
                            byteArrayForThumbnailCopy = ConvertImageToByteArray(ResizeImage(fullsizeImage, iMaxWidthForThumbnailCopy), sFileExtension);
                        }
                    }
                    fullsizeImage = null;
                }
                // ---------------------------------------------------------------------------------------

                sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

                sqlConn.Open();

                if (byteArrayForMediumCopy == null)
                    byteArrayForMediumCopy = byteArrayForLargeCopy;
                if (byteArrayForThumbnailCopy == null)
                    byteArrayForThumbnailCopy = byteArrayForMediumCopy;

                using (sqlConn)
                {

                    sSql = " Insert into " + sSqlDbToUse + ".FileLibrary" +
                        " (" +
                         " flFileName" +
                        ", flFileExtension" +
                        ", flFileType" +
                        ", flFileDescription" +
                        ", flAreaWhereUsed" +
                        ", flExpectOneOrManyUses" +
                        ", flFileSizeInKb" +
                        ", flCreationStamp" +
                        ", flCreatorUsername" +
                        ", flCreatorEmployeeNum" +
                        ", flVisibleToServrightTech" +
                        ", flVisibleToCustomer" +
                        ", flBinaryOriginal" +
                        ", flBinaryWithSizeLimit" +
                        ", flBinaryThumbnail" +
                        ") values(" +
                        " @FileName" +
                       ", @FileExtension" +
                       ", @FileType" +
                       ", @FileDescription" +
                       ", @AreaWhereUsed" +
                       ", @ExpectOneOrManyUses" +
                       ", @FileSizeInKb" +
                       ", @CreationStamp" +
                       ", @CreatorUsername" +
                       ", @CreatorEmployeeNum" +
                       ", @VisibleToServrightTech" +
                       ", @VisibleToCustomer" +
                       ", @BinaryOriginal" +
                       ", @BinaryWithSizeLimit" +
                       ", @BinaryThumbnail" +
                        ")";
                    sqlCmd = new SqlCommand(sSql, sqlConn);

                    sCreationStamp = DateTime.Now.ToString("o");

                    sqlCmd.Parameters.AddWithValue("@FileName", sFileName);
                    sqlCmd.Parameters.AddWithValue("@FileExtension", sFileExtension);
                    sqlCmd.Parameters.AddWithValue("@FileType", fileType);
                    sqlCmd.Parameters.AddWithValue("@FileDescription", description);
                    sqlCmd.Parameters.AddWithValue("@AreaWhereUsed", areaWhereUsed);
                    sqlCmd.Parameters.AddWithValue("@ExpectOneOrManyUses", expectOneOrManyUses);
                    sqlCmd.Parameters.AddWithValue("@FileSizeInKb", iFileSizeInKb);
                    sqlCmd.Parameters.AddWithValue("@CreationStamp", sCreationStamp);
                    sqlCmd.Parameters.AddWithValue("@CreatorUsername", creatorUsername);
                    sqlCmd.Parameters.AddWithValue("@CreatorEmployeeNum", iCreatorEmployeeNum);
                    sqlCmd.Parameters.AddWithValue("@VisibleToServrightTech", 1); // If the FST just loaded the picture, let them see it...
                    sqlCmd.Parameters.AddWithValue("@VisibleToCustomer", 0);
                    sqlCmd.Parameters.AddWithValue("@BinaryOriginal", byteArrayForLargeCopy);
                    sqlCmd.Parameters.AddWithValue("@BinaryWithSizeLimit", byteArrayForMediumCopy);
                    sqlCmd.Parameters.AddWithValue("@BinaryThumbnail", byteArrayForThumbnailCopy);

                    int iRowsAffected = sqlCmd.ExecuteNonQuery();
                    if (iRowsAffected > 0)
                    {
                        sSql = " Select Max(flId) as newId from " + sSqlDbToUse + ".FileLibrary where flCreationStamp = @CreationStamp";
                        sqlCmd = new SqlCommand(sSql, sqlConn);
                        sqlCmd.Parameters.AddWithValue("@CreationStamp", sCreationStamp);

                        DataTable dt = new DataTable("");
                        using (sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default))
                        {
                            dt.Load(sqlReader);
                            foreach (DataRow row in dt.Rows)  // You should only be getting one record because of the max
                            {
                                if (int.TryParse(row["newId"].ToString().Trim(), out iNewFileLibraryId) == false)
                                    iNewFileLibraryId = -1;
                            }
                        }
                        if (iNewFileLibraryId > 0)
                        {
                            // Write the "FilesWhereUsed" record
                            //sResult = iNewFileLibraryId.ToString();
                            iRowsAffected = Insert_FileWhereUsed(
                                areaWhereUsed,
                                areaItemId,
                                iNewFileLibraryId,
                                creatorUsername,
                                iCreatorEmployeeNum,
                                sCreationStamp); // Passing the same stamp as the file itself will show the first connection was where the file was created.
                            if (iRowsAffected > 0)
                            {
                                sErrorOrNewId = iNewFileLibraryId.ToString();
                            }
                            else
                            {
                                sErrorOrNewId = "Error: the record failed to be inserted";
                            }
                        }
                        else
                            sErrorOrNewId = "Error: The ID of the new file could not be retrieved.";
                    }
                    else
                    {
                        sErrorOrNewId = "Error: Attempt to save the file failed.";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            myPage = new MyPage();
            myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            myPage = null;
        }
        finally
        {
            sqlCmd.Dispose();
            if (sqlConn.State != ConnectionState.Closed)
                sqlConn.Close(); // It was closed by the using clause (but just in case of a crash) 
        }

        return sErrorOrNewId;
    }
    // ========================================================================
    private int Get_LibraryFileUseCount(int fileRecordId)
    {
        int iFileUseCount = 0;

        string sSql = "";
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            sqlConn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

            sqlConn2.Open();

            using (sqlConn2)
            {
                sSql = "Select" +
                     " count(wuId) as fileUseCount" +
                    " from " + sSqlDbToUse + ".FilesWhereUsed" +
                " where wuFileId = @FileRecordId";

                sqlCmd = new SqlCommand(sSql, sqlConn2);

                sqlCmd.Parameters.AddWithValue("@FileRecordId", fileRecordId);

                using (sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default))
                {
                    dt.Load(sqlReader);

                    foreach (DataRow row in dt.Rows)
                    {

                        if (int.TryParse(row["fileUseCount"].ToString().Trim(), out iFileUseCount) == false)
                            iFileUseCount = 0;
                    }
                }
            }

        }
        catch (Exception ex)
        {
            myPage = new MyPage();
            myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            myPage = null;
        }
        finally
        {
            sqlCmd.Dispose();
            if (sqlConn2.State != ConnectionState.Closed)
                sqlConn2.Close(); // It was closed by the using clause (but just in case of a crash) 
        }

        return iFileUseCount;
    }
    // ========================================================================
    private int Get_ProcedureFileCount(int procedureRecordId)
    {
        int iProcedureFileCount = 0;

        string sSql = "";
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");

        try
        {
            sqlConn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

            sqlConn2.Open();

            using (sqlConn2)
            {
                sSql = "Select" +
                     " count(wuId) as fileUseCount" +
                    " from " + sSqlDbToUse + ".FilesWhereUsed" +
                " where wuAreaItemId = @ProcedureRecordId";

                sqlCmd = new SqlCommand(sSql, sqlConn2);

                sqlCmd.Parameters.AddWithValue("@ProcedureRecordId", procedureRecordId.ToString());

                using (sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default))
                {
                    dt.Load(sqlReader);

                    foreach (DataRow row in dt.Rows)
                    {

                        if (int.TryParse(row["fileUseCount"].ToString().Trim(), out iProcedureFileCount) == false)
                            iProcedureFileCount = 0;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            myPage = new MyPage();
            myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            myPage = null;
        }
        finally
        {
            sqlCmd.Dispose();
            if (sqlConn2.State != ConnectionState.Closed)
                sqlConn2.Close(); // It was closed by the using clause (but just in case of a crash) 
        }

        return iProcedureFileCount;
    }
    // ========================================================================
    public int Delete_File(int fileRecordId)
    {
        string sSql = "";
        int iRowsAffected = 0;

        if (fileRecordId > 0)
        {
            sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

            try
            {
                sqlConn.Open();

                sSql = "Delete from " + sSqlDbToUse + ".FileLibrary" +
                    " where flId = @Id";

                sqlCmd = new SqlCommand(sSql, sqlConn);

                sqlCmd.Parameters.AddWithValue("@Id", fileRecordId);

                iRowsAffected = sqlCmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                myPage = new MyPage();
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
                myPage = null;
            }
            finally
            {
                sqlCmd.Dispose();
                if (sqlConn.State != ConnectionState.Closed)
                    sqlConn.Close(); // It was closed by the using clause (but just in case of a crash) 
            }
        }
        return iRowsAffected;
    }
    // ========================================================================
    public int Delete_FileLink(int linkRecordId)
    {
        string sSql = "";
        int iRowsAffected = 0;

        if (linkRecordId > 0)
        {
            sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

            try
            {
                sqlConn.Open();

                sSql = "Delete from " + sSqlDbToUse + ".FilesWhereUsed" +
                    " where wuId = @Id";

                sqlCmd = new SqlCommand(sSql, sqlConn);

                sqlCmd.Parameters.AddWithValue("@Id", linkRecordId);

                iRowsAffected = sqlCmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                myPage = new MyPage();
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
                myPage = null;

            }
            finally
            {
                sqlCmd.Dispose();
                if (sqlConn.State != ConnectionState.Closed)
                    sqlConn.Close(); // It was closed by the using clause (but just in case of a crash) 
            }
        }
        return iRowsAffected;
    }
    // ========================================================================
    public int Delete_FileLinksForFilesBeingDeleted(int fileRecordId)
    {
        string sSql = "";
        int iRowsAffected = 0;

        if (fileRecordId > 0)
        {
            sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

            try
            {
                sqlConn.Open();

                sSql = "Delete from " + sSqlDbToUse + ".FilesWhereUsed" +
                    " where wuFileId = @Id";

                sqlCmd = new SqlCommand(sSql, sqlConn);

                sqlCmd.Parameters.AddWithValue("@Id", fileRecordId);

                iRowsAffected = sqlCmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                myPage = new MyPage();
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
                myPage = null;
            }
            finally
            {
                sqlCmd.Dispose();
                if (sqlConn.State != ConnectionState.Closed)
                    sqlConn.Close(); // It was closed by the using clause (but just in case of a crash) 
            }
        }
        return iRowsAffected;
    }
    // ========================================================================
    public int Delete_Procedure(int procedureRecordId)
    {
        string sSql = "";
        int iRowsAffected = 0;

        if (procedureRecordId > 0)
        {
            sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

            try
            {
                sqlConn.Open();

                sSql = "Delete from " + sSqlDbToUse + ".FileProcedureTypes" +
                    " where ptId = @Id";

                sqlCmd = new SqlCommand(sSql, sqlConn);

                sqlCmd.Parameters.AddWithValue("@Id", procedureRecordId);

                iRowsAffected = sqlCmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                myPage = new MyPage();
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
                myPage = null;
            }
            finally
            {
                sqlCmd.Dispose();
                if (sqlConn.State != ConnectionState.Closed)
                    sqlConn.Close(); // It was closed by the using clause (but just in case of a crash) 
            }
        }
        return iRowsAffected;
    }
    // ========================================================================
    public int Delete_ProcedureLinksForProceduresBeingDeleted(int procedureRecordId)
    {
        string sSql = "";
        int iRowsAffected = 0;

        if (procedureRecordId > 0)
        {
            sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

            try
            {
                sqlConn.Open();

                sSql = "Delete from " + sSqlDbToUse + ".FilesWhereUsed" +
                    " where wuAreaItemId = @AreaItemId";

                sqlCmd = new SqlCommand(sSql, sqlConn);

                sqlCmd.Parameters.AddWithValue("@AreaItemId", procedureRecordId.ToString());

                iRowsAffected = sqlCmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                myPage = new MyPage();
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
                myPage = null;
            }
            finally
            {
                sqlCmd.Dispose();
                if (sqlConn.State != ConnectionState.Closed)
                    sqlConn.Close(); // It was closed by the using clause (but just in case of a crash) 
            }
        }
        return iRowsAffected;
    }
    // ========================================================================
    public DataTable Select_Procedures(string procedure, string areaWhereUsed)
    {
        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        string sSql = "";

        sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

        areaWhereUsed = areaWhereUsed.ToLower();

        try
        {
            sqlConn.Open();

            sSql = "Select" +
                 " ptId" +
                ", ptSummary" +
                ", ptAreaWhereUsed" +
                ", ptCreationStamp" +
                ", ptCreatorUsername" +
                ", ptCreatorEmployeeNum" +
                " from " + sSqlDbToUse + ".FileProcedureTypes" +
                " where ptId > 0";

            if (!String.IsNullOrEmpty(procedure))
                sSql += " and ptSummary = @Procedure";
            if (!String.IsNullOrEmpty(areaWhereUsed))
                sSql += " and ptAreaWhereUsed = @AreaWhereUsed";

            sSql += " order by ptAreaWhereUsed, ptSummary";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            if (!String.IsNullOrEmpty(procedure))
                sqlCmd.Parameters.AddWithValue("@Procedure", procedure);
            if (!String.IsNullOrEmpty(areaWhereUsed))
                sqlCmd.Parameters.AddWithValue("@AreaWhereUsed", areaWhereUsed);

            using (sqlReader = sqlCmd.ExecuteReader(CommandBehavior.Default))
            {
                if (sqlReader.HasRows) { }
                dt.Load(sqlReader);
            }
            dt.Columns.Add(MakeColumn("ProcedureWithArea"));


            int iPtId = 0;
            int iProcedureFileCount = 0;
            string sTemp = "";
            foreach (DataRow row in dt.Rows)
            {
                if (int.TryParse(row["ptId"].ToString().Trim(), out iPtId) == false)
                    iPtId = -1;

                if (iPtId > 0)
                {
                    iProcedureFileCount = Get_ProcedureFileCount(iPtId);
                }

                sTemp = "[" + row["ptAreaWhereUsed"].ToString().Trim() + "] " + row["ptSummary"].ToString().Trim();
                if (iProcedureFileCount == 1)
                {
                    sTemp += " (" + iProcedureFileCount + " file)";
                }
                else if (iProcedureFileCount > 1)
                {
                    sTemp += " (" + iProcedureFileCount + " files)";
                }
                else
                {
                    sTemp += " (empty)";
                }

                row["ProcedureWithArea"] = sTemp;
            }

            dt.AcceptChanges();
        }
        catch (Exception ex)
        {
            myPage = new MyPage();
            myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            myPage = null;
        }
        finally
        {
            sqlCmd.Dispose();
            if (sqlConn.State != ConnectionState.Closed)
                sqlConn.Close(); // It was closed by the using clause (but just in case of a crash) 

        }
        return dt;
    }
    // ========================================================================
    public int Insert_Procedure(string procedure, string areaWhereUsed, string userName, int employeeNum)
    {
        int iRowsAffected = 0;
        string sSql = "";
        sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

        areaWhereUsed = areaWhereUsed.ToLower();

        try
        {
            sqlConn.Open();

            if (procedure != "" && procedure.Length > 200)
                procedure = procedure.Substring(0, 200);

            sSql = "insert into " + sSqlDbToUse + ".FileProcedureTypes" +
                " (ptSummary, ptAreaWhereUsed, ptCreationStamp, ptCreatorUsername, ptCreatorEmployeeNum)" +
                " values(@Summary, @AreaWhereUsed, @CreationStamp, @CreatorUsername, @CreatorEmployeeNum)";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.AddWithValue("@Summary", procedure);
            sqlCmd.Parameters.AddWithValue("@AreaWhereUsed", areaWhereUsed);
            sqlCmd.Parameters.AddWithValue("@CreationStamp", DateTime.Now.ToString("o"));
            sqlCmd.Parameters.AddWithValue("@CreatorUsername", userName);
            sqlCmd.Parameters.AddWithValue("@CreatorEmployeeNum", employeeNum);

            iRowsAffected = sqlCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            myPage = new MyPage();
            myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            myPage = null;
        }
        finally
        {
            sqlCmd.Dispose();
            if (sqlConn.State != ConnectionState.Closed)
                sqlConn.Close(); // It was closed by the using clause (but just in case of a crash) 
        }
        return iRowsAffected;
    }
    // ========================================================================
    public int Insert_FileWhereUsed
        (
        string areaWhereUsed,
        string areaItemId,
        int fileId,
        string connectorUserName,
        int connectorEmployeeNum,
        string creationStamp
        )
    {
        int iRowsAffected = 0;
        string sSql = "";

        areaWhereUsed = areaWhereUsed.ToLower();

        sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

        try
        {
            sqlConn.Open();

            sSql = "insert into " + sSqlDbToUse + ".FilesWhereUsed" +
                " (wuAreaWhereUsed, wuAreaItemId, wuFileId, wuConnectorUsername, wuConnectorEmployeeNum, wuCreationStamp)" +
                " values(@AreaWhereUsed, @AreaItemId, @FileId, @ConnectorUserName, @ConnectorEmployeeNum, @CreationStamp)";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.AddWithValue("@AreaWhereUsed", areaWhereUsed);
            sqlCmd.Parameters.AddWithValue("@AreaItemId", areaItemId);
            sqlCmd.Parameters.AddWithValue("@FileId", fileId);
            sqlCmd.Parameters.AddWithValue("@ConnectorUserName", connectorUserName);
            sqlCmd.Parameters.AddWithValue("@ConnectorEmployeeNum", connectorEmployeeNum);
            sqlCmd.Parameters.AddWithValue("@CreationStamp", creationStamp);

            iRowsAffected = sqlCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            myPage = new MyPage();
            myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            myPage = null;
        }
        finally
        {
            sqlCmd.Dispose();
            if (sqlConn.State != ConnectionState.Closed)
                sqlConn.Close(); // It was closed by the using clause (but just in case of a crash) 

        }
        return iRowsAffected;
    }
    // ========================================================================
    public int Update_FileDescription(int fileRecordId, string newDescription)
    {
        int iRowsAffected = 0;
        string sSql = "";
        sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);

        try
        {
            sqlConn.Open();

            if (newDescription != "" && newDescription.Length > 500)
                newDescription = newDescription.Substring(0, 500);

            sSql = "update " + sSqlDbToUse + ".FileLibrary set" +
                " flFileDescription = @FileDescription" +
                " where flId = @Id";

            sqlCmd = new SqlCommand(sSql, sqlConn);

            sqlCmd.Parameters.AddWithValue("@FileDescription", newDescription);
            sqlCmd.Parameters.AddWithValue("@Id", fileRecordId);

            iRowsAffected = sqlCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            myPage = new MyPage();
            myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            myPage = null;
        }
        finally
        {
            sqlCmd.Dispose();
            if (sqlConn.State != ConnectionState.Closed)
                sqlConn.Close(); // It was closed by the using clause (but just in case of a crash) 
        }
        return iRowsAffected;
    }
    // ========================================================================
    public string Select_AreaItemIdDetail(string area, string itemId)
    {
        string sDetail = "";

        DataTable dt = new DataTable(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString() + "_Values");
        string sSql = "";
        int iCs1 = 0;
        int iCs2 = 0;
        int iCtr = 0;
        int iTck = 0;

        string[] saParms = { "" };
        if (!String.IsNullOrEmpty(itemId))
            saParms = itemId.Split('-');
        if (saParms.Length > 1)
        {
            if (area == "ticket")
            {
                if (int.TryParse(saParms[0], out iCtr) == false)
                    iCtr = -1;
                if (int.TryParse(saParms[1], out iTck) == false)
                    iTck = -1;
            }
            else if (area == "customer")
            {
                if (int.TryParse(saParms[0], out iCs1) == false)
                    iCs1 = -1;
                if (int.TryParse(saParms[1], out iCs2) == false)
                    iCs2 = -1;
            }
        }
        if ((iCtr > 0 && iTck > 0) || (iCs1 > 0 && iCs2 > -1))
        {
            odbcConn = new OdbcConnection(ConfigurationManager.ConnectionStrings["As400Conn"].ConnectionString);

            try
            {
                odbcConn.Open();

                sSql = "Select" +
                     " CUSTNM" +
                    ", CSTRNR" +
                    ", CSTRCD" +
                    ", SADDR1" +
                    ", SADDR2" +
                    ", CITY" +
                    ", STATE" +
                    ", ZIPCD" +
                    " from " + sLibrary + ".CUSTMAST";
                if (iCtr > 0 && iTck > 0)
                    sSql += ", " + sLibrary + ".SVRTICK";
                sSql += " where cstrnr > 0";
                if (iCtr > 0 && iTck > 0)
                    sSql += " and cstrnr = stcus1 and cstrcd = stcus2";
                if (iCtr > 0 && iTck > 0)
                    sSql += " and tccent = ? and ticknr = ?";
                else
                    sSql += " and cstrnr = ? and cstrcd = ?";

                odbcCmd = new OdbcCommand(sSql, odbcConn);

                if (iCtr > 0 && iTck > 0)
                {
                    odbcCmd.Parameters.AddWithValue("@Ctr", iCtr);
                    odbcCmd.Parameters.AddWithValue("@Tck", iTck);
                }
                else
                {
                    odbcCmd.Parameters.AddWithValue("@Cs1", iCs1);
                    odbcCmd.Parameters.AddWithValue("@Cs2", iCs2);
                }

                using (odbcReader = odbcCmd.ExecuteReader(CommandBehavior.Default))
                {
                    //if (odbcReader.HasRows) { }
                    dt.Load(odbcReader);
                }

                foreach (DataRow row in dt.Rows)
                {
                    sDetail =
                        row["CUSTNM"].ToString().Trim() + " " +
                        row["SADDR1"].ToString().Trim() + " " +
                        row["CITY"].ToString().Trim() + " " +
                        row["STATE"].ToString().Trim();
                }

            }
            catch (Exception ex)
            {
                myPage = new MyPage();
                myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
                myPage = null;
            }
            finally
            {
                odbcCmd.Dispose();
                if (odbcConn.State != ConnectionState.Closed)
                    odbcConn.Close(); // It was closed by the using clause (but just in case of a crash) 

            }
        }
        return sDetail;
    }
    // ========================================================================
    #endregion // end mySqls
    // ========================================================================

    // ========================================================================
    #region misc
    // ========================================================================
    // dt.Columns.Add(MakeColumn("Strings"));
    public DataColumn MakeColumn(string name)
    {
        DataColumn dc = new DataColumn();
        dc.DataType = Type.GetType("System.String");
        dc.ColumnName = name;

        return dc;
    }
    // ========================================================================
    public byte[] ResizeImageToByteArray(Image image, int maxWidth, string fileExtension)
    {
        byte[] bytes = new byte[1];

        int iNewWidth = maxWidth; // Current Max width
        int iNewHeight = (int)((double)image.Height * (double)((double)maxWidth / (double)image.Width));

        Image imNewImage = image.GetThumbnailImage(iNewWidth, iNewHeight, null, IntPtr.Zero);

        using (MemoryStream ms = new MemoryStream())
        {
            Bitmap bmp = new Bitmap(imNewImage);
            if (fileExtension.ToLower() == ".jpg" || fileExtension.ToLower() == ".jpeg")
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            else if (fileExtension.ToLower() == ".png")
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            else if (fileExtension.ToLower() == ".gif")
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            else if (fileExtension.ToLower() == ".bmp")
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

            bytes = ms.ToArray();
        }

        return bytes;
    }
    // ========================================================================
    public byte[] ConvertImageToByteArray(Image image, string fileExtension)
    {
        byte[] bytes = new byte[1];

        using (MemoryStream ms = new MemoryStream())
        {
            Bitmap bmp = new Bitmap(image);
            if (fileExtension.ToLower() == ".jpg" || fileExtension.ToLower() == ".jpeg")
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            else if (fileExtension.ToLower() == ".png")
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            else if (fileExtension.ToLower() == ".gif")
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            else if (fileExtension.ToLower() == ".bmp")
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

            bytes = ms.ToArray();
        }

        return bytes;
    }
    //// ========================================================================
    //public Image ResizeImage2(Image sourceImage, int width)
    //{
    //    int iNewHeight = (int)((double)sourceImage.Height * (double)((double)width / (double)sourceImage.Width));

    //    var targetRect = new Rectangle(0, 0, width, iNewHeight);
    //    var targetImage = new Bitmap(width, iNewHeight);

    //    targetImage.SetResolution(sourceImage.HorizontalResolution, sourceImage.VerticalResolution);

    //    using (var graphics = Graphics.FromImage(targetImage))
    //    {
    //        graphics.CompositingMode = CompositingMode.SourceCopy;
    //        graphics.CompositingQuality = CompositingQuality.HighQuality;

    //        using (var wrapMode = new ImageAttributes())
    //        {
    //            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
    //            graphics.DrawImage(sourceImage, targetRect, 0, 0, sourceImage.Width, sourceImage.Height, GraphicsUnit.Pixel, wrapMode);
    //        }
    //    }
    //    return (Image)targetImage;
    //}
    // ========================================================================
    public Image ResizeImage(Image sourceImage, int newWidth)
    {
        Bitmap bmPhoto = new Bitmap(sourceImage);
        int sourceWidth = sourceImage.Width;
        int sourceHeight = sourceImage.Height;

        try
        {
            //int iNewWidth = newWidth; // Current Max width
            int iNewHeight = (int)((double)sourceHeight * (double)((double)newWidth / (double)sourceWidth));

            //Consider vertical pics
            if (sourceWidth < sourceHeight)
            {
                int buff = newWidth;

                newWidth = iNewHeight;
                iNewHeight = buff;
            }

            int sourceX = 0, sourceY = 0, destX = 0, destY = 0;
            float nPercent = 0, nPercentW = 0, nPercentH = 0;

            nPercentW = ((float)newWidth / (float)sourceWidth);
            nPercentH = ((float)iNewHeight / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((newWidth -
                          (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((iNewHeight -
                          (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);


            bmPhoto = new Bitmap(newWidth, iNewHeight,
                          PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(sourceImage.HorizontalResolution,
                         sourceImage.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.White);
            grPhoto.InterpolationMode =
                System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(sourceImage,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            //sourceImage.Dispose();
        }
        catch (Exception ex)
        {
            myPage = new MyPage();
            myPage.SaveError(ex.Message.ToString(), ex.ToString(), "");
            myPage = null;
        }
        finally
        {
        }

        return bmPhoto;
    }
    // ========================================================================
    #endregion // end misc
    // ========================================================================

    // ========================================================================
    // ========================================================================
}