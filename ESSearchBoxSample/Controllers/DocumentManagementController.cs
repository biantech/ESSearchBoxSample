using ESSearchBoxSample.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESSearchBoxSample.Models;
using System.Data.Entity;
using Elasticsearch.Net;

namespace ESSearchBoxSample.Controllers
{
    public class DocumentManagementController : Controller
    {
        private static string indexName = "sample";
        private SampleEntities db = new SampleEntities();

        // GET: DocumentManagement
        public ActionResult Index()
        {
            var documents = db.DocumentModels.ToList();
            return View(documents);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(DocumentModel document)
        {
            if (ModelState.IsValid)
            {
                db.DocumentModels.Add(document);
                db.SaveChanges();
                Index(document, "create");
                return RedirectToAction("Index");
            }
            return View(document);
        }

        // GET: /DocumentManager/Edit/5
        public ActionResult Edit(int id)
        {
            DocumentModel documentModel = db.DocumentModels.Find(id);
            return View(documentModel);
        }

        [HttpPost]
        public ActionResult Edit(DocumentModel document)
        {
            if (ModelState.IsValid)
            {
                db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();
                Index(document, "update");
                return RedirectToAction("Index");
            }
            return View(document);
        }

        public ActionResult Delete(int id)
        {
            DocumentModel document = db.DocumentModels.Find(id);
            db.DocumentModels.Remove(document);
            db.SaveChanges();
            Index(document, "delete");
            return RedirectToAction("Index");
        }

        public ActionResult ReIndexAll()
        {
            var documents = db.DocumentModels.ToList();

            var uriString = ConfigurationManager.AppSettings["SEARCHBOX_URL"];
            var searchBoxUri = new Uri(uriString);

            var settings = new ConnectionSettings(searchBoxUri);
            settings.SetDefaultIndex(indexName);

            var client = new ElasticClient(settings);

            // delete index if exists at startup
            if (client.IndexExists(indexName).Exists)
            {
                client.DeleteIndex(indexName);
            }

            // Create a new "sample" index with default settings
            //client.CreateIndex("sample", new IndexSettings());
            ICreateIndexRequest iCreateIndexReq = new CreateIndexRequest(indexName);
            iCreateIndexReq.IndexSettings = new IndexSettings();
            iCreateIndexReq.IndexSettings.NumberOfReplicas = 10;

            //client.CreateIndex(iCreateIndexReq);

            var resCreate = client.CreateIndex(indexName, s => s.AddMapping<DocumentModel>(f => f.MapFromAttributes()).NumberOfReplicas(1).NumberOfShards(10));

            //client.CreateIndex()
            // Index all documents
            client.IndexMany<DocumentModel>(documents);
            //client.Index()

            ViewBag.Message = "Reindexing all database is complete!";

            return RedirectToAction("Index");
        }



        private static void Index(DocumentModel document, String operation)
        {
            var uriString = ConfigurationManager.AppSettings["SEARCHBOX_URL"];
            var searchBoxUri = new Uri(uriString);

            var settings = new ConnectionSettings(searchBoxUri);
            settings.SetDefaultIndex(indexName);

            var client = new ElasticClient(settings);

            if (!client.IndexExists(indexName).Exists)
            {
                // Create a new "sample" index with default settings
                ICreateIndexRequest iCreateIndexReq = new CreateIndexRequest(indexName);
                iCreateIndexReq.IndexSettings = new IndexSettings();
                iCreateIndexReq.IndexSettings.NumberOfReplicas = 10;

                //iCreateIndexReq.IndexSettings.Mappings = new List<RootObjectMapping>();
                //RootObjectMapping rootObjectMapping = new RootObjectMapping();
                //rootObjectMapping.AllFieldMapping()
                //iCreateIndexReq.IndexSettings.Mappings.
                //client.CreateIndex(iCreateIndexReq);
                //client.CreateIndex(indexName,s=>s.)
                var resCreate = client.CreateIndex(indexName, s => s.AddMapping<DocumentModel>(f => f.MapFromAttributes()).NumberOfReplicas(1).NumberOfShards(10));

                //client.CreateIndex(indexName, new IndexSettings());
                //client.create
            }

            if (operation.Equals("delete"))
            {
                //client.DeleteById(indexName, "documents", document.DocumentId);
                IDeleteByQueryRequest iDeleteByQueryRequest = new DeleteByQueryRequest();
                //IDeleteIndexRequest delReq = new DeleteIndexRequest(indexName);
                //client.DeleteIndex()
                //client.DeleteByQuery(new DeleteByQueryRequest())
                client.Delete<DocumentModel>(f => f.Id(document.DocumentId).Index(indexName).Refresh());
                //var response = this.Client.Delete<ElasticsearchProject>(f=>f.Id(newDocument.Id).Index(newIndex).Refresh());
            }
            else
            {
               //IIndexRequest<DocumentModel> indexRequest = IndexRequest<DocumentModel>();
               //client.Index(i)
               //client.Index(document, indexName, "documents", document.DocumentId);
               //IndexDescriptor indexDesc = IndexDescriptor;
               //IndexRequestParameters indexParameter = new IndexRequestParameters();
               // indexParameter.Replication(1);
                client.Index(document, i => i.Id(document.DocumentId).Index(indexName));
                //client.Index();
                //client.Index()
            }
        }
    }
}