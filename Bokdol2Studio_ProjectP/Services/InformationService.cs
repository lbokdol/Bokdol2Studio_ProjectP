using System.Net.Http;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Bokdol2Studio_ProjectP.metaObject;
using MySqlConnector;

namespace Bokdol2Studio_ProjectP.Services
{
    public class InformationService
    {
        
        private Method.Querymanager _querymanager;
        private EvaluationAlgorithm.Evaluationmanager _evaluationmanager = new EvaluationAlgorithm.Evaluationmanager();
        private Method.EtcMethod _etcMethod = new Method.EtcMethod();
        private MySqlConnection _connection;

        public InformationService()
        {
            this._querymanager = new Method.Querymanager("api.nexon.co.kr");
            Server_Settings settings = setSettings("Settings.json");
            this._connection = this._querymanager.ConnectSQL(settings.sql_ip, settings.sql_database, settings.sql_name, settings.sql_password);
        }

        public async Task<object> getUser(string username)
        {
            var response = await this._querymanager.getResponse(HttpMethod.Get, $"/fifaonline4/v1.0/users?nickname={username}", this._etcMethod.getNexonAPIkey());
            var settings = this._querymanager.getjsonsettings();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return await response.Content.ReadFromJsonAsync<fifa_user>(settings);
            else
                return await response.Content.ReadFromJsonAsync<fifa_error>(settings);
        }

        public Server_Settings setSettings(string path)
        {
            if(File.Exists(path))
            {
                Stream SettingsStream = File.OpenRead(path);

                return JsonSerializer.Deserialize<Server_Settings>(SettingsStream);
            }
            else
            {
                Server_Settings settings = new Server_Settings() { sql_ip="localhost", sql_name="root", sql_password="1234", sql_database="default"};

                File.WriteAllText("Settings.json", JsonSerializer.Serialize(settings));

                return settings;
            }
        }

        public async Task<object> setUserInfo_DB(string username)
        {
            //넥슨에서 정보를 받아 DB에 저장해야함 모두 완료 후 데이터 가공해서 리턴
            var user = await getUser(username);

            if(user.GetType() == typeof(fifa_user))
            {
                fifa_user userinfo = user as fifa_user;
                var task = this._etcMethod.getUserInformation(userinfo);
                this._querymanager.insertData(this._connection, userinfo.accessId, JsonSerializer.Serialize(task.Result, this._querymanager.getjsonsettings()), DateTime.Now);

                return task.Result;
            }
            else
            {
                return user;
            }
        }

        public async Task<object> getUserInfo_DB(string username)
        {
            var user = await getUser(username);
            if (user.GetType() == typeof(fifa_user))
            {
                fifa_user userinfo = user as fifa_user;
                var value = this._querymanager.getData(this._connection, userinfo.accessId);

                return value;
            }

            return null;
        }


    }
}
