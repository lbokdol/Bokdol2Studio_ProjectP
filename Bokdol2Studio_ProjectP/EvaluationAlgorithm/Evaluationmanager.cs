using Bokdol2Studio_ProjectP.metaObject;

namespace Bokdol2Studio_ProjectP.EvaluationAlgorithm
{
    public class Evaluationmanager
    {
        public static double calculateMannerRate(string accessid, fifa_match matchinfo)
        {
            var userMatch = Array.Find(matchinfo.matchInfo, x => x.accessId == accessid);

            var matchDetail = userMatch.matchDetail;
            int endtype = 0;
            if(matchDetail.matchEndType == 2)
                endtype = 2;

            double result = 5 - ((matchDetail.foul * 0.1) + (matchDetail.yellowCards * 0.25) + (matchDetail.redCards * 0.5) + (matchDetail.systemPause * 0.1) + endtype);

            return result;
        }

        public static int calculatePlaytype(string accessid, fifa_match matchinfo)
        {
            var userMatch = Array.Find(matchinfo.matchInfo, x => x.accessId == accessid);

            return 0;
        }

        public static int calculatePasstype(string accessid, fifa_match matchinfo)
        {
            var userMatch = Array.Find(matchinfo.matchInfo, x => x.accessId == accessid);

            var passInfo = userMatch.pass;

            double shortpass = (passInfo.shortPassSuccess+1.0) / (passInfo.shortPassTry + 1.0);
            double longpass = (passInfo.longPassSuccess+1.0) / (passInfo.longPassTry + 1.0);

            if (shortpass > longpass)
                return 0;
            else if (shortpass < longpass)
                return 1;
            else
                return 2;
        }

        public static GoalAssist[] calculateGoalAssistPosition(string accessid, fifa_match matchinfo)
        {
            var userMatch = Array.Find(matchinfo.matchInfo, x => x.accessId == accessid);
            var detail = userMatch.shootDetail;

            if (detail != null)
            {
                List<GoalAssist> goalAssist = new List<GoalAssist>();

                foreach (var item in detail)
                {
                    
                    if (item.result == 3)
                    {
                        goalAssist.Add(new GoalAssist() { goal = new XY() { x = item.x, y = item.y }, assist = new XY() { x = item.assistX, y = item.assistY } });
                    }
                }

                return goalAssist.ToArray();
            }
            else
            {
                return null;
            }
        }
    }
}
