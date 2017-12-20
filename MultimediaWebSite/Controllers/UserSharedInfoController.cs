using MultimediaWebSite.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MultimediaWebSite.Controllers
{
    public class UserSharedInfoController : Controller
    {
        // GET: UserSharedInfo
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult VideoInfo(string id)
        {
            return PartialView();
        }
        public ActionResult PlayVideo(Guid id)
        {
            FileTableInfo fi = new FileTableInfo();
            // DataTable file = fi.GetAFile(id);    //using old way of access database 
            DataTable file = fi.GetAFileFromFileTable(id);
            DataRow row = file.Rows[0];

            string name = (string)row["video_name"];
            string contentType = (string)row["content_Type"];
            Byte[] video = (Byte[])row["data"];
            string mimeType;
            switch (contentType.ToUpper())
            {
                case "MOV":
                    mimeType = "video/quicktime";
                    break;
                case "MP4":
                    mimeType = "video/mp4";
                    break;
                case "FLV":
                    mimeType = "video/x-flv";
                    break;
                case "AVI":
                    mimeType = "video/x-msvideo";
                    break;
                case "WMV":
                    mimeType = "video/x-ms-wmv";
                    break;
                case "MJPG":
                    mimeType = "video/x-motion-jpeg";
                    break;
                case "TS":
                    mimeType = "video/MP2T";
                    break;
                default:
                    mimeType = "video/mp4";
                    break;
            }
            return File(video, mimeType);
        }
    }
}