// ***********************************************************************
// Assembly         : Uninf.Auth
// Author           : cz
// Created          : 01-08-2015
//
// Last Modified By : cz
// Last Modified On : 01-08-2015
// ***********************************************************************
// <copyright file="AuthManagers.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Auth
{
    /// <summary>
    /// 用户身份管理入口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AuthManagers<T> where T : class,IAuthInfo
    {
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="obj">The object.</param>
        public static void Login(T obj)
        {
            var op = AuthContainer.Current.GetInstance<IAuthOperator<T>>();
            var ser = op.Serializer();
            var value = ser.Serialize(obj);
            var enc = op.Encryptor().Encrypt(value);
            op.Storager().Save(op.StorageName(), enc);
        }

        /// <summary>
        /// 退出
        /// </summary>
        public static void Logout()
        {
            var op = AuthContainer.Current.GetInstance<IAuthOperator<T>>();
            op.Storager().Clear(op.StorageName());
        }

        /// <summary>
        /// 是否登陆
        /// </summary>
        /// <returns><c>true</c> if this instance is log; otherwise, <c>false</c>.</returns>
        public static bool IsLog()
        {
            return GetNowUser()!=null;
        }

        /// <summary>
        /// 获取当前登陆信息
        /// </summary>
        /// <returns>T.</returns>
        public static T GetNowUser()
        {
            var op = AuthContainer.Current.GetInstance<IAuthOperator<T>>();
            var enc = op.Storager().Load(op.StorageName());
            if (string.IsNullOrEmpty(enc)) return null;
            var value = op.Encryptor().Decrypt(enc);
            return op.Serializer().Deserialize(value);
        }
    }
}