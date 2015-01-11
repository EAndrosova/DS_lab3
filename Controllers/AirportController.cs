using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using DS_lab3.Models;

namespace DS_lab3.Controllers
{
    public class AirportController : Controller
    {
        private DS_2Entities3 db = new DS_2Entities3();

        //
        // GET: /Airport/

        public ActionResult Index()
        {
            return View(db.Airports.ToList());
        }

        //
        // GET: /Airport/Details/5

        public ActionResult Details(int id = 0)
        {
            Airports airports = db.Airports.Find(id);
            if (airports == null)
            {
                return HttpNotFound();
            }
            return View(airports);
        }

        //
        // GET: /Airport/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Airport/Create

        [HttpPost]
        public ActionResult Create(Airports airports)
        {
            if (ModelState.IsValid)
            {
                /*db.Airports.Add(airports);
                db.SaveChanges();*/
                var portJson = (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(airports);
                var httpRequest = WebRequest.CreateHttp("http://localhost:53391/api/Airports/PostPort");
                httpRequest.Method = "POST";
                var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
                streamWriter.Write(portJson);
                streamWriter.Flush(); streamWriter.Close();
                return RedirectToAction("Index");
            }

            return View(airports);
        }

        //
        // GET: /Airport/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Airports airports = db.Airports.Find(id);
            if (airports == null)
            {
                return HttpNotFound();
            }
            return View(airports);
        }

        //
        // POST: /Airport/Edit/5

        [HttpPost]
        public ActionResult Edit(Airports airports)
        {
            if (ModelState.IsValid)
            {
                /*db.Entry(airports).State = EntityState.Modified;
                db.SaveChanges();*/
                var portJson = (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(airports);
                var httpRequest = WebRequest.CreateHttp(String.Format("http://localhost:53391/api/Airports/PutPort?id={0}", airports.AirportID);
                httpRequest.Method = "PUT";
                var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
                streamWriter.Write(portJson);
                streamWriter.Flush(); streamWriter.Close();

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                return RedirectToAction("Index");
            }
            return View(airports);
        }

        //
        // GET: /Airport/Delete/5

        public ActionResult Delete(int id = 0)
        {
            /*Airports airports = db.Airports.Find(id);
            if (airports == null)
            {
                return HttpNotFound();
            }*/
            var httpRequest = WebRequest.CreateHttp(String.Format("http://localhost:53391/api/Airports/DeletePort?id={0}", id));
            httpRequest.Method = "DELETE";
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            return RedirectToAction("Index");
        }

        //
        // POST: /Airport/Delete/5

        /*[HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Airports airports = db.Airports.Find(id);
            db.Airports.Remove(airports);
            db.SaveChanges();
            return RedirectToAction("Index");
        }*/

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}