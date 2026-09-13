using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.GameOptions;
using Assets.CoreScripts;
using HarmonyLib;
using Hazel;
using InnerNet;
using TheOtherRoles.CustomGameModes;
using TheOtherRoles.Objects;
using TheOtherRoles.Utilities;
using TMPro;
using UnityEngine;
using static TheOtherRoles.TheOtherRoles;
using static TheOtherRoles.GameHistory;
using Object = UnityEngine.Object;

namespace TheOtherRoles.Patches;

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
public static class PlayerControlFixedUpdatePatch
{
    // Helpers

    public static PlayerControl setTarget(bool onlyCrewmates = false, bool targetPlayersInVents = false,
        List<PlayerControl> untargetablePlayers = null, PlayerControl targetingPlayer = null)
    {
        PlayerControl result = null;
        var num = LegacyGameOptions.KillDistances[
            Mathf.Clamp(GameOptionsManager.Instance.currentNormalGameOptions.KillDistance, 0, 2)];
        if (!MapUtilities.CachedShipStatus) return result;
        if (targetingPlayer == null) targetingPlayer = PlayerControl.LocalPlayer;
        if (targetingPlayer.Data.IsDead) return result;

        var truePosition = targetingPlayer.GetTruePosition();
        foreach (var playerInfo in GameData.Instance.AllPlayers.GetFastEnumerator())
            if (!playerInfo.Disconnected && playerInfo.PlayerId != targetingPlayer.PlayerId && !playerInfo.IsDead &&
                (!onlyCrewmates || !playerInfo.Role.IsImpostor))
            {
                var @object = playerInfo.Object;
                if (untargetablePlayers != null && untargetablePlayers.Any(x => x == @object))
                    // if that player is not targetable: skip check
                    continue;

                if (@object && (!@object.inVent || targetPlayersInVents))
                {
                    var vector = @object.GetTruePosition() - truePosition;
                    var magnitude = vector.magnitude;
                    if (magnitude <= num && !PhysicsHelpers.AnyNonTriggersBetween(truePosition, vector.normalized,
                            magnitude, Constants.ShipAndObjectsMask))
                    {
                        result = @object;
                        num = magnitude;
                    }
                }
            }

        return result;
    }

    public static void setPlayerOutline(PlayerControl target, Color color)
    {
        if (target == null || target.cosmetics?.currentBodySprite?.BodySprite == null) return;

        color = color.SetAlpha(Chameleon.visibility(target.PlayerId));

        target.cosmetics.currentBodySprite.BodySprite.material.SetFloat("_Outline", 1f);
        target.cosmetics.currentBodySprite.BodySprite.material.SetColor("_OutlineColor", color);
    }

    // Update functions

    private static void setBasePlayerOutlines()
    {
        foreach (var target in PlayerControl.AllPlayerControls)
        {
            if (target == null || target.cosmetics?.currentBodySprite?.BodySprite == null) continue;

            var isMorphedMorphling = target == Morphling.morphling && Morphling.morphTarget != null &&
                                     Morphling.morphTimer > 0f;
            var hasVisibleShield = false;
            var color = Medic.shieldedColor;
            if (Camouflager.camouflageTimer <= 0f && !Helpers.MushroomSabotageActive() && Medic.shieldVisible(target))
                hasVisibleShield = true;

            if (Camouflager.camouflageTimer <= 0f && !Helpers.MushroomSabotageActive() &&
                TORMapOptions.firstKillPlayer != null && TORMapOptions.shieldFirstKill &&
                ((target == TORMapOptions.firstKillPlayer && !isMorphedMorphling) ||
                 (isMorphedMorphling && Morphling.morphTarget == TORMapOptions.firstKillPlayer)))
            {
                hasVisibleShield = true;
                color = Color.blue;
            }

            if (PlayerControl.LocalPlayer.Data.IsDead && Armored.armored != null && target == Armored.armored &&
                !Armored.isBrokenArmor && !hasVisibleShield)
            {
                hasVisibleShield = true;
                color = Color.yellow;
            }

            if (hasVisibleShield)
            {
                target.cosmetics.currentBodySprite.BodySprite.material.SetFloat("_Outline", 1f);
                target.cosmetics.currentBodySprite.BodySprite.material.SetColor("_OutlineColor", color);
            }
            else
            {
                target.cosmetics.currentBodySprite.BodySprite.material.SetFloat("_Outline", 0f);
            }
        }
    }

