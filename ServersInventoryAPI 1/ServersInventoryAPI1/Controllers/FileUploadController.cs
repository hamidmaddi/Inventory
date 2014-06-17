using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Xml.Linq;

namespace ServersInventoryAPI1
{
    public class FileUploadController : ApiController
    {
        [HttpPost]
        public async Task<FileResult> UploadFile()
        {
            string uploadFolder = WebConfigurationManager.AppSettings["Source"];

            // Verify that this is an HTML Form file upload request
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
            }
            // Create a stream provider for setting up output streams
            MultipartFormDataStreamProvider streamProvider = new MultipartFormDataStreamProvider(uploadFolder);

            // Read the MIME multipart asynchronously content using the stream provider we just created.
            await Request.Content.ReadAsMultipartAsync(streamProvider);

            string localfileName = streamProvider.FileData.Select(entry => entry.LocalFileName).FirstOrDefault();
            string filename = HttpContext.Current.Request.QueryString["filename"];
            string uploadType = "inventory";
            uploadType = HttpContext.Current.Request.QueryString["uploadtype"];
            LoggingManager.Instance.GetLogger.InfoFormat("FileName: {0}, UploadType: {1}", filename, uploadType);

            FileResult result = new FileResult();
            if (string.IsNullOrEmpty(filename))
            {
                LoggingManager.Instance.GetLogger.Error("Submitted File Name does not exist..Aborting.");
                result.Success = "Failure";
            }
            else
            {
                try
                {
                    if (File.Exists(localfileName))
                    {
                        // Upload inventory file
                        try
                        {
                            string filenameEncoded = filename.Replace(".", "%2E");
                            string uri = "http://85.15.17.29:8091/ServersInventoryAPI/api/FileUpload?filename=" + filenameEncoded + "&uploadtype=" + uploadType;
                            WebClient client = new WebClient();
                            client.UploadFile(uri, localfileName);

                            //Delete file if upload is successfull
                            try
                            {
                                string destinationPathProcessed = WebConfigurationManager.AppSettings["DestinationProcessed"];
                                File.Copy(localfileName, Path.Combine(destinationPathProcessed, filename), true);
                                File.Delete(localfileName);
                                LoggingManager.Instance.GetLogger.Info(string.Format("File {0} was copied successfully", localfileName));
                                result.Success = "Success";
                            }
                            catch (Exception ex)
                            {
                                LoggingManager.Instance.GetLogger.Error(string.Format("There was an error while trying to copy the file {1}. Exception: {0}", ex, localfileName));
                                result.Success = "Failure";
                            }
                        }
                        catch (Exception ex)
                        {
                            LoggingManager.Instance.GetLogger.Error(string.Format("There was an error while trying to upload the file {1}. Exception: {0}", ex, localfileName));
                            result.Success = "Failure";
                        }
                    }
                    else
                    {
                        LoggingManager.Instance.GetLogger.Error(String.Format("File <{0}> could not be found. Aborting...", localfileName));
                    }

                }
                catch (Exception ex)
                {
                    LoggingManager.Instance.GetLogger.Error("Fatal exception thrown while processing file. Aborting...", ex);
                    result.Success = "Failure";
                }
            }

            return result;

        }
    }
}