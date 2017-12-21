using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace MultimediaWebSite.Models
{
    public class FileTableInfo
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public Int64 FileSize { get; set; }
        public int CarId { get; set; }
        public int SubInstId { get; set; }
        public string VideoStart { get; set; }
        public string VideoEnd { get; set; }
        public string Note { get; set; }
        public string FilePath { get; set; }
        //public MemoryStream FileStream { get; set; }
        
        public FileTableInfo()
        {
        }
        public List<FileTableInfo> GetListofFileInfo()   //StoredVideoFiles 
        {
            List<FileTableInfo> listFileInfo = new List<FileTableInfo>();            
            var path = @"C:\Users\Boco_PC\source\repos\MultimediaWebSite\MultimediaWebSite\Files\Videos";
            var files = Directory.GetFiles(path);
            int index = 0;
            foreach (var file in files)
            {                
                var info = GetFileInfo(file);
                info.Id = index++;
                listFileInfo.Add(info);
            }            
            return listFileInfo;
        }
        public FileTableInfo GetFileInfo(string filePath)
        {
            FileTableInfo fileInfo = new FileTableInfo();
            var mimeType = MimeMapping.GetMimeMapping(filePath);
            FileInfo info = new FileInfo(filePath);
            fileInfo.FileName = info.Name;
            fileInfo.FileSize = info.Length;
            fileInfo.FilePath = filePath;
            fileInfo.FileType = mimeType;
            return fileInfo;
        }
        public FileTableInfo PlayFile(List<FileTableInfo> list,int id)
        {
            return list.Where(m => m.Id == id).FirstOrDefault();
        }
        public byte[] GetFileByte(string filePath)
        {
            FileInfo info = new FileInfo(filePath);
            byte[] dataByte = null;
            using (var stream = info.OpenRead())
            {
                dataByte = new byte[stream.Length];
                stream.Read(dataByte, 0, (int)stream.Length);
            }
            return dataByte;
        }
        public byte[] GetFileBlock(string filePath, long start, long length)
        {
            FileInfo info = new FileInfo(filePath);
            byte[] dataByte = new byte[length]; 
            
            using (var stream = info.OpenRead())
            {
                int count = 0;
                if (length + start > stream.Length)
                    count = (int)(stream.Length - start);
                else
                    count = (int)length;
                stream.Seek(start, SeekOrigin.Begin);
                stream.Read(dataByte, 0, count);
            }
            return dataByte;
        }
        public DataTable GetAFileFromFileTable(Guid id)
        {
            DataTable file = new DataTable();
            return file;
        }
    }
}