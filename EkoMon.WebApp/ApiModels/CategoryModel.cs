using EkoMon.DomainModel.Db;
namespace EkoMon.WebApp.ApiModels
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        
        public CategoryModel(Category category)
        {
            Id = category.Id;
            Title = category.Title;
        }
    }
}
