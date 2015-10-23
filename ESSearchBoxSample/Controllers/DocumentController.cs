using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nest;
using System.Configuration;
using ESSearchBoxSample.Models;

namespace ESSearchBoxSample.Controllers
{
    public class DocumentController : Controller
    {
        private SampleEntities db = new SampleEntities();

        // GET: Document
        public ActionResult Index()
        {
            //List<DocumentModel> list = new List<DocumentModel>();
            //DocumentModel model = new DocumentModel();
            //model.DocumentId = 1;
            //model.Name = "name-bian";
            //model.Text = "text-jianquan";
            //list.Add(model);
            var uriString = ConfigurationManager.AppSettings["SEARCHBOX_URL"];
            var documents = db.DocumentModels.ToList();
            return View(documents);
        }

        public ActionResult Search(string searchString)
        {

            // return first 5 results, default is 10
            var result = ElasticClient.Search<DocumentModel>(body =>body.Size(5).Query(query =>query.QueryString(qs => qs.Query(searchString))));

            //SearchDescriptor<DocumentModel> searchDescriptor = new SearchDescriptor<DocumentModel>();
            //QueryContainer queryContainer = Query<DocumentModel>.Term(p => p.CityInfo.CityId, 1);
            //QueryStringQueryDescriptor<DocumentModel> queryStringQueryDescriptor = new QueryStringQueryDescriptor<DocumentModel>();
            //queryStringQueryDescriptor.Query(searchString);
            //queryStringQueryDescriptor.
            //queryStringQueryDescriptor.
            //QueryContainer queryContainer = Query<DocumentModel>.QueryString(qs=>qs.Query(searchString));
            //QueryContainer queryContainer = new QueryContainer();
            //queryContainer.
            //QueryContainer queryContainer = Query<DocumentModel>.QueryString(queryStringQueryDescriptor);
            //queryContainer.

            ViewBag.Query = searchString;
            ViewBag.Total = result.Total;
            return View("SearchResult", result.Documents.ToList());
        }

        private static ElasticClient ElasticClient
        {
            get
            {
                try
                {
                    var uriString = ConfigurationManager.AppSettings["SEARCHBOX_URL"];
                    var searchBoxUri = new Uri(uriString);
                    var settings = new ConnectionSettings(searchBoxUri);
                    settings.SetDefaultIndex("sample");
                    return new ElasticClient(settings);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

    }
}