// ***********************************************************************
// Assembly         : Uninf.Upload
// Author           : cz
// Created          : 01-19-2015
//
// Last Modified By : cz
// Last Modified On : 01-22-2015
// ***********************************************************************
// <copyright file="DefaultUploadSettingBase.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Upload
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// 内置上传配置类
    /// 此类实现了分文件夹上传
    /// </summary>
    public abstract class DefaultUploadSettingBase:IUploadSetting
    {
        /// <summary>
        /// 获取一个文件夹内可以存放的子文件夹的个数
        /// </summary>
        /// <returns>子文件夹个数</returns>
        public abstract int GetMaxFolderCnt();
        /// <summary>
        /// 获取一个文件夹中最多存放的文件个数
        /// </summary>
        /// <returns>文件个数</returns>
        public abstract int GetMaxFileCnt();
        /// <summary>
        /// 图片显示在网页上的http基本地址
        /// </summary>
        /// <returns>System.String.</returns>
        /// <remarks>如：http://img.website.com/</remarks>
        public abstract string GetImgWebHost();
        /// <summary>
        /// 保存上传文件的根目录
        /// </summary>
        /// <returns>System.String.</returns>
        public abstract string GetUploadRootPath();

        /// <summary>
        /// 上穿服务器地址
        /// </summary>
        /// <returns>System.String.</returns>
        public abstract string GetUploadIp();

        /// <summary>
        /// 生成2级目录保存文件
        /// </summary>
        /// <param name="rootDir">The root dir.</param>
        /// <returns>System.String.</returns>
        /// <remarks>如:
        /// <para>/root/0/0/file.jpg</para><para>/root/0/1/file.jpg</para><para>/root/1/0/file.jpg</para><para>/root/1/1/file.jpg</para><para>通过设置的文件夹下子文件夹和文件最大数量来计算</para></remarks>
        public virtual string GeneratPathFromRoot(string rootDir)
        {
            //var rootpath = this.GetUploadRootPath() + "\\" + rootDir;
            var rootpath = this.GetUploadIp().TrimEnd(new[]{'\\','/'}) + "\\"+this.GetUploadRootPath().Trim(new[]{'\\','/'})+"\\" + rootDir;
            if (!Directory.Exists(rootpath))
            {
                Directory.CreateDirectory(rootpath);
            }
            var root = new DirectoryInfo(rootpath);
            if (root.GetDirectories().Length == 0)
            {
                var path = root.FullName + "\\0\\0";
                Directory.CreateDirectory(path);
                root = null;
                return path;
            }
            var debug =
                root.GetDirectories()
                    .SelectMany(x => x.GetDirectories())
                    .Select(x => new KeyValuePair<string, int>(x.FullName, x.GetFiles().Length)).ToList();
            var dir =root.GetDirectories().SelectMany(x => x.GetDirectories()).FirstOrDefault(x => x.GetFiles().Length < this.GetMaxFileCnt());
            if (dir != null)
            {
                var result = dir.FullName;
                root = null;
                dir = null;
                return result;
            }
            var lev1 = root.GetDirectories().FirstOrDefault(x => x.GetDirectories().Length < this.GetMaxFolderCnt());
            if (lev1 != null)
            {
                var last = lev1.GetDirectories().OrderBy(x => x.Name).Last().Name;
                var path = lev1.FullName + "\\" + (Convert.ToInt32(last) + 1);
                Directory.CreateDirectory(path);

                root = null;
                dir = null;
                lev1 = null;

                return path;
            }
            var lev1last = root.GetDirectories().OrderBy(x => x.Name).Last().Name;
            var lev1path = root.FullName + "\\" + (Convert.ToInt32(lev1last) + 1) + "\\0";
            Directory.CreateDirectory(lev1path);
            root = null;
            dir = null;
            lev1 = null;
            lev1last = null;
            return lev1path;
        }

        /// <summary>
        /// 通过一个文件路径，算出他存储在库里应该是
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <returns>System.String.</returns>
        /// <remarks>生成以files:开头的路径格式</remarks>
        public virtual string GetSaveToDBPath(string filepath)
        {
            var name = filepath;
            if (name.StartsWith(@"\\"))
            {
                return "files:" + name.Replace("\\", "/");
            }
            return name;
        }

        /// <summary>
        /// 通过一个文件路径，算出他展示在网页上的地址
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <returns>System.String.</returns>
        public virtual string GetWebShowPath(string filepath)
        {
            var rootDir = this.GetUploadRootPath().ToLower().Trim(new[] { '\\', '/' });
            var fmtLocal = "\\" + rootDir + "\\";
            var fmtWeb = "/" + rootDir + "/";
            var path = filepath;
            if (filepath.Contains(fmtLocal))
            {
                var index = filepath.IndexOf(fmtLocal);
                path = filepath.Substring(index);
            }
            else if (filepath.Contains(fmtWeb))
            {
                var index = filepath.IndexOf(fmtWeb);
                path = filepath.Substring(index);
            }

            //var webPath = filepath.Replace(this.GetUploadIp(), string.Empty);
            //webPath = webPath.Replace(@"files:", string.Empty).Replace(this.GetUploadIp().Replace("\\", "/"), string.Empty);
            return this.GetImgWebHost().TrimEnd('/') + ("/"+ path.Replace("\\","/")).Replace("//","/");
        }

        /// <summary>
        /// 通过数据库内存储的，获取实际文件
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>FileInfo.</returns>
        public FileInfo GetFileFromDBPath(string path)
        {
            return new FileInfo(path.Replace("files:", string.Empty).Replace("/", "\\"));
        }

        /// <summary>
        /// 网络路径到硬盘路径
        /// </summary>
        /// <param name="webpath">The webpath.</param>
        /// <returns>System.String.</returns>
        public string WebPathToFilePath(string webpath)
        {
            var rootDir = this.GetUploadRootPath().ToLower().Trim(new[] { '\\', '/' });

            var dir = "/" + rootDir + "/";
            var rightPath = webpath.Substring(webpath.IndexOf(dir));

            return this.GetUploadIp().TrimEnd(new[]{'/','\\'}) + rightPath.Replace("/","\\");
        }
    }
}