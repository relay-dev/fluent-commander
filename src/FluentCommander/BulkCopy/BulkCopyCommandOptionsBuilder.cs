namespace FluentCommander.BulkCopy
{
    public class BulkCopyCommandOptionsBuilder
    {
        private readonly BulkCopyRequest _request;

        public BulkCopyCommandOptionsBuilder(BulkCopyRequest request)
        {
            _request = request;
            _request.Options ??= new BulkCopyCommandOptions();
        }

        /// <summary>
        /// When specified, **AllowEncryptedValueModifications** enables bulk copying of encrypted data between tables or databases, without decrypting the data. Typically, an application would select data from encrypted columns from one table without decrypting the data (the app would connect to the database with the column encryption setting keyword set to disabled) and then would use this option to bulk insert the data, which is still encrypted. Use caution when specifying **AllowEncryptedValueModifications** as this may lead to corrupting the database because the driver does not check if the data is indeed encrypted, or if it is correctly encrypted using the same encryption type, algorithm and key as the target column
        /// </summary>
        public BulkCopyCommandOptionsBuilder AllowEncryptedValueModifications(bool flag = true)
        {
            _request.Options.AllowEncryptedValueModifications = flag;

            return this;
        }

        /// <summary>
        /// Check constraints while data is being inserted. By default, constraints are not checked
        /// </summary>
        public BulkCopyCommandOptionsBuilder CheckConstraints(bool flag = true)
        {
            _request.Options.CheckConstraints = flag;

            return this;
        }

        /// <summary>
        /// When specified, cause the server to fire the insert triggers for the rows being inserted into the database
        /// </summary>
        public BulkCopyCommandOptionsBuilder FireTriggers(bool flag = true)
        {
            _request.Options.FireTriggers = flag;

            return this;
        }

        /// <summary>
        /// Preserve source identity values. When not specified, identity values are assigned by the destination
        /// </summary>
        public BulkCopyCommandOptionsBuilder KeepIdentity(bool flag = true)
        {
            _request.Options.KeepIdentity = flag;

            return this;
        }

        /// <summary>
        /// Preserve null values in the destination table regardless of the settings for default values. When not specified, null values are replaced by default values where applicable
        /// </summary>
        public BulkCopyCommandOptionsBuilder KeepNulls(bool flag = true)
        {
            _request.Options.KeepNulls = flag;

            return this;
        }

        /// <summary>
        /// Obtain a bulk update lock for the duration of the bulk copy operation. When not specified, row locks are used
        /// </summary>
        public BulkCopyCommandOptionsBuilder TableLock(bool flag = true)
        {
            _request.Options.TableLock = flag;

            return this;
        }

        /// <summary>
        /// When specified, each batch of the bulk-copy operation will occur within a transaction. If you indicate this option and also provide a <see cref="T:Microsoft.Data.SqlClient.SqlTransaction" /> object to the constructor, an <see cref="T:System.ArgumentException" /> occurs
        /// </summary>
        public BulkCopyCommandOptionsBuilder UseInternalTransaction(bool flag = true)
        {
            _request.Options.UseInternalTransaction = flag;

            return this;
        }
    }
}
