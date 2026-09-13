global using Il2CppInterop.Runtime;
global using Il2CppInterop.Runtime.Attributes;
global using Il2CppInterop.Runtime.Injection;
global using Il2CppInterop.Runtime.InteropTypes;
global using Il2CppInterop.Runtime.InteropTypes.Arrays;
global using TheOtherRoles.Roles;
global using TheOtherRoles.Roles.Crewmate;
global using TheOtherRoles.Roles.Impostor;
global using TheOtherRoles.Roles.Modifier;
global using TheOtherRoles.Roles.Neutral;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AmongUs.Data;
using AmongUs.Data.Player;
using AmongUs.GameOptions;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Hazel;
using InnerNet;
using TheOtherRoles.Modules;
using TheOtherRoles.Modules.CustomHats;
using TheOtherRoles.Objects;
using TheOtherRoles.Patches;
using TheOtherRoles.Utilities;
using TMPro;
using UnityEngine;
using static UnityEngine.UI.Button;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace TheOtherRoles;

[BepInPlugin(Id, "The Other Roles Reactivated", VersionString)]
[BepInDependency(SubmergedCompatibility.SUBMERGED_GUID, BepInDependency.DependencyFlags.SoftDependency)]
[BepInProcess("Among Us.exe")]
public class TheOtherRolesPlugin : BasePlugin
{
    public const string Id = "me.eisbison.theotherroles";
    public const string VersionString = "5.0.0";
    public static bool isBeta = true;

    public static Version Version = Version.Parse(VersionString);
    internal static ManualLogSource Logger;
    public static TheOtherRolesPlugin Instance;

    public static int optionsPage = 2;

    public static Sprite ModStamp;

    public Harmony Harmony { get; } = new(Id);

    public static ConfigEntry<bool> DebugMode { get; private set; }
    public static ConfigEntry<bool> GhostsSeeInformation { get; set; }
    public static ConfigEntry<bool> GhostsSeeRoles { get; set; }
    public static ConfigEntry<bool> GhostsSeeModifier { get; set; }
    public static ConfigEntry<bool> GhostsSeeVotes { get; set; }
    public static ConfigEntry<bool> ShowRoleSummary { get; set; }
    public static ConfigEntry<bool> ShowLighterDarker { get; set; }
    public static ConfigEntry<bool> EnableSoundEffects { get; set; }
    public static ConfigEntry<bool> EnableHorseMode { get; set; }
    public static ConfigEntry<bool> ShowVentsOnMap { get; set; }
    public static ConfigEntry<bool> ShowChatNotifications { get; set; }
    public static ConfigEntry<string> ShowPopUpVersion { get; set; }
    public static ConfigEntry<int> DevModeBotCount { get; set; }
    public static ConfigEntry<string> DevModeBotRoles { get; set; }
    public static ConfigEntry<string> CustomServerIp { get; set; }
    public static ConfigEntry<int> CustomServerPort { get; set; }

    // This is part of the Mini.RegionInstaller, Licensed under GPLv3
    // file="RegionInstallPlugin.cs" company="miniduikboot">
    public static void UpdateRegions()
    {
        var serverManager = FastDestroyableSingleton<ServerManager>.Instance;
        var regionList = new List<IRegionInfo>
        {
            new StaticHttpRegionInfo("TheOtherRoles Asia", StringNames.NoTranslation, "imp.amongusclub.cn",
                new Il2CppReferenceArray<ServerInfo>(new ServerInfo[1]
                    { new("TheOtherRoles Asia", "https://imp.amongusclub.cn", 443, false) })).CastFast<IRegionInfo>()
        };

        // Optional self-hosted server (e.g. a local Impostor instance), configured via the [Custom] "Custom Server IP"/"Custom Server Port" settings
        if (!string.IsNullOrWhiteSpace(CustomServerIp.Value))
            regionList.Add(new StaticHttpRegionInfo("Custom Server", StringNames.NoTranslation, CustomServerIp.Value,
                new Il2CppReferenceArray<ServerInfo>(new ServerInfo[1]
                {
                    new("Custom Server", "http://" + CustomServerIp.Value, (ushort)CustomServerPort.Value, false)
                })).CastFast<IRegionInfo>());

        var regions = regionList.ToArray();

        var currentRegion = serverManager.CurrentRegion;
        Logger.LogInfo($"Adding {regions.Length} regions");
        foreach (var region in regions)
            if (region == null)
            {
                Logger.LogError("Could not add region");
            }
            else
            {
                if (currentRegion != null && region.Name.Equals(currentRegion.Name, StringComparison.OrdinalIgnoreCase))
                    currentRegion = region;
                serverManager.AddOrUpdateRegion(region);
            }

        // AU remembers the previous region that was set, so we need to restore it
        if (currentRegion != null)
        {
            Logger.LogDebug("Resetting previous region");
            serverManager.SetRegion(currentRegion);
        }
    }

