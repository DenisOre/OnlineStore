namespace OnlineStore.Models
{
    public class Category: IComparable<Category>
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public int CompareTo(Category obj)
        {
           return Name.CompareTo(obj.Name);
        }
    }
}
