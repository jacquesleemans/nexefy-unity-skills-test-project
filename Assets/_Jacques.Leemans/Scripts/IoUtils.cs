using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JacquesLeemans
{
    /// <summary>
    /// IoUtils
    /// </summary>
    public static class IoUtils
    {
        /// <summary>
        /// GetProjectPath
        /// </summary>
        /// <returns></returns>
        private static string GetProjectPath()
        {
            return System.IO.Path.GetFullPath(System.IO.Path.Combine(Application.dataPath));
        }

        /// <summary>
        /// GetOutputDirPath
        /// </summary>
        /// <param name="dirName"></param>
        /// <returns></returns>
        private static string GetOutputDirPath(string dirName)
        {
            return System.IO.Path.GetFullPath(System.IO.Path.Combine(GetProjectPath(), @"./../../", "Outputs",
                dirName));
        }

        /// <summary>
        /// CreateDir
        /// </summary>
        /// <param name="path"></param>
        private static void CreateDir(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// GetScreenShotName
        /// </summary>
        /// <param name="dirName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static string GetScreenShotName(string dirName, string fileName)
        {
            string dirPath = GetOutputDirPath(dirName);

            CreateDir(GetOutputDirPath(dirPath));

            return System.IO.Path.Combine(dirPath, fileName);
        }
        
        /// <summary>
        /// SaveTexture2d
        /// </summary>
        /// <param name="texture2D"></param>
        /// <param name="dirName"></param>
        /// <param name="fileName"></param>
        public static void SaveTexture2d(Texture2D texture2D, string dirName, string fileName)
        {
            if (texture2D == null)
            {
                Debug.Log("texture2D cant be null");
                return;
            }

            //Make sure fileName is NOT NullOrEmpty
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = $"screenshot-{System.DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";    
            }
            
            
            byte[] bytes = texture2D.EncodeToPNG();

            string filename = GetScreenShotName(dirName, fileName);

            System.IO.File.WriteAllBytes(filename, bytes);
        }
    }
}