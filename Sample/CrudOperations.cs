using Dapper;
using Harry.SqlBuilder;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Sample
{
    internal sealed class CrudOperations
    {
        private readonly ISqlBuilder builder;
        private readonly IDbConnection cnn;
        public CrudOperations(IDbConnection cnn, ISqlBuilder builder)
        {
            this.builder = builder;
            this.cnn = cnn;
        }


        public int Insert(UserModel model)
        {
            var cmd = builder.Insert("users")
                .Column("ID", model.ID)
                .Column("UserName", model.UserName)
                .Column("Password", model.Password)
                //access中,日期格式需要格式化成标准日期字符串
                .Column(model.JoinTime != null, () => new SqlBuilderParameter("JoinTime", model.JoinTime.Value.ToString("yyyy-MM-dd HH:mm:ss")))
                //access中,布尔值需要指定数据类型
                .Column("IsAdmin", model.IsAdmin, DbType.Boolean)
                .ToCommand();

            return cnn.Execute(cmd);
        }
        public int Update(UserModel model)
        {
            var cmd = builder.Update("users")
                .Column("UserName", model.UserName)
                .Column("Password", model.Password)
                //access中,日期格式需要格式化成标准日期字符串
                .Column(model.JoinTime != null, () => new SqlBuilderParameter("JoinTime", model.JoinTime.Value.ToString("yyyy-MM-dd HH:mm:ss")))
                //access中,布尔值需要指定数据类型
                .Column("IsAdmin", model.IsAdmin, DbType.Boolean)
                .Where("ID=@ID", new SqlBuilderParameter("ID", model.ID))
                .ToCommand();

            return cnn.Execute(cmd);
        }

        public List<UserModel> Query(bool? isAdmin = null)
        {
            var cmd = builder.Select("*").From("users")
                .Where(isAdmin != null, "IsAdmin=@IsAdmin", () => new SqlBuilderParameter("IsAdmin", isAdmin, DbType.Boolean))
                .ToCommand();

            return cnn.Query<UserModel>(cmd)
                .ToList();
        }

        public List<UserModel> Query(int top, bool? isAdmin = null)
        {
            var cmd = builder.Select("*").From("users")
              .Where(isAdmin != null, "IsAdmin=@IsAdmin", () => new SqlBuilderParameter("IsAdmin", isAdmin, DbType.Boolean))
              .OrderBy("ID")
              .ToCommand(top);

            return cnn.Query<UserModel>(cmd)
              .ToList();
        }

        public List<UserModel> Query(int page, int size, bool? isAdmin = null)
        {
            var cmd = builder.Select("*").From("users")
              .Where(isAdmin != null, "IsAdmin=@IsAdmin", () => new SqlBuilderParameter("IsAdmin", isAdmin, DbType.Boolean))
              .OrderBy("ID")
              .ToCommand(page, size);

            return cnn.Query<UserModel>(cmd)
             .ToList();
        }

        public UserModel Get(int id)
        {
            var cmd = builder.Select("*").From("users")
                .Where("ID=@id", new SqlBuilderParameter("id", id))
                .ToCommand();

            return cnn.Query<UserModel>(cmd).FirstOrDefault();
        }

        public int Delete(bool? isAdmin = null)
        {
            var cmd = builder.Delete("users")
                .Where("1=1") //删除和更新操作,必须保证至少有一个where条件
                .Where(isAdmin != null, "IsAdmin=@IsAdmin", () => new SqlBuilderParameter("IsAdmin", isAdmin, DbType.Boolean))
                .ToCommand();

            return cnn.Execute(cmd);
        }


        public void Test()
        {
            UserModel admin = new UserModel() { ID = 1, IsAdmin = true, UserName = "admin", Password = "admin" };
            UserModel user1 = new UserModel() { ID = 2, IsAdmin = false, UserName = "user1", Password = "password", JoinTime = DateTime.Now };

            UserModel temp;
            List<UserModel> aryTemp;
            //测试插入
            Console.WriteLine($"Insert 'admin' is {(Insert(admin) == 1 ? "OK" : "fail")}");
            Console.WriteLine($"Insert 'user1' is {(Insert(user1) == 1 ? "OK" : "fail")}");
            //测试查询
            Console.WriteLine($"Query 'all' is {(Query().Count == 2 ? "OK" : "fail")}");
            //测试带条件查询
            aryTemp = Query(true);
            Console.WriteLine($"Query 'admin' is {(aryTemp.Count == 1 && aryTemp[0].ID == admin.ID ? "OK" : "fail")}");
            //测试top查询
            temp = Query(1).FirstOrDefault();
            Console.WriteLine($"Query 'top' is {(temp != null && temp.ID == admin.ID ? "OK" : "fail")}");
            //测试带条件top查询
            temp = Query(1, true).FirstOrDefault();
            Console.WriteLine($"Query 'top' is {(temp != null && temp.ID == admin.ID ? "OK" : "fail")}");
            //测试分页
            aryTemp = Query(1, 1);
            Console.WriteLine($"Query 'PageList' is {(aryTemp != null && aryTemp.Count == 1 && aryTemp[0].ID == admin.ID ? "OK" : "fail")}");
            aryTemp = Query(2, 1);
            Console.WriteLine($"Query 'PageList' is {(aryTemp != null && aryTemp.Count == 1 && aryTemp[0].ID == user1.ID ? "OK" : "fail")}");
            //测试带条件分页
            aryTemp = Query(1, 1, false);
            Console.WriteLine($"Query 'PageList' is {(aryTemp != null && aryTemp.Count == 1 && aryTemp[0].ID == user1.ID ? "OK" : "fail")}");
            //测试更新
            user1.Password = "newpassword";
            Console.WriteLine($"Update is {(Update(user1) == 1 && Get(user1.ID).Password == user1.Password ? "OK" : "fail")}");

            //测试删除
            Console.WriteLine($"Delete is {(Delete(false) == 1 && (temp = Query().FirstOrDefault()) != null && temp.ID == admin.ID ? "OK" : "fail")}");
            Console.WriteLine($"Delete is {(Delete(true) == 1 ? "OK" : "fail")}");

            Console.WriteLine("测试结束");
        }
    }
}
