namespace FilamentsAPI.Model.Storageboxes
{
    /// <summary>
    /// 
    /// </summary>
    public class StorageboxHeaderModel
    {
        /// <summary>
        /// DB ID
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// The name of this storage box.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// If there are any notes, they can be added here.
        /// </summary>
        public string Notes { get; set; } = string.Empty;

        /// <summary>
        /// When was the dessicant in this box last changed?
        /// </summary>
        public string LastDessicantChange { get; set; } = string.Empty;

        /// <summary>
        /// Unique ID of the uploaded photo, if any.
        /// </summary>
        public string Photo { get; set; } = string.Empty;
    }
}
