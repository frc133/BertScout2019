﻿using Common.JSON;

namespace BertScout2019.Models
{
    public partial class EventTeamMatch
    {
        // ToJson and ToString routines

        public JObject ToJson()
        {
            JObject result = new JObject()
            {
                { "Id", Id },
                { "EventKey", EventKey },
                { "TeamNumber", TeamNumber },
                { "MatchNumber", MatchNumber },
                { "SandstormMoveType", SandstormMoveType },
                { "SandstormOffPlatform", SandstormOffPlatform },
                { "SandstormHatches", SandstormHatches },
                { "SandstormCargo", SandstormCargo },
                { "CargoShipHatches", CargoShipHatches },
                { "CargoShipCargo", CargoShipCargo },
                { "RocketHatches", RocketHatches },
                { "RocketCargo", RocketCargo },
                { "RocketHighestHatch", RocketHighestHatch },
                { "RocketHighestCargo", RocketHighestCargo },
                { "EndgamePlatform", EndgamePlatform },
                { "EndgameBuddyClimb", EndgameBuddyClimb },
                { "Defense", Defense },
                { "Cooperation", Cooperation },
                { "Fouls", Fouls },
                { "Broken", Broken },
                { "AllianceResult", AllianceResult },
                { "RocketRankingPoint", RocketRankingPoint },
                { "HabRankingPoint", HabRankingPoint },
                { "ScouterName", ScouterName },
                { "Comments", Comments },
            };
            return result;
        }

        public override string ToString()
        {
            return ToJson().ToString();
        }
    }
}