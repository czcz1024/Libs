// ***********************************************************************
// Assembly         : Uninf.Data.Lucene
// Author           : cz
// Created          : 01-21-2015
//
// Last Modified By : cz
// Last Modified On : 01-22-2015
// ***********************************************************************
// <copyright file="LuceneSearchBase.cs" company="Uninf">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Uninf.Data.Lucene
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using global::Lucene.Net.Analysis;
    using global::Lucene.Net.Analysis.PanGu;
    using global::Lucene.Net.Documents;
    using global::Lucene.Net.Index;
    using global::Lucene.Net.Search;
    using global::Lucene.Net.Store;

    /// <summary>
    /// LuceneSearchBase. 类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TSearch">The type of the t search.</typeparam>
    public abstract class LuceneSearchBase<T,TSearch> : ILuceneSearch<T,TSearch>
    {
        /// <summary>
        /// 索引保存路径
        /// </summary>
        /// <returns>System.String.</returns>
        protected abstract string IndexPath();

        /// <summary>
        /// 重建时读取原始数据
        /// </summary>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        protected abstract IEnumerable<T> GetData();

        /// <summary>
        /// 使用的analyzer
        /// </summary>
        /// <returns>Analyzer.</returns>
        protected abstract Analyzer GetAnalyzer();

        /// <summary>
        /// 将元素转换成lucenen的document
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>Document.</returns>
        protected abstract Document ConvertToLuceneDocument(T item);

        /// <summary>
        /// 将lucene document转换成元素
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <returns>T.</returns>
        protected abstract T ConvertToObj(Document doc);

        /// <summary>
        /// id对应的名称
        /// </summary>
        /// <returns>System.String.</returns>
        protected abstract string IdKey();

        /// <summary>
        /// id对应的值
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.String.</returns>
        protected abstract string IdValue(T item);

        /// <summary>
        /// Rebuilds the specified force.
        /// </summary>
        /// <param name="force">if set to <c>true</c> [force].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Rebuild(bool force = false)
        {
            string indexPath = IndexPath();
            using (var directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory()))
            {
                bool isExist = IndexReader.IndexExists(directory);
                if (isExist)
                {
                    if (IndexWriter.IsLocked(directory))
                    {
                        if (force)
                        {
                            IndexWriter.Unlock(directory);
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                using (
                    var writer = new IndexWriter(
                        directory,
                        GetAnalyzer(),
                        !isExist,
                        IndexWriter.MaxFieldLength.UNLIMITED))
                {

                    var list = GetData();
                    writer.DeleteAll();
                    foreach (var item in list)
                    {
                        var document = ConvertToLuceneDocument(item);
                        writer.AddDocument(document);
                    }
                    return true;
                }
            }

        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="force">if set to <c>true</c> [force].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Add(T item, bool force = false)
        {
            string indexPath = IndexPath();
            using (var directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory()))
            {
                bool isExist = IndexReader.IndexExists(directory);
                if (isExist)
                {
                    if (IndexWriter.IsLocked(directory))
                    {
                        if (force)
                        {
                            IndexWriter.Unlock(directory);
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                using (
                    var writer = new IndexWriter(
                        directory,
                        GetAnalyzer(),
                        !isExist,
                        IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    var document = ConvertToLuceneDocument(item);
                    writer.AddDocument(document);
                    return true;
                }
            }
        }

        /// <summary>
        /// Updates the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="force">if set to <c>true</c> [force].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Update(T item, bool force = false)
        {
            string indexPath = IndexPath();
            using (var directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory()))
            {
                bool isExist = IndexReader.IndexExists(directory);
                if (isExist)
                {
                    if (IndexWriter.IsLocked(directory))
                    {
                        if (force)
                        {
                            IndexWriter.Unlock(directory);
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                using (
                    var writer = new IndexWriter(
                        directory,
                        GetAnalyzer(),
                        false,
                        IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    var document = ConvertToLuceneDocument(item);
                    writer.UpdateDocument(new Term(IdKey(), IdValue(item)), document);
                    writer.Optimize();
                    return true;
                }
            }
        }

        /// <summary>
        /// Deletes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="force">if set to <c>true</c> [force].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Delete(T item, bool force = false)
        {
            string indexPath = IndexPath();
            using (var directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory()))
            {
                bool isExist = IndexReader.IndexExists(directory);
                if (isExist)
                {
                    if (IndexWriter.IsLocked(directory))
                    {
                        if (force)
                        {
                            IndexWriter.Unlock(directory);
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                using (
                    var writer = new IndexWriter(
                        directory,
                        GetAnalyzer(),false,
                        IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    var document = ConvertToLuceneDocument(item);
                    writer.DeleteDocuments(new Term(IdKey(), IdValue(item)));
                    writer.Optimize();
                    return true;
                }
            }
        }

        /// <summary>
        /// Searches the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <param name="all">All.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public IEnumerable<T> Search(TSearch query, int skip, int take, out long all)
        {
            string indexPath = IndexPath();
            using (var directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory()))
            {
                bool isExist = IndexReader.IndexExists(directory);
                if (isExist)
                {
                    using (var reader = IndexReader.Open(directory, true))
                    {
                        using (var searcher = new IndexSearcher(reader))
                        {

                            var collector = TopScoreDocCollector.Create(SearchCount(), true);
                            var q = GetQuery(query);
                            searcher.Search(q, GetFilter(), collector);
                            all = collector.TotalHits;
                            var page = collector.TopDocs(skip, take).ScoreDocs;
                            var r = new List<T>();
                            foreach (var item in page)
                            {
                                var doc = searcher.Doc(item.Doc);
                                var obj = ConvertToObj(doc);
                                r.Add(obj);
                                //yield return obj;
                            }
                            //var r=
                            //    collector.TopDocs()
                            //        .ScoreDocs.Select(item => searcher.Doc(item.Doc))
                            //        .Select(this.ConvertToObj)
                            //        .ToList();
                            return r;
                        }
                    }
                }
                all = 0;
                return new List<T>();
            }
        }

        /// <summary>
        /// 搜索结果最大数量
        /// </summary>
        /// <returns>System.Int32.</returns>
        protected abstract int SearchCount();

        /// <summary>
        /// 搜索方式
        /// </summary>
        /// <param name="search">The search.</param>
        /// <returns>Query.</returns>
        protected abstract Query GetQuery(TSearch search);

        /// <summary>
        /// 过滤方式
        /// </summary>
        /// <returns>Filter.</returns>
        protected virtual Filter GetFilter()
        {
            return null;
        }
    }
}