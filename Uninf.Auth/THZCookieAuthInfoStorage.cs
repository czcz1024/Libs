// ***********************************************************************
// Assembly         : Uninf.Auth
// Author           : cz
// Created          : 01-08-2015
//
// Last Modified By : cz
// Last Modified On : 01-08-2015
// ***********************************************************************
// <copyright file="THZCookieAuthInfoStorage.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Auth
{
    using System;
    using System.Web;

    /// <summary>
    /// 基于Cookie的身份保存
    /// </summary>
    public class THZCookieAuthInfoStorage : IAuthInfoStorage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="THZCookieAuthInfoStorage"/> class.
        /// </summary>
        public THZCookieAuthInfoStorage()
        {
            this.path = "/";
            this.domain = "";
            this.expire = null;
            this.httpOnly = false;
            this.secure = false;
        }

        //public THZCookieAuthInfoStorage(string path, string domain, DateTime? expire, bool httpOnly, bool secure)
        //{
        //    this.path = path;
        //    this.domain = domain;
        //    this.expire = expire;
        //    this.httpOnly = httpOnly;
        //    this.secure = secure;
        //}

        /// <summary>
        /// The path
        /// </summary>
        protected string path;
        /// <summary>
        /// The domain
        /// </summary>
        protected string domain;
        /// <summary>
        /// The expire
        /// </summary>
        protected DateTime? expire;
        /// <summary>
        /// The HTTP only
        /// </summary>
        protected bool httpOnly;
        /// <summary>
        /// The secure
        /// </summary>
        protected bool secure;

        /// <summary>
        /// 保存到客户端
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public virtual void Save(string name, string value)
        {
            var cookie = new HttpCookie(name, value);
            cookie.Path = this.path;
            cookie.Domain = this.domain;
            cookie.HttpOnly = this.httpOnly;
            cookie.Secure = this.secure;
            if (this.expire.HasValue)
            {
                cookie.Expires = this.expire.Value;
            }
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 从客户端读取
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>System.String.</returns>
        public virtual string Load(string name)
        {
            var cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value)) return string.Empty;
            return cookie.Value;
        }

        /// <summary>
        /// 清除客户端保存的数据
        /// </summary>
        /// <param name="name">The name.</param>
        public virtual void Clear(string name)
        {
            var cookie = new HttpCookie(name, string.Empty);
            cookie.Value = string.Empty;
            cookie.Path = this.path;
            cookie.Domain = this.domain;
            cookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Add(cookie);
            //HttpContext.Current.Response.Cookies.Remove(name);
        }
    }
}