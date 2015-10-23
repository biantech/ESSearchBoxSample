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
        public ActionResult Create(ESSearchBoxSample.Models.DocumentModel document)
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
            ESSearchBoxSample.Models.DocumentModel documentModel = db.DocumentModels.Find(id);
            return View(documentModel);
        }

        [HttpPost]
        public ActionResult Edit(ESSearchBoxSample.Models.DocumentModel document)
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
            ESSearchBoxSample.Models.DocumentModel document = db.DocumentModels.Find(id);
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

            client.CreateIndex(iCreateIndexReq);

            //client.CreateIndex()
            // Index all documents
            client.IndexMany<ESSearchBoxSample.Models.DocumentModel>(documents);

            ViewBag.Message = "Reindexing all database is complete!";

            return RedirectToAction("Index");
        }

        private static void Index(ESSearchBoxSample.Models.DocumentModel document, String operation)
        {
            var uriString = ConfigurationManager.AppSettings["SEARCHBOX_URL"];
            var searchBoxUri = new Uri(uriString);

            var settings = new ConnectionSettings(searchBoxUri);
            settings.SetDefaultIndex("sample");

            var client = new ElasticClient(settings);

            if (!client.IndexExists(indexName).Exists)
            {
                // Create a new "sample" index with default settings
                ICreateIndexRequest iCreateIndexReq = new CreateIndexRequest(indexName);
                iCreateIndexReq.IndexSettings = new IndexSettings();
                iCreateIndexReq.IndexSettings.NumberOfReplicas = 10;
                //client.CreateIndex("sample", new IndexSettings());
            }

            if (operation.Equals("delete"))
            {
                //client.DeleteById(indexName, "documents", document.DocumentId);
                IDeleteByQueryRequest iDeleteByQueryRequest = new DeleteByQueryRequest();
                //iDeleteByQueryRequest.
                //IDeleteIndexRequest delReq = new DeleteIndexRequest(indexName);
                //delReq.
                //client.DeleteIndex()
                //client.DeleteByQuery(new DeleteByQueryRequest())
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