using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SeleniumManager
{
    public class FolderManagement
    {
        public static void DeleteFile(string fileName)
        {
            File.SetAttributes(fileName, FileAttributes.Normal);
            File.Delete(fileName);
        }

        public static void DeleteAllDirectories(string directoryPath)
        {
            try
            {
                var downloadsInfo = new DirectoryInfo(directoryPath);
                foreach (var directory in downloadsInfo.GetDirectories())
                    directory.Delete();
            }
            catch(Exception err)
            {
                MessageHandler.ExceptionMessage(err);
                throw new Exception(DefaultMessages.ErrorDeleteFoldersFromFolder);
            }
            
        }

        public static void DeleteAllFiles(string directoryPath)
        {
            try
            {
                var downloadsInfo = new DirectoryInfo(directoryPath);
                foreach (var file in downloadsInfo.GetFiles())
                    DeleteFile(file.FullName);
            }
            catch(Exception err)
            {
                MessageHandler.ExceptionMessage(err);
                throw new Exception(DefaultMessages.ErrorDeleteFilesFromFolders);
            }
            
        }
    }
}
