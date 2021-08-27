using CoreLibrary.Models;
using Newtonsoft.Json;
using System;
using System.IO;

namespace API.Extensions
{
    public class APICredentialAuth
    {
        public static ResponseDetails APIKeyCheck(string key)
        {
            var fileName = Path.Combine("Resources", "Keys", "credential.json");
            var path = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            if (!File.Exists(path))
                return new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "Đường dẫn thư mục không tồn tại" };

            var json = File.ReadAllText(path);
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            var keyObj = jsonObj["api_key"];

            if(key == Convert.ToString(keyObj))
                return new ResponseDetails() { StatusCode = ResponseCode.Success, Message = "API key hợp lệ" };
            else
                return new ResponseDetails() { StatusCode = ResponseCode.Error, Message = "API key không hợp lệ" };
        }
    }
}
