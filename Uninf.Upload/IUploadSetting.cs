// ***********************************************************************
// Assembly         : Uninf.Upload
// Author           : cz
// Created          : 01-19-2015
//
// Last Modified By : cz
// Last Modified On : 01-22-2015
// ***********************************************************************
// <copyright file="IUploadSetting.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Upload
{
    using System.IO;

    /// <summary>
    /// 上传设置接口
    /// </summary>
    /// <remarks><para>可为不同类型的上传提供不同的此接口的实现。</para>
    /// <para>通过此接口，可以设置：</para>
    /// <para>文件保存根路径</para>
    /// <para>此类型存储的文件夹</para>
    /// <para>一个文件夹下最多子文件夹个数，最多文件个数</para>
    /// <para>显示在网页上的基本地址</para>
    /// <para>保存在库里的数据格式</para></remarks>
    public interface IUploadSetting
    {
        /// <summary>
        /// 一个目录下最多存放多少个子目录
        /// </summary>
        /// <returns>System.Int32.</returns>
        int GetMaxFolderCnt();

        /// <summary>
        /// 一个目录下最多存放多少个文件
        /// </summary>
        /// <returns>System.Int32.</returns>
        int GetMaxFileCnt();
        /// <summary>
        /// 图片网络访问地址前缀
        /// </summary>
        /// <returns>System.String.</returns>
        string GetImgWebHost();
        /// <summary>
        /// 图片保存文件夹根目录
        /// </summary>
        /// <returns>System.String.</returns>
        string GetUploadRootPath();
        /// <summary>
        /// 上传服务器地址
        /// </summary>
        /// <returns>System.String.</returns>
        string GetUploadIp();

        /// <summary>
        /// 生成一个实际存储地址
        /// </summary>
        /// <param name="rootDir">The root dir.</param>
        /// <returns>System.String.</returns>
        string GeneratPathFromRoot(string rootDir);
        /// <summary>
        /// 通过一个文件路径，算出他存储在库里应该是
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <returns>System.String.</returns>
        string GetSaveToDBPath(string filepath);
        /// <summary>
        /// 通过一个文件路径，算出他展示在网页上的地址
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <returns>System.String.</returns>
        string GetWebShowPath(string filepath);

        /// <summary>
        /// 通过数据库内存储的，获取实际文件
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>FileInfo.</returns>
        FileInfo GetFileFromDBPath(string path);

        /// <summary>
        /// 网络路径到硬盘路径
        /// </summary>
        /// <param name="webpath">The webpath.</param>
        /// <returns>System.String.</returns>
        string WebPathToFilePath(string webpath);

    }
}