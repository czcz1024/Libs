using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Uninf.Upload.Test
{
    using Moq;

    [TestClass]
    public class 地址转换
    {
        [TestMethod]
        public void 网络访问地址()
        {
            var src1 = "files://192.168.1.112/upload/thz/microblog/0/0/a.jpg";
            var src2 = "/THZ/MicroBlog/0/0/a.jpg";
            var src3 = @"e:\upload\thz\microblog\0\0\a.jpg";
            var src4 = "files://uninf_web/thz/microblog/0/0/a.jpg";

            var shouldBe = "http://img.thz.com/thz/microblog/0/0/a.jpg";
            var obj = new SettingTest();

            var r1 = obj.GetWebShowPath(src1).ToLower();
            Assert.AreEqual(shouldBe, r1);

            var r2 = obj.GetWebShowPath(src2).ToLower();
            Assert.AreEqual(shouldBe, r2);

            var r3 = obj.GetWebShowPath(src3).ToLower();
            Assert.AreEqual(shouldBe, r3);

            var r4 = obj.GetWebShowPath(src4).ToLower();
            Assert.AreEqual(shouldBe, r4);
        }

        [TestMethod]
        public void 网址转硬盘()
        {
            var web1 = "http://img.thz.com/thz/microblog/0/0/a.jpg";
            var obj = new SettingTest();
            var r1 = obj.WebPathToFilePath(web1);
            //var should1 = @"e:\upload\thz\microblog\0\0\a.jpg";
            var should1 = @"\\192.168.1.112\upload\thz\microblog\0\0\a.jpg";
            Assert.AreEqual(should1, r1);
        }
    }

    public class SettingTest:DefaultUploadSettingBase
    {
        public override int GetMaxFolderCnt()
        {
            throw new NotImplementedException();
        }

        public override int GetMaxFileCnt()
        {
            throw new NotImplementedException();
        }

        public override string GetImgWebHost()
        {
            return "http://img.thz.com";
        }

        public override string GetUploadRootPath()
        {
            return "thz";
        }

        public override string GetUploadIp()
        {
            //return @"e:\upload\";
            return @"\\192.168.1.112\upload";
        }
    }
}
