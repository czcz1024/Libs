using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Uninf.Upload.Test
{
    using System.Drawing;
    using System.IO;

    using Uninf.Images;

    [TestClass]
    public class 缩略
    {
        static string basepath = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf(@"\bin")) + @"\imgs\";

        static string resultPath = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf(@"\bin")) + @"\uploads\test\";
        private void DeleteDir(string dir)
        {
            if (Directory.Exists(dir))
            {
                var di = new DirectoryInfo(dir);
                di.Delete(true);
            }
        }

        [TestMethod]
        public void 等比缩小()
        {
            var dir = resultPath + @"sl1";
            DeleteDir(dir);

            var resizer = new ImageResize();
            var oldpath = basepath + "pic300-300.jpg";
            var savepath = @"sl1";
            var config = new ResizeConfig { 
                ResizeToWidth=200,
                ResizeToHeight=200,
                ResizeType= ResizeType.FixWidthAndHeight
            };
            var uploader = new UploaderBase(new TestUploadSetting());
            var result=resizer.Resize(oldpath, savepath,"a.jpg",uploader, config);
            var shouldBe = resultPath+savepath.Replace(".jpg", "_200x200.jpg")+@"\0\0\a.jpg";
            Assert.AreEqual(shouldBe, result);
            var realFile = result.Insert(result.LastIndexOf("."), "_200x200");
            Assert.IsTrue(File.Exists(realFile));
            var img = Image.FromFile(realFile);
            Assert.AreEqual(200, img.Width);
            Assert.AreEqual(200, img.Height);
        }

        [TestMethod]
        public void 等比放大()
        {
            var dir = resultPath + @"sl2";
            DeleteDir(dir);

            var resizer = new ImageResize();
            var oldpath = basepath + "pic300-300.jpg";
            var savepath = @"sl2";
            var config = new ResizeConfig
            {
                ResizeToWidth = 400,
                ResizeToHeight = 400,
                ResizeType = ResizeType.FixWidthAndHeight,
                ResizeToBigger=true
            };
            var uploader = new UploaderBase(new TestUploadSetting());
            var result = resizer.Resize(oldpath, savepath, "a.jpg", uploader, config);
            var shouldBe = resultPath + savepath.Replace(".jpg", "_400x400.jpg") + @"\0\0\a.jpg";
            Assert.AreEqual(shouldBe, result);
            var realFile = result.Insert(result.LastIndexOf("."), "_400x400");
            Assert.IsTrue(File.Exists(realFile));
            var img = Image.FromFile(realFile);
            Assert.AreEqual(400, img.Width);
            Assert.AreEqual(400, img.Height);
        }

        [TestMethod]
        public void 不放大()
        {
            var dir = resultPath + @"sl3";
            DeleteDir(dir);

            var resizer = new ImageResize();
            var oldpath = basepath + "pic300-300.jpg";
            var savepath = @"sl3";
            var config = new ResizeConfig
            {
                ResizeToWidth = 400,
                ResizeToHeight = 400,
                ResizeType = ResizeType.FixWidthAndHeight,
                ResizeToBigger = false
            };
            var uploader = new UploaderBase(new TestUploadSetting());
            var result = resizer.Resize(oldpath, savepath, "a.jpg", uploader, config);
            var shouldBe = resultPath + savepath.Replace(".jpg", "_400x400.jpg") + @"\0\0\a.jpg";
            Assert.AreEqual(shouldBe, result);
            var realFile = result.Insert(result.LastIndexOf("."), "_400x400");
            Assert.IsTrue(File.Exists(realFile));
            var img = Image.FromFile(realFile);
            Assert.AreEqual(300, img.Width);
            Assert.AreEqual(300, img.Height);
        }

        [TestMethod]
        public void 长图变正方()
        {
            
        }

        [TestMethod]
        public void 小长图变正方()
        {

        }

        [TestMethod]
        public void 宽图变正方()
        {
            
        }

        [TestMethod]
        public void 小宽图变正方()
        {

        }

        [TestMethod]
        public void 定宽()
        {
            
        }

        [TestMethod]
        public void 批量()
        {
            var dir = resultPath + @"sl4";
            DeleteDir(dir);

            var resizer = new ImageResize();
            var oldpath = basepath + "pic300-300.jpg";
            var savepath = @"sl4";
            var config1 = new ResizeConfig
            {
                ResizeToWidth = 100,
                ResizeToHeight = 100,
                ResizeType = ResizeType.FixWidthAndHeight,
                ResizeToBigger = false
            };
            var config2 = new ResizeConfig
            {
                ResizeToWidth = 200,
                ResizeToHeight = 200,
                ResizeType = ResizeType.FixWidthAndHeight,
                ResizeToBigger = false
            };
            var config3 = new ResizeConfig
            {
                ResizeToWidth = 120,
                ResizeToHeight = 120,
                ResizeType = ResizeType.FixWidthAndHeight,
                ResizeToBigger = false
            };
            var config4 = new ResizeConfig
            {
                ResizeToWidth = 150,
                ResizeToHeight = 150,
                ResizeType = ResizeType.FixWidthAndHeight,
                ResizeToBigger = false
            };
            var uploader = new UploaderBase(new TestUploadSetting());
            var result = resizer.Resize(oldpath, savepath, "a.jpg", uploader, config1,config2,config3,config4);

            var shouldBe = resultPath + savepath+ @"\0\0\a.jpg";
            Assert.AreEqual(shouldBe, result);
            var realFile1 = result.Insert(result.LastIndexOf("."), "_100x100");
            Assert.IsTrue(File.Exists(realFile1));
            var img = Image.FromFile(realFile1);
            Assert.AreEqual(100, img.Width);
            Assert.AreEqual(100, img.Height);

            var realFile2 = result.Insert(result.LastIndexOf("."), "_200x200");
            Assert.IsTrue(File.Exists(realFile2));
            var img2 = Image.FromFile(realFile2);
            Assert.AreEqual(200, img2.Width);
            Assert.AreEqual(200, img2.Height);
        }
    }
}