    public override void Load()
    {
        Logger = Log;
        Instance = this;

        ModTranslation.Load();

        _ = CredentialsPatch.MOTD.loadMOTDs();

        DebugMode = Config.Bind("Custom", "Enable Debug Mode", false,
            "Enables the debug hotkeys (F: spawn one blank dummy, F5: spawn the configured Dev Mode bots, L: force-end the round).");
        DevModeBotCount = Config.Bind("DevMode", "Bot Count", 6,
            "Number of bots spawned by the Dev Mode hotkey (F5, requires Debug Mode password). Automatically raised to fit Bot Roles if that list is longer.");
        DevModeBotRoles = Config.Bind("DevMode", "Bot Roles", "",
            "Comma-separated role names (e.g. \"Godfather,Sheriff,Jester\") force-assigned in order to the bots spawned via F5. Leave an entry empty to let that bot keep its default role.");
        CustomServerIp = Config.Bind("Custom", "Custom Server IP", "",
            "Optional: IP or hostname of a self-hosted Impostor server. When set, adds a \"Custom Server\" entry to the in-game server list. Leave empty to disable. Requires a game restart to take effect.");
        CustomServerPort = Config.Bind("Custom", "Custom Server Port", 22023,
            "Port of the server configured in Custom Server IP (Impostor's default port is 22023).");
        GhostsSeeInformation = Config.Bind("Custom", "Ghosts See Remaining Tasks", true);
        GhostsSeeRoles = Config.Bind("Custom", "Ghosts See Roles", true);
        GhostsSeeModifier = Config.Bind("Custom", "Ghosts See Modifier", true);
        GhostsSeeVotes = Config.Bind("Custom", "Ghosts See Votes", true);
        ShowRoleSummary = Config.Bind("Custom", "Show Role Summary", true);
        ShowLighterDarker = Config.Bind("Custom", "Show Lighter / Darker", true);
        EnableSoundEffects = Config.Bind("Custom", "Enable Sound Effects", true);
        EnableHorseMode = Config.Bind("Custom", "Enable Horse Mode", false);
        ShowPopUpVersion = Config.Bind("Custom", "Show PopUp", "0");
        ShowVentsOnMap = Config.Bind("Custom", "Show vent positions on minimap", false);
        ShowChatNotifications = Config.Bind("Custom", "Show Chat Notifications", true);

        // Removes vanilla Servers   More extensive testing is needed because I removed the Reactor, so after testing, TOR can temporarily run on the Innerslot server
        // ServerManager.DefaultRegions = new Il2CppReferenceArray<IRegionInfo>(new IRegionInfo[0]);
        UpdateRegions();

        // Reactor Credits (future use?)
        // Reactor.Utilities.ReactorCredits.Register("TheOtherRoles-R", VersionString, isBeta, location => location == Reactor.Utilities.ReactorCredits.Location.PingTracker);

        Harmony.PatchAll();

        CustomOptionHolder.Load();
        CustomColors.Load();
        CustomHatManager.LoadHats();

        AddComponent<ModUpdater>();

        EventUtility.Load();
        SubmergedCompatibility.Initialize();
        MainMenuPatch.addSceneChangeCallbacks();
        _ = CustomRoleManager.loadReadme();
        AddToKillDistanceSetting.addKillDistance();

        // AMCI: Register mod GUID for mod-only matchmaking
        AmciRegistration.Register();

        Logger.LogInfo("Loading TOR completed!");
    }
}

// Deactivate bans, since I always leave my local testing game and ban myself
[HarmonyPatch(typeof(PlayerBanData), nameof(PlayerBanData.IsBanned), MethodType.Getter)]
public static class IsBannedPatch
{
    public static void Postfix(out bool __result)
    {
        __result = false;
    }
}

[HarmonyPatch(typeof(ChatController), nameof(ChatController.Awake))]
public static class ChatControllerAwakePatch
{
    private static void Prefix()
    {
        if (!EOSManager.Instance.isKWSMinor)
            DataManager.Settings.Multiplayer.ChatMode = QuickChatModes.FreeChatOrQuickChat;
    }
}

// Debugging tools
[HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.Update))]
public static class DebugManager
{
    public static void Postfix(KeyboardJoystick __instance)
    {
        if (!TheOtherRolesPlugin.DebugMode.Value) return;


        var inLobby = AmongUsClient.Instance.GameState != InnerNetClient.GameStates.Started;

        // Spawn a single blank dummy (lobby only)
        if (inLobby && Input.GetKeyDown(KeyCode.F)) DevMode.SpawnDummy();

        // Spawn the configured Dev Mode bots (see [DevMode] section in the config), each with its own role
        // (lobby only - spawning bots mid-round is what breaks the round, see spawnedBots' history)
        if (inLobby && Input.GetKeyDown(KeyCode.F5)) DevMode.SpawnConfiguredBots();

        // Open/close a role picker for your own role (round only)
        // F9, not F6 - F6 collided with the user's Discord mute keybind.
        if (!inLobby && Input.GetKeyDown(KeyCode.F9)) DevMode.ToggleRoleMenu();

        // Scroll the role picker with the mouse wheel while it's open
        var scroll = Input.mouseScrollDelta.y;
        if (scroll > 0) DevMode.ScrollRoleMenu(-1);
        else if (scroll < 0) DevMode.ScrollRoleMenu(1);

        // Experimental: cycle control between yourself and your spawned bots (round only)
        if (!inLobby && Input.GetKeyDown(KeyCode.F4)) DevMode.CyclePossession();

        // Terminate round
        if (Input.GetKeyDown(KeyCode.L))
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.ForceEnd, SendOption.Reliable);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.forceEnd();
        }
    }
}

// Dev Mode: solo-testing helpers to spawn dummy bots and force-assign them roles.
// Gated behind the same DebugMode password as the rest of DebugManager.
public static class DevMode
{
    private static readonly Random random = new((int)DateTime.Now.Ticks);
    public static readonly List<PlayerControl> spawnedBots = new();

    private static readonly string[] botNames =
    {
        "Ziggy", "Nova", "Blip", "Rusty", "Widget", "Cosmo", "Glitch", "Pixel",
        "Sprocket", "Nebula", "Turbo", "Buzz", "Circuit", "Byte", "Static", "Echo",
        "Gizmo", "Vector", "Bolt", "Fizz", "Clank", "Nimbus", "Radar", "Sonar",
        "Wobble", "Zap", "Quirk", "Fuzz", "Splice", "Tinker"
    };

    private static string RandomBotName()
    {
        var taken = spawnedBots.Select(b => b.Data?.PlayerName).ToHashSet();
        var available = botNames.Where(n => !taken.Contains(n)).ToList();
        if (available.Count == 0) available = botNames.ToList();
        return available[random.Next(available.Count)];
    }

    public static PlayerControl SpawnDummy()
    {
        // Don't spawn past the lobby's own player cap - DynamicLobbies.LobbyLimit is the actual
        // effective max (the mod always forces the underlying network MaxPlayers to 15 and enforces
        // the real limit itself, see DynamicLobbies.cs), so that's the one to check against, not
        // GameOptionsManager's MaxPlayers directly.
        if (GameData.Instance.PlayerCount >= DynamicLobbies.LobbyLimit)
        {
            TheOtherRolesPlugin.Logger.LogWarning(
                $"Dev Mode: refused to spawn bot, lobby is already at its max of {DynamicLobbies.LobbyLimit} players");
            return null;
        }

        var playerControl = Object.Instantiate(AmongUsClient.Instance.PlayerPrefab);
        playerControl.PlayerId = (byte)GameData.Instance.GetAvailableId();

        GameData.Instance.AddDummy(playerControl);
        AmongUsClient.Instance.Spawn(playerControl);

        // Spread bots out a bit so they don't all stack on the exact same spot as the local player
        // (which made their nametags overlap and become unreadable when spawning several at once).
        var offset = new Vector3((float)(random.NextDouble() - 0.5) * 2f, (float)(random.NextDouble() - 0.5) * 2f);
        playerControl.transform.position = PlayerControl.LocalPlayer.transform.position + offset;
        playerControl.GetComponent<DummyBehaviour>().enabled = true;
        playerControl.NetTransform.enabled = false;
        var botName = RandomBotName();
        playerControl.RpcSetName(botName);
        var colorId = (byte)random.Next(Palette.PlayerColors.Length);
        playerControl.SetColor(colorId);
        playerControl.Data.RpcSetTasks(new byte[0]);

        // Just for style - random cosmetics from whatever's actually unlocked on this account, so they're
        // always valid/already-cached instead of guessing at hat/visor/skin/pet ID strings that might not
        // exist or might not be loaded. setLook (Helpers.cs) is already-proven code (used for morphing
        // etc.), reused here rather than reinventing the RawSetHat/RawSetVisor/skin-swap/RawSetPet dance.
        var hats = FastDestroyableSingleton<HatManager>.Instance.GetUnlockedHats();
        var visors = FastDestroyableSingleton<HatManager>.Instance.GetUnlockedVisors();
        var skins = FastDestroyableSingleton<HatManager>.Instance.GetUnlockedSkins();
        var pets = FastDestroyableSingleton<HatManager>.Instance.GetUnlockedPets();
        var hatId = hats.Length > 0 ? hats[random.Next(hats.Length)].ProdId : "";
        var visorId = visors.Length > 0 ? visors[random.Next(visors.Length)].ProdId : "";
        var skinId = skins.Length > 0 ? skins[random.Next(skins.Length)].ProdId : "";
        var petId = pets.Length > 0 ? pets[random.Next(pets.Length)].ProdId : "";

        // Resolved (was never a bug): user saw cosmetics visually change ~30s after spawn and it turned
        // out to be the game's own one-time round-start "load everyone's real outfit" pass finally
        // succeeding for bots (it used to crash on them, see CosmeticsCacheClearUnusedCosmeticsPatch below)
        // and upgrading from our own raw one-off setLook() preview to the properly-loaded HatParent/
        // PetBehaviour assets - confirmed via [CosmeticDiag] logging that the ID never changes, only the
        // rendering does.
        //
        // First fix attempt (calling NetworkedPlayerInfo.UpdateHat/UpdateVisor/UpdateSkin/UpdatePet
        // directly) made it worse - those apparently only mark network state dirty, no immediate render,
        // and it dropped the RawSetName() name fix that setLook() carried along for free. Reverted, then
        // found the real answer via reflection: PlayerControl.cosmetics (a CosmeticsLayer component) has
        // its own SetHat/SetVisor/SetSkin/SetPetIdle methods that do a REAL asynchronous asset load - this
        // is confirmed to be the legitimate "give a player their full rendered look" API already used
        // elsewhere in this exact codebase for REAL players (IntroPatch.cs: `player.cosmetics.SetHat(...)`
        // during the round intro) - i.e. this is what a real player's client runs the moment they're
        // visible, not something exclusive to round start. RawSetHat/RawSetVisor/RawSetPet (setLook, the
        // old approach) were a cruder, lower-level sprite swap that never triggered the same asset load.
        var outfit = new NetworkedPlayerInfo.PlayerOutfit
        {
            PlayerName = botName,
            ColorId = colorId,
            HatId = hatId,
            VisorId = visorId,
            SkinId = skinId,
            PetId = petId,
            HatSequenceId = byte.MaxValue,
            PetSequenceId = byte.MaxValue,
            SkinSequenceId = byte.MaxValue,
            VisorSequenceId = byte.MaxValue,
            NamePlateSequenceId = byte.MaxValue
        };
        playerControl.Data.SetOutfit(PlayerOutfitType.Default, outfit);
        playerControl.RawSetName(botName); // setLook() used to carry this along - see the "Lobby shows ???" fix in [[dev_mode_status]]
        playerControl.cosmetics.SetHat(hatId, colorId);
        playerControl.cosmetics.SetVisor(visorId, colorId);
        playerControl.cosmetics.SetSkin(skinId, colorId, null);
        // Pets rendered but sat at a fixed spot instead of following their owning bot.
        // SetPetSource() (tried first) didn't fix it - reflection also showed PetBehaviour itself has a
        // SetTargetPlayer(PlayerControl) method, which sounds like the actual position-follow binding
        // (SetPetSource may be for something else, e.g. a color/visibility reference, not position).
        // SetPetIdle() loads the pet asset asynchronously (hence the Action callback param) - GetPet()
        // right after calling it would likely return null/the wrong pre-load state, so the
        // SetTargetPlayer() call has to happen inside the onComplete callback, once the real PetBehaviour
        // instance actually exists.
        playerControl.cosmetics.SetPetIdle(petId, colorId, (Action)(() =>
        {
            var pet = playerControl.cosmetics.GetPet();
            if (pet != null) pet.SetTargetPlayer(playerControl);
        }));

        // Experimental: the host's own game-start flow waits for every player's ClientData to be
        // marked ready before the round actually begins ("Timeout while waiting for other player
        // data" otherwise, since a dummy never sends that confirmation itself). Dummies never get a
        // real ClientData (they're not a real connected client), so fake a minimal one here and mark
        // it ready/in-scene immediately so the host doesn't wait on it forever. Doesn't fully fix the
        // timeout (still reproduces), but got noticeably further than without it - kept per user request.
        // Use the PlayerId directly as the client id: several vanilla lookups (e.g. CoCensorNameAsync)
        // expect small sequential client ids matching join order, not an arbitrary offset. The host
        // already owns PlayerId/ClientId 0, so bots (which never get PlayerId 0) can't collide with it.
        var clientData = new ClientData(playerControl.PlayerId, botName, new PlatformSpecificData(), 0, "", "");
        clientData.Character = playerControl;
        clientData.InScene = true;
        clientData.IsReady = true;
        // GetOrCreateClient may only use the Id to look up/create its own internal record rather than
        // actually storing our object (fields we set above wouldn't take effect anywhere) - add directly
        // to the same list the rest of the game's code (and TOR's own GameStartManagerPatch) iterates.
        AmongUsClient.Instance.allClients.Add(clientData);

        spawnedBots.Add(playerControl);
        return playerControl;
    }

