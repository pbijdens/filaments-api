namespace FilamentsAPI
{
    /// <summary>
    /// 
    /// </summary>
    public static class RandomUtilities
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] AsByteArray(this Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string Base64Encode(this byte[] data) => Convert.ToBase64String(data ?? []);
    }
}
