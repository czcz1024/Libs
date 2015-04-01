using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Uninf.Data.Lucene.Test
{
    using System.Collections.Generic;

    using global::Lucene.Net.Analysis;
    using global::Lucene.Net.Analysis.Standard;
    using global::Lucene.Net.Index;
    using global::Lucene.Net.QueryParsers;
    using global::Lucene.Net.Search;

    [TestClass]
    public class 模糊搜索
    {
        [TestMethod]
        public void 模糊搜索测试()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf(@"\bin")) + @"\indexer\like";

            var s = new LikeSearch("like");
            s.Rebuild();

            long all;

            var r = s.Search("a", 0, 10, out all);
            Assert.AreEqual(3, all);
        }

        [TestMethod]
        public void 多字段查询()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf(@"\bin")) + @"\indexer\col";

            var s = new ColSearch("col");
            s.Rebuild();

            long all;

            var r = s.Search("去", 0, 10, out all);
            Assert.AreEqual(3, all);

            var r1 = s.Search("我们", 0, 10, out all);
            Assert.AreEqual(1, all);

            var r2 = s.Search("冬天", 0, 10, out all);
            Assert.AreEqual(3, all);
        }
    }

    public class LikeSearch:TestSearch
    {
        public LikeSearch(string path)
            : base(path)
        {
        }

        protected override System.Collections.Generic.IEnumerable<TestContent> GetData()
        {
            return new List<TestContent> { 
                new TestContent{Id=1,Title="a",Body="a"},
                new TestContent{Id=2,Title="a b",Body="a"},
                new TestContent{Id=3,Title="a c",Body="a"},
                new TestContent{Id=4,Title="d",Body="a x"},
                new TestContent{Id=5,Title="e",Body="a y"},
            };
        }

        protected override global::Lucene.Net.Search.Query GetQuery(string search)
        {
            var query = new PhraseQuery();
            query.Add(new Term("title", search));
            return query;
        }
    }

    public class ColSearch : TestSearch
    {
        public ColSearch(string path)
            : base(path)
        {
        }

        protected override System.Collections.Generic.IEnumerable<TestContent> GetData()
        {
            return new List<TestContent> { 
                new TestContent{Id=1,Title="我们",Body="爸爸去哪儿"},
                new TestContent{Id=2,Title="去哪儿网",Body="旅游"},
                new TestContent{Id=3,Title="冬天",Body="冬天旅游圣地"},
                new TestContent{Id=4,Title="不出去",Body="冬天在家里"},
                new TestContent{Id=5,Title="where to go",Body="in winter,冬天"},
            };
        }

        protected override global::Lucene.Net.Analysis.Analyzer GetAnalyzer()
        {
            //return base.GetAnalyzer();
            //return new KeywordAnalyzer();
            //return new SimpleAnalyzer();
            return new StandardAnalyzer(global::Lucene.Net.Util.Version.LUCENE_CURRENT);

        }

        protected override global::Lucene.Net.Search.Query GetQuery(string search)
        {
            var q = new BooleanQuery();

            var p1 =
                new QueryParser(global::Lucene.Net.Util.Version.LUCENE_CURRENT, "title", GetAnalyzer());
            p1.DefaultOperator = QueryParser.Operator.OR;
            var q1 = p1.Parse(search);
            q.Add(q1, Occur.SHOULD);

            var p2 =
                new QueryParser(global::Lucene.Net.Util.Version.LUCENE_CURRENT, "body", GetAnalyzer());
            p2.DefaultOperator = QueryParser.Operator.OR;
            var q2 = p2.Parse(search);
            q.Add(q2, Occur.SHOULD);

            return q;
        }
    }
}