    private static void setPetVisibility()
    {
        var localalive = PlayerControl.LocalPlayer.Data.IsDead;
        foreach (var player in PlayerControl.AllPlayerControls)
        {
            var playeralive = !player.Data.IsDead;
            player.cosmetics.SetPetVisible((localalive && playeralive) || !localalive);
        }
    }

    public static void deputyCheckPromotion(bool isMeeting = false)
    {
        // If LocalPlayer is Deputy, the Sheriff is disconnected and Deputy promotion is enabled, then trigger promotion
        if (Deputy.deputy == null || Deputy.deputy != PlayerControl.LocalPlayer) return;
        if (Deputy.promotesToSheriff == 0 || Deputy.deputy.Data.IsDead ||
            (Deputy.promotesToSheriff == 2 && !isMeeting)) return;
        if (Sheriff.sheriff == null || Sheriff.sheriff?.Data?.Disconnected == true || Sheriff.sheriff.Data.IsDead)
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.DeputyPromotes, SendOption.Reliable);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.deputyPromotes();
        }
    }

    private static void impostorSetTarget()
    {
        if (!PlayerControl.LocalPlayer.Data.Role.IsImpostor || !PlayerControl.LocalPlayer.CanMove ||
            PlayerControl.LocalPlayer.Data.IsDead)
        {
            // !isImpostor || !canMove || isDead
            FastDestroyableSingleton<HudManager>.Instance.KillButton.SetTarget(null);
            return;
        }

        PlayerControl target = null;
        if (Spy.spy != null || Sidekick.wasSpy || Jackal.wasSpy)
        {
            if (Spy.impostorsCanKillAnyone)
                target = setTarget(false, true);
            else
                target = setTarget(true, true,
                    new List<PlayerControl>
                    {
                        Spy.spy, Sidekick.wasTeamRed ? Sidekick.sidekick : null,
                        Jackal.wasTeamRed ? Jackal.jackal : null
                    });
        }
        else
        {
            target = setTarget(true, true,
                new List<PlayerControl>
                    { Sidekick.wasImpostor ? Sidekick.sidekick : null, Jackal.wasImpostor ? Jackal.jackal : null });
        }

        FastDestroyableSingleton<HudManager>.Instance.KillButton
            .SetTarget(target); // Includes setPlayerOutline(target, Palette.ImpstorRed);
    }

    public static void playerSizeUpdate(PlayerControl p)
    {
        // Set default player size
        var collider = p.Collider.CastFast<CircleCollider2D>();

        p.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
        collider.radius = Mini.defaultColliderRadius;
        collider.offset = Mini.defaultColliderOffset * Vector2.down;

        // Set adapted player size to Mini and Morphling
        if (Mini.mini == null || Camouflager.camouflageTimer > 0f || Helpers.MushroomSabotageActive() ||
            (Mini.mini == Morphling.morphling && Morphling.morphTimer > 0)) return;

        var growingProgress = Mini.growingProgress();
        var scale = growingProgress * 0.35f + 0.35f;
        var correctedColliderRadius =
            Mini.defaultColliderRadius * 0.7f /
            scale; // scale / 0.7f is the factor by which we decrease the player size, hence we need to increase the collider size by 0.7f / scale

        if (p == Mini.mini)
        {
            p.transform.localScale = new Vector3(scale, scale, 1f);
            collider.radius = correctedColliderRadius;
        }

        if (Morphling.morphling != null && p == Morphling.morphling && Morphling.morphTarget == Mini.mini &&
            Morphling.morphTimer > 0f)
        {
            p.transform.localScale = new Vector3(scale, scale, 1f);
            collider.radius = correctedColliderRadius;
        }
    }

    public static void updatePlayerInfo()
    {
        var colorBlindTextMeetingInitialLocalPos = new Vector3(0.3384f, -0.16666f, -0.01f);
        var colorBlindTextMeetingInitialLocalScale = new Vector3(0.9f, 1f, 1f);
        foreach (var p in PlayerControl.AllPlayerControls)
        {
            // Colorblind Text in Meeting
            var playerVoteArea = MeetingHud.Instance?.playerStates?.FirstOrDefault(x => x.PlayerId == p.PlayerId);
            if (playerVoteArea != null && playerVoteArea.ColorBlindName.gameObject.active)
            {
                playerVoteArea.ColorBlindName.transform.localPosition =
                    colorBlindTextMeetingInitialLocalPos + new Vector3(0f, 0.4f, 0f);
                playerVoteArea.ColorBlindName.transform.localScale = colorBlindTextMeetingInitialLocalScale * 0.8f;
            }

            // Colorblind Text During the round
            if (p.cosmetics.colorBlindText != null && p.cosmetics.showColorBlindText &&
                p.cosmetics.colorBlindText.gameObject.active)
                p.cosmetics.colorBlindText.transform.localPosition = new Vector3(0, -1f, 0f);

            p.cosmetics.nameText.transform.parent
                .SetLocalZ(-0.0001f); // This moves both the name AND the colorblindtext behind objects (if the player is behind the object), like the rock on polus

            if ((Lawyer.lawyerKnowsRole && PlayerControl.LocalPlayer == Lawyer.lawyer && p == Lawyer.target) ||
                p == PlayerControl.LocalPlayer || PlayerControl.LocalPlayer.Data.IsDead ||
                DevMode.spawnedBots.Contains(p))
            {
                var playerInfoTransform = p.cosmetics.nameText.transform.parent.FindChild("Info");
                var playerInfo = playerInfoTransform != null ? playerInfoTransform.GetComponent<TextMeshPro>() : null;
                if (playerInfo == null)
                {
                    playerInfo = Object.Instantiate(p.cosmetics.nameText, p.cosmetics.nameText.transform.parent);
                    playerInfo.transform.localPosition += Vector3.up * 0.225f;
                    playerInfo.fontSize *= 0.75f;
                    playerInfo.gameObject.name = "Info";
                    playerInfo.color = playerInfo.color.SetAlpha(1f);
                }

                var meetingInfoTransform = playerVoteArea != null
                    ? playerVoteArea.NameText.transform.parent.FindChild("Info")
                    : null;
                var meetingInfo = meetingInfoTransform != null
                    ? meetingInfoTransform.GetComponent<TextMeshPro>()
                    : null;
                if (meetingInfo == null && playerVoteArea != null)
                {
                    meetingInfo = Object.Instantiate(playerVoteArea.NameText, playerVoteArea.NameText.transform.parent);
                    meetingInfo.transform.localPosition += Vector3.down * 0.2f;
                    meetingInfo.fontSize *= 0.60f;
                    meetingInfo.gameObject.name = "Info";
                }

                // Set player name higher to align in middle
                if (meetingInfo != null && playerVoteArea != null)
                {
                    var playerName = playerVoteArea.NameText;
                    playerName.transform.localPosition = new Vector3(0.3384f, 0.0311f, -0.1f);
                }

                var (tasksCompleted, tasksTotal) = TasksHandler.taskInfo(p.Data);
                var roleNames = CustomRoleManager.GetRolesString(p, true, false);
                var roleText = CustomRoleManager.GetRolesString(p, true, TORMapOptions.ghostsSeeModifier);
                var taskInfo = tasksTotal > 0 ? $"<color=#FAD934FF>({tasksCompleted}/{tasksTotal})</color>" : "";

                var playerInfoText = "";
                var meetingInfoText = "";
                if (p == PlayerControl.LocalPlayer)
                {
                    if (p.Data.IsDead) roleNames = roleText;
                    playerInfoText = $"{roleNames}";
                    if (p == Swapper.swapper)
                        playerInfoText = $"{roleNames}" + Helpers.cs(Swapper.color, $" ({Swapper.charges})");
                    if (HudManager.Instance.TaskPanel != null)
                    {
                        var tabText = HudManager.Instance.TaskPanel.tab.transform.FindChild("TabText_TMP")
                            .GetComponent<TextMeshPro>();
                        tabText.SetText(string.Format(ModTranslation.GetString("PlayerInfo-Text", 1), taskInfo));
                    }

                    meetingInfoText = $"{roleNames} {taskInfo}".Trim();
                }
                else if (DevMode.spawnedBots.Contains(p))
                {
                    // Dev Mode: always show a bot's role above its name, regardless of ghost settings,
                    // so their assigned role (from Bot Roles / random assignment) is visible at a glance.
                    playerInfoText = $"{roleText} {taskInfo}".Trim();
                    meetingInfoText = playerInfoText;
                }
                else if (TORMapOptions.ghostsSeeRoles && TORMapOptions.ghostsSeeInformation)
                {
                    playerInfoText = $"{roleText} {taskInfo}".Trim();
                    meetingInfoText = playerInfoText;
                }
                else if (TORMapOptions.ghostsSeeInformation)
                {
                    playerInfoText = $"{taskInfo}".Trim();
                    meetingInfoText = playerInfoText;
                }
                else if (TORMapOptions.ghostsSeeRoles || (Lawyer.lawyerKnowsRole &&
                                                          PlayerControl.LocalPlayer == Lawyer.lawyer &&
                                                          p == Lawyer.target))
                {
                    playerInfoText = $"{roleText}";
                    meetingInfoText = playerInfoText;
                }

                playerInfo.text = playerInfoText;
                playerInfo.gameObject.SetActive(p.Visible);
                if (meetingInfo != null)
                    meetingInfo.text = MeetingHud.Instance.state == MeetingHud.MeetingStates.Results
                        ? ""
                        : meetingInfoText;
            }
        }
    }

    public static void Postfix(PlayerControl __instance)
    {
        if (AmongUsClient.Instance.GameState != InnerNetClient.GameStates.Started ||
            GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek) return;

        // Mini and Morphling shrink
        if (!PropHunt.isPropHuntGM) playerSizeUpdate(__instance);

        // set position of colorblind text
        foreach (var pc in PlayerControl.AllPlayerControls)
        {
            //pc.cosmetics.colorBlindText.gameObject.transform.localPosition = new Vector3(0, 0, -0.0001f);
        }

        if (PlayerControl.LocalPlayer == __instance)
        {
            // Update player outlines
            setBasePlayerOutlines();

            // Update Role Description
            Helpers.refreshRoleDescription(__instance);

            // Update Player Info
            updatePlayerInfo();

            //Update pet visibility
            setPetVisibility();

            // Objects that update every frame
            Garlic.UpdateAll();
            Trap.Update();
            NinjaTrace.UpdateAll();
            Silhouette.UpdateAll();
            Chameleon.update();
            Bomb.update();
            PropHunt.update();

            // RoleBase lifecycle: let each role run its own per-frame logic
            CustomRoleManager.Instance.PlayerFixedUpdate(__instance);
        }

        // RoleBase lifecycle: Update runs for ALL players (not just local)
        CustomRoleManager.Instance.PlayerUpdate(__instance);
    }
}

[HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.WalkPlayerTo))]
internal class PlayerPhysicsWalkPlayerToPatch
{
    private static Vector2 offset = Vector2.zero;

    public static void Prefix(PlayerPhysics __instance)
    {
        var correctOffset = Camouflager.camouflageTimer <= 0f && !Helpers.MushroomSabotageActive() &&
                            (__instance.myPlayer == Mini.mini || (Morphling.morphling != null &&
                                                                  __instance.myPlayer == Morphling.morphling &&
                                                                  Morphling.morphTarget == Mini.mini &&
                                                                  Morphling.morphTimer > 0f));
        correctOffset = correctOffset && !(Mini.mini == Morphling.morphling && Morphling.morphTimer > 0f);
        if (correctOffset)
        {
            var currentScaling = (Mini.growingProgress() + 1) * 0.5f;
            __instance.myPlayer.Collider.offset = currentScaling * Mini.defaultColliderOffset * Vector2.down;
        }
    }
}

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.ReportDeadBody))]
internal class PlayerControlReportDeadBodyPatch
{
    public static bool Prefix(PlayerControl __instance)
    {
        if (HideNSeek.isHideNSeekGM || PropHunt.isPropHuntGM) return false;
        Helpers.handleVampireBiteOnBodyReport();
        return true;
    }
}

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.LocalPlayer.ReportDeadBody))]
internal class BodyReportPatch
{
    static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] NetworkedPlayerInfo target)
    {
        // Medic or Detective report
        bool isMedicReport = Medic.medic != null && Medic.medic == PlayerControl.LocalPlayer && __instance.PlayerId == Medic.medic.PlayerId;
        bool isDetectiveReport = Detective.detective != null && Detective.detective == PlayerControl.LocalPlayer && __instance.PlayerId == Detective.detective.PlayerId;
        if (isMedicReport || isDetectiveReport)
        {
            DeadPlayer deadPlayer = deadPlayers?.Where(x => x.player?.PlayerId == target?.PlayerId)?.FirstOrDefault();

            if (deadPlayer != null && deadPlayer.killerIfExisting != null)
            {
                float timeSinceDeath = ((float)(DateTime.UtcNow - deadPlayer.timeOfDeath).TotalMilliseconds);
                string msg = "";

                if (isMedicReport)
                {
                    msg = string.Format(ModTranslation.GetString("Game-Medic", 1), Math.Round(timeSinceDeath / 1000));
                }
                else if (isDetectiveReport)
                {
                    if (timeSinceDeath < Detective.reportNameDuration * 1000)
                    {
                        msg = string.Format(ModTranslation.GetString("Game-Detective", 1), deadPlayer.killerIfExisting.Data.PlayerName);
                    }
                    else if (timeSinceDeath < Detective.reportColorDuration * 1000)
                    {
                        var typeOfColor = ModTranslation.GetString("Button", Helpers.isLighterColor(deadPlayer.killerIfExisting) ? 5 : 6);
                        msg = string.Format(ModTranslation.GetString("Game-Detective", 2), typeOfColor);
                    }
                    else
                    {
                        msg = ModTranslation.GetString("Game-Detective", 3);
                    }
                }

                if (!string.IsNullOrWhiteSpace(msg))
                {
                    if (AmongUsClient.Instance.AmClient && FastDestroyableSingleton<HudManager>.Instance)
                    {
                        FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, msg);
                        // Ghost Info
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ShareGhostInfo, Hazel.SendOption.Reliable, -1);
                        writer.Write(PlayerControl.LocalPlayer.PlayerId);
                        writer.Write((byte)RPCProcedure.GhostInfoTypes.DetectiveOrMedicInfo);
                        writer.Write(msg);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                    }
                    if (msg.IndexOf("who", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        FastDestroyableSingleton<UnityTelemetry>.Instance.SendWho();
                    }
                }
            }
        }
    }
}

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]
public static class MurderPlayerPatch
{
    public static bool resetToCrewmate;
    public static bool resetToDead;

