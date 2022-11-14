namespace OnlineStore.Models
{
    public class Role: IComparable<Role>
    {
        public int Id { get; set; }
        public string? Name { get; set; }        

        public int CompareTo(Role obj)
        {
            return Name.CompareTo(obj.Name);
        }
    }
}
