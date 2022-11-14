namespace OnlineStore.Models
{
    public class ManufacturerIsSelect: Manufacturer
    {
        public ManufacturerIsSelect() : base()
        { }

        public bool IsSelected { get; set; }
        public ManufacturerIsSelect(Manufacturer manufacturer)
        { 
            this.Id = manufacturer.Id;
            this.Name = manufacturer.Name;
            this.IsSelected = false;
        }
    }
}
