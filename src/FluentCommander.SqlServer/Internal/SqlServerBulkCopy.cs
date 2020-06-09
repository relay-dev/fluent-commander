//using FluentCommander.BulkCopy;
//using Microsoft.Data.SqlClient;
//using System;
//using System.Reflection;
//using System.Text.RegularExpressions;
//using System.Threading;
//using System.Threading.Tasks;

//namespace FluentCommander.SqlServer.Internal
//{
//    public class SqlServerBulkCopy : IBulkCopy
//    {
//        public BulkCopyResult BulkCopy(BulkCopyRequest request)
//        {
//            using var connection = GetDbConnection();
//            using var command = new SqlBulkCopy(connection.ConnectionString)
//            {
//                DestinationTableName = request.TableName
//            };

//            if (request.Timeout.HasValue)
//            {
//                command.BulkCopyTimeout = request.Timeout.Value.Seconds;
//            }

//            if (request.ColumnMapping != null)
//            {
//                foreach (ColumnMap columnMap in request.ColumnMapping.ColumnMaps)
//                {
//                    command.ColumnMappings.Add(columnMap.Source, columnMap.Destination);
//                }
//            }

//            try
//            {
//                connection.Open();
//                command.WriteToServer(request.DataTable);
//                connection.Close();
//            }
//            catch (Exception e)
//            {
//                HandleBulkCopyException(e, command);

//                throw;
//            }

//            return new BulkCopyResult(request.DataTable.Rows.Count);
//        }

//        public async Task<BulkCopyResult> BulkCopyAsync(BulkCopyRequest request, CancellationToken cancellationToken)
//        {
//            await using var connection = GetDbConnection();
//            using var command = new SqlBulkCopy(connection)
//            {
//                DestinationTableName = request.TableName
//            };

//            if (request.Timeout.HasValue)
//            {
//                command.BulkCopyTimeout = request.Timeout.Value.Seconds;
//            }

//            if (request.ColumnMapping != null)
//            {
//                foreach (ColumnMap columnMap in request.ColumnMapping.ColumnMaps)
//                {
//                    command.ColumnMappings.Add(columnMap.Source, columnMap.Destination);
//                }
//            }

//            try
//            {
//                connection.Open();
//                await command.WriteToServerAsync(request.DataTable, cancellationToken);
//                connection.Close();
//            }
//            catch (Exception e)
//            {
//                HandleBulkCopyException(e, command);

//                throw;
//            }

//            return new BulkCopyResult(request.DataTable.Rows.Count);
//        }

//        private void HandleBulkCopyException(Exception e, SqlBulkCopy sqlBulkCopy)
//        {
//            // Credit: http://stackoverflow.com/questions/10442686/received-an-invalid-column-length-from-the-bcp-client-for-colid-6
//            if (!e.Message.Contains("Received an invalid column length from the bcp client for colid"))
//            {
//                return;
//            }

//            try
//            {
//                string pattern = @"\d+";
//                Match match = Regex.Match(e.Message, pattern);
//                var index = Convert.ToInt32(match.Value) - 1;

//                FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings", BindingFlags.NonPublic | BindingFlags.Instance);
//                if (fi == null) return;
//                var sortedColumns = fi.GetValue(sqlBulkCopy);
//                var items = (object[])sortedColumns.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(sortedColumns);

//                if (items == null) return;
//                FieldInfo itemData = items[index].GetType().GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
//                if (itemData == null) return;
//                var metadata = itemData.GetValue(items[index]);

//                var column = metadata.GetType().GetField("column", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(metadata);
//                var length = metadata.GetType().GetField("length", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(metadata);

//                throw new Exception($"Column: {column} contains data with a length greater than: {length}");
//            }
//            catch (Exception)
//            {
//                throw e;
//            }
//        }
//    }
//}