    // Force a player onto a specific role, flipping its base Impostor/Crewmate side first if needed.
    // Works for bots and for the local player alike. Clears whatever role the player held before,
    // since setRole() only ever assigns the new role's static holder field and never clears the old one
    // (it was only ever meant to be called once per player, at initial assignment).
    public static void ForceRole(PlayerControl player, RoleId roleId)
    {
        ClearRole(player);

        if (RoleInfo.roleInfoById.TryGetValue(roleId, out var info) && player.Data.Role.IsImpostor != info.isImpostor)
            FastDestroyableSingleton<RoleManager>.Instance.SetRole(player,
                info.isImpostor ? RoleTypes.Impostor : RoleTypes.Crewmate);

        if (roleId is RoleId.Impostor or RoleId.Crewmate) return; // Base role only, nothing more to assign

        var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
            (byte)CustomRPC.SetRole, SendOption.Reliable);
        writer.Write((byte)roleId);
        writer.Write(player.PlayerId);
        AmongUsClient.Instance.FinishRpcImmediately(writer);
        RPCProcedure.setRole((byte)roleId, player.PlayerId);

        // setRole()'s tail grants the vanilla base-game Engineer role (native vent access) whenever the
        // new role passes roleCanUseVents() - but there's no matching revert when it doesn't, so a player
        // who ever held a vent-capable role (Engineer, vent-enabled Jackal, ...) keeps the Vent button
        // forever after switching away. ClearRole() above already nulled the old role's holder field, so
        // roleCanUseVents() now correctly reflects the NEW role - revert to the plain base type if it's false.
        if (!player.roleCanUseVents())
        {
            var baseType = player.Data.Role.IsImpostor ? RoleTypes.Impostor : RoleTypes.Crewmate;
            player.RpcSetRole(baseType);
            player.CoSetRole(baseType, true);
        }
    }

    // Mirrors every case in RPCProcedure.setRole()'s switch, nulling out whichever role's static holder
    // field currently points at this player (if any) before a new role gets assigned to them.
    private static void ClearRole(PlayerControl player)
    {
        if (Jester.jester == player) Jester.jester = null;
        if (Mayor.mayor == player) Mayor.mayor = null;
        if (Portalmaker.portalmaker == player) Portalmaker.portalmaker = null;
        if (Engineer.engineer == player) Engineer.engineer = null;
        if (Sheriff.sheriff == player) Sheriff.sheriff = null;
        if (Deputy.deputy == player) Deputy.deputy = null;
        if (Lighter.lighter == player) Lighter.lighter = null;
        if (Godfather.godfather == player) Godfather.godfather = null;
        if (Mafioso.mafioso == player) Mafioso.mafioso = null;
        if (Janitor.janitor == player) Janitor.janitor = null;
        if (Detective.detective == player) Detective.detective = null;
        if (TimeMaster.timeMaster == player) TimeMaster.timeMaster = null;
        if (Medic.medic == player) Medic.medic = null;
        if (Shifter.shifter == player) Shifter.shifter = null;
        if (Swapper.swapper == player) Swapper.swapper = null;
        if (Seer.seer == player) Seer.seer = null;
        if (Morphling.morphling == player) Morphling.morphling = null;
        if (Camouflager.camouflager == player) Camouflager.camouflager = null;
        if (Hacker.hacker == player) Hacker.hacker = null;
        if (Tracker.tracker == player) Tracker.tracker = null;
        if (Vampire.vampire == player) Vampire.vampire = null;
        if (Snitch.snitch == player) Snitch.snitch = null;
        if (Jackal.jackal == player) Jackal.jackal = null;
        if (Sidekick.sidekick == player) Sidekick.sidekick = null;
        if (Eraser.eraser == player) Eraser.eraser = null;
        if (Spy.spy == player) Spy.spy = null;
        if (Trickster.trickster == player) Trickster.trickster = null;
        if (Cleaner.cleaner == player) Cleaner.cleaner = null;
        if (Warlock.warlock == player) Warlock.warlock = null;
        if (SecurityGuard.securityGuard == player) SecurityGuard.securityGuard = null;
        if (Arsonist.arsonist == player) Arsonist.arsonist = null;
        if (Guesser.evilGuesser == player) Guesser.evilGuesser = null;
        if (Guesser.niceGuesser == player) Guesser.niceGuesser = null;
        if (BountyHunter.bountyHunter == player) BountyHunter.bountyHunter = null;
        if (Vulture.vulture == player) Vulture.vulture = null;
        if (Medium.medium == player) Medium.medium = null;
        if (Trapper.trapper == player) Trapper.trapper = null;
        if (Lawyer.lawyer == player)
        {
            Lawyer.lawyer = null;
            Lawyer.isProsecutor = false;
        }

        if (Pursuer.pursuer == player) Pursuer.pursuer = null;
        if (Witch.witch == player) Witch.witch = null;
        if (Ninja.ninja == player) Ninja.ninja = null;
        if (Thief.thief == player) Thief.thief = null;
        if (SchrodingersCat.cat == player) SchrodingersCat.cat = null;
        if (Bomber.bomber == player) Bomber.bomber = null;
        if (Yoyo.yoyo == player) Yoyo.yoyo = null;
    }

    private static readonly List<ActionButton> roleMenuButtons = new();
    private static readonly List<TextMeshPro> roleMenuTexts = new();
    private static List<RoleInfo> roleMenuRoles;
    private static bool roleMenuOpen;
    private static int roleMenuScroll;

    private const int RoleMenuVisibleRows = 14;
    private const float RoleMenuRowHeight = 0.48f;
    private const float RoleMenuTopY = 3.3f;

    // Toggleable role-picker for the local player, built the same way TOR's own Draft Mode builds its
    // role-choice buttons (cloned ActionButtons) - that's a proven-working technique in this exact game,
    // unlike the OnGUI/cloned-menu-screen/3rd-party-plugin attempts that all failed for the same purpose.
    //
    // Rebuilt as a scrollable plain-text list (user's explicit request after the previous card-icon
    // version worked click-wise but the icons were way too big: "mach doch einfach eine scrollable list
    // die die namen einfach anzeigt so warum mit icons"). This is a *virtualized* list: only
    // RoleMenuVisibleRows physical buttons ever exist, created once and never destroyed/deactivated again
    // while the menu is open - scrolling just re-points each slot's text/color/click-role. That's
    // deliberate: the previous version's "click doesn't match what's shown" bug got fixed by adding the
    // OverrideText/Effects.Lerp reset call below, and that fix might not survive a button being
    // deactivated and reactivated later (untested, unknown internals) - so slots are reused in place
    // instead of risking that fix being undone by scrolling.
    public static void ToggleRoleMenu()
    {
        // User reports F6 "does nothing" even after the round-4 uniform-scale fix. LogOutput.log has no
        // exception recorded for it, but this method never logged anything itself either, so absence of
        // an error there proves nothing. Logging every step now instead of guessing at a 3rd visual fix -
        // this will tell us for certain whether the method even runs, how many roles/buttons it thinks it
        // created, and catch+log any exception that Harmony might otherwise be swallowing silently.
        TheOtherRolesPlugin.Logger.LogInfo($"[F6Diag] ToggleRoleMenu called, roleMenuOpen was {roleMenuOpen}");

        if (roleMenuOpen)
        {
            foreach (var button in roleMenuButtons) button?.gameObject?.Destroy();
            roleMenuButtons.Clear();
            roleMenuTexts.Clear();
            roleMenuOpen = false;
            return;
        }

        try
        {
            roleMenuOpen = true;
            roleMenuScroll = 0;
            // TOR's own in-round settings panel (CustomOptions.OpenSettings) proves HudManager.Instance.transform
            // IS the right screen-anchored parent - my previous "fix" (KillButton's own parent) was a wrong
            // guess in the other direction. The real bug was the Z depth: that panel uses -500, not the -10
            // this menu used before, and -500 is what actually renders reliably on top of everything.
            var buttonParent = HudManager.Instance.transform;
            roleMenuRoles = CustomRoleManager.Instance.allRoleInfos.Where(r => !r.isModifier).ToList();
            var visibleRows = Math.Min(RoleMenuVisibleRows, roleMenuRoles.Count);
            TheOtherRolesPlugin.Logger.LogInfo($"[F6Diag] roleMenuRoles.Count={roleMenuRoles.Count}, visibleRows={visibleRows}, buttonParent={(buttonParent == null ? "NULL" : buttonParent.name)}");

            for (var slot = 0; slot < visibleRows; slot++)
            {
                var button = Object.Instantiate(HudManager.Instance.KillButton, buttonParent);
                button.gameObject.SetActive(true);
                button.gameObject.name = "DevRoleMenuButton";
                button.transform.localPosition = new Vector3(-2f, RoleMenuTopY - slot * RoleMenuRowHeight, -500f);
                // Must stay UNIFORM (x == y). A non-uniform scale here (this used to be (2.4, 0.42, 1) to fake
                // a "wide flat strip" look) also scales every child non-uniformly - the text child below got
                // squished vertically to near-nothing and shoved sideways by the localPosition-times-parent-
                // scale multiplication. Reverted to uniform last round - user reports still nothing visible,
                // so either this wasn't the only issue or something else is going on; kept uniform regardless
                // since it's still more correct than before either way.
                button.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
                button.SetCoolDown(0, 0);
                button.buttonLabelText.gameObject.SetActive(false);

                HudManager.Instance.StartCoroutine(Effects.Lerp(0.5f, new Action<float>(p => { button.OverrideText(""); })));

                var textHolder = new GameObject("roleMenuText");
                var text = textHolder.AddComponent<TextMeshPro>();
                text.fontSize = 1.3f; // sized down from the card version's 1.6 - rows are much closer together now
                text.horizontalAlignment = HorizontalAlignmentOptions.Left;
                text.outlineWidth = 0.15f;
                text.outlineColor = Color.black;
                textHolder.layer = button.gameObject.layer;
                textHolder.transform.SetParent(button.transform, false);
                textHolder.transform.localPosition = new Vector3(-0.7f, 0, -1);

                var thisSlot = slot;
                var passiveButton = button.GetComponent<PassiveButton>();
                passiveButton.OnClick = new ButtonClickedEvent();
                passiveButton.OnClick.AddListener((Action)(() => PickRoleMenuSlot(thisSlot)));

                roleMenuButtons.Add(button);
                roleMenuTexts.Add(text);
            }

            RefreshRoleMenuLabels();
            TheOtherRolesPlugin.Logger.LogInfo($"[F6Diag] Finished, roleMenuButtons.Count={roleMenuButtons.Count}, first button active={(roleMenuButtons.Count > 0 ? roleMenuButtons[0].gameObject.activeSelf.ToString() : "n/a")}, first button world pos={(roleMenuButtons.Count > 0 ? roleMenuButtons[0].transform.position.ToString() : "n/a")}");
        }
        catch (Exception e)
        {
            TheOtherRolesPlugin.Logger.LogError($"[F6Diag] ToggleRoleMenu threw: {e}");
            roleMenuOpen = false;
        }
    }

    private static void PickRoleMenuSlot(int slot)
    {
        var index = roleMenuScroll + slot;
        if (index < 0 || index >= roleMenuRoles.Count) return;
        var role = roleMenuRoles[index];
        ForceRole(PlayerControl.LocalPlayer, role.roleId);
        TheOtherRolesPlugin.Logger.LogInfo($"Dev Mode: switched your role to {role.name}");
        ToggleRoleMenu(); // close after picking
    }

    // Called every frame from DebugManager while the menu is open (mouse wheel input) - one wheel notch
    // scrolls by one row. Only ever changes which role each already-existing slot shows, per the
    // don't-touch-the-buttons-once-created rule explained above.
    public static void ScrollRoleMenu(int rows)
    {
        if (!roleMenuOpen || rows == 0) return;
        var maxScroll = Math.Max(0, roleMenuRoles.Count - roleMenuButtons.Count);
        var newScroll = Math.Clamp(roleMenuScroll + rows, 0, maxScroll);
        if (newScroll == roleMenuScroll) return;
        roleMenuScroll = newScroll;
        RefreshRoleMenuLabels();
    }

    private static void RefreshRoleMenuLabels()
    {
        for (var slot = 0; slot < roleMenuButtons.Count; slot++)
        {
            var index = roleMenuScroll + slot;
            // Only possible when there happen to be fewer roles than visible rows (not the case today,
            // ~50 roles vs. 14 rows) - guarded anyway rather than assumed away.
            var hasRole = index < roleMenuRoles.Count;
            roleMenuButtons[slot].gameObject.SetActive(hasRole);
            if (!hasRole) continue;

            var role = roleMenuRoles[index];
            roleMenuTexts[slot].text = $"<b>{role.name}</b>";
            roleMenuTexts[slot].color = role.color;
        }
    }

    private static PlayerControl originalSelf;

    // Experimental, 2nd attempt: PlayerControl.LocalPlayer alone wasn't enough (1st attempt tested live -
    // no camera follow, no movement, and it broke the real player's own Vent/Report/Use). Two concrete,
    // reflection-confirmed reasons why:
    // - The camera doesn't read LocalPlayer at all; it's driven by a separate FollowerCamera component
    //   with its own cached Target, set via SetTarget()/SnapToTarget().
    // - PlayerControl has a plain `moveable` bool field, most likely false on a freshly spawned dummy
    //   (they were never meant to walk under player input) - movement input is probably gated on this,
    //   and a stuck-in-place bot would also explain the broken Vent/Report/Use as simple distance checks
    //   failing, not a deeper per-action ownership validation as first assumed.
    // Original OwnerId (host, typically 0) of each bot at the moment we first possess it, so it can be
    // handed back when we move on - never touched for originalSelf, since that one is already ours.
    private static readonly Dictionary<PlayerControl, int> capturedOwnerIds = new();

    // Explicit cursor into { originalSelf, spawnedBots... } instead of deriving "where are we right now"
    // by searching for PlayerControl.LocalPlayer in that list. Found via the [TaskDiag]-style log approach
    // (LogOutput.log showed CyclePossession successfully cycling through all 14 bots once, then EVERY
    // subsequent F4 press logging "back in control of yourself" forever, never resuming to bot 1) that the
    // old `chain.IndexOf(PlayerControl.LocalPlayer)` approach broke permanently right after the one
    // wraparound back to originalSelf - consistent with IL2Cpp interop returning a fresh managed wrapper
    // object on some LocalPlayer reads, which a reference-equality-based IndexOf can silently fail to
    // match even though it's the same underlying native player. Tracking our own index sidesteps needing
    // to identify "where we are" via object identity at all - provably correct regardless of wrapper
    // semantics, rather than another guess at exactly why IndexOf failed.
    private static int possessionIndex;

    public static void CyclePossession()
    {
        // User reported F4 froze the whole client mid-round. Prime suspect: spawnedBots was never pruned
        // of bots that died/got voted out/disconnected over the course of the round - cycling onto one of
        // those (a destroyed Unity object, "fake null") and then directly setting properties on it
        // (target.OwnerId = ..., target.moveable = true, no null-guard on those two specifically) is
        // exactly the kind of thing that can lock up the IL2Cpp runtime instead of throwing a clean,
        // catchable .NET exception. Prune before building the chain so a destroyed bot can never be picked
        // as a target again.
        spawnedBots.RemoveAll(b => b == null);

        // `??=` checks plain CLR null, which never catches Unity's "fake null" (a destroyed object that's
        // still a non-null C# reference) - originalSelf and spawnedBots are static and survive across
        // rounds, and Among Us destroys+respawns every PlayerControl (including your own) at round start,
        // so a stale originalSelf from an earlier round in the same game session would never get refreshed
        // by `??=`. The explicit `== null` below does go through Unity's overloaded operator instead.
        if (originalSelf == null)
        {
            originalSelf = PlayerControl.LocalPlayer;
            possessionIndex = 0;
        }

        var chain = new List<PlayerControl> { originalSelf };
        chain.AddRange(spawnedBots);
        if (chain.Count <= 1) return; // No bots to possess

        var current = PlayerControl.LocalPlayer;
        possessionIndex = (possessionIndex + 1) % chain.Count;
        var target = chain[possessionIndex];
        if (current == null || target == null) return; // Defensive - should be unreachable after the prune above

        // Release ownership of whoever we were just controlling before claiming the next target.
        // AmOwner/OwnerId (not the LocalPlayer reference) is what actually drives per-object movement
        // simulation - leaving it pointed at us was why every previously-possessed bot kept moving in
        // lockstep with the new one (and, since walk-animation is separately gated on
        // PlayerControl.LocalPlayer == this, why only the current target actually animated while walking).
        //
        // Bug found after live test: this block used to only run `if (current != originalSelf)`, which
        // meant `moveable` was NEVER turned off for your real self when moving away from it (its OwnerId
        // is legitimately always yours, so that part correctly stays untouched - but moveable still needs
        // to be turned off same as for a bot, or your real player keeps reading movement input forever in
        // parallel with whatever you're now possessing, which is exactly the "moves both" symptom that
        // persisted even after the OwnerId-revert fix). moveable is now unconditional; only the
        // OwnerId/NetTransform revert (which doesn't apply to originalSelf) stays gated.
        current.moveable = false;
        if (current != originalSelf && capturedOwnerIds.TryGetValue(current, out var originalOwnerId))
        {
            current.OwnerId = originalOwnerId;
            if (current.MyPhysics != null) current.MyPhysics.OwnerId = originalOwnerId;
            if (current.NetTransform != null)
            {
                current.NetTransform.OwnerId = originalOwnerId;
                current.NetTransform.enabled = false;
            }
        }

        if (target != originalSelf) capturedOwnerIds.TryAdd(target, target.OwnerId);

        if (target.NetTransform != null) target.NetTransform.enabled = true;
        target.moveable = true;
        // 3rd lever, also reflection-confirmed: InnerNetObject.OwnerId/AmOwner (PlayerControl and
        // PlayerPhysics both derive from InnerNetObject) is the standard "who has local simulation
        // authority over this networked object" flag - separate from moveable, and separate from the
        // LocalPlayer UI-focus flag. moveable alone didn't fix movement, so claim ownership too.
        var myClientId = AmongUsClient.Instance.ClientId;
        target.OwnerId = myClientId;
        if (target.MyPhysics != null) target.MyPhysics.OwnerId = myClientId;
        if (target.NetTransform != null) target.NetTransform.OwnerId = myClientId;
        PlayerControl.LocalPlayer = target;

        var cam = Object.FindObjectOfType<FollowerCamera>();
        if (cam != null)
        {
            cam.SetTarget(target);
            cam.SnapToTarget();
        }

        // Vision radius was still using whichever player originally called this once at round start
        // ("everything goes dark far from your real body") - reflection on PlayerControl/LightSource found
        // AdjustLighting() as the public re-entry point the game itself must use to (re)apply both the
        // radius and, per LightSource.SetupLightingForGameplay(bool, float, Transform)'s Transform param,
        // which transform the light actually follows. Same category of fix as FollowerCamera.SetTarget()
        // above: an explicit re-hookup call, not a guess at unfamiliar internals. Unverified live.
        target.AdjustLighting();

        TheOtherRolesPlugin.Logger.LogInfo(target == originalSelf
            ? "Dev Mode: back in control of yourself"
            : $"Dev Mode: now controlling {target.Data.PlayerName}");

        // Diagnostic for the "wrong role's ability buttons show up after switching" report (e.g. Seer
        // showing Kill+Vent, Mayor showing Vent+Emergency merged onto one button). Leading theory: IL2Cpp
        // interop returning a fresh managed wrapper object on some PlayerControl.LocalPlayer reads - the
        // same root cause that broke possessionIndex tracking via reference-based IndexOf - could make a
        // `SomeRole.holder == PlayerControl.LocalPlayer` check in Buttons.cs spuriously true or false
        // depending on which wrapper instance is being compared. Logging exactly which buttons evaluate
        // HasButton()==true right after a switch, next to the target's actual assigned role, should confirm
        // or rule this out the next time the mismatch reproduces - same technique that solved the
        // stuck-cycling and task-win-diagnostic bugs (see [[dev_mode_status]] memory).
        var activeButtons = CustomButton.buttons.Where(b => b.HasButton()).Select(b => b.Sprite?.name ?? "?");
        var roleNames = string.Join("+", CustomRoleManager.getRoleInfoForPlayer(target).Select(r => r.name));
        TheOtherRolesPlugin.Logger.LogInfo(
            $"[F4ButtonDiag] target={target.Data.PlayerName} (id={target.PlayerId}) role={roleNames} activeButtons=[{string.Join(", ", activeButtons)}]");
    }

    // Spawns TheOtherRolesPlugin.DevModeBotCount bots and force-assigns the roles configured
    // in TheOtherRolesPlugin.DevModeBotRoles to them, in order (empty entries keep the default role).
    // Every bot ends up on a different role (user request) - a configured role that's already taken by an
    // earlier bot this spawn (or left empty/unrecognized) falls back to a random still-unused role instead
    // of leaving bots on the default/duplicate role.
    public static void SpawnConfiguredBots()
    {
        // Experimental: Practice/Freeplay is a real, first-class NetworkMode in the same engine
        // (not a separate offline system like the tutorial) - it's what Innersloth's own AI-bot solo
        // mode runs under, and it likely skips the "wait for every player's data" round-start check
        // that dummy bots otherwise get stuck on. Flip it here to test whether that alone helps,
        // without switching away from our own lobby/settings flow.
        AmongUsClient.Instance.NetworkMode = NetworkModes.FreePlay;

        var roleNames = TheOtherRolesPlugin.DevModeBotRoles.Value.Split(',');
        var count = Math.Max(TheOtherRolesPlugin.DevModeBotCount.Value, roleNames.Length);
        var allRoles = CustomRoleManager.Instance.allRoleInfos.Where(r => !r.isModifier).ToList();
        var usedRoles = new HashSet<RoleId>();

        for (var i = 0; i < count; i++)
        {
            var bot = SpawnDummy();
            if (bot == null) break; // Lobby is full - SpawnDummy already logged why, no point looping further

            RoleId? roleId = null;
            var roleName = i < roleNames.Length ? roleNames[i].Trim() : "";
            if (roleName != "")
            {
                if (Enum.TryParse<RoleId>(roleName, true, out var parsed))
                    roleId = parsed;
                else
                    TheOtherRolesPlugin.Logger.LogWarning($"Dev Mode: unknown role \"{roleName}\" in Bot Roles config");
            }

            if (roleId == null || !usedRoles.Add(roleId.Value))
            {
                var available = allRoles.Where(r => !usedRoles.Contains(r.roleId)).ToList();
                if (available.Count == 0)
                {
                    TheOtherRolesPlugin.Logger.LogWarning("Dev Mode: ran out of unique roles for bots, leaving remaining bots on their default role");
                    continue;
                }

                roleId = available[random.Next(available.Count)].roleId;
                usedRoles.Add(roleId.Value);
            }

            ForceRole(bot, roleId.Value);
        }
    }
}

