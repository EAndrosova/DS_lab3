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
    public class FlightController : Controller
    {
        private DS_2EntitiesFlight db = new DS_2EntitiesFlight();

        //
        // GET: /Flight/

        public ActionResult Index()
        {
            var client = new WebClient();
            var resp = client.DownloadString("http://localhost:57040/api/Flights");
            var fl = (new System.Web.Script.Serialization.JavaScriptSerializer()).Deserialize<List<Flights>>(resp);
            return View(fl);
        }

        //
        // GET: /Flight/Details/5

        public ActionResult Details(Guid id)
        {
            /*Flights flights = db.Flights.Find(id);
            if (flights == null)
            {
                return HttpNotFound();
            }*/
            var client = new WebClient();
            var resp = client.DownloadString(String.Format("http://localhost:57040/api/Flights/GetFlight?id={0}", id));
            var fl = (new System.Web.Script.Serialization.JavaScriptSerializer()).Deserialize<Flights>(resp);
            return View(fl);
        }

        //
        // GET: /Flight/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Flight/Create

        [HttpPost]
        public ActionResult Create(Flights flights)
        {
            if (ModelState.IsValid)
            {
                /*flights.FlightID = Guid.NewGuid();
                db.Flights.Add(flights);
                db.SaveChanges();*/
                flights.FlightID = Guid.NewGuid();
                var ftJson = (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(flights);
                var httpRequest = WebRequest.CreateHttp("http://localhost:57040/api/Flights/PostFlight");
                httpRequest.Method = "POST";
                var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
                streamWriter.Write(ftJson);
                streamWriter.Flush(); streamWriter.Close();

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                return RedirectToAction("Index");
            }

            return View(flights);
        }

        //
        // GET: /Flight/Edit/5

        public ActionResult Edit(Guid id)
        {
            /*var httpRequest = WebRequest.CreateHttp(String.Format("http://localhost:57040/api/Flights/Get?id={0}", id));
            httpRequest.Method = "GET";
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            var streamReader = new StreamReader(httpResponse.GetResponseStream());
            var result = streamReader.ReadToEnd();*/
            //var data = (new System.Web.Script.Serialization.JavaScriptSerializer()).Deserialize<JsonResult>(result);
            //var flights = (new System.Web.Script.Serialization.JavaScriptSerializer()).Deserialize<Models.Flights>(result);


            var client = new WebClient();
            var resp = client.DownloadString(String.Format("http://localhost:57040/api/Flights/GetFlight?id={0}", id));
            var fl = (new System.Web.Script.Serialization.JavaScriptSerializer()).Deserialize<Flights>(resp);
            return View(fl);
        }

        //
        // POST: /Flight/Edit/5

        [HttpPost]
        public ActionResult Edit(Flights flights)
        {
            if (ModelState.IsValid)
            {
                /*db.Entry(flights).State = EntityState.Modified;
                db.SaveChanges();*/
                var portJson = (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(flights);
                var httpRequest = WebRequest.CreateHttp(String.Format("http://localhost:57040/api/Flights/PutFlight?id={0}", flights.FlightID));
                httpRequest.Method = "PUT";
                var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
                streamWriter.Write(portJson);
                streamWriter.Flush(); streamWriter.Close();

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            
                return RedirectToAction("Index");
            }
            return View(flights);
        }

        //
        // GET: /Flight/Delete/5
        public ActionResult Delete(Guid id)
        {
            /*Flights flights = db.Flights.Find(id);
            if (flights == null)
            {
                return HttpNotFound();
            }*/
            var httpRequest = WebRequest.CreateHttp(String.Format("http://localhost:57040/api/Flights/DeleteFlight?id={0}", id));
            httpRequest.Method = "DELETE";
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
            return RedirectToAction("Index");
        }

        //
        // POST: /Flight/Delete/5

        /*[HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Flights flights = db.Flights.Find(id);
            db.Flights.Remove(flights);
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