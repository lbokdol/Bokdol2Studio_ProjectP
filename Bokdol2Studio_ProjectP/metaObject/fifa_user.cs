namespace Bokdol2Studio_ProjectP.metaObject
{
    public class fifa_user
    {
        public string accessId { get; set; }
        public string nickname { get; set; }
        public int level { get; set; }
    }

    public class fifa_user_division
    {
        public int matchType { get; set; }
        public int division { get; set; }
        public string achievementDate { get; set; }
    }

    public class fifa_user_Information
    {
        public string accessid { get; set; }
        public string nickname { get; set; }
        public int division { get; set; }
        public double maner { get; set; }
        public int passtype { get; set; }
        public List<GoalAssist[]> goalAssists { get; set; }
        public Pass passInfo { get; set; }

        public Maner manerInfo { get; set; }
        public DateTime date { get; set; }

    }

    public class fifa_user_trade
    {
        public string tradeDate { get; set; }
        public string saleSn { get; set; }
        public int spid { get; set; }
        public int grade { get; set; }
        public int value { get; set; }
    }
}
