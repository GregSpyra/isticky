<%@ WebHandler Language="C#" Class="downloader" %>

using System;
using System.Web;
using System.IO;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;

public class downloader : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) 
    {
        try
        {
            string fileName = Guid.NewGuid().ToString() + ".docx";
            string path = context.Server.MapPath("~/docs/") + fileName;
            if (!(string.IsNullOrEmpty(context.Request["what"])) && (context.Request["what"].ToLower() == "saveasword")
                && !(string.IsNullOrEmpty(context.Request.Form[0])))
            {
                SaveAsWord(context.Request.Form[0], path, fileName);
            } 

        }
        catch (Exception ex)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(ex.ToString());
            System.Diagnostics.Trace.WriteLine(ex.ToString());
        }
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }


    /// <summary>
    /// Wrapper function to save the richtext values as word
    /// </summary>
    /// <param name="input">rich text</param>
    private void SaveAsWord(string input, string fullFilePath, string fileNameOnly)
    {
        CreateDocument(fullFilePath);
        input = ReplaceMailMerge(input);
        generateWordDocument(input, fullFilePath, fileNameOnly);

    }


    /// <summary>
    /// Replace the merge field with some custom text
    /// In real world this may come from database
    /// </summary>
    /// <param name="input">input text</param>
    /// <returns>Mail merged text</returns>
    private string ReplaceMailMerge(string input)
    {
        input = input.Replace("{^FirstName^}", "Billy");
        input = input.Replace("{^LastName^}", "Bob");
        input = input.Replace("{^Title^}", "Dr.");
        return input;
    }

    /// <summary>
    /// Create a new docx
    /// </summary>
    /// <param name="path">Path where the doc file is to be created</param>
    private void CreateDocument(string path)
    {

        // Create a Wordprocessing document. 
        using (WordprocessingDocument myDoc = WordprocessingDocument.Create(path, WordprocessingDocumentType.Document))
        {
            // Add a new main document part. 
            MainDocumentPart mainPart = myDoc.AddMainDocumentPart();
            //Create DOM tree for simple document. 
            mainPart.Document = new Document();
            Body body = new Body();
            Paragraph p = new Paragraph();
            Run r = new Run();
            Text t = new Text("");
            //Append elements appropriately. 
            r.Append(t);
            p.Append(r);
            body.Append(p);
            mainPart.Document.Append(body);
            // Save changes to the main document part. 
            mainPart.Document.Save();
        }
    }

    /// <summary>
    /// Add the HTML markup in the file
    /// Save the document
    /// Send the path of this document to the caller
    /// </summary>
    /// <param name="htmlMarkup">Rich text</param>
    /// <param name="fullFilePath">Path of the file</param>
    /// <param name="fileNameOnly">Name of the file</param>
    public void generateWordDocument(string htmlMarkup, string fullFilePath, string fileNameOnly)
    {
        try
        {
            /*----------- Generate the Document -----------------------*/
            //put some title
            string pageTitle = Guid.NewGuid().ToString();
            //open the document
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fullFilePath, true))
            {
                //get the document
                MainDocumentPart mainPart = wordDoc.MainDocumentPart;
                int altChunkIdCounter = 1;
                int blockLevelCounter = 1;

                string mainhtml = "<html><head><style type='text/css'>.catalogGeneralTable{border-collapse: collapse;text-align: left;} .catalogGeneralTable td, th{ padding: 5px; border: 1px solid #999999; }</style></head><body style='font-family:Trebuchet MS;font-size:.9em;'>" + htmlMarkup + "</body></html>";
                string altChunkId = String.Format("AltChunkId{0}", altChunkIdCounter++);

                //Import data as html content using Altchunk
                AlternativeFormatImportPart chunk = mainPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.Html, altChunkId);

                //add the chunk to the doc
                using (Stream chunkStream = chunk.GetStream(FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter stringWriter = new StreamWriter(chunkStream, Encoding.UTF8)) //Encoding.UTF8 is important to remove special characters
                    {
                        stringWriter.Write(mainhtml);
                    }
                }

                AltChunk altChunk = new AltChunk();
                altChunk.Id = altChunkId;
                //insert the text in the doc
                mainPart.Document.Body.InsertAt(altChunk, blockLevelCounter++);
                //save the document
                mainPart.Document.Save();
            }
            /*----------- End Generate the Document -----------------------*/
            
            /* ------- Send the response -----------*/
            //clear the response object
            HttpContext.Current.Response.ClearContent();
            //add the demilited string to the response object and write it. 
            string url = HttpContext.Current.Request.ApplicationPath + "/docs/" + fileNameOnly;
            HttpContext.Current.Response.Write(url);
            HttpContext.Current.Response.End();

            /* -------End Send the response -----------*/
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.Message.ToString());
        }
    }


    /// <summary>
    /// Function to download the newly generated doc file
    /// </summary>
    /// <param name="completeFilePath">File path</param>
    /// <param name="contentType">Content type</param>
    private void DownloadFile(string completeFilePath, string fileNameOnly, string contentType)
    {
        Stream iStream = null;

        // Buffer to read 10K bytes in chunk:
        byte[] buffer = new Byte[10000];

        // Length of the file:
        int length;

        // Total bytes to read:
        long dataToRead;

        try
        {
            // Open the file.
            iStream = new FileStream(completeFilePath, FileMode.Open,
            FileAccess.Read, FileShare.Read);

            // Total bytes to read:
            dataToRead = iStream.Length;

            HttpContext.Current.Response.ContentType = contentType;
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileNameOnly);

            // Read the bytes.
            while (dataToRead > 0)
            {
                // Verify that the client is connected.
                if (HttpContext.Current.Response.IsClientConnected)
                {
                    // Read the data in buffer.
                    length = iStream.Read(buffer, 0, 10000);

                    // Write the data to the current output stream.
                    HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);

                    // Flush the data to the HTML output.
                    HttpContext.Current.Response.Flush();

                    buffer = new Byte[10000];
                    dataToRead = dataToRead - length;
                }
                else
                {
                    //prevent infinite loop if user disconnects
                    dataToRead = -1;
                }
            }
        }
        catch (Exception ex)
        {
            // Trap the error, if any.
            HttpContext.Current.Response.Write("Error : " + ex.Message);
        }
        finally
        {
            if (iStream != null)
            {
                //Close the file.
                iStream.Close();
            }
            HttpContext.Current.Response.Close();
        }
    }


}