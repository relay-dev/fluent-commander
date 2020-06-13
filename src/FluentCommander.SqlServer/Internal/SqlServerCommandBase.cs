using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FluentCommander.SqlServer.Internal
{
    internal abstract class SqlServerCommandBase
    {
        protected SqlParameter[] ToSqlParameters(List<DatabaseCommandParameter> databaseCommandParameters)
        {
            return databaseCommandParameters?.Select(ToSqlParameter).ToArray();
        }

        private SqlParameter ToSqlParameter(DatabaseCommandParameter databaseCommandParameter)
        {
            var parameter = new SqlParameter
            {
                ParameterName = databaseCommandParameter.Name,
                Value = databaseCommandParameter.Value,
                Direction = databaseCommandParameter.Direction,
                Size = databaseCommandParameter.Size
            };

            if (!string.IsNullOrEmpty(databaseCommandParameter.DatabaseType))
            {
                if (!Enum.TryParse(databaseCommandParameter.DatabaseType, true, out SqlDbType sqlDbType))
                {
                    throw new InvalidOperationException($"Could not parse databaseType of '{databaseCommandParameter.DatabaseType}' to a System.Data.DbType");
                }

                parameter.SqlDbType = sqlDbType;
            }

            return parameter;
        }

        protected TFlag SetFlag<TFlag>(TFlag allFlags, TFlag flag, bool? isSet) where TFlag : Enum
        {
            if (!isSet.HasValue)
            {
                return allFlags;
            }

            int intAllFlags = (int)(object)allFlags;
            int intFlag = (int)(object)flag;

            if (isSet.Value)
            {
                intAllFlags |= intFlag;
            }
            else
            {
                intAllFlags &= ~intFlag;
            }

            return (TFlag)(object)intAllFlags;
        }
    }
}
