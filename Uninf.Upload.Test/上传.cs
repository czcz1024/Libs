using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Uninf.Upload.Test
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    [TestClass]
    public class 上传
    {
        static string basepath = AppDomain.CurrentDomain.BaseDirectory.Substring(0,AppDomain.CurrentDomain.BaseDirectory.IndexOf(@"\bin")) + @"\imgs\";

        static string resultPath = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf(@"\bin")) + @"\uploads\test\";

        [TestMethod]
        public void 图片上传()
        {
            var path = resultPath + @"up1";
            DeleteDir(path);

            var img1path = basepath + "pic300-300.jpg";
            var img = Image.FromFile(img1path);
            var uploader = new UploaderBase(new TestUploadSetting());
            var result=uploader.Upload(img, "up1", "test.jpg", ImageFormat.Jpeg);
            var shouldBe = resultPath + @"up1\0\0\test.jpg";
            Assert.AreEqual(shouldBe, result);
            Assert.IsTrue(File.Exists(shouldBe));
        }

        [TestMethod]
        public void 超出文件夹限制()
        {
            var path = resultPath + @"\up2";
            DeleteDir(path);

            var img1path = basepath + "pic300-300.jpg";
            var img = Image.FromFile(img1path);
            var uploader = new UploaderBase(new TestUploadSetting());
            var result1=uploader.Upload(img, "up2", "test1.jpg", ImageFormat.Jpeg);
            var result2=uploader.Upload(img, "up2", "test2.jpg", ImageFormat.Jpeg);
            var result3=uploader.Upload(img, "up2", "test3.jpg", ImageFormat.Jpeg);
            var result4=uploader.Upload(img, "up2", "test4.jpg", ImageFormat.Jpeg);
            var shouldBe = resultPath + @"up2\0\1\test4.jpg";
            Assert.AreEqual(shouldBe, result4);
            Assert.IsTrue(File.Exists(shouldBe));
        }

        private void DeleteDir(string dir)
        {
            if (Directory.Exists(dir))
            {
                var di = new DirectoryInfo(dir);
                di.Delete(true);
            }
        }
    }

    public class TestUploadSetting:DefaultUploadSettingBase
    {
        public override int GetMaxFolderCnt()
        {
            return 3;
        }

        public override int GetMaxFileCnt()
        {
            return 3;
        }

        public override string GetImgWebHost()
        {
            return "http://img.thz.com";
        }

        public override string GetUploadRootPath()
        {
            return "test";
        }

        public override string GetUploadIp()
        {
            return AppDomain.CurrentDomain.BaseDirectory + "\\..\\..\\uploads\\";
        }
    }
}
