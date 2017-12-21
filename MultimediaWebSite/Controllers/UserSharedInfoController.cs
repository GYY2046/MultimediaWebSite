using MultimediaWebSite.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MultimediaWebSite.Controllers
{
    public class UserSharedInfoController : Controller
    {
        private static List<FileTableInfo> list = new List<FileTableInfo>();
        private static readonly int size = 1048576 * 5;//限制5M
        // GET: UserSharedInfo
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult VideoInfo()
        {
            FileTableInfo fi = new FileTableInfo();
            list.Clear();
            var fileInfoList = fi.GetListofFileInfo();
            list.AddRange(fileInfoList);
            return View(list);
        }
        public ActionResult PlayVideo(int Id)
        {
            FileTableInfo fi = new FileTableInfo();
            fi = list.Where(m => m.Id == Id).FirstOrDefault();
            string name = fi.FileName;
            string contentType = fi.FileType;
            //var stream = fi.FileStream;
            //Byte[] video = (Byte[])row["data"];
            //string mimeType;
            //switch (contentType.ToUpper())
            //{
            //    case "MOV":
            //        mimeType = "video/quicktime";
            //        break;
            //    case "MP4":
            //        mimeType = "video/mp4";
            //        break;
            //    case "FLV":
            //        mimeType = "video/x-flv";
            //        break;
            //    case "AVI":
            //        mimeType = "video/x-msvideo";
            //        break;
            //    case "WMV":
            //        mimeType = "video/x-ms-wmv";
            //        break;
            //    case "MJPG":
            //        mimeType = "video/x-motion-jpeg";
            //        break;
            //    case "TS":
            //        mimeType = "video/MP2T";
            //        break;
            //    default:
            //        mimeType = "video/mp4";
            //        break;
            //}
            //return File(stream, contentType);
            var data = fi.GetFileByte(fi.FilePath);
            Response.ContentType = contentType;
            Response.AddHeader("Content-Accept", Response.ContentType);
            return File(data, Response.ContentType);

        }

        public ActionResult PlayVideoSync(int Id)
        {
            FileTableInfo fi = new FileTableInfo();
            fi = list.Where(m => m.Id == Id).FirstOrDefault();
            string name = fi.FileName;
            string contentType = fi.FileType;
            long fSize = fi.FileSize;
            long startbyte = 0;
            long endbyte = fSize - 1;
            int statusCode = 200;
            if ((Request.Headers["Range"] != null))
            {
                //Get the actual byte range from the range header string, and set the starting byte.
                string[] range = Request.Headers["Range"].Split(new char[] { '=', '-' });
                startbyte = Convert.ToInt64(range[1]);
                if (range.Length > 2 && range[2] != "")
                {
                    endbyte = Convert.ToInt64(range[2]);
                }
                else
                {
                    endbyte = startbyte + size - 1; //5M 字节
                    if (endbyte >= fSize - 1)
                        endbyte = fSize - 1;
                }
                //If the start byte is not equal to zero, that means the user is requesting partial content.                
                if (startbyte != 0 || endbyte != fSize - 1 || range.Length > 2 && range[2] == "")
                { statusCode = 206; }//Set the status code of the response to 206 (Partial Content) and add a content range header.                                    
            }
            long desSize = endbyte - startbyte + 1;
            var data = fi.GetFileBlock(fi.FilePath, startbyte, desSize);
            desSize = data.Count();
            //Headers
            Response.StatusCode = statusCode;
            Response.ContentType = contentType;
            Response.AddHeader("Content-Accept", Response.ContentType);
            Response.AddHeader("Content-Length", desSize.ToString());
            Response.AddHeader("Content-Range", string.Format("bytes {0}-{1}/{2}", startbyte, endbyte, fSize));

            return File(data, Response.ContentType);
        }
    }
}