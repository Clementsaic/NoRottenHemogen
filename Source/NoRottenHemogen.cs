using HarmonyLib;
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace NoRottenHemogen {
    public class NoRottenHemogen : Mod {
        public NoRottenHemogen(ModContentPack content) : base(content) {
            var harmony = new Harmony("Clementsaic.NoRottenHemogen");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(Recipe_ExtractHemogen), nameof(Recipe_ExtractHemogen.AvailableReport))]
    public static class NoBloodRotReportPatch {
        public static void Postfix(ref AcceptanceReport __result, Thing thing) {
            if (!__result.Accepted) return;
            var pawn = thing as Pawn;
            if (pawn!.health.hediffSet.HasHediff(CLE_HediffDefOf.BloodRot)) {
                __result = "CLE_RottenHemogen".Translate();
            }
        }
    }

    [HarmonyPatch(typeof(Recipe_ExtractHemogen), nameof(Recipe_ExtractHemogen.CompletableEver))]
    public static class NoBloodRotCompletablePatch {
        public static void Postfix(ref bool __result, Pawn surgeryTarget) {
            if (!__result) return;
            if (surgeryTarget.health.hediffSet.HasHediff(CLE_HediffDefOf.BloodRot)) {
                __result = false;
            }
        }
    }

    [DefOf]
    public static class CLE_HediffDefOf {
        [MayRequireRoyalty] [UsedImplicitly]
        public static HediffDef BloodRot;

        static CLE_HediffDefOf() {
            DefOfHelper.EnsureInitializedInCtor(typeof(CLE_HediffDefOf));
        }
    }
}