using Microsoft.AspNetCore.Mvc.Rendering;
//using System.Web.Mvc;

using PhotoBank.Data;
using PhotoBank.Models;

namespace PhotoBank.ViewModels
{
    public class PhotoVM
    {
        public Photo Photo { get; set; }
     
        public IEnumerable<SelectListItem> Categories { get; set; }
       
    }
}