// AmongUsClient.CoStartGame() crashes with a NullReferenceException in CosmeticsCache.ClearUnusedCosmetics()
// when a Dev Mode dummy is present - its outfit/cosmetics were never populated the way a real player's are,
// which otherwise kills the whole round on start. This cache cleanup is a pure memory-optimization pass
// (frees cosmetic assets no current player uses), so skipping it while bots exist is harmless.
[HarmonyPatch(typeof(CosmeticsCache), nameof(CosmeticsCache.ClearUnusedCosmetics))]
public static class CosmeticsCacheClearUnusedCosmeticsPatch
{
    public static bool Prefix()
    {
        return DevMode.spawnedBots.Count == 0;
    }
}

// Diagnostic: user reports bot cosmetics (pets especially) keep changing ~30s after spawn despite two
// fix attempts (SetOutfit() for persistence, then max-value sequence IDs) - neither confirmed to work,
// and there's no existing log line anywhere that shows who's re-applying a bot's outfit later. Logging
// every call to these on a Dev Mode bot specifically, with a stack trace, so the next repro shows exactly
// which system is doing it instead of a third blind guess.
[HarmonyPatch]
public static class CosmeticDiagPatches
{
    private static bool IsSpawnedBot(byte playerId) => DevMode.spawnedBots.Any(b => b != null && b.PlayerId == playerId);

