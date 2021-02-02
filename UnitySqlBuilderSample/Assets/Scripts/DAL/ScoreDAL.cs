using Harry.SqlBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harry.SqlBuilder.SQLite;
using Mono.Data.Sqlite;

namespace ns
{
    public class ScoreDAL
    {
        private IDbConnectionFactory connFactory;
        private ISqlBuilderFactory builderFactory;
        public ScoreDAL(IDbConnectionFactory connFactory, ISqlBuilderFactory builderFactory)
        {
            this.connFactory = connFactory;
            this.builderFactory = builderFactory;
        }

        /// <summary>
        /// 获取ID
        /// </summary>
        /// <returns></returns>
        public int GetMaxID()
        {
            //获取可用ID
            var cmd = builderFactory.Select("Max(id)").From("Scores").ToCommand();

            var oldID = cmd.ExecuteScalar(connFactory);
            return oldID == null || oldID.ToString() == "" ? 1 : Convert.ToInt32(oldID) + 1;
        }

        public int Add(ScoreModel model)
        {
            //插入数据
            return builderFactory.Insert("Scores")
                .Column("ID", model.ID)
                .Column("LevelName", model.LevelName)
                .Column("Score", model.Score)
                .ToCommand()
                .ExecuteNonQuery(connFactory);
        }

        
        public int Update(ScoreModel model)
        {
            return builderFactory.Update("Scores")
                .Column("LevelName", model.LevelName)
                .Column("Score", model.Score)
                .Where("id=@id", new SqlBuilderParameter("id", model.ID))
                .ToCommand()
                .ExecuteNonQuery(connFactory);
        }

        public int DeleteAll()
        {
            return builderFactory
                .Delete("Scores")
                .Where("1=1")
                .ToCommand()
                .ExecuteNonQuery(connFactory);
        }

        public List<ScoreModel> GetList(int page, int size)
        {
            List<ScoreModel> results = new List<ScoreModel>();
            return builderFactory
                //.Raw().Append("select * from Scores limit 3,2").ToCommand()
                .Select("*")
                .From("Scores")
                .ToCommand(page, size)
                .ExecuteReader(connFactory, reader =>
                {
                    while (reader.Read())
                    {
                        ScoreModel model = new ScoreModel();
                        model.ID = reader.GetInt32(0);
                        model.LevelName = reader.GetString(1);
                        model.Score = reader.GetInt32(2);
                        results.Add(model);
                    }
                    return results;
                });
        }
    }
}
