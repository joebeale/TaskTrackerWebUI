using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebFront.Models
{
    public interface IOrganization
    {
        public int pkOrgID { get; set; }
        public string org_ShortName { get; set; }
        public string org_LongName { get; set; }
        public bool org_IsActive { get; set; }
    }

    public class Organization : IOrganization
    {
        public int pkOrgID { get; set; }
        public string org_ShortName { get; set; }
        public string org_LongName { get; set; }
        public bool org_IsActive { get; set; }
    }
}
