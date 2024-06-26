namespace DotnetAPI.DTOs
{
    public partial class UserForLoginConfirmationDTO
    {
        byte[] PasswordHash { get; set; }
        byte[] PasswordSalt { get; set; }

        public UserForLoginConfirmationDTO()
        {
            PasswordHash = [];
            PasswordSalt = [];
        }
    }
}