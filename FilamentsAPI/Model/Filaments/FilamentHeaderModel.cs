namespace FilamentsAPI.Model.Filaments
{
    /// <summary>
    /// Basic data for the filament.
    /// </summary>
    public class FilamentHeaderModel
    {
        /// <summary>
        /// DB ID
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Description of the filament.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// If there are any notes, they can be added here.
        /// </summary>
        public string Notes { get; set; } = string.Empty;

        /// <summary>
        /// Brand of Filament.
        /// </summary>
        public string Brand { get; set; } = string.Empty;

        /// <summary>
        /// Kind of filament.
        /// </summary>
        public string Kind { get; set; } = string.Empty;

        /// <summary>
        /// Current weight, including the spool.
        /// </summary>
        public int Weight { get; set; } = 0;

        /// <summary>
        /// How much did we pay for this particular filament per KG?
        /// </summary>
        public double PricePerKG { get; set; } = 0.0;

        /// <summary>
        /// Initial weight, including the spool.
        /// </summary>
        public int InitialWeight { get; set; } = 0;

        /// <summary>
        /// When was this first added?
        /// </summary>
        public string FirstAdded { get; set; } = string.Empty;

        /// <summary>
        /// When was this last changed?
        /// </summary>
        public string LastUpdated { get; set; } = string.Empty;

        /// <summary>
        /// Color 1 (HEX code)
        /// </summary>
        public string Color1 { get; set; } = string.Empty;

        /// <summary>
        /// Color 2 (HEX code)
        /// </summary>
        public string Color2 { get; set; } = string.Empty;

        /// <summary>
        /// Binary thumbnail.
        /// </summary>
        public string Photo { get; set; } = string.Empty;

        /// <summary>
        /// ID of the storage box.
        /// </summary>
        public int StorageBoxID { get; set; } = -1;

        /// <summary>
        /// Informative, name of the storage box.
        /// </summary>
        public string StorageBoxName { get; set; } = string.Empty;
    }
}
