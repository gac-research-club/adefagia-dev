using System.Collections.Generic;
using Adefagia.RobotSystem;

namespace Adefagia.BattleMechanism
{
    [System.Serializable]
    public class Team
    {
        public string teamName;

        public Team(string teamName)
        {
            this.teamName = teamName;
        }
    }
}