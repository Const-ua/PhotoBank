using Microsoft.AspNetCore.Mvc.Rendering;

//using System.Web.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
using PhotoBank.Models;

namespace PhotoBank.Data.Repository.IRepository
{
    public interface IPhotoRepository : IRepository<Photo> 
    {
        Photo Find(Guid id);
        public IEnumerable<SelectListItem> GetCategoryList();
    }
}
