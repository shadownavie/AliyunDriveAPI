using NUnit.Framework;
using System.Text;
using System.Threading.Tasks;
using System;
using AliyunDriveAPI.Models;
using System.Text.Json.Nodes;
using System.Reflection.Metadata;
using Newtonsoft.Json.Linq;

namespace AliyunDriveAPI.Test;

public partial class AliDriveClientTest
{
    
    [Test(Description = "获取扫码登陆")]
    public async Task Qrcode()
    {
        AliyunDriveApiClient client = new AliyunDriveApiClient(this.reftoken);
        var res = await client.GetQrcodeAsync();
        Assert.IsTrue(!res.HasError);
        while (true)
        {
            var qrcodeResutl = await client.QueryQrcodeStatusAsync(res.T, res.Ck);
            if(qrcodeResutl.QueryStatus.Status== Models.Types.QrCodeQueryType.S)
            {
                this.reftoken = qrcodeResutl.QueryStatus.refreshToken;
                this.DriveId = qrcodeResutl.QueryStatus.DriveId;
                break;
            }
            else if (qrcodeResutl.QueryStatus.Status == Models.Types.QrCodeQueryType.OUT)
            {
                break;
            }
            await Task.Delay(1000);
        }
 
        Assert.IsTrue(!string.IsNullOrEmpty(this.reftoken));
        Assert.IsTrue(!string.IsNullOrEmpty(this.DriveId));
      

    }
}