    public static void Prefix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl target)
    {
        // Allow everyone to murder players
        resetToCrewmate = !__instance.Data.Role.IsImpostor;
        resetToDead = __instance.Data.IsDead;
        __instance.Data.Role.TeamType = RoleTeamTypes.Impostor;
        __instance.Data.IsDead = false;
    }

    public static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl target)
    {
        // Collect dead player info
        var deadPlayer = new DeadPlayer(target, DateTime.UtcNow, DeadPlayer.CustomDeathReason.Kill, __instance);
        deadPlayers.Add(deadPlayer);

        // Reset killer to crewmate if resetToCrewmate
        if (resetToCrewmate) __instance.Data.Role.TeamType = RoleTeamTypes.Crewmate;
        if (resetToDead) __instance.Data.IsDead = true;

        // Remove fake tasks when player dies
        if (target.hasFakeTasks() || target == Lawyer.lawyer || target == Pursuer.pursuer || target == Thief.thief)
            target.clearAllTasks();

        // First kill (set before lover suicide)
        if (TORMapOptions.firstKillName == "") TORMapOptions.firstKillName = target.Data.PlayerName;

        // Lover suicide trigger on murder
        if ((Lovers.lover1 != null && target == Lovers.lover1) || (Lovers.lover2 != null && target == Lovers.lover2))
        {
            var otherLover = target == Lovers.lover1 ? Lovers.lover2 : Lovers.lover1;
            if (otherLover != null && !otherLover.Data.IsDead && Lovers.bothDie)
            {
                otherLover.MurderPlayer(otherLover);
                overrideDeathReasonAndKiller(otherLover, DeadPlayer.CustomDeathReason.LoverSuicide);
            }
        }

        // Sidekick promotion trigger on murder
        if (Sidekick.promotesToJackal && Sidekick.sidekick != null && !Sidekick.sidekick.Data.IsDead &&
            target == Jackal.jackal && Jackal.jackal == PlayerControl.LocalPlayer)
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.SidekickPromotes, SendOption.Reliable);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.sidekickPromotes();
        }

        // Pursuer promotion trigger on murder (the host sends the call such that everyone recieves the update before a possible game End)
        if (target == Lawyer.target && AmongUsClient.Instance.AmHost && Lawyer.lawyer != null)
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.LawyerPromotesToPursuer, SendOption.Reliable);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.lawyerPromotesToPursuer();
        }

        // Seer show flash and add dead player position
        if (Seer.seer != null && (PlayerControl.LocalPlayer == Seer.seer || Helpers.shouldShowGhostInfo()) &&
            !Seer.seer.Data.IsDead && Seer.seer != target && Seer.mode <= 1)
            Helpers.showFlash(new Color(42f / 255f, 187f / 255f, 245f / 255f), message: "Seer Info: Someone Died");
        if (Seer.deadBodyPositions != null) Seer.deadBodyPositions.Add(target.transform.position);

        // Tracker store body positions
        if (Tracker.deadBodyPositions != null) Tracker.deadBodyPositions.Add(target.transform.position);

        // Medium add body
        if (Medium.deadBodies != null)
            Medium.futureDeadBodies.Add(new Tuple<DeadPlayer, Vector3>(deadPlayer, target.transform.position));

        // Set bountyHunter cooldown
        if (BountyHunter.bountyHunter != null && PlayerControl.LocalPlayer == BountyHunter.bountyHunter &&
            __instance == BountyHunter.bountyHunter)
        {
            if (target == BountyHunter.bounty)
            {
                BountyHunter.bountyHunter.SetKillTimer(BountyHunter.bountyKillCooldown);
                BountyHunter.bountyUpdateTimer = 0f; // Force bounty update
            }
            else
            {
                BountyHunter.bountyHunter.SetKillTimer(
                    GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown + BountyHunter.punishmentTime);
            }
        }

        // Mini Set Impostor Mini kill timer (Due to mini being a modifier, all "SetKillTimers" must have happened before this!)
        if (Mini.mini != null && __instance == Mini.mini && __instance == PlayerControl.LocalPlayer)
        {
            var multiplier = 1f;
            if (Mini.mini != null && PlayerControl.LocalPlayer == Mini.mini) multiplier = Mini.isGrownUp() ? 0.66f : 2f;
            Mini.mini.SetKillTimer(__instance.killTimer * multiplier);
        }

        // Cleaner Button Sync
        if (Cleaner.cleaner != null && PlayerControl.LocalPlayer == Cleaner.cleaner && __instance == Cleaner.cleaner &&
            HudManagerStartPatch.cleanerCleanButton != null)
            HudManagerStartPatch.cleanerCleanButton.Timer = Cleaner.cleaner.killTimer;

        // Witch Button Sync
        if (Witch.triggerBothCooldowns && Witch.witch != null && PlayerControl.LocalPlayer == Witch.witch &&
            __instance == Witch.witch && HudManagerStartPatch.witchSpellButton != null)
            HudManagerStartPatch.witchSpellButton.Timer = HudManagerStartPatch.witchSpellButton.MaxTimer;

        // Warlock Button Sync
        if (Warlock.warlock != null && PlayerControl.LocalPlayer == Warlock.warlock && __instance == Warlock.warlock &&
            HudManagerStartPatch.warlockCurseButton != null)
            if (Warlock.warlock.killTimer > HudManagerStartPatch.warlockCurseButton.Timer)
                HudManagerStartPatch.warlockCurseButton.Timer = Warlock.warlock.killTimer;

        // Ninja Button Sync
        if (Ninja.ninja != null && PlayerControl.LocalPlayer == Ninja.ninja && __instance == Ninja.ninja &&
            HudManagerStartPatch.ninjaButton != null)
            HudManagerStartPatch.ninjaButton.Timer = HudManagerStartPatch.ninjaButton.MaxTimer;

        // Bait
        if (Bait.bait.FindAll(x => x.PlayerId == target.PlayerId).Count > 0)
        {
            var reportDelay = (float)rnd.Next((int)Bait.reportDelayMin, (int)Bait.reportDelayMax + 1);
            Bait.active.Add(deadPlayer, reportDelay);

            if (Bait.showKillFlash && __instance == PlayerControl.LocalPlayer)
                Helpers.showFlash(new Color(204f / 255f, 102f / 255f, 0f / 255f));
        }

        // Add Bloody Modifier
        if (Bloody.bloody.FindAll(x => x.PlayerId == target.PlayerId).Count > 0)
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.Bloody, SendOption.Reliable);
            writer.Write(__instance.PlayerId);
            writer.Write(target.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.bloody(__instance.PlayerId, target.PlayerId);
        }

        // VIP Modifier
        if (Vip.vip.FindAll(x => x.PlayerId == target.PlayerId).Count > 0)
        {
            var color = Color.yellow;
            if (Vip.showColor)
            {
                color = Color.white;
                if (target.Data.Role.IsImpostor) color = Color.red;
                else if (CustomRoleManager.getRoleInfoForPlayer(target, false).FirstOrDefault().isNeutral)
                    color = Color.blue;
            }

            Helpers.showFlash(color, 1.5f);
        }

        // HideNSeek
        if (HideNSeek.isHideNSeekGM)
        {
            var visibleCounter = 0;
            var bottomLeft = IntroCutsceneOnDestroyPatch.bottomLeft + new Vector3(-0.25f, -0.25f, 0);
            foreach (var p in PlayerControl.AllPlayerControls)
            {
                if (!TORMapOptions.playerIcons.ContainsKey(p.PlayerId) || p.Data.Role.IsImpostor) continue;
                if (p.Data.IsDead || p.Data.Disconnected)
                {
                    TORMapOptions.playerIcons[p.PlayerId].gameObject.SetActive(false);
                }
                else
                {
                    TORMapOptions.playerIcons[p.PlayerId].transform.localPosition =
                        bottomLeft + Vector3.right * visibleCounter * 0.35f;
                    visibleCounter++;
                }
            }
        }

        // Snitch
        if (Snitch.snitch != null && PlayerControl.LocalPlayer.PlayerId == Snitch.snitch.PlayerId &&
            MapBehaviourPatch.herePoints.Keys.Any(x => x == target.PlayerId))
            foreach (var a in MapBehaviourPatch.herePoints.Where(x => x.Key == target.PlayerId))
            {
                Object.Destroy(a.Value);
                MapBehaviourPatch.herePoints.Remove(a.Key);
            }

        // Schrödinger's Cat: revive if killed without team (handled by OnMurderPlayer in SchrodingersCat)
        // RoleBase lifecycle
        CustomRoleManager.Instance.OnMurderPlayer(__instance, target);
        CustomRoleManager.Instance.OnPlayerDeath(target);
    }
}

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetKillTimer))]
internal class PlayerControlSetCoolDownPatch
{
    public static bool Prefix(PlayerControl __instance, [HarmonyArgument(0)] float time)
    {
        if (GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek) return true;
        if (GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown <= 0f) return false;
        var multiplier = 1f;
        var addition = 0f;
        if (Mini.mini != null && PlayerControl.LocalPlayer == Mini.mini) multiplier = Mini.isGrownUp() ? 0.66f : 2f;
        if (BountyHunter.bountyHunter != null && PlayerControl.LocalPlayer == BountyHunter.bountyHunter)
            addition = BountyHunter.punishmentTime;

        __instance.killTimer = Mathf.Clamp(time, 0f,
            GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown * multiplier + addition);
        FastDestroyableSingleton<HudManager>.Instance.KillButton.SetCoolDown(__instance.killTimer,
            GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown * multiplier + addition);
        return false;
    }
}