    [HarmonyPatch(typeof(NetworkedPlayerInfo), nameof(NetworkedPlayerInfo.UpdatePet))]
    [HarmonyPostfix]
    public static void UpdatePetPostfix(NetworkedPlayerInfo __instance, string petId)
    {
        if (!IsSpawnedBot(__instance.PlayerId)) return;
        TheOtherRolesPlugin.Logger.LogInfo($"[CosmeticDiag] UpdatePet({petId}) on bot {__instance.PlayerName}\n{Environment.StackTrace}");
    }

    [HarmonyPatch(typeof(NetworkedPlayerInfo), nameof(NetworkedPlayerInfo.UpdateHat))]
    [HarmonyPostfix]
    public static void UpdateHatPostfix(NetworkedPlayerInfo __instance, string hat)
    {
        if (!IsSpawnedBot(__instance.PlayerId)) return;
        TheOtherRolesPlugin.Logger.LogInfo($"[CosmeticDiag] UpdateHat({hat}) on bot {__instance.PlayerName}\n{Environment.StackTrace}");
    }

    [HarmonyPatch(typeof(NetworkedPlayerInfo), nameof(NetworkedPlayerInfo.UpdateSkin))]
    [HarmonyPostfix]
    public static void UpdateSkinPostfix(NetworkedPlayerInfo __instance, string skin)
    {
        if (!IsSpawnedBot(__instance.PlayerId)) return;
        TheOtherRolesPlugin.Logger.LogInfo($"[CosmeticDiag] UpdateSkin({skin}) on bot {__instance.PlayerName}\n{Environment.StackTrace}");
    }

    [HarmonyPatch(typeof(NetworkedPlayerInfo), nameof(NetworkedPlayerInfo.UpdateVisor))]
    [HarmonyPostfix]
    public static void UpdateVisorPostfix(NetworkedPlayerInfo __instance, string visor)
    {
        if (!IsSpawnedBot(__instance.PlayerId)) return;
        TheOtherRolesPlugin.Logger.LogInfo($"[CosmeticDiag] UpdateVisor({visor}) on bot {__instance.PlayerName}\n{Environment.StackTrace}");
    }

    [HarmonyPatch(typeof(NetworkedPlayerInfo), nameof(NetworkedPlayerInfo.SetOutfit))]
    [HarmonyPostfix]
    public static void SetOutfitPostfix(NetworkedPlayerInfo __instance, PlayerOutfitType outfitType)
    {
        if (!IsSpawnedBot(__instance.PlayerId)) return;
        TheOtherRolesPlugin.Logger.LogInfo($"[CosmeticDiag] SetOutfit({outfitType}) on bot {__instance.PlayerName}\n{Environment.StackTrace}");
    }
}