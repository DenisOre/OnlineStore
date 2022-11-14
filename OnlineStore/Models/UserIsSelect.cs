namespace OnlineStore.Models
{
    public class UserIsSelect: User
    {
        public UserIsSelect() : base()
        { 
        }
        public bool IsSelected { get; set; }
        public UserIsSelect(User user)
        {
            this.Id= user.Id;
            this.Name = user.Name;
            this.Surname = user.Surname;
            this.Email = user.Email;
            this.Password = user.Password;
            this.Phone = user.Phone;
            this.Role = user.Role;
            this.IsSelected = false;
        }
    }
}
