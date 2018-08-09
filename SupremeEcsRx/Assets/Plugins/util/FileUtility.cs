using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NO1Software.Plugins.util
{
    public class FileUtility
    {
        /// <summary>
        /// Read to array of bytes from file 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] FileConvertToBytes(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = new byte[fileStream.Length];
                fileStream.Read(bytes, 0, bytes.Length);
                return bytes;
            }
        }

        /// <summary>
        /// write to file from array of bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="filePath"></param>
        public static void BytesConvertToFile(byte[] bytes, string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="filePath"></param>
        public static void BytesConvertToFileIfFileCurrentDirectoryNotExistThenCreateDirectoryFirst(byte[] bytes, string filePath)
        {
            int endIndex = filePath.LastIndexOf("/");

            string folderPath = filePath.Substring(0, endIndex);

            if (Directory.Exists(folderPath) == false)
            {
                Directory.CreateDirectory(folderPath);
            }

            BytesConvertToFile(bytes, filePath);
        }

        /// <summary>
        /// if file was exist,return true
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool CheckFileIsExist(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        /// delete specific file
        /// </summary>
        /// <param name="filePath"></param>
        public static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// delete specific folder
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="isRecursive"></param>
        public static void DeleteDirectory(string folderPath, bool isRecursive)
        {
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, isRecursive);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        public static void IfDeletedFileCurrentDirectoryIsEmptyDeleteRecursively(string filePath)
        {
            int endIndex = filePath.LastIndexOf("/");

            if (endIndex < 0)
            {
                return;
            }

            string folderPath = filePath.Substring(0, endIndex);

            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);

            FileSystemInfo[] fileSystemInfo = directoryInfo.GetFileSystemInfos();

            if (fileSystemInfo.Length == 0)
            {
                Directory.Delete(folderPath);

                IfDeletedFileCurrentDirectoryIsEmptyDeleteRecursively(folderPath);
            }
        }
    }
}
