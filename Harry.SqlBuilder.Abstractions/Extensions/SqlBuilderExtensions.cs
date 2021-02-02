using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Harry.SqlBuilder
{
    public static class SqlBuilderExtensions
    {
        #region Insert
        public static IInsertBuilder Column(this IInsertBuilder builder, string columnName, object value, DbType? dbType = null, ParameterDirection? direction = null, int? size = null)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException($"{nameof(columnName)}不能为空", nameof(columnName));
            //if (value == null)
            //    throw new ArgumentException($"{nameof(value)}不能为空", nameof(value));

            builder.Column(new SqlBuilderParameter(columnName, value, dbType, direction, size));

            return builder;
        }

        public static IInsertBuilder Column(this IInsertBuilder builder, bool condition, Func<SqlBuilderParameter> func)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));
            func = func ?? throw new ArgumentNullException(nameof(func));

            if (condition)
            {
                return builder.Column(func.Invoke());
            }
            else
            {
                return builder;
            }
        }

        #endregion

        #region Update
        public static IUpdateBuilder Column(this IUpdateBuilder builder, string columnName, object value, DbType? dbType = null, ParameterDirection? direction = null, int? size = null)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException($"{nameof(columnName)}不能为空", nameof(columnName));
            //if (value == null)
            //    throw new ArgumentException($"{nameof(value)}不能为空", nameof(value));

            builder.Column(new SqlBuilderParameter(columnName, value, dbType, direction, size));

            return builder;
        }

        public static IUpdateBuilder Column(this IUpdateBuilder builder, bool condition, Func<string> func)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));
            func = func ?? throw new ArgumentNullException(nameof(func));

            if (condition)
            {
                return builder.Column(func.Invoke());
            }

            return builder;
        }

        public static IUpdateBuilder Column(this IUpdateBuilder builder, bool condition, Func<SqlBuilderParameter> func)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));
            func = func ?? throw new ArgumentNullException(nameof(func));

            if (condition)
            {
                return builder.Column(func.Invoke());
            }
            else
            {
                return builder;
            }
        }

        #endregion



        #region Where 无条件

        //public static T Where<T>(this T builder, string sql)
        //    where T : class, IWhere
        //{
        //    builder = builder ?? throw new ArgumentNullException(nameof(builder));

        //    builder.Where(sql);
        //    return builder;
        //}

        //public static T Where<T>(this T builder, string sql, SqlBuilderParameter parameter)
        //    where T : class, IWhere
        //{
        //    builder = builder ?? throw new ArgumentNullException(nameof(builder));

        //    builder.Where(sql, parameter);
        //    return builder;
        //}

        //public static T Where<T>(this T builder, string sql, params SqlBuilderParameter[] parameters)
        //    where T : class, IWhere
        //{
        //    builder = builder ?? throw new ArgumentNullException(nameof(builder));

        //    builder.Where(sql, parameters);
        //    return builder;
        //}

        //public static T Where<T>(this T builder, string sql, object dynamicParams)
        //    where T : class, IWhere
        //{
        //    builder = builder ?? throw new ArgumentNullException(nameof(builder));

        //    builder.Where(sql, dynamicParams);
        //    return builder;
        //}

        #endregion

        #region Where 有条件

        public static T Where<T>(this T builder, bool condition, Func<string> func)
            where T : class, IWhere
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));
            func = func ?? throw new ArgumentNullException(nameof(func));

            if (condition)
            {
                builder.Where(func.Invoke());
            }
            return builder;
        }


        public static T Where<T>(this T builder, bool condition, string sql, Func<SqlBuilderParameter> func)
            where T : class, IWhere
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));
            func = func ?? throw new ArgumentNullException(nameof(func));

            if (condition)
            {
                builder.Where(sql, func.Invoke());
            }
            return builder;
        }

        public static T Where<T>(this T builder, bool condition, string sql, Func<SqlBuilderParameter[]> func)
            where T : class, IWhere
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));
            func = func ?? throw new ArgumentNullException(nameof(func));

            if (condition)
            {
                builder.Where(sql, func.Invoke());
            }
            return builder;
        }

        public static T Where<T>(this T builder, bool condition, string sql, Func<object> func)
            where T : class, IWhere
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));
            func = func ?? throw new ArgumentNullException(nameof(func));

            if (condition)
            {
                builder.Where(sql, func.Invoke());
            }
            return builder;
        }
        #endregion

        #region Raw
        public static IRawBuilder Append(this IRawBuilder builder, bool condition, Func<string> func)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));
            func = func ?? throw new ArgumentNullException(nameof(func));

            if (condition)
            {
                return builder.Append(func.Invoke());
            }
            else
            {
                return builder;
            }
        }

        public static IRawBuilder Append(this IRawBuilder builder, bool condition, Func<string> sqlFunc, Func<SqlBuilderParameter> paramsFunc)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            if (condition)
            {
                if (paramsFunc != null)
                {
                    return builder.Append(sqlFunc?.Invoke(), paramsFunc.Invoke());
                }
                else
                {
                    return builder.Append(sqlFunc?.Invoke());
                }

            }
            else
            {
                return builder;
            }
        }

        public static IRawBuilder Append(this IRawBuilder builder, bool condition, Func<string> sqlFunc, Func<SqlBuilderParameter[]> paramsFunc)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            if (condition)
            {
                return builder.Append(sqlFunc?.Invoke(), paramsFunc?.Invoke());
            }
            else
            {
                return builder;
            }
        }

        public static IRawBuilder Append(this IRawBuilder builder, bool condition, Func<string> sqlFunc, Func<object> paramsFunc)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            if (condition)
            {
                return builder.Append(sqlFunc?.Invoke(), paramsFunc?.Invoke());
            }
            else
            {
                return builder;
            }
        }
        #endregion
    }
}
