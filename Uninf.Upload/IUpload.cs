// ***********************************************************************
// Assembly         : Uninf.Upload
// Author           : cz
// Created          : 01-08-2015
//
// Last Modified By : cz
// Last Modified On : 01-19-2015
// ***********************************************************************
// <copyright file="IUpload.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Upload
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Web;

    /// <summary>
    /// 统一上传接口
    /// </summary>
    public interface IUpload
    {

        /// <summary>
        /// 上传文件到指定文件夹
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="saveDir">The save dir.</param>
        /// <param name="compute">if set to <c>true</c> [compute].</param>
        /// <returns>System.String.</returns>
        string Upload(HttpPostedFileBase file, string saveDir,bool compute=true);

        /// <summary>
        /// 上传到指定文件夹并指定名字
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="saveDir">The save dir.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="compute">if set to <c>true</c> [compute].</param>
        /// <returns>System.String.</returns>
        string Upload(HttpPostedFileBase file, string saveDir, string filename, bool compute = true);

        /// <summary>
        /// Uploads the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="saveDir">The save dir.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="compute">if set to <c>true</c> [compute].</param>
        /// <returns>System.String.</returns>
        string Upload(Stream stream, string saveDir, string filename, bool compute = true);
        /// <summary>
        /// Uploads the specified img.
        /// </summary>
        /// <param name="img">The img.</param>
        /// <param name="saveDir">The save dir.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="formate">The formate.</param>
        /// <param name="compute">if set to <c>true</c> [compute].</param>
        /// <returns>System.String.</returns>
        string Upload(Image img, string saveDir, string filename, ImageFormat formate, bool compute = true);
        
    }
}