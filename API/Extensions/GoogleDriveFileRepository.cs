using CoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Google.Apis.Upload;
using System.Net.Http.Headers;

namespace API.Extensions
{
    public class GoogleDriveFileRepository
    {
        static string[] Scopes = { DriveService.Scope.DriveReadonly };
        static string ApplicationName = "Truyen Drive API .NET";

        public static DriveService GetService()
        {
            UserCredential credential;
            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }


            DriveService service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });

            return service;
        }

        public static List<Truyen> GetDriveFiles()
        {
            DriveService service = GetService();

            FilesResource.ListRequest FileListRequest = service.Files.List();

            FileListRequest.Fields = "nextPageToken, files(id, name, size, version, createdTime)";

            IList<Google.Apis.Drive.v3.Data.File> files = FileListRequest.Execute().Files;

            List<Truyen> fileList = new List<Truyen>();

            if(files != null && files.Count > 0)
            {
                foreach(var file in files)
                {
                    Console.WriteLine("{0} ({1})", file.Name, file.Id);
                    //Truyen t = new Truyen
                    //{
                    //    TruyenID = file.Id,
                    //};
                    //fileList.Add(t);
                }
            }
            return fileList;
        }

        public static ResponseDetails FileUpload(IFormFile file)
        {
            if(file != null && file.Length > 0)
            {
                DriveService service = GetService();

                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fullPath = Path.Combine(pathToSave, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var fileMetaData = new Google.Apis.Drive.v3.Data.File();
                fileMetaData.Name = Path.GetFileName(file.FileName);
                fileMetaData.MimeType = MimeKit.MimeTypes.GetMimeType(fullPath);

                using (var fsSource = new FileStream(file.FileName, FileMode.Open))
                {
                    // Create a new file, with metadata and stream.
                    var request = service.Files.Create(fileMetaData, fsSource, fileMetaData.MimeType);
                    request.Fields = "id";
                    var results = request.Upload();

                    if (results.Status == UploadStatus.Failed)
                    {
                        return new ResponseDetails()
                        {
                            StatusCode = ResponseCode.Error,
                            Message = results.Exception.ToString(),
                            Value = ""
                        };
                    }
                }
            }
            return new ResponseDetails()
            {
                StatusCode = ResponseCode.Error,
                Message = "Upload thành công",
                Value = ""
            };
        }
    }
}
