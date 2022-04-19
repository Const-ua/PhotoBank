using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using PhotoBank.Data.Repository.IRepository;

using PhotoBank.Models;



namespace PhotoBank.Data.Repository
{
    public class PhotoRepository: Repository<Photo>, IPhotoRepository
    {
        private readonly PbContext _db;
        private readonly ILogger<Repository<Photo>> _logger;

        public PhotoRepository(PbContext db,ILogger<Repository<Photo>> logger) : base(db, logger)
        {
            _db=db;
            _logger=logger;
        }

        public Photo Find(Guid id)
        {
            Photo photo = _db.Photos.Find(id);
            return photo;
        }

        public IEnumerable<SelectListItem> GetCategoryList()
        {
            IEnumerable<SelectListItem> selectList = _db.Categories.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id.ToString()
            });

            return selectList;
        }
    }
}