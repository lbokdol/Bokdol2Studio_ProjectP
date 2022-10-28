namespace Bokdol2Studio_ProjectP.metaObject
{
    public class fifa_match
    {
        public string matchId { get; set; }
        public string matchDate { get; set; }
        public int matchType { get; set; }
        public MatchInfo[] matchInfo { get; set; }
    }

    public class MatchInfo
    {
        public string accessId { get; set; }
        public string nickname { get; set; }
        public MatchDetail matchDetail { get; set; }
        public Shoot shoot { get; set; }
        public ShootDetail[] shootDetail { get; set; }
        public Pass pass { get; set; }
        public Defence defence { get; set; }
        public Player[] player { get; set; }
    }

    public class MatchDetail
    {
        public int seasonId { get; set; }
        public string matchResult { get; set; }
        public int matchEndType { get;set; }
        public int systemPause { get; set; }
        public int foul { get; set; }
        public int redCards { get; set; }
        public int yellowCards { get; set; }
        public int dribble { get; set; }
        public int cornerKick { get; set; }
        public int possession { get; set; }
        public int OffsideCount { get; set; }
        public double averageRating { get; set; }
        public string controller { get; set; }
    }

    public class Shoot
    {
        public int shootTotal { get; set; }
        public int effectiveShootTotal { get; set; }
        public int shootOutScore { get; set; }
        public int goalTotal { get; set; }
        public int goalTotalDisplay { get; set; }
        public int ownGoal { get; set; }
        public int shootHeading { get; set; }
        public int goalHeading { get; set; }
        public int shootFreekick { get; set; }
        public int goalFreekick { get; set; }
        public int shootInPenalty { get; set; }
        public int goalInPenalty { get; set; }
        public int shootOutPenalty { get; set; }
        public int goalOutPenalty { get; set; }
        public int shootPenaltyKick { get; set; }
        public int goalPenaltyKick { get; set; }
    }

    public class ShootDetail
    {
        public int goalTime { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public int type { get; set; }
        public int result { get; set; }
        public int spId { get; set; }
        public int spGrade { get; set; }
        public int spLevel { get; set; }
        public bool spIdType { get; set; }
        public bool assist { get; set; }
        public int assistSpId { get; set; }
        public double assistX { get; set; }
        public double assistY { get; set; }
        public bool hitPost { get; set; }
        public bool inPenalty { get; set; }
    }

    public class Pass
    {
        public int passTry { get; set; }
        public int passSuccess { get; set; }
        public int shortPassTry { get; set; }
        public int shortPassSuccess { get; set; }
        public int longPassTry { get; set; }
        public int longPassSuccess { get; set; }
        public int bouncingLobPassTry { get; set; }
        public int bouncingLobPassSuccess { get; set; }
        public int drivenGroundPassTry { get; set; }
        public int driventGroundPassSuccess { get; set; }
        public int throughPassTry { get; set; }
        public int throughPassSuccess { get; set; }
        public int lobbedThroughPassTry { get; set; }
        public int lobbedThroughPassSuccess { get; set; }

    }

    public class Defence
    {
        public int blockTry { get; set; }
        public int blockSuccess { get; set; }
        public int tackleTry { get; set; }
        public int tackleSuccess { get; set; }
    }

    public class Player
    {
        public int spId { get; set; }
        public int spPosition { get; set; }
        public int spGrade { get; set; }
        public Status status { get; set; }
    }

    public class Status
    {
        public int shoot { get; set; }
        public int effectiveShoot { get; set; }
        public int assist { get; set; }
        public int goal { get; set; }
        public int dribble { get; set; }
        public int intercept { get; set; }
        public int defending { get; set; }
        public int passTry { get; set; }
        public int passSuccess { get; set; }
        public int dribbleTry { get; set; }
        public int dribbleSuccess { get; set; }
        public int ballPossesionTry { get; set; }
        public int ballPossesionSuccess { get; set; }
        public int aerialTry { get; set; }
        public int aerialSuccess { get; set; }
        public int blockTry { get; set; }
        public int block { get; set; }
        public int tackleTry { get; set; }
        public int tackle { get; set; }
        public int yellowCards { get; set; }
        public int redCards { get; set; }
        public float spRating { get; set; }
    }

    public class XY
    {
        public double x { get; set; }
        public double y { get; set; }
    }

    public class GoalAssist
    {
        public XY goal { get; set; }
        public XY assist { get; set; }
    }

    public class Maner
    {
        public int foul { get; set; }
        public int yellowCard { get; set; }
        public int redCard { get; set; }
        public int pause { get; set; }
        public int forcedExit { get; set; }
    }

}
