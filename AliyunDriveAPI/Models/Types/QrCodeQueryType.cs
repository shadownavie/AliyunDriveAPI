using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AliyunDriveAPI.Models.Types;
public enum QrCodeQueryType
{
    [Description("等待扫码")]
    NEW = 0,
    [Description("扫码成功")]
    S = 1,  
    [Description("过期/错误")]
    OUT = -2,
}
