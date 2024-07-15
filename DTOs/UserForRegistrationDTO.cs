namespace DotnetAPI.DTOs
{
    public partial class UserForRegistrationDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }

        public UserForRegistrationDTO()
        {
            Email ??= "";
            Password ??= "";
            PasswordConfirmation ??= "";
            FirstName ??= "";
            LastName ??= "";
            Gender ??= "";
        }
    }


}