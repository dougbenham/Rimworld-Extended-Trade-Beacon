using System.Collections.Generic;
using System.Linq;
using Harmony;
using HugsLib;
using RimWorld;
using Verse;

namespace ExtendedTradeBeacon
{
    public class Main : ModBase
    {
        public override string ModIdentifier => "ExtendedTradeBeacon";
    }

    [HarmonyPatch(typeof(Building_OrbitalTradeBeacon), "TradeableCellsAround")]
    public static class Building_OrbitalTradeBeacon_TradeableCellsAround_Patch
    {
        [HarmonyPrefix]
        public static bool TradeableCellsAround(ref List<IntVec3> __result, IntVec3 pos, Map map)
        {
            if (!pos.InBounds(map))
            {
                __result = new List<IntVec3>();
                return false;
            }
            Region region = pos.GetRegion(map);
            if (region == null)
            {
                __result = new List<IntVec3>();
                return false;
            }

            __result = new List<IntVec3>();
            var result = __result;
            RegionTraverser.BreadthFirstTraverse(region, (from, r) => r.door == null, delegate (Region r)
            {
                result.AddRange(r.Cells.Where(item => item.InHorDistOf(pos, 30f)));
                return false;
            }, 40);
            return false;
        }
    }
}
