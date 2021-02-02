using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;


namespace Harry.SqlBuilder
{
    [StructLayout(LayoutKind.Auto)]
    public struct SqlBuilderCommand
    {
        public SqlBuilderCommand(string sql) : this(sql, null)
        {
            if (string.IsNullOrEmpty(sql))
                throw new ArgumentException($"{nameof(sql)}不能为空", nameof(sql));
        }
        public SqlBuilderCommand(string sql, SqlBuilderParameter parameter) : this(sql, new SqlBuilderParameter[] { parameter })
        {
            if (string.IsNullOrEmpty(sql))
                throw new ArgumentException($"{nameof(sql)}不能为空", nameof(sql));
        }
        public SqlBuilderCommand(string sql, IEnumerable<SqlBuilderParameter> parameters, IEnumerable<object> dynamicParameters = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            if (string.IsNullOrEmpty(sql))
                throw new ArgumentException($"{nameof(sql)}不能为空", nameof(sql));
            Sql = sql.Trim();
            Parameters = parameters;
            this.DynamicParameters = dynamicParameters;
            this.Transaction = transaction;
            this.CommandTimeout = commandTimeout;
            this.CommandType = commandType;

        }
        /// <summary>
        /// 获取sql语句
        /// </summary>
        public string Sql { get; private set; }

        /// <summary>
        /// 获取参数列表
        /// </summary>
        public IEnumerable<SqlBuilderParameter> Parameters { get; private set; }

        public IEnumerable<object> DynamicParameters { get; private set; }

        public IDbTransaction Transaction { get; private set; }

        public int? CommandTimeout { get; private set; }

        public CommandType? CommandType { get; private set; }

        public override int GetHashCode()
        {
            var code = Sql.GetHashCode();
            if (Parameters != null)
            {
                unchecked
                {
                    foreach (var item in Parameters)
                    {
                        code ^= item.GetHashCode();
                    }
                }
            }

            if (DynamicParameters != null)
            {
                unchecked
                {
                    foreach (var item in DynamicParameters)
                    {
                        code ^= item.GetHashCode();
                    }
                }
            }
            return code;
        }

        public bool Equals(SqlBuilderCommand obj)
        {
            return GetHashCode() == obj.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            //struct 不适用于下面的代码
            //if (ReferenceEquals(this, obj)) return true;

            if (typeof(SqlBuilderCommand) != obj.GetType()) return false;

            return GetHashCode() == ((SqlBuilderCommand)obj).GetHashCode();
        }

        //public override string ToString()
        //{
        //    if (string.IsNullOrEmpty(Sql)) return base.ToString();
        //    return Sql;
        //}
    }
}
