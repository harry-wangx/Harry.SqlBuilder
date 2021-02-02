/************************************************************
作者:
版本:
说明:
更新日期:
************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Harry.SqlBuilder;
using Harry.SqlBuilder.SQLite;
using System.Data;
using System.Text;
using Mono.Data.Sqlite;
using System.Linq;

namespace ns
{
    public class SQLiteSqlBuilderTestScript : MonoBehaviour
    {
        ISqlBuilderFactory builderFactory = new SQLiteBuilderFactory();
        ScoreDAL dal = null;
        private const string dbName = "data.db"; // 数据库名称
        string dbPath = "";
        private Text info;

        string str = "";
        private void Start()
        {
            GameObject.Find("InsertButton").GetComponent<Button>().onClick.AddListener(OnInsertButtonClick);
            GameObject.Find("UpdateButton").GetComponent<Button>().onClick.AddListener(OnUpdateButtonClick);
            GameObject.Find("DeleteButton").GetComponent<Button>().onClick.AddListener(OnDeleteButtonClick);
            GameObject.Find("SelectButton").GetComponent<Button>().onClick.AddListener(OnSelectButtonClick);
            GameObject.Find("ExceptionButton").GetComponent<Button>().onClick.AddListener(OnExceptionButtonClick);

            info = GameObject.Find("Info").GetComponent<Text>();

            init();
        }

        private void init()
        {

            if (Application.platform == RuntimePlatform.WindowsEditor
    || Application.platform == RuntimePlatform.WindowsPlayer) // win 平台
                dbPath = Application.streamingAssetsPath + "/" + dbName;
            else if (Application.platform == RuntimePlatform.Android) //  android平台
            {
                dbPath = Application.persistentDataPath + "/" + dbName;
                addAndShowInfo("数据库是否存在:" + File.Exists(dbPath));
                if (!File.Exists(dbPath)) // 找数据库
                    StartCoroutine(CopyDB(dbPath));  // 如果没有,复制一份过来
            }

            dbPath = "URI=file:" + dbPath;
            dal = new ScoreDAL(new SQLiteConnectionFactory(dbPath), new SQLiteBuilderFactory());

            addAndShowInfo("平台:" + Application.platform.ToString());
            addAndShowInfo("连接字符串:" + dbPath);
        }

        /// <summary>
        /// 复制数据库文件到persistentDataPath下面
        /// </summary>
        /// <returns></returns>
        IEnumerator CopyDB(string targetPath)
        {
            addAndShowInfo("准备拷贝数据库");
            WWW www = new WWW(Application.streamingAssetsPath + "/" + dbName); // 从streamingAssets目录下载
            yield return www; // 等待下载完成
            File.WriteAllBytes(targetPath, www.bytes); //  下载完成后把文件写到persistentDataPath里面
            addAndShowInfo("拷贝数据库成功");

        }

        /// <summary>
        /// 引发异常
        /// </summary>
        private void OnExceptionButtonClick()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 插入数据
        /// </summary>
        private void OnInsertButtonClick()
        {
            showInfo("准备插入数据");
            try
            {
                ScoreModel model = new ScoreModel();
                model.ID = dal.GetMaxID();
                model.LevelName = "LevelName_" + model.ID.ToString();
                model.Score = DateTime.Now.Second + 40;
                var result = dal.Add(model);
                showInfo(string.Format("插入数据条数:" + result.ToString()));
            }
            catch (Exception ex)
            {
                showInfo(ex.Message);
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        private void OnUpdateButtonClick()
        {
            //获取第一条记录
            var model = dal.GetList(1, 1).FirstOrDefault();
            if (model==null)
            {
                showInfo("请先插入数据");
                return;
            }

            model.Score = DateTime.Now.Second + 40;

            var result = dal.Update(model);

            showInfo(string.Format("更新数据条数:" + result.ToString()));
        }

        /// <summary>
        /// 删除全部数据
        /// </summary>
        private void OnDeleteButtonClick()
        {
            var result = dal.DeleteAll();

            showInfo(string.Format("删除数据条数:" + result.ToString()));
        }


        /// <summary>
        /// 查询数据
        /// </summary>
        private void OnSelectButtonClick()
        {
            var results = dal.GetList(1, 20);
            if (results == null || results.Count <=0)
            {
                showInfo("未查到数据,请先添加");
                return;
            }
            
            StringBuilder sb = new StringBuilder("查询结果:" + Environment.NewLine);

            results.ForEach(m=> {
                sb.AppendFormat("ID:{0}, LevelName:{1}, Score:{2} {3}",
                m.ID.ToString(),
                m.LevelName,
                m.Score.ToString(),
                Environment.NewLine
                );
            });

            showInfo(sb.ToString());
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }


        private void showInfo(string msg)
        {
            this.info.text = msg;
        }

        private void addAndShowInfo(string msg)
        {
            str += msg + Environment.NewLine;
            this.info.text = str;
        }
    }
}