using System;
using System.Linq;
using System.Threading.Tasks;

namespace YFFL.Server.GameDataService.Domain
{
    public enum PassingStat
    {
        ATT, COMP, YDS, INT, TD
    }

    public enum RushingStat
    {
        CAR, YDS, TD
    }

    public enum ReceivingStat
    {
       TGT, REC, YDS, TD
    }

    public enum KickingStat
    {
        FGM, FGA, XPM, XPA
    }

    public enum KickReturnsStat
    {
        TD
    }

    public enum NFLPosition { NA, QB, RB, WR, TE, PK }

}
