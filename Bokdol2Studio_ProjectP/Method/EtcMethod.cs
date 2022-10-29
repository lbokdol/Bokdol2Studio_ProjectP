using Bokdol2Studio_ProjectP.metaObject;
using Bokdol2Studio_ProjectP.EvaluationAlgorithm;
using MySqlConnector;

namespace Bokdol2Studio_ProjectP.Method
{
    public class EtcMethod
    {
        private string nexonAPIkey = "";
        private Querymanager _querymanager;

        public EtcMethod()
        {
            this._querymanager = new Method.Querymanager("api.nexon.co.kr");
            this.nexonAPIkey = setNexonAPIkey("nexonKey.txt");
        }

        public string getNexonAPIkey()
        {
            return this.nexonAPIkey;
        }

        public string setNexonAPIkey(string path)
        {
            return File.ReadAllText(path);
        }

        public Pass sumPassInformation(Pass dest, Pass source)
        {
            dest.longPassTry += source.longPassTry;
            dest.longPassSuccess += source.longPassSuccess;
            dest.shortPassSuccess += source.shortPassSuccess;
            dest.shortPassTry += source.shortPassTry;
            dest.passTry += source.passTry;
            dest.passSuccess += source.passSuccess;
            dest.bouncingLobPassSuccess += source.bouncingLobPassSuccess;
            dest.bouncingLobPassTry += source.bouncingLobPassTry;
            dest.lobbedThroughPassTry += source.lobbedThroughPassTry;
            dest.lobbedThroughPassSuccess += source.lobbedThroughPassSuccess;
            dest.throughPassTry += source.throughPassTry;
            dest.throughPassSuccess += source.throughPassSuccess;
            dest.drivenGroundPassTry += source.drivenGroundPassTry;
            dest.driventGroundPassSuccess += source.driventGroundPassSuccess;

            return dest;
        }

        public Maner sumManerInformation(Maner dest, MatchDetail source)
        {
            dest.foul += source.foul;
            dest.pause += source.systemPause;
            dest.yellowCard += source.yellowCards;
            dest.redCard += source.redCards;

            if (source.matchEndType == 2)
                dest.forcedExit++;

            return dest;
        }

        public Pass avgPassInformation(Pass dest, int count)
        {
            dest.longPassTry /= count;
            dest.longPassSuccess /= count;
            dest.shortPassSuccess /= count;
            dest.shortPassTry /= count;
            dest.passTry /= count;
            dest.passSuccess /= count;
            dest.bouncingLobPassSuccess /= count;
            dest.bouncingLobPassTry /= count;
            dest.lobbedThroughPassTry /= count;
            dest.lobbedThroughPassSuccess /= count;
            dest.throughPassTry /= count;
            dest.throughPassSuccess /= count;
            dest.drivenGroundPassTry /= count;
            dest.driventGroundPassSuccess /= count;

            return dest;
        }

        public Maner avgManerInformation(Maner dest, int count)
        {
            dest.foul /= count;
            dest.pause /= count;
            dest.yellowCard /= count;
            dest.redCard /= count;
            dest.forcedExit /= count;

            return dest;
        }
        public async Task<object> getUser_maxdivision(string accessid)
        {
            var response = await this._querymanager.getResponse(HttpMethod.Get, $"/fifaonline4/v1.0/users/{accessid}/maxdivision", this.nexonAPIkey);
            var settings = this._querymanager.getjsonsettings();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return await response.Content.ReadFromJsonAsync<fifa_user_division[]>(settings);
            else
                return await response.Content.ReadFromJsonAsync<fifa_error>(settings);

        }

        public async Task<object> getUser_match(string accessid, int matchtype, int offset = 0, int limit = 10)
        {
            var response = await this._querymanager.getResponse(HttpMethod.Get, $"/fifaonline4/v1.0/users/{accessid}/matches?matchtype={matchtype}&offset={offset}&limit={limit}", this.nexonAPIkey);
            var settings = this._querymanager.getjsonsettings();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return await response.Content.ReadFromJsonAsync<string[]>(settings);
            else
                return await response.Content.ReadFromJsonAsync<fifa_error>(settings);

        }

        public async Task<object> getMatchInfo(string matchid)
        {
            var response = await this._querymanager.getResponse(HttpMethod.Get, $"/fifaonline4/v1.0/matches/{matchid}", this.nexonAPIkey);
            var settings = this._querymanager.getjsonsettings();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return await response.Content.ReadFromJsonAsync<fifa_match>(settings);
            else
                return await response.Content.ReadFromJsonAsync<fifa_error>(settings);

        }

        public async Task<object> getUserInformation(fifa_user userinfo)
        {
            var division = getUser_maxdivision(userinfo.accessId);
            var match = await getUser_match(userinfo.accessId, 50);
            List<object> matches = new List<object>();

            if (match.GetType() == typeof(string[]))
            {
                foreach (string item in match as string[])
                {
                    var task = await getMatchInfo(item);
                    matches.Add(task);
                }
            }

            double maner = 0;
            List<int> passtype = new List<int>();
            List<GoalAssist[]> goalAssists = new List<GoalAssist[]>();
            fifa_user_Information resultInfo = new fifa_user_Information();
            resultInfo.passInfo = new Pass();
            resultInfo.manerInfo = new Maner();

            foreach (object item in matches)
            {
                if (item.GetType() == typeof(fifa_match))
                {
                    fifa_match obj = item as fifa_match;
                    maner += Evaluationmanager.calculateMannerRate(userinfo.accessId, obj);
                    passtype.Add(Evaluationmanager.calculatePasstype(userinfo.accessId, obj));
                    goalAssists.Add(Evaluationmanager.calculateGoalAssistPosition(userinfo.accessId, obj));

                    var userMatch = Array.Find(obj.matchInfo, x => x.accessId == userinfo.accessId);
                    resultInfo.passInfo = sumPassInformation(resultInfo.passInfo, userMatch.pass);
                    resultInfo.manerInfo = sumManerInformation(resultInfo.manerInfo, userMatch.matchDetail);
                }
                else
                {

                }
            }

            maner /= matches.Count;

            var duplicates = passtype.GroupBy(x => x)
           .Where(g => g.Count() > 1)
           .Select(y => new { Name = y.Key, Count = y.Count() })
           .ToList();

            var o = duplicates.Max(x => x.Count);
            var oo = duplicates.Find(x => x.Count == o);

            int division_int = 0;

            if (division.Result.GetType() == typeof(fifa_user_division[]))
            {
                var division_obj = division.Result as fifa_user_division[];
                division_int = division_obj[^1].division;
            }

            resultInfo.maner = maner * 20;
            resultInfo.nickname = userinfo.nickname;
            resultInfo.accessid = userinfo.accessId;
            resultInfo.division = division_int;
            resultInfo.passtype = oo.Name;
            resultInfo.passInfo = avgPassInformation(resultInfo.passInfo, matches.Count);
            resultInfo.manerInfo = avgManerInformation(resultInfo.manerInfo, matches.Count);
            resultInfo.goalAssists = goalAssists;
            resultInfo.date = DateTime.Now;

            

            return resultInfo;
        }
    }
}
