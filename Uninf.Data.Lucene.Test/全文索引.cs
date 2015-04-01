using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Uninf.Data.Lucene.Test
{
    using System.Collections.Generic;
    using System.IO;

    using global::Lucene.Net.Analysis;
    using global::Lucene.Net.Analysis.PanGu;
    using global::Lucene.Net.Analysis.Standard;
    using global::Lucene.Net.Documents;
    using global::Lucene.Net.Index;
    using global::Lucene.Net.QueryParsers;
    using global::Lucene.Net.Search;

    [TestClass]
    public class 全文索引
    {
        [TestMethod]
        public void 重建索引测试()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf(@"\bin")) + @"\indexer\rebuildTest";

            //DeleteDir(path);

            var s = new TestSearch("rebuildTest");
            s.Rebuild();

            var exsit = Directory.Exists(path);
            Assert.IsTrue(exsit);

            long all;
            var r = s.Search("a", 0, 10, out all);
            Assert.AreEqual(1, all);
        }

        [TestMethod]
        public void 搜索测试()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf(@"\bin")) + @"\indexer\search";

            DeleteDir(path);

            var s = new TestSearch("search");
            s.Rebuild();

            long all;
            var result = s.Search("a", 0, 100, out all);
            Assert.AreEqual(1, all);
            
        }

        [TestMethod]
        public void 添加测试()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf(@"\bin")) + @"\indexer\add";

            DeleteDir(path);

            var s = new TestSearch("add");
            s.Rebuild();

            long all;
            var result = s.Search("a", 0, 100, out all);
            Assert.AreEqual(1, all);

            s.Add(new TestContent {Id=6,Title="a",Body="abc" });

            var result1 = s.Search("a", 0, 100, out all);
            Assert.AreEqual(2, all);
        }

        [TestMethod]
        public void 删除测试()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf(@"\bin")) + @"\indexer\del";

            DeleteDir(path);

            var s = new TestSearch("del");
            s.Rebuild();

            long all;
            var result = s.Search("a", 0, 100, out all);
            Assert.AreEqual(1, all);

            s.Delete(new TestContent { Id = 1, Title = "a", Body = "zzzzzz" });

            var result1 = s.Search("a", 0, 100, out all);
            Assert.AreEqual(0, all);
        }

        [TestMethod]
        public void 更新测试()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf(@"\bin")) + @"\indexer\update";

            DeleteDir(path);

            var s = new TestSearch("update");
            s.Rebuild();

            long all;
            var result = s.Search("a", 0, 100, out all);
            Assert.AreEqual(1, all);

            s.Update(new TestContent { Id = 1, Title = "t", Body = "tttttt" });

            var result1 = s.Search("a", 0, 100, out all);
            Assert.AreEqual(0, all);

            var result2 = s.Search("t", 0, 100, out all);
            Assert.AreEqual(1, all);
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

    public class TestContent
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }
    }

    public class TestSearch:LuceneSearchBase<TestContent,string>
    {
        private string path;

        public TestSearch(string path)
        {
            this.path = path;
        }

        protected override string IndexPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf(@"\bin")) + @"\indexer\"+path+@"\";
        }

        protected override IEnumerable<TestContent> GetData()
        {
            return new List<TestContent> { 
                new TestContent{Id=1,Title="a",Body="zzzzzz"},
                new TestContent{Id=2,Title="b",Body="yyyyyy"},
                new TestContent{Id=3,Title="c",Body="xxxxxx"},
                new TestContent{Id=4,Title="d",Body="wwwwww"},
                new TestContent{Id=5,Title="e",Body="uuuuuu"},
            };
        }

        protected override Analyzer GetAnalyzer()
        {
            //return new StandardAnalyzer(global::Lucene.Net.Util.Version.LUCENE_CURRENT);
            //return new SimpleAnalyzer();
            return new PanGuAnalyzer();
        }

        protected override Document ConvertToLuceneDocument(TestContent item)
        {
            var doc = new Document();
            doc.Add(new Field("id", item.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("title", item.Title, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("body", item.Body, Field.Store.YES, Field.Index.ANALYZED));
            return doc;
        }

        protected override string IdKey()
        {
            return "id";
        }

        protected override string IdValue(TestContent item)
        {
            return item.Id.ToString();
        }

        protected override int SearchCount()
        {
            return 100;
        }

        protected override Query GetQuery(string search)
        {
            var q = new PhraseQuery();
            q.Add(new Term("title", search));
            //q.Add(new Term("body", search));
            return q;

            //var q = new BooleanQuery();
            //var parse = new QueryParser(global::Lucene.Net.Util.Version.LUCENE_CURRENT, "title", GetAnalyzer());
            //parse.DefaultOperator = QueryParser.Operator.OR;
            //q.Add(parse.Parse(search), Occur.MUST);

            //var parse1 = new QueryParser(global::Lucene.Net.Util.Version.LUCENE_CURRENT, "body", GetAnalyzer());
            //parse1.DefaultOperator = QueryParser.Operator.OR;
            //q.Add(parse1.Parse(search), Occur.MUST);
            
            //return q;
        }

        protected override TestContent ConvertToObj(Document doc)
        {
            var obj = new TestContent();
            obj.Id = int.Parse(doc.Get("id"));
            obj.Title = doc.Get("title");
            obj.Body = doc.Get("body");
            return obj;
        }
    }
}
