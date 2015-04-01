// ***********************************************************************
// Assembly         : Uninf.Auth
// Author           : cz
// Created          : 01-08-2015
//
// Last Modified By : cz
// Last Modified On : 01-08-2015
// ***********************************************************************
// <copyright file="AuthOperator.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Auth
{
    /// <summary>
    /// 身份操作基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AuthOperator<T>:IAuthOperator<T> where T:IAuthInfo
    {
        /// <summary>
        /// The encryptor
        /// </summary>
        private IAuthInfoEncryptor encryptor;

        /// <summary>
        /// The serializer
        /// </summary>
        private IAuthInfoSerializer<T> serializer;

        /// <summary>
        /// The storage
        /// </summary>
        private IAuthInfoStorage storage;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthOperator{T}"/> class.
        /// </summary>
        /// <param name="encryptor">The encryptor.</param>
        /// <param name="serializer">The serializer.</param>
        /// <param name="storage">The storage.</param>
        public AuthOperator(IAuthInfoEncryptor encryptor, IAuthInfoSerializer<T> serializer, IAuthInfoStorage storage)
        {
            this.encryptor = encryptor;
            this.serializer = serializer;
            this.storage = storage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthOperator{T}"/> class.
        /// </summary>
        /// <param name="serializer">The serializer.</param>
        public AuthOperator(IAuthInfoSerializer<T> serializer)
            : this(new THZDesEncryptor(), serializer, new THZCookieAuthInfoStorage())
        {
        }

        /// <summary>
        /// Encryptors this instance.
        /// </summary>
        /// <returns>IAuthInfoEncryptor.</returns>
        public IAuthInfoEncryptor Encryptor()
        {
            return this.encryptor;
        }

        /// <summary>
        /// Serializers this instance.
        /// </summary>
        /// <returns>IAuthInfoSerializer&lt;T&gt;.</returns>
        public IAuthInfoSerializer<T> Serializer()
        {
            return this.serializer;
        }

        /// <summary>
        /// Storagers this instance.
        /// </summary>
        /// <returns>IAuthInfoStorage.</returns>
        public IAuthInfoStorage Storager()
        {
            return this.storage;
        }

        /// <summary>
        /// Storages the name.
        /// </summary>
        /// <returns>System.String.</returns>
        public abstract string StorageName();
    }
}