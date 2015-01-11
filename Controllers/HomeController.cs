using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using DS_lab3.Models;

namespace DS_lab3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // Airports
        #region

        public ActionResult PostAirport()
        {
            var port = new Airports();
            port.ABB = "MSA"; port.AirportID = 52; port.City = "Marsa Alam"; port.Title = "Marsa Alam Int";
            var portJson = (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(port);
            var httpRequest = WebRequest.CreateHttp("http://localhost:53391/api/Airports/PostPort");
            httpRequest.Method = "POST";
            var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
            streamWriter.Write(portJson);
            streamWriter.Flush(); streamWriter.Close();

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            var streamReader = new StreamReader(httpResponse.GetResponseStream());
            var result = streamReader.ReadToEnd();
                //HttpStatusCodeResult
            return View("Index", "Home");
        }

        public ActionResult PutAirport()
        {
            var port = new Airports();
            port.ABB = "MSA"; port.AirportID = 52; port.City = "1Marsa Alam"; port.Title = "1Marsa Alam Int";
            var portJson = (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(port);
            var httpRequest = WebRequest.CreateHttp("http://localhost:53391/api/Airports/PutPort?id=52");
            httpRequest.Method = "PUT";
            var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
            streamWriter.Write(portJson);
            streamWriter.Flush(); streamWriter.Close();

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            string resp = "";
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }

            return View("Index", "Home");
        }

        public ActionResult DeleteAirport()
        {
            var httpRequest = WebRequest.CreateHttp("http://localhost:53391/api/Airports/DeletePort?id=52");
            httpRequest.Method = "DELETE";
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }

            return View("Index", "Home");
        }

        #endregion

        // Flights
        #region

        public ActionResult PostFlight()
        {
            var ft = new Flights();
            ft.AirlineID = 1; ft.AirplaneID = 1; ft.ArrivalTime = DateTime.Now; ft.CreateDate = DateTime.Now; 
                ft.DepartureTime = DateTime.Now.AddHours(3); ft.Destination = 1; ft.FlightID = Guid.NewGuid(); 
                ft.FlightNumber = "q1"; ft.FreeSeats = 35; ft.Origin = 2; ft.Price = 123;

            var ftJson = (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(ft);
            var httpRequest = WebRequest.CreateHttp("http://localhost:57040/api/Flights/PostFlight");
            httpRequest.Method = "POST";
            var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
            streamWriter.Write(ftJson);
            streamWriter.Flush(); streamWriter.Close();

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            var streamReader = new StreamReader(httpResponse.GetResponseStream());
            var result = streamReader.ReadToEnd();
                //HttpStatusCodeResult
            return View("Index", "Home");
        }

        public ActionResult PutFlight()
        {
            var ft = new Flights();
            ft.AirlineID = 2; ft.AirplaneID = 2; ft.ArrivalTime = DateTime.Now; ft.CreateDate = DateTime.Now;
                ft.DepartureTime = DateTime.Now.AddHours(3); ft.Destination = 3; ft.FlightID = Guid.Parse("5FD3C1B9-B274-4596-9AB2-5F5D8646A613");
                ft.FlightNumber = "s2"; ft.FreeSeats = 54; ft.Origin = 4; ft.Price = 543;
            var portJson = (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(ft);
            var httpRequest = WebRequest.CreateHttp(String.Format("http://localhost:57040/api/Flights/PutFlight?id={0}", ft.FlightID));
            httpRequest.Method = "PUT";
            var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
            streamWriter.Write(portJson);
            streamWriter.Flush(); streamWriter.Close();

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            var streamReader = new StreamReader(httpResponse.GetResponseStream());
            var result = streamReader.ReadToEnd();
            return View("Index", "Home");
        }

        public ActionResult DeleteFlight()
        {
            var db = new DS_2EntitiesFlight(); var ftt = db.Flights.ElementAt(2);
            var httpRequest = WebRequest.CreateHttp(String.Format("http://localhost:57040/api/Flights/DeleteFlight?id={0}", ftt.FlightID));
            httpRequest.Method = "DELETE";
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }

            return View("Index", "Home");
        }

        #endregion

        // Session
        #region

        
        #endregion


    }
}
