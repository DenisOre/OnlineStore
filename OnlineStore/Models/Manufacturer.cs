namespace OnlineStore.Models
{
    public class Manufacturer: IComparable<Manufacturer>
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public int CompareTo(Manufacturer manufacturer)
        {
            return Name.CompareTo(manufacturer.Name);
        }
    }
}
