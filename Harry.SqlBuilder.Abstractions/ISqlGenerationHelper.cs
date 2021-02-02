using System;
using System.Collections.Generic;
using System.Text;

namespace Harry.SqlBuilder
{
    public interface ISqlGenerationHelper
    {
        /// <summary>
        /// SQL语句分隔符
        /// </summary>
        string StatementTerminator { get; }

        /// <summary>
        /// 批量SQL语句的分隔符
        /// </summary>
        string BatchTerminator { get; }

        /// <summary>
        /// 为给定的名称生成有效的参数名
        /// </summary>
        string GenerateParameterName(string name);

        /// <summary>
        /// 将有效的参数名写入StringBuilder
        /// </summary>
        void GenerateParameterName(StringBuilder builder, string name);

        /// <summary>
        /// 为给定的名称生成有效的参数占位符名称
        /// </summary>
        string GenerateParameterNamePlaceholder(string name);

        /// <summary>
        /// 将有效的参数占位符名称写入StringBuilder
        /// </summary>
        void GenerateParameterNamePlaceholder(StringBuilder builder, string name);

        /// <summary>
        /// 生成标准标识符
        /// </summary>
        string DelimitIdentifier(string identifier);

        /// <summary>
        /// 将生成的标准标识符写入StringBuilder
        /// </summary>
        void DelimitIdentifier(StringBuilder builder, string identifier);

        /// <summary>
        /// 生成带分隔符的标识符
        /// </summary>
        string DelimitIdentifier(string name, string schema);

        /// <summary>
        /// 将生成的带分隔符的标识符写入StringBuilder
        /// </summary>
        void DelimitIdentifier(StringBuilder builder, string name, string schema);

        /// <summary>
        /// 生成分页脚本
        /// </summary>
        /// <returns></returns>
        string GeneratePaginationSql();

        void GeneratePaginationSql(StringBuilder builder);
    }
}
