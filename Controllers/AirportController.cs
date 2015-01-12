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
            var client = new WebClient();
            var resp = client.DownloadString("http://localhost:53391/api/Airports");
            var fl = (new System.Web.Script.Serialization.JavaScriptSerializer()).Deserialize<List<Airports>>(resp);
            return View(fl);
        }

        public ActionResult AirportsInTheCity()
        {
            if (Session["s_id"] == null)
                return View("ErrorSession");
            var httpRequest = WebRequest.CreateHttp("http://localhost:54266/api/Additional/IsLogged?session="+Session["s_id"].ToString());
            var s = Session["s_id"].ToString();
            httpRequest.Method = "POST";
            httpRequest.ContentLength = 0;
            
            try
            {
                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                var streamReader = new StreamReader(httpResponse.GetResponseStream());
                var result = streamReader.ReadToEnd();
                //Session["s_id"] = result;
                if (httpResponse.StatusCode != HttpStatusCode.OK)
                    return View("ErrorSession");


                var client = new WebClient();
                var str = "http://localhost:54266/api/Help/getcity?session=" + Session["s_id"].ToString();
                var city = client.DownloadString(str);
                if (Response.StatusCode != 200)
                    return View("ErrorSession");
                //var city = "Berlin";
                var js = client.DownloadString("http://localhost:53391/Additional/PortsInCity?city=" + city);
                var ports = (new System.Web.Script.Serialization.JavaScriptSerializer()).Deserialize<List<Airports>>(js);
                return View("Index", ports);
            }
            catch (Exception ex)
                {
                    return View("ErrorSession");
                }
        }

        //
        // GET: /Airport/Details/5

        public ActionResult Details(int id = 0)
        {
            var client = new WebClient();
            var resp = client.DownloadString(String.Format("http://localhost:53391/api/Airports/GetPort?id={0}", id));
            var fl = (new System.Web.Script.Serialization.JavaScriptSerializer()).Deserialize<Airports>(resp);
            return View(fl);
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
                var ftJson = (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(airports);
                var httpRequest = WebRequest.CreateHttp("http://localhost:53391/api/Airports/PostPort");
                httpRequest.Method = "POST";
                var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
                streamWriter.Write(ftJson);
                streamWriter.Flush(); streamWriter.Close();

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                return RedirectToAction("Index");
            }

            return View(airports);
        }

        //
        // GET: /Airport/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var client = new WebClient();
            var resp = client.DownloadString(String.Format("http://localhost:53391/api/Airports/GetPort?id={0}", id));
            var fl = (new System.Web.Script.Serialization.JavaScriptSerializer()).Deserialize<Airports>(resp);
            return View(fl);
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
                var httpRequest = WebRequest.CreateHttp(String.Format("http://localhost:53391/api/Airports/PutPort?id={0}", airports.AirportID));
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
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
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

        public ActionResult LogOut()
        {
            if (Session["s_id"] == null)
                return View("ErrorSession");
            //WebSecurity.Logout();
            var client = new WebClient();
            var str = "http://localhost:54266/api/Session/Login?name=111&pwd=111111";
            //Session["s_id"] = client.DownloadString(str);
            var httpRequest = WebRequest.CreateHttp("http://localhost:54266/api/Session/Logout?session=" + Session["s_id"].ToString());
            httpRequest.Method = "POST";
            httpRequest.ContentLength = 0;


            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                //var streamReader = new StreamReader(httpResponse.GetResponseStream());
                //var result = streamReader.ReadToEnd();
                //Session["s_id"] = result;
                //var s = Session["s_id"];

                return RedirectToAction("Index", "Home");
            }

            return View("ErrorSession");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}