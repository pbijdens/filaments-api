namespace FilamentsAPI.Model.Accounts
{
    /// <summary>
    /// Request login.
    /// </summary>
    public class LoginRequestModel
    {
        /// <summary>
        /// Username, case sensitive.
        /// </summary>
        public string Username { get; set; } = string.Empty;
        /// <summary>
        /// Password, case sensitive.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
