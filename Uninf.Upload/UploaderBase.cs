// ***********************************************************************
// Assembly         : Uninf.Upload
// Author           : cz
// Created          : 01-08-2015
//
// Last Modified By : cz
// Last Modified On : 01-19-2015
// ***********************************************************************
// <copyright file="UploaderBase.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Upload
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Web;

    /// <summary>
    /// 内置上传
    /// </summary>
    public class UploaderBase : IUpload
    {
        /// <summary>
        /// The setting
        /// </summary>
        private readonly IUploadSetting setting;

        /// <summary>
        /// 实例化上传类
        /// </summary>
        /// <param name="setting">上传配置</param>
        public UploaderBase(IUploadSetting setting)
        {
            this.setting = setting;
        }


        /// <summary>
        /// 上传文件到指定文件夹
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="saveDir">The save dir.</param>
        /// <param name="compute">if set to <c>true</c> [compute].</param>
        /// <returns>System.String.</returns>
        public virtual string Upload(HttpPostedFileBase file, string saveDir, bool compute = true)
        {
            return this.Upload(file, saveDir, this.GetRandomName(Path.GetExtension(file.FileName)),compute);
        }

        /// <summary>
        /// 上传到指定文件夹并指定名字
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="saveDir">The save dir.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="compute">if set to <c>true</c> [compute].</param>
        /// <returns>System.String.</returns>
        public virtual string Upload(HttpPostedFileBase file, string saveDir, string filename, bool compute = true)
        {
            //var path = this.setting.GeneratPathFromRoot(saveDir);
            //var save = path + "\\"+filename;
            //file.SaveAs(save);
            //return save;
            return Upload(file.InputStream, saveDir, filename,compute);
        }

        /// <summary>
        /// Uploads the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="saveDir">The save dir.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="compute">if set to <c>true</c> [compute].</param>
        /// <returns>System.String.</returns>
        public string Upload(Stream stream, string saveDir, string filename,bool compute=true)
        {
            var path = saveDir;
            if (compute)
            {
                path = this.setting.GeneratPathFromRoot(saveDir);
            }
            var save = path + "\\" + filename;
            
            var buffer = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(buffer, 0, buffer.Length);
            var fs = new FileStream(save, FileMode.OpenOrCreate, FileAccess.Write);
            try
            {
                fs.Write(buffer, 0, buffer.Length);
                fs.Flush();
            }
            finally
            {
                fs.Close();
            }

            return save;
        }

        /// <summary>
        /// Uploads the specified img.
        /// </summary>
        /// <param name="img">The img.</param>
        /// <param name="saveDir">The save dir.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="formate">The formate.</param>
        /// <param name="compute">if set to <c>true</c> [compute].</param>
        /// <returns>System.String.</returns>
        public string Upload(Image img, string saveDir, string filename, ImageFormat formate, bool compute = true)
        {
            var stream = new MemoryStream();
            img.Save(stream, formate);
            return Upload(stream, saveDir, filename,compute);
        }

        /// <summary>
        /// 生成随机文件名
        /// </summary>
        /// <param name="ext">扩展名</param>
        /// <returns>System.String.</returns>
        /// <remarks>当前日期年月日时分秒毫秒4位+3随机数</remarks>
        public virtual string GetRandomName(string ext)
        {
            var basename = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            var rnd = new Random();
            for (var i = 0; i < 3; i++)
            {
                basename += rnd.Next(9);
            }
            return basename + ext;
        }
    }
}