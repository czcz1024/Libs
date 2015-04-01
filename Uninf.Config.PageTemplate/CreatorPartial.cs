// ***********************************************************************
// Assembly         : Uninf.Config.PageTemplate
// Author           : cz
// Created          : 01-08-2015
//
// Last Modified By : cz
// Last Modified On : 01-26-2015
// ***********************************************************************
// <copyright file="CreatorPartial.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Config.PageTemplate
{
    using System.IO;
    using System.Text;

    using Uninf.Config;

    /// <summary>
    /// ConfigCshtmlCreator. 类
    /// 运行tt模板生成mvc view页面
    /// </summary>
    public partial class ConfigCshtmlCreator
    {
        /// <summary>
        /// 运行模板
        /// </summary>
        /// <typeparam name="T">config类型，此处传入您的config类型</typeparam>
        /// <param name="path">页面保存地址</param>
        /// <param name="postAction">表单提交到的controller/action地址</param>
        public static void CreateCshtml<T>(string path, string postAction) where T : XmlFileConfigBase<T>
        {
            var creator = new ConfigCshtmlCreator();
            creator.Session = new Microsoft.VisualStudio.TextTemplating.TextTemplatingSession();
            creator.Session["configType"] = typeof(T);
            creator.Session["postAction"] = postAction;
            // Add other parameter values to t.Session here.
            creator.Initialize(); // Must call this to transfer values.
            var html = creator.TransformText();
            var finfo = new FileInfo(path);
            if (!finfo.Directory.Exists)
            {
                finfo.Directory.Create();
            }
            File.WriteAllText(path, html, Encoding.UTF8);
        }
    }
}