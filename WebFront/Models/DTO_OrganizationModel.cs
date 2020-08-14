using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace WebFront.Models
{
    public class DTO_Organization : Organization
    {      
        public Organization Parent { get; set; }

        public List<Organization> Children { get; set; }
    }
}