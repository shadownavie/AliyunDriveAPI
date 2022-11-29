using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using static System.Net.Mime.MediaTypeNames;

namespace AliyunDriveAPI.Models.Response
{
 
    public class QrCodeResponse
    {
        public bool HasError { get; set; }
        public QrCodeContent Content { get; set; }

        public string CodeContent
        {
            get
            {
                return Content?.Data?.CodeContent;
            }
        }

        public string Ck
        {
            get
            {
                return Content?.Data?.Ck;
            }
        }

        public long? ResultCode
        {
            get
            {
                return Content?.Data?.ResultCode;
            }
        }

        public long? T
        {
            get
            {
                return Content?.Data?.T;
            }
        }

        /// <summary>
        /// 查询扫码状态
        /// </summary>
        public (QrCodeQueryType Status, string refreshToken, string DriveId) QueryStatus
        {
            get
            {
                (QrCodeQueryType Status, string refreshToken, string DriveId) info = new();
                var s = Content?.Data?.QrCodeStatus ?? "";
                info.Status = QrCodeQueryType.NEW;
                switch (s)
                {
                    case "NEW":
                        info.Status = QrCodeQueryType.NEW;
                        break;

                    case "CONFIRMED":
                        info.Status = QrCodeQueryType.S;

                        var base64data = Encoding.UTF8.GetString(Convert.FromBase64String(Content?.Data?.bizExt ?? ""));
                        if (!string.IsNullOrEmpty(base64data))
                        {
                            JsonNode j = JsonNode.Parse(base64data);
                            if (j != null && j["pds_login_result"]!=null)
                            {
                                if(j["pds_login_result"]["refreshToken"]!=null && j["pds_login_result"]["defaultDriveId"] != null)
                                {
                                    info.refreshToken = j["pds_login_result"]["refreshToken"].GetValue<string>();
                                    info.DriveId = j["pds_login_result"]["defaultDriveId"].GetValue<string>();
                                }
                               
                            }
                            

                          

                        }
                        break;
                    case "EXPIRED":
                        info.Status = QrCodeQueryType.OUT;
                        break;
                    default:
                        info.Status = QrCodeQueryType.NEW;
                        break;
                }
                return info;

            }
        }


         
    }

    public class QrCodeContent
    {
   
       
        public int Status { get; set; }


        public bool Success { get; set; }
        public QrCodeContentData Data { get; set; }
    }
    public class QrCodeContentData
    {
        public long T { get; set; }

        [JsonPropertyName("codeContent")]
        public string CodeContent { get; set; }

        public string Ck { get; set; }

        [JsonPropertyName("resultCode")]
        public long ResultCode { get; set; }


        [JsonPropertyName("qrCodeStatus")]
        public string QrCodeStatus { get; set; }

        /// <summary>
        /// 登陆成功返回的info
        /// </summary>
        [JsonPropertyName("bizExt")]
        public string bizExt { get; set; }


    }

}