[HarmonyPatch(typeof(KillAnimation._CoPerformKill_d__2), "MoveNext")]
internal class KillAnimationCoPerformKillPatch
{
    public static bool hideNextAnimation;

    public static void Prefix(KillAnimation._CoPerformKill_d__2 __instance)
    {
        if (hideNextAnimation)
            __instance.source = __instance.target;
        hideNextAnimation = false;
    }
}

[HarmonyPatch(typeof(KillAnimation), nameof(KillAnimation.SetMovement))]
internal class KillAnimationSetMovementPatch
{
    private static int? colorId;

    public static void Prefix(PlayerControl source, bool canMove)
    {
        var color = source.cosmetics.currentBodySprite.BodySprite.material.GetColor("_BodyColor");
        if (Morphling.morphling != null && source.Data.PlayerId == Morphling.morphling.PlayerId)
        {
            var index = Palette.PlayerColors.IndexOf(color);
            if (index != -1) colorId = index;
        }
    }

    public static void Postfix(PlayerControl source, bool canMove)
    {
        if (colorId.HasValue) source.RawSetColor(colorId.Value);
        colorId = null;
    }
}

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Exiled))]
public static class ExilePlayerPatch
{
    public static void Postfix(PlayerControl __instance)
    {
        // Collect dead player info
        var deadPlayer = new DeadPlayer(__instance, DateTime.UtcNow, DeadPlayer.CustomDeathReason.Exile, null);
        deadPlayers.Add(deadPlayer);


        // Remove fake tasks when player dies
        if (__instance.hasFakeTasks() || __instance == Lawyer.lawyer || __instance == Pursuer.pursuer ||
            __instance == Thief.thief)
            __instance.clearAllTasks();

        // Lover suicide trigger on exile
        if ((Lovers.lover1 != null && __instance == Lovers.lover1) ||
            (Lovers.lover2 != null && __instance == Lovers.lover2))
        {
            var otherLover = __instance == Lovers.lover1 ? Lovers.lover2 : Lovers.lover1;
            if (otherLover != null && !otherLover.Data.IsDead && Lovers.bothDie)
            {
                otherLover.Exiled();
                overrideDeathReasonAndKiller(otherLover, DeadPlayer.CustomDeathReason.LoverSuicide);
            }
        }

        // Sidekick promotion trigger on exile
        if (Sidekick.promotesToJackal && Sidekick.sidekick != null && !Sidekick.sidekick.Data.IsDead &&
            __instance == Jackal.jackal && Jackal.jackal == PlayerControl.LocalPlayer)
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.SidekickPromotes, SendOption.Reliable);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.sidekickPromotes();
        }

        // Pursuer promotion trigger on exile & suicide (the host sends the call such that everyone recieves the update before a possible game End)
        if (Lawyer.lawyer != null && __instance == Lawyer.target)
        {
            var lawyer = Lawyer.lawyer;
            if (AmongUsClient.Instance.AmHost &&
                ((Lawyer.target != Jester.jester && !Lawyer.isProsecutor) || Lawyer.targetWasGuessed))
            {
                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.LawyerPromotesToPursuer, SendOption.Reliable);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.lawyerPromotesToPursuer();
            }

            if (!Lawyer.targetWasGuessed && !Lawyer.isProsecutor)
            {
                if (Lawyer.lawyer != null) Lawyer.lawyer.Exiled();
                if (Pursuer.pursuer != null) Pursuer.pursuer.Exiled();

                var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                    (byte)CustomRPC.ShareGhostInfo, SendOption.Reliable);
                writer.Write(PlayerControl.LocalPlayer.PlayerId);
                writer.Write((byte)RPCProcedure.GhostInfoTypes.DeathReasonAndKiller);
                writer.Write(lawyer.PlayerId);
                writer.Write((byte)DeadPlayer.CustomDeathReason.LawyerSuicide);
                writer.Write(lawyer.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                overrideDeathReasonAndKiller(lawyer, DeadPlayer.CustomDeathReason.LawyerSuicide,
                    lawyer); // TODO: only executed on host?!
            }
        }

        // RoleBase lifecycle
        CustomRoleManager.Instance.OnPlayerExiled(__instance);
    }
}

[HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.FixedUpdate))]
public static class PlayerPhysicsFixedUpdate
{
    public static void Postfix(PlayerPhysics __instance)
    {
        var shouldInvert = Invert.invert.FindAll(x => x.PlayerId == PlayerControl.LocalPlayer.PlayerId).Count > 0 &&
                           Invert.meetings > 0;
        if (__instance.AmOwner &&
            AmongUsClient.Instance &&
            AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started &&
            !PlayerControl.LocalPlayer.Data.IsDead &&
            shouldInvert &&
            GameData.Instance &&
            __instance.myPlayer.CanMove)
            __instance.body.velocity *= -1;
    }
}

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.IsFlashlightEnabled))]
public static class IsFlashlightEnabledPatch
{
    public static bool Prefix(ref bool __result)
    {
        if (GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek)
            return true;
        __result = false;
        if (!PlayerControl.LocalPlayer.Data.IsDead && Lighter.lighter != null &&
            Lighter.lighter.PlayerId == PlayerControl.LocalPlayer.PlayerId) __result = true;

        return false;
    }
}

[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.AdjustLighting))]
public static class AdjustLight
{
    public static bool Prefix(PlayerControl __instance)
    {
        if (__instance == null || PlayerControl.LocalPlayer == null || Lighter.lighter == null) return true;

        var hasFlashlight = !PlayerControl.LocalPlayer.Data.IsDead &&
                            Lighter.lighter.PlayerId == PlayerControl.LocalPlayer.PlayerId;
        __instance.SetFlashlightInputMethod();
        __instance.lightSource.SetupLightingForGameplay(hasFlashlight, Lighter.flashlightWidth,
            __instance.TargetFlashlight.transform);

        return false;
    }
}

[HarmonyPatch(typeof(RoleManager), nameof(RoleManager.AssignRoleOnDeath))]
public static class AssignRoleOnDeathPatch
{
    public static bool Prefix(PlayerControl player)
    {
        // Prevent ghost role assignment for Schrödinger's Cat when it has no team yet
        if (SchrodingersCat.cat != null && player == SchrodingersCat.cat && !SchrodingersCat.hasTeam())
            return false;
        return true;
    }
}

[HarmonyPatch(typeof(GameData), nameof(GameData.HandleDisconnect), typeof(PlayerControl), typeof(DisconnectReasons))]
public static class GameDataHandleDisconnectPatch
{
    public static void Prefix(GameData __instance, PlayerControl player, DisconnectReasons reason)
    {
        if (MeetingHud.Instance) MeetingHudPatch.swapperCheckAndReturnSwap(MeetingHud.Instance, player.PlayerId);
    }
}