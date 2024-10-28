namespace FilamentsAPI.Model
{
    /// <summary>
    /// Application settings.
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// The secret used for signing JWT tokens.
        /// </summary>
        public string Secret { get; set; } = string.Empty;
        /// <summary>
        /// An ACL with tis ID must be present and is used to identify the admins of the system.
        /// </summary>
        public int AdminACLId { get; set; } = -1;
        /// <summary>
        /// Secret used for the backup API, if implemented.
        /// </summary>
        public string BackupSecret { get; set; } = string.Empty;
        /// <summary>
        /// Default username.
        /// </summary>
        public string DefaultUser {  get; set; } = string.Empty;
        /// <summary>
        /// Default password hash, calculated as follows:
        /// 1. choose 4-character hash (abcd)
        /// 2. prefix password with the 4-character hash (abcdMyPassword!1)
        /// 3. calculate sha256 over the prefixed password (echo -n 'abcdMyPassword!1' | sha256sum | cut -d' ' -f1 => fcc24a9890cb514be545f04e32a11caa255c15702ef859ce2181243bcbd71d62)
        /// 4. prefix sha256 hash with the hash and put that in this field (abcdfcc24a9890cb514be545f04e32a11caa255c15702ef859ce2181243bcbd71d62)
        /// </summary>
        public string DefaultUserHash {  get; set; } = string.Empty;
        /// <summary>
        /// Photo folder path.
        /// </summary>
        public string PhotoPath {  get; set; } = string.Empty;
    }
}
