
using Newtonsoft.Json;
using consumingWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Web.Mvc;

namespace consumingWebAPI.Controllers
{
    public class PReviewController : Controller
    {
        // GET: ToDo
        [HttpGet]
        public ActionResult GetToDoList()
        {
            List<ToDo> TD = new List<ToDo>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44328/api/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("nl-NL"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                var responseTask = client.GetAsync("ToDo/GetTaskList");
                responseTask.Wait();

                HttpResponseMessage response = responseTask.Result;
                if (response.IsSuccessStatusCode)
                {
                    
                    var readTask = response.Content.ReadAsAsync<List<ToDo>>();
                    readTask.Wait();
                    TD = readTask.Result;


                }
            }
            return View(TD);
        }
        public ActionResult GetReviewList()
        {
            List<TaskReview> TR = new List<TaskReview>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44328/api/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("nl-NL"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                var responseTask = client.GetAsync("Review/GetTaskReviewList");
                responseTask.Wait();

                HttpResponseMessage response = responseTask.Result;
                if (response.IsSuccessStatusCode)
                {
                  
                    var readTask = response.Content.ReadAsAsync<List<TaskReview>>();
                    readTask.Wait();
                    TR = readTask.Result;


                }
            }
            return View(TR);
        }



        [HttpGet]
        public ActionResult GetTaskReviewbytheirID(int ToDoId)
        {
            List<TaskReview> TR = new List<TaskReview>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44328/api/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("nl-NL"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

               
                var responseTask = client.GetAsync("Review/GetTaskReviewbytheirID?ToDoId=" + ToDoId.ToString());
               
                responseTask.Wait();

                HttpResponseMessage response = responseTask.Result;
                if (response.IsSuccessStatusCode)
                {
                    // Get back a single task review object
                    var readTask = response.Content.ReadAsAsync<List<TaskReview>>();
                    readTask.Wait();
                    TR = readTask.Result;
                }
            }
            return View(TR);
        }

        [HttpGet]
        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( TaskReview Tr)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44328/api/Review");

                    var response = await client.PostAsJsonAsync("Review/PostReview", Tr);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("GetTaskReviewbytheirID", new { ToDoId= Tr.ToDoId});
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    }
                }
            }
            return View(Tr);
        }
        public async Task<ActionResult> Edit(int ReviewID)
        {
            if (ReviewID == 0)
            {
                return View();
            }
            TaskReview Tr = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44328/api/Review/");

                var result = await client.GetAsync("GetReviewById?ReviewID=" + ReviewID.ToString());

                if (result.IsSuccessStatusCode)
                {
                    Tr = await result.Content.ReadAsAsync<TaskReview>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            if (Tr == null)
            {
                return View();
            }
            return View(Tr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TaskReview Tr)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44328/api/Review/");
                    var response = await client.PutAsJsonAsync("Put", Tr);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("GetTaskReviewbytheirID", new { ToDoId= Tr.ToDoId});
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    }
                }
                return RedirectToAction("GetReviewList");
            }
            return View(Tr);
        }

        public async Task<ActionResult> Delete(string ReviewID)
        {
            if (ReviewID == null)
            {
                return View();
            }
            TaskReview Tr = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44328/api/Review/");

                var result = await client.GetAsync("GetReviewById?ReviewID=" + ReviewID);

                if (result.IsSuccessStatusCode)
                {
                    Tr = await result.Content.ReadAsAsync<TaskReview>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }

            if (Tr == null)
            {
                return View();
            }
            return View(Tr);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string ReviewID)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44328/api/Review/");

                var response = await client.DeleteAsync("DeleteReview/?ReviewID="+ ReviewID);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetToDoList");
                }
                else
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
            }
            return View();
        }

     
    }
}


   

    