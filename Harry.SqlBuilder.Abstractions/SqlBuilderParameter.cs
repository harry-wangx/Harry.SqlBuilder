using System;
using System.Data;
using System.Runtime.InteropServices;

namespace Harry.SqlBuilder
{
    [StructLayout(LayoutKind.Auto)]
    public struct SqlBuilderParameter
    {
        public SqlBuilderParameter(string name, object value, DbType? dbType = default(DbType?), ParameterDirection? direction = default(ParameterDirection?), int? size = default(int?))
        {
            this.Name = name != null ? name.Trim() : throw new ArgumentException($"{nameof(name)}不能为空", nameof(name));
            this.Value = value;// ?? throw new ArgumentException($"{nameof(value)}不能为空", nameof(value));
            DbType = dbType;
            Direction = direction;
            Size = size;
        }
        /// <summary>
        /// 获取参数Name值
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 获取参数值
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public DbType? DbType { get; private set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public ParameterDirection? Direction { get; private set; }

        public int? Size { get; private set; }

        public override int GetHashCode()
        {
            int code = 0;
            unchecked
            {
                code = Name.GetHashCode() ^ Value.GetHashCode();
                if (DbType != null)
                    code ^= DbType.GetHashCode();
                if (Direction != null)
                    code ^= Direction.GetHashCode();
                if (Size != null)
                    code ^= Size.GetHashCode();
            }
            return code;
        }

        public bool Equals(SqlBuilderParameter obj)
        {
            return GetHashCode() == obj.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            //struct 不适用于下面的代码
            //if (ReferenceEquals(this, obj)) return true;

            if (typeof(SqlBuilderParameter) != obj.GetType()) return false;

            return GetHashCode() == ((SqlBuilderParameter)obj).GetHashCode();
        }
    }
}
