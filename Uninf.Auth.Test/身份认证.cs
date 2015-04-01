using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Uninf.Auth.Test
{
    using System.Collections.Generic;

    using Autofac;
    using Autofac.Extras.CommonServiceLocator;

    using Moq;

    using Uninf.Auth.CommonServiceLocator;

    [TestClass]
    public class 身份认证
    {
        [TestInitialize]
        public void Init()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<LoginInfoSerializor>().AsImplementedInterfaces();
            builder.RegisterType<THZDesEncryptor>().AsImplementedInterfaces();
            builder.RegisterType<MemoryStorage>().AsImplementedInterfaces();
            builder.RegisterType<TestAuthOperator>().AsImplementedInterfaces();

            var container = builder.Build();
            Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));

            AuthContainer.SetAuthContainer(new CommonServiceLocatorAuthContainer());

            MemoryStorage.dic = new Dictionary<string, string>();
        }

        [TestMethod]
        public void 登陆测试()
        {
            var u0 = AuthManagers<LoginInfo>.GetNowUser();
            var islog0 = AuthManagers<LoginInfo>.IsLog();
            Assert.IsFalse(islog0);
            Assert.IsNull(u0);

            AuthManagers<LoginInfo>.Login(new LoginInfo {Id=1,Name="a" });

            var u = AuthManagers<LoginInfo>.GetNowUser();
            var islog = AuthManagers<LoginInfo>.IsLog();
            Assert.IsTrue(islog);
            Assert.AreEqual(1, u.Id);
            Assert.AreEqual("a", u.Name);
        }

        [TestMethod]
        public void 退出测试()
        {
            MemoryStorage.dic["Test"] =new THZDesEncryptor().Encrypt("1,a");

            var u0 = AuthManagers<LoginInfo>.GetNowUser();
            var islog0 = AuthManagers<LoginInfo>.IsLog();
            Assert.IsTrue(islog0);
            Assert.AreEqual(1, u0.Id);
            Assert.AreEqual("a", u0.Name);

            AuthManagers<LoginInfo>.Logout();

            var islog = AuthManagers<LoginInfo>.IsLog();
            var u = AuthManagers<LoginInfo>.GetNowUser();

            Assert.IsFalse(islog);
            Assert.IsNull(u);
        }
    }

    public class LoginInfo:IAuthInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string AuthRole()
        {
            return "User";
        }
    }
    public class LoginInfoSerializor:IAuthInfoSerializer<LoginInfo>
    {
        public string Serialize(LoginInfo obj)
        {
            return obj.Id + "," + obj.Name;
        }

        public LoginInfo Deserialize(string src)
        {
            var arr = src.Split(',');
            return new LoginInfo { 
                Id=int.Parse(arr[0]),
                Name=arr[1]
            };
        }
    }

    public class MemoryStorage:IAuthInfoStorage
    {
        public static Dictionary<string, string> dic = new Dictionary<string, string>();
        public void Save(string name, string value)
        {
            dic[name] = value;
        }

        public string Load(string name)
        {
            try
            {
                return dic[name];
            }
            catch
            {
                return string.Empty;
            }
        }

        public void Clear(string name)
        {
            dic.Remove(name);
        }
    }

    public class TestAuthOperator : AuthOperator<LoginInfo>
    {
        public TestAuthOperator(IAuthInfoEncryptor encryptor, IAuthInfoSerializer<LoginInfo> serializer, IAuthInfoStorage storage)
            : base(encryptor, serializer, storage)
        {
        }

        public TestAuthOperator(IAuthInfoSerializer<LoginInfo> serializer)
            : base(serializer)
        {
        }

        public override string StorageName()
        {
            return "Test";
        }
    }
}
