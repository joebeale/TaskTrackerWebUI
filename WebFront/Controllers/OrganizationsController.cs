using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebFront.Models;
//using System.Text.Json;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using Newtonsoft.Json.Converters;
using Microsoft.VisualBasic;

public class OrganizationsController : Controller
{
    [Route("{controller}")]
    [HttpGet]
    public ActionResult Index()
    {
        IEnumerable<DTO_Organization> orgs = null;

        using (var client = new HttpClient())
        {
            MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            HttpResponseMessage response = client.GetAsync("https://webapiserver.strickland.local/windatabroker/api/vm_organizations/").Result;
            string strData = response.Content.ReadAsStringAsync().Result;
            orgs = JsonConvert.DeserializeObject<IEnumerable<DTO_Organization>>(strData, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });
        }
        return View(orgs);

    }

    #region Create
    [HttpGet]
    public ActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public ActionResult Create(DTO_Organization neworg)
    {
        
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("https://webapiserver.strickland.local/windatabroker/api/");
            string jsonOrg = JsonConvert.SerializeObject(neworg);
            StringContent myPostContent = new StringContent(jsonOrg, Encoding.UTF8, "application/json");
            var postTask = client.PostAsync("organizations", myPostContent);

            var result = postTask.Result;

            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to add new organization");
        }

        return View(neworg);
    }
    #endregion

    #region Update
    [HttpGet("{controller}/Update/{id}")]
    public ActionResult Update(int id)
    {
        DTO_Organization thisOrg = null;

        using (var client = new HttpClient())
        {
            MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            client.BaseAddress = new Uri("https://webapiserver.strickland.local/windatabroker/api/vm_organizations/");                     
            HttpResponseMessage response = client.GetAsync(id.ToString()).Result;
            string strData = response.Content.ReadAsStringAsync().Result;
            thisOrg = JsonConvert.DeserializeObject<DTO_Organization>(strData, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });

        }
        EditOrganizationViewModel pageModel = new EditOrganizationViewModel(thisOrg);
        return View(pageModel);
    }

    [HttpPost]
    public ActionResult Update(DTO_Organization deltaOrg)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("https://webapiserver.strickland.local/windatabroker/api/vm_organizations/");
            string jsonOrg = JsonConvert.SerializeObject(deltaOrg);
            StringContent myPostContent = new StringContent(jsonOrg, Encoding.UTF8, "application/json");
            var postTask = client.PutAsync("update", myPostContent);
            var result = postTask.Result;

            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to update organization");
        }

        return View(deltaOrg);
    }
    #endregion
}