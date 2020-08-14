using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WebFront.Models
{
    public class EditOrganizationViewModel
    {
        public DTO_Organization editMe { set; get; }

        public string strParentID { set; get; }
        public List<string> strChildIDs { set; get; }
        public List<SelectListItem> ChildOrgListItems { get; }
        public List<SelectListItem> ParentOrgListItems { get; }
        public EditOrganizationViewModel(DTO_Organization thisOrg)
        {
            editMe = thisOrg;
            strParentID = !(editMe.Parent is null) ? editMe.Parent.pkOrgID.ToString() : "";
            strChildIDs = new List<string>();
            if (!(editMe.Children is null))
            {
                foreach (var child in editMe.Children)
                {
                    strChildIDs.Add(child.pkOrgID.ToString());
                }
            }
            ChildOrgListItems = new List<SelectListItem>();
            ParentOrgListItems = new List<SelectListItem>();
            using (var client = new HttpClient())
            {
                MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                HttpResponseMessage response = client.GetAsync("https://webapiserver.strickland.local/windatabroker/api/vm_organizations/readonly/ActiveOrgs").Result;
                string strData = response.Content.ReadAsStringAsync().Result;
                var AllActiveOrgs = JsonConvert.DeserializeObject<IEnumerable<Organization>>(strData, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });
                ParentOrgListItems.Add(new SelectListItem { Value = "-1", Text = "None" });
                ParentOrgListItems[0].Selected = true;
                foreach (var org in AllActiveOrgs)
                {
                    SelectListItem nxtItem = new SelectListItem { Value = org.pkOrgID.ToString(), Text = org.org_ShortName };
                    if (nxtItem.Value == strParentID)
                    { nxtItem.Selected = true; }                    
                    ParentOrgListItems.Add(nxtItem);
                }

            }
            using (var client = new HttpClient())
            {
                MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                HttpResponseMessage response = client.GetAsync("https://webapiserver.strickland.local/windatabroker/api/vm_organizations/readonly/OrphanOrgs").Result;
                string strData = response.Content.ReadAsStringAsync().Result;
                var OrphanOrgs = JsonConvert.DeserializeObject<IEnumerable<Organization>>(strData, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto });
                foreach (var org in OrphanOrgs)
                {
                    ChildOrgListItems.Add(new SelectListItem { Value = org.pkOrgID.ToString(), Text = org.org_ShortName });
                }
            }
        }

    }   
    
}
