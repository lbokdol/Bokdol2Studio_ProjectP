using System.Net.Http.Headers;
using System.Text.Json;
using MySqlConnector;

namespace Bokdol2Studio_ProjectP.Method
{
    public class Querymanager
    {
        private HttpClient _httpClient;

        public Querymanager(string uri)
        {
            this._httpClient = setClient(uri);
        }

        private HttpClient setClient(string uri)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            HttpClient httpClient = new HttpClient(clientHandler);

            httpClient.BaseAddress = new Uri(string.Format($"https://{uri}"));
            httpClient.Timeout = new TimeSpan(0, 0, 10);
            httpClient.MaxResponseContentBufferSize = 2000000000L;
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
        }

        public HttpClient getClient()
        {
            if (this._httpClient != null)
                return this._httpClient;
            else
                return null;
        }

        public async Task<HttpResponseMessage> getResponse(HttpMethod type, string uri, string apikey)
        {
            var httpClient = getClient();

            var request = new HttpRequestMessage(type, uri);
            request.Headers.Add("Authorization", apikey);

            return await httpClient.SendAsync(request);
        }

        public JsonSerializerOptions getjsonsettings()
        {
            System.Text.Encodings.Web.TextEncoderSettings encoderSettings = new();
            encoderSettings.AllowRange(System.Text.Unicode.UnicodeRanges.All);

            JsonSerializerOptions settings = new()
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(encoderSettings),
                AllowTrailingCommas = true
            };

            return settings;
        }

        public MySqlConnection ConnectSQL(string ip, string database, string user, string password)
        {
            string connectionString = $"server={ip};user={user};database={database};password={password}";
            var connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
                return connection;
            }
            catch
            {
                return null;
            }
        }

        public void insertData(MySqlConnection connection, string accessid, string information, DateTime date)
        {
            string sql = $"INSERT INTO fifa_user (accessid, information, date) VALUES ('{accessid}','{information}','{date.ToString("yyyy-MM-dd HH:mm:ss")}') ON DUPLICATE KEY UPDATE information='{information}', date='{date.ToString("yyyy-MM-dd HH:mm:ss")}';";
            var mysqlCommand = new MySqlCommand(sql, connection);
            var mysqldataReader = mysqlCommand.ExecuteReader();
            mysqldataReader.Close();
        }

        public metaObject.fifa_user_Information getData(MySqlConnection connection, string accessid)
        {
            string sql = $"SELECT * FROM fifa_user WHERE accessid='{accessid}'";
            MySqlDataAdapter adpt = new MySqlDataAdapter(sql, connection);
            
            System.Data.DataSet ds = new System.Data.DataSet();
            adpt.Fill(ds);

            var returnVal = ds.Tables[0].Rows[0]["information"];

            return JsonSerializer.Deserialize<metaObject.fifa_user_Information>(returnVal as string);
        }
    }
}
