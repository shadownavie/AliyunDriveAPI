using AliyunDriveAPI.Models;
using AliyunDriveAPI.Models.Request;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AliyunDriveAPI.Test;

public partial class AliDriveClientTest
{
    private AliyunDriveApiClient _client;

    //refresh_token=JSON.parse(window.localStorage.getItem("token")).refresh_token
    //DriveId=JSON.parse(window.localStorage.getItem("token")).default_drive_id

    private string DriveId = "44882611";//default_drive_id:"44882611"
    private string ParentFileId = "root";//"6383756f6adc12c6de134c738a44926556b2c915";

    private string reftoken = "";

    [SetUp]
    public async void Setup()
    {
 
        //FileSystemWatcher fsw = new FileSystemWatcher();
        //fsw.Path = path;
        //fsw.IncludeSubdirectories = true;   //设置监控C盘目录下的所有子目录
        //fsw.Filter = "*.*";   //设置监控文件的类型
        //fsw.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Size;   //设置文件的文件名、目录名及文件的大小改动会触发Changed事件
        //fsw.Created += new FileSystemEventHandler(fileSystemWatcher_EventHandle);  //绑定事件触发后处理数据的方法。
        //fsw.Deleted += new FileSystemEventHandler(fileSystemWatcher_EventHandle);
        //fsw.Changed += new FileSystemEventHandler(fileSystemWatcher_EventHandle);
        //fsw.Renamed += new RenamedEventHandler(fileSystemWatcher_Renamed);  //重命名事件与增删改传递的参数不一样。

        //fsw.EnableRaisingEvents = true;  //启动监控


        string refreshToken = File.Exists("refresh_token") ?
            File.ReadAllText("refresh_token") :
            Environment.GetEnvironmentVariable("REFRESH_TOKEN");
        _client = new AliyunDriveApiClient(refreshToken);
    }
 

    [Test(Description = "列出文件")]
    public async Task FileListAsync()
    {
        var res = await _client.FileListAsync(new()
        {
            DriveId = DriveId
        });
        Assert.IsTrue(res.Items.Length > 0);
    }

    [Test]
    public async Task FileGetAsync()
    {
        var res = await _client.FileGetAsync(DriveId, ParentFileId);
        Assert.IsTrue(!string.IsNullOrEmpty(res.FileId));
    }

    [Test]
    public async Task FileShareAsync()
    {
        var res = await _client.ShareAsync(new FileShareRequest(DriveId, ParentFileId, TimeSpan.FromDays(1)));
        Assert.AreEqual(res.ShareUrl, $"https://www.aliyundrive.com/s/{res.ShareId}");
    }

    [Test]
    public async Task FileShareWithPwdAsync()
    {
        var password = "6666";
        var res = await _client.ShareAsync(new FileShareRequest(DriveId, ParentFileId, TimeSpan.FromDays(1), password));
        Assert.AreEqual(res.ShareUrl, $"https://www.aliyundrive.com/s/{res.ShareId}");
        Assert.AreEqual(res.SharePwd, password);
    }


    [Test]
    public async Task FileShareWithoutExpirationAsync()
    {
        var res = await _client.ShareAsync(new FileShareRequest(DriveId, ParentFileId));
        Assert.AreEqual(res.ShareUrl, $"https://www.aliyundrive.com/s/{res.ShareId}");
        Assert.IsNull(res.Expiration);
    }
}