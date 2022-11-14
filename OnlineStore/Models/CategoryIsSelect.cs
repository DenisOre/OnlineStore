namespace OnlineStore.Models
{
    public class CategoryIsSelect: Category
    {
        public CategoryIsSelect(): base()
        {}

        public bool IsSelected { get; set; }
        public CategoryIsSelect(Category category)
        {
            this.Id = category.Id;
            this.Name = category.Name;
            this.IsSelected = false;
        }
    }
}
