namespace FilamentsAPI.Model.Accounts
{
    /// <summary>
    /// ACL
    /// </summary>
    public class UserACLModel
    {
        /// <summary>
        /// Persistence ID
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// Username
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}