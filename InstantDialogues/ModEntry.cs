using HarmonyLib;
using StardewModdingAPI;

namespace InstantDialogues
{
    internal sealed class ModEntry : Mod
    {
        internal static IMonitor ModMonitor { get; private set; } = null!;
        
        public override void Entry(IModHelper helper)
        {
            ModMonitor = this.Monitor;
            
            var harmony = new Harmony(this.ModManifest.UniqueID);
            harmony.Patch(
                original: AccessTools.Method(typeof(StardewValley.Menus.DialogueBox), nameof(StardewValley.Menus.DialogueBox.update)),
                prefix: new HarmonyMethod(typeof(DialogueBoxPatches), nameof(DialogueBoxPatches.update_Prefix))
            );
        }
    }
    
    internal class DialogueBoxPatches
    {
        internal static bool update_Prefix(StardewValley.Menus.DialogueBox __instance)
        {
            try
            {
                __instance.showTyping = false;
                __instance.safetyTimer = 0;
                return true;
            }
            catch (Exception ex)
            {
                ModEntry.ModMonitor.Log($"Failed in {nameof(update_Prefix)}:\n{ex}", LogLevel.Error);
                return true;
            }
        }
    }
}