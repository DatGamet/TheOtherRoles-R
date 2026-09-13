using System.Collections.Generic;
using TheOtherRoles.Modules;
using TheOtherRoles.Utilities;
using UnityEngine;
using Types = TheOtherRoles.Modules.CustomOption.CustomOptionType;

namespace TheOtherRoles;

public class CustomOptionHolder
{
    public static TranslationInfo[] rates = new[]
        {new TranslationInfo("0%"), new TranslationInfo("10%"), new TranslationInfo("20%"), new TranslationInfo("30%"), new TranslationInfo("40%"), new TranslationInfo("50%"), new TranslationInfo("60%"), new TranslationInfo("70%"), new TranslationInfo("80%"), new TranslationInfo("90%"), new TranslationInfo("100%")};

    public static TranslationInfo[] ratesModifier = new[]
        { new TranslationInfo("1"), new TranslationInfo("2"), new TranslationInfo("3"), new TranslationInfo("4"), new TranslationInfo("5"), new TranslationInfo("6"), new TranslationInfo("7"), new TranslationInfo("8"), new TranslationInfo("9"), new TranslationInfo("10"), new TranslationInfo("11"), new TranslationInfo("12"), new TranslationInfo("13"), new TranslationInfo("14"), new TranslationInfo("15") };

    public static TranslationInfo[] presets = new[]
    {
        new TranslationInfo("Opt-General", 1), new TranslationInfo("Opt-General", 2), new TranslationInfo("Opt-General", 3), new TranslationInfo("Opt-General", 4),
        new TranslationInfo("Opt-General", 5), new TranslationInfo("Opt-General", 6), new TranslationInfo("Opt-General", 7), new TranslationInfo("Opt-General", 8)
    };

    public static CustomOption presetSelection;
    public static CustomOption activateRoles;
    public static CustomOption crewmateRolesCountMin;
    public static CustomOption crewmateRolesCountMax;
    public static CustomOption crewmateRolesFill;
    public static CustomOption neutralRolesCountMin;
    public static CustomOption neutralRolesCountMax;
    public static CustomOption impostorRolesCountMin;
    public static CustomOption impostorRolesCountMax;
    public static CustomOption modifiersCountMin;
    public static CustomOption modifiersCountMax;

    public static CustomOption isDraftMode;
    public static CustomOption draftModeAmountOfChoices;
    public static CustomOption draftModeTimeToChoose;
    public static CustomOption draftModeShowRoles;
    public static CustomOption draftModeHideImpRoles;
    public static CustomOption draftModeHideNeutralRoles;

    public static CustomOption anyPlayerCanStopStart;
    public static CustomOption enableEventMode;
    public static CustomOption eventReallyNoMini;
    public static CustomOption eventKicksPerRound;
    public static CustomOption eventHeavyAge;
    public static CustomOption deadImpsBlockSabotage;

    public static CustomOption mafiaSpawnRate;
    public static CustomOption janitorCooldown;

    public static CustomOption morphlingSpawnRate;
    public static CustomOption morphlingCooldown;
    public static CustomOption morphlingDuration;

    public static CustomOption camouflagerSpawnRate;
    public static CustomOption camouflagerCooldown;
    public static CustomOption camouflagerDuration;

    public static CustomOption vampireSpawnRate;
    public static CustomOption vampireKillDelay;
    public static CustomOption vampireCooldown;
    public static CustomOption vampireCanKillNearGarlics;

    public static CustomOption eraserSpawnRate;
    public static CustomOption eraserCooldown;
    public static CustomOption eraserCanEraseAnyone;
    public static CustomOption guesserSpawnRate;
    public static CustomOption guesserIsImpGuesserRate;
    public static CustomOption guesserNumberOfShots;
    public static CustomOption guesserHasMultipleShotsPerMeeting;
    public static CustomOption guesserKillsThroughShield;
    public static CustomOption guesserEvilCanKillSpy;
    public static CustomOption guesserSpawnBothRate;
    public static CustomOption guesserCantGuessSnitchIfTaksDone;

    public static CustomOption jesterSpawnRate;
    public static CustomOption jesterCanCallEmergency;
    public static CustomOption jesterHasImpostorVision;

    public static CustomOption arsonistSpawnRate;
    public static CustomOption arsonistCooldown;
    public static CustomOption arsonistDuration;

    public static CustomOption jackalSpawnRate;
    public static CustomOption jackalKillCooldown;
    public static CustomOption jackalCreateSidekickCooldown;
    public static CustomOption jackalCanSabotageLights;
    public static CustomOption jackalCanUseVents;
    public static CustomOption jackalCanCreateSidekick;
    public static CustomOption sidekickPromotesToJackal;
    public static CustomOption sidekickCanKill;
    public static CustomOption sidekickCanUseVents;
    public static CustomOption sidekickCanSabotageLights;
    public static CustomOption jackalPromotedFromSidekickCanCreateSidekick;
    public static CustomOption jackalCanCreateSidekickFromImpostor;
    public static CustomOption jackalAndSidekickHaveImpostorVision;

    public static CustomOption bountyHunterSpawnRate;
    public static CustomOption bountyHunterBountyDuration;
    public static CustomOption bountyHunterReducedCooldown;
    public static CustomOption bountyHunterPunishmentTime;
    public static CustomOption bountyHunterShowArrow;
    public static CustomOption bountyHunterArrowUpdateIntervall;

    public static CustomOption witchSpawnRate;
    public static CustomOption witchCooldown;
    public static CustomOption witchAdditionalCooldown;
    public static CustomOption witchCanSpellAnyone;
    public static CustomOption witchSpellCastingDuration;
    public static CustomOption witchTriggerBothCooldowns;
    public static CustomOption witchVoteSavesTargets;

    public static CustomOption ninjaSpawnRate;
    public static CustomOption ninjaCooldown;
    public static CustomOption ninjaKnowsTargetLocation;
    public static CustomOption ninjaTraceTime;
    public static CustomOption ninjaTraceColorTime;
    public static CustomOption ninjaInvisibleDuration;

    public static CustomOption mayorSpawnRate;
    public static CustomOption mayorCanSeeVoteColors;
    public static CustomOption mayorTasksNeededToSeeVoteColors;
    public static CustomOption mayorMeetingButton;
    public static CustomOption mayorMaxRemoteMeetings;
    public static CustomOption mayorChooseSingleVote;

    public static CustomOption portalmakerSpawnRate;
    public static CustomOption portalmakerCooldown;
    public static CustomOption portalmakerUsePortalCooldown;
    public static CustomOption portalmakerLogOnlyColorType;
    public static CustomOption portalmakerLogHasTime;
    public static CustomOption portalmakerCanPortalFromAnywhere;

    public static CustomOption engineerSpawnRate;
    public static CustomOption engineerNumberOfFixes;
    public static CustomOption engineerHighlightForImpostors;
    public static CustomOption engineerHighlightForTeamJackal;

    public static CustomOption sheriffSpawnRate;
    public static CustomOption sheriffCooldown;
    public static CustomOption sheriffCanKillNeutrals;
    public static CustomOption deputySpawnRate;

    public static CustomOption deputyNumberOfHandcuffs;
    public static CustomOption deputyHandcuffCooldown;
    public static CustomOption deputyGetsPromoted;
    public static CustomOption deputyKeepsHandcuffs;
    public static CustomOption deputyHandcuffDuration;
    public static CustomOption deputyKnowsSheriff;

    public static CustomOption lighterSpawnRate;
    public static CustomOption lighterModeLightsOnVision;
    public static CustomOption lighterModeLightsOffVision;
    public static CustomOption lighterFlashlightWidth;

    public static CustomOption detectiveSpawnRate;
    public static CustomOption detectiveAnonymousFootprints;
    public static CustomOption detectiveFootprintIntervall;
    public static CustomOption detectiveFootprintDuration;
    public static CustomOption detectiveReportNameDuration;
    public static CustomOption detectiveReportColorDuration;

    public static CustomOption timeMasterSpawnRate;
    public static CustomOption timeMasterCooldown;
    public static CustomOption timeMasterRewindTime;
    public static CustomOption timeMasterShieldDuration;

    public static CustomOption medicSpawnRate;
    public static CustomOption medicShowShielded;
    public static CustomOption medicShowAttemptToShielded;
    public static CustomOption medicSetOrShowShieldAfterMeeting;
    public static CustomOption medicShowAttemptToMedic;
    public static CustomOption medicSetShieldAfterMeeting;

    public static CustomOption swapperSpawnRate;
    public static CustomOption swapperCanCallEmergency;
    public static CustomOption swapperCanOnlySwapOthers;
    public static CustomOption swapperSwapsNumber;
    public static CustomOption swapperRechargeTasksNumber;

    public static CustomOption seerSpawnRate;
    public static CustomOption seerMode;
    public static CustomOption seerSoulDuration;
    public static CustomOption seerLimitSoulDuration;

    public static CustomOption hackerSpawnRate;
    public static CustomOption hackerCooldown;
    public static CustomOption hackerHackeringDuration;
    public static CustomOption hackerOnlyColorType;
    public static CustomOption hackerToolsNumber;
    public static CustomOption hackerRechargeTasksNumber;
    public static CustomOption hackerNoMove;

    public static CustomOption trackerSpawnRate;
    public static CustomOption trackerUpdateIntervall;
    public static CustomOption trackerResetTargetAfterMeeting;
    public static CustomOption trackerCanTrackCorpses;
    public static CustomOption trackerCorpsesTrackingCooldown;
    public static CustomOption trackerCorpsesTrackingDuration;
    public static CustomOption trackerTrackingMethod;

    public static CustomOption snitchSpawnRate;
    public static CustomOption snitchLeftTasksForReveal;
    public static CustomOption snitchMode;
    public static CustomOption snitchTargets;

    public static CustomOption spySpawnRate;
    public static CustomOption spyCanDieToSheriff;
    public static CustomOption spyImpostorsCanKillAnyone;
    public static CustomOption spyCanEnterVents;
    public static CustomOption spyHasImpostorVision;

    public static CustomOption tricksterSpawnRate;
    public static CustomOption tricksterPlaceBoxCooldown;
    public static CustomOption tricksterLightsOutCooldown;
    public static CustomOption tricksterLightsOutDuration;

    public static CustomOption cleanerSpawnRate;
    public static CustomOption cleanerCooldown;

    public static CustomOption warlockSpawnRate;
    public static CustomOption warlockCooldown;
    public static CustomOption warlockRootTime;

    public static CustomOption securityGuardSpawnRate;
    public static CustomOption securityGuardCooldown;
    public static CustomOption securityGuardTotalScrews;
    public static CustomOption securityGuardCamPrice;
    public static CustomOption securityGuardVentPrice;
    public static CustomOption securityGuardCamDuration;
    public static CustomOption securityGuardCamMaxCharges;
    public static CustomOption securityGuardCamRechargeTasksNumber;
    public static CustomOption securityGuardNoMove;

    public static CustomOption vultureSpawnRate;
    public static CustomOption vultureCooldown;
    public static CustomOption vultureNumberToWin;
    public static CustomOption vultureCanUseVents;
    public static CustomOption vultureShowArrows;

    public static CustomOption mediumSpawnRate;
    public static CustomOption mediumCooldown;
    public static CustomOption mediumDuration;
    public static CustomOption mediumOneTimeUse;
    public static CustomOption mediumChanceAdditionalInfo;

    public static CustomOption lawyerSpawnRate;
    public static CustomOption lawyerIsProsecutorChance;
    public static CustomOption lawyerTargetCanBeJester;
    public static CustomOption lawyerVision;
    public static CustomOption lawyerKnowsRole;
    public static CustomOption lawyerCanCallEmergency;
    public static CustomOption pursuerCooldown;
    public static CustomOption pursuerBlanksNumber;

    public static CustomOption thiefSpawnRate;
    public static CustomOption thiefCooldown;
    public static CustomOption thiefHasImpVision;
    public static CustomOption thiefCanUseVents;
    public static CustomOption thiefCanKillSheriff;
    public static CustomOption thiefCanStealWithGuess;

    public static CustomOption schrodingersCatSpawnRate;
    public static CustomOption schrodingersCatKillCooldown;
    public static CustomOption schrodingersCatKillsKiller;
    public static CustomOption schrodingersCatCantKillUntilLastOne;
    public static CustomOption schrodingersCatExileType;
    public static CustomOption schrodingersCatHideRole;
    public static CustomOption schrodingersCatCanChooseTeam;

    public static CustomOption trapperSpawnRate;
    public static CustomOption trapperCooldown;
    public static CustomOption trapperMaxCharges;
    public static CustomOption trapperRechargeTasksNumber;
    public static CustomOption trapperTrapNeededTriggerToReveal;
    public static CustomOption trapperAnonymousMap;
    public static CustomOption trapperInfoType;
    public static CustomOption trapperTrapDuration;

    public static CustomOption bomberSpawnRate;
    public static CustomOption bomberBombDestructionTime;
    public static CustomOption bomberBombDestructionRange;
    public static CustomOption bomberBombHearRange;
    public static CustomOption bomberDefuseDuration;
    public static CustomOption bomberBombCooldown;
    public static CustomOption bomberBombActiveAfter;

    public static CustomOption yoyoSpawnRate;
    public static CustomOption yoyoBlinkDuration;
    public static CustomOption yoyoMarkCooldown;
    public static CustomOption yoyoMarkStaysOverMeeting;
    public static CustomOption yoyoHasAdminTable;
    public static CustomOption yoyoAdminTableCooldown;
    public static CustomOption yoyoSilhouetteVisibility;


    public static CustomOption modifiersAreHidden;

    public static CustomOption modifierBait;
    public static CustomOption modifierBaitQuantity;
    public static CustomOption modifierBaitReportDelayMin;
    public static CustomOption modifierBaitReportDelayMax;
    public static CustomOption modifierBaitShowKillFlash;

    public static CustomOption modifierLover;
    public static CustomOption modifierLoverImpLoverRate;
    public static CustomOption modifierLoverBothDie;
    public static CustomOption modifierLoverEnableChat;

    public static CustomOption modifierBloody;
    public static CustomOption modifierBloodyQuantity;
    public static CustomOption modifierBloodyDuration;

    public static CustomOption modifierAntiTeleport;
    public static CustomOption modifierAntiTeleportQuantity;

    public static CustomOption modifierTieBreaker;

    public static CustomOption modifierSunglasses;
    public static CustomOption modifierSunglassesQuantity;
    public static CustomOption modifierSunglassesVision;

    public static CustomOption modifierMini;
    public static CustomOption modifierMiniGrowingUpDuration;
    public static CustomOption modifierMiniGrowingUpInMeeting;

    public static CustomOption modifierVip;
    public static CustomOption modifierVipQuantity;
    public static CustomOption modifierVipShowColor;

    public static CustomOption modifierInvert;
    public static CustomOption modifierInvertQuantity;
    public static CustomOption modifierInvertDuration;

    public static CustomOption modifierChameleon;
    public static CustomOption modifierChameleonQuantity;
    public static CustomOption modifierChameleonHoldDuration;
    public static CustomOption modifierChameleonFadeDuration;
    public static CustomOption modifierChameleonMinVisibility;

    public static CustomOption modifierArmored;

    public static CustomOption modifierShifter;
    public static CustomOption modifierShifterShiftsMedicShield;

    public static CustomOption maxNumberOfMeetings;
    public static CustomOption blockSkippingInEmergencyMeetings;
    public static CustomOption noVoteIsSelfVote;
    public static CustomOption hidePlayerNames;
    public static CustomOption allowParallelMedBayScans;
    public static CustomOption shieldFirstKill;
    public static CustomOption finishTasksBeforeHauntingOrZoomingOut;
    public static CustomOption camsNightVision;
    public static CustomOption camsNoNightVisionIfImpVision;

    public static CustomOption dynamicMap;
    public static CustomOption dynamicMapEnableSkeld;
    public static CustomOption dynamicMapEnableMira;
    public static CustomOption dynamicMapEnablePolus;
    public static CustomOption dynamicMapEnableAirShip;
    public static CustomOption dynamicMapEnableFungle;
    public static CustomOption dynamicMapEnableSubmerged;
    public static CustomOption dynamicMapSeparateSettings;

    //Guesser Gamemode
    public static CustomOption guesserGamemodeCrewNumber;
    public static CustomOption guesserGamemodeNeutralNumber;
    public static CustomOption guesserGamemodeImpNumber;
    public static CustomOption guesserForceJackalGuesser;
    public static CustomOption guesserForceThiefGuesser;
    public static CustomOption guesserGamemodeHaveModifier;
    public static CustomOption guesserGamemodeNumberOfShots;
    public static CustomOption guesserGamemodeHasMultipleShotsPerMeeting;
    public static CustomOption guesserGamemodeKillsThroughShield;
    public static CustomOption guesserGamemodeEvilCanKillSpy;
    public static CustomOption guesserGamemodeCantGuessSnitchIfTaksDone;
    public static CustomOption guesserGamemodeCrewGuesserNumberOfTasks;
    public static CustomOption guesserGamemodeSidekickIsAlwaysGuesser;

    // Hide N Seek Gamemode
    public static CustomOption hideNSeekHunterCount;
    public static CustomOption hideNSeekKillCooldown;
    public static CustomOption hideNSeekHunterVision;
    public static CustomOption hideNSeekHuntedVision;
    public static CustomOption hideNSeekTimer;
    public static CustomOption hideNSeekCommonTasks;
    public static CustomOption hideNSeekShortTasks;
    public static CustomOption hideNSeekLongTasks;
    public static CustomOption hideNSeekTaskWin;
    public static CustomOption hideNSeekTaskPunish;
    public static CustomOption hideNSeekCanSabotage;
    public static CustomOption hideNSeekMap;
    public static CustomOption hideNSeekHunterWaiting;

    public static CustomOption hunterLightCooldown;
    public static CustomOption hunterLightDuration;
    public static CustomOption hunterLightVision;
    public static CustomOption hunterLightPunish;
    public static CustomOption hunterAdminCooldown;
    public static CustomOption hunterAdminDuration;
    public static CustomOption hunterAdminPunish;
    public static CustomOption hunterArrowCooldown;
    public static CustomOption hunterArrowDuration;
    public static CustomOption hunterArrowPunish;

    public static CustomOption huntedShieldCooldown;
    public static CustomOption huntedShieldDuration;
    public static CustomOption huntedShieldRewindTime;
    public static CustomOption huntedShieldNumber;

    // Prop Hunt Settings
    public static CustomOption propHuntMap;
    public static CustomOption propHuntTimer;
    public static CustomOption propHuntNumberOfHunters;
    public static CustomOption hunterInitialBlackoutTime;
    public static CustomOption hunterMissCooldown;
    public static CustomOption hunterHitCooldown;
    public static CustomOption hunterMaxMissesBeforeDeath;
    public static CustomOption propBecomesHunterWhenFound;
    public static CustomOption propHunterVision;
    public static CustomOption propVision;
    public static CustomOption propHuntRevealCooldown;
    public static CustomOption propHuntRevealDuration;
    public static CustomOption propHuntRevealPunish;
    public static CustomOption propHuntUnstuckCooldown;
    public static CustomOption propHuntUnstuckDuration;
    public static CustomOption propHuntInvisCooldown;
    public static CustomOption propHuntInvisDuration;
    public static CustomOption propHuntSpeedboostCooldown;
    public static CustomOption propHuntSpeedboostDuration;
    public static CustomOption propHuntSpeedboostSpeed;
    public static CustomOption propHuntSpeedboostEnabled;
    public static CustomOption propHuntInvisEnabled;
    public static CustomOption propHuntAdminCooldown;
    public static CustomOption propHuntFindCooldown;
    public static CustomOption propHuntFindDuration;

    internal static Dictionary<byte, byte[]> blockedRolePairings = new();

    public static string cs(Color c, string s)
    {
        return string.Format("<color=#{0:X2}{1:X2}{2:X2}{3:X2}>{4}</color>", ToByte(c.r), ToByte(c.g), ToByte(c.b),
            ToByte(c.a), s);
    }

    private static byte ToByte(float f)
    {
        f = Mathf.Clamp01(f);
        return (byte)(f * 255);
    }

    public static bool isMapSelectionOption(CustomOption option)
    {
        return option == propHuntMap && option == hideNSeekMap;
    }

    public static void Load()
    {
        CustomOption.vanillaSettings = TheOtherRolesPlugin.Instance.Config.Bind("Preset0", "VanillaOptions", "");

        // Role Options
        presetSelection = new CustomOption(0, Types.General, new TranslationInfo("Opt-General", 9, new Color(204f / 255f, 204f / 255f, 0, 1f)),
            presets, "", null, true);

        if (EventUtility.canBeEnabled)
            enableEventMode = CustomOption.Create(Types.General, new TranslationInfo("Opt-General", 10, Color.green), true,
                null, true);

        isDraftMode = CustomOption.Create(Types.General, new TranslationInfo("Opt-General", 11, Color.yellow), false, null, true,
            null, new TranslationInfo("Opt-Heading", 1));
        draftModeAmountOfChoices = CustomOption.Create(Types.General,
            new TranslationInfo("Opt-General", 12, Color.yellow), 5f, 2f, 15f, 1f, isDraftMode);
        draftModeTimeToChoose = CustomOption.Create(Types.General, new TranslationInfo("Opt-General", 13, Color.yellow), 5f, 3f,
            20f, 1f, isDraftMode);
        draftModeShowRoles =
            CustomOption.Create(Types.General, new TranslationInfo("Opt-General", 14, Color.yellow), false, isDraftMode);
        draftModeHideImpRoles = CustomOption.Create(Types.General, new TranslationInfo("Opt-General", 15, Color.yellow), false,
            draftModeShowRoles);
        draftModeHideNeutralRoles = CustomOption.Create(Types.General, new TranslationInfo("Opt-General", 16, Color.yellow),
            false, draftModeShowRoles);

        // Using new id's for the options to not break compatibilty with older versions
        crewmateRolesCountMin = CustomOption.Create(Types.General,
            new TranslationInfo("Opt-General", 17, new Color(204f / 255f, 204f / 255f, 0, 1f)), 15f, 0f, 15f, 1f, null, true,
            heading: new TranslationInfo("Opt-Heading", 2));
        crewmateRolesCountMax = CustomOption.Create(Types.General,
            new TranslationInfo("Opt-General", 18, new Color(204f / 255f, 204f / 255f, 0, 1f)), 15f, 0f, 15f, 1f);
        neutralRolesCountMin = CustomOption.Create(Types.General,
            new TranslationInfo("Opt-General", 19, new Color(204f / 255f, 204f / 255f, 0, 1f)), 15f, 0f, 15f, 1f);
        neutralRolesCountMax = CustomOption.Create(Types.General,
            new TranslationInfo("Opt-General", 20, new Color(204f / 255f, 204f / 255f, 0, 1f)), 15f, 0f, 15f, 1f);
        impostorRolesCountMin = CustomOption.Create(Types.General,
            new TranslationInfo("Opt-General", 21, new Color(204f / 255f, 204f / 255f, 0, 1f)), 15f, 0f, 15f, 1f);
        impostorRolesCountMax = CustomOption.Create(Types.General,
            new TranslationInfo("Opt-General", 22, new Color(204f / 255f, 204f / 255f, 0, 1f)), 15f, 0f, 15f, 1f);
        modifiersCountMin = CustomOption.Create(Types.General,
            new TranslationInfo("Opt-General", 23, new Color(204f / 255f, 204f / 255f, 0, 1f)), 15f, 0f, 15f, 1f);
        modifiersCountMax = CustomOption.Create(Types.General,
            new TranslationInfo("Opt-General", 24, new Color(204f / 255f, 204f / 255f, 0, 1f)), 15f, 0f, 15f, 1f);
        crewmateRolesFill = CustomOption.Create(Types.General,
            new TranslationInfo("Opt-General", 25, new Color(204f / 255f, 204f / 255f, 0, 1f)), false);

        mafiaSpawnRate = CustomOption.Create(Types.Impostor, new TranslationInfo(RoleId.Mafioso, Janitor.color), rates, null, true);
        janitorCooldown =
            CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Mafia", 1), 30f, 10f, 60f, 2.5f, mafiaSpawnRate);

        morphlingSpawnRate =
            CustomOption.Create(Types.Impostor, new TranslationInfo(RoleId.Morphling, Morphling.color), rates, null, true);
        morphlingCooldown = CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Morphling", 1), 30f, 10f, 60f, 2.5f,
            morphlingSpawnRate);
        morphlingDuration =
            CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Morphling", 2), 10f, 1f, 20f, 0.5f, morphlingSpawnRate);

        camouflagerSpawnRate =
            CustomOption.Create(Types.Impostor, new TranslationInfo(RoleId.Camouflager, Camouflager.color), rates, null, true);
        camouflagerCooldown = CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Camouflager", 1), 30f, 10f, 60f, 2.5f,
            camouflagerSpawnRate);
        camouflagerDuration =
            CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Camouflager", 2), 10f, 1f, 20f, 0.5f, camouflagerSpawnRate);

        vampireSpawnRate = CustomOption.Create(Types.Impostor, new TranslationInfo(RoleId.Vampire, Vampire.color), rates, null, true);
        vampireKillDelay =
            CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Vampire", 1), 10f, 1f, 20f, 1f, vampireSpawnRate);
        vampireCooldown =
            CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Vampire", 2), 30f, 10f, 60f, 2.5f, vampireSpawnRate);
        vampireCanKillNearGarlics =
            CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Vampire", 3), true, vampireSpawnRate);

        eraserSpawnRate = CustomOption.Create(Types.Impostor, new TranslationInfo(RoleId.Eraser, Eraser.color), rates, null, true);
        eraserCooldown =
            CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Eraser", 1), 30f, 10f, 120f, 5f, eraserSpawnRate);
        eraserCanEraseAnyone =
            CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Eraser", 2), false, eraserSpawnRate);

        tricksterSpawnRate =
            CustomOption.Create(Types.Impostor, new TranslationInfo(RoleId.Trickster, Trickster.color), rates, null, true);
        tricksterPlaceBoxCooldown = CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Trickster", 1), 10f, 2.5f, 30f,
            2.5f, tricksterSpawnRate);
        tricksterLightsOutCooldown = CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Trickster", 2), 30f, 10f,
            60f, 5f, tricksterSpawnRate);
        tricksterLightsOutDuration = CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Trickster", 3), 15f, 5f,
            60f, 2.5f, tricksterSpawnRate);

        cleanerSpawnRate = CustomOption.Create(Types.Impostor, new TranslationInfo(RoleId.Cleaner, Cleaner.color), rates, null, true);
        cleanerCooldown =
            CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Cleaner", 1), 30f, 10f, 60f, 2.5f, cleanerSpawnRate);

        warlockSpawnRate = CustomOption.Create(Types.Impostor, new TranslationInfo(RoleId.Warlock, Warlock.color), rates, null, true);
        warlockCooldown =
            CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Warlock", 1), 30f, 10f, 60f, 2.5f, warlockSpawnRate);
        warlockRootTime =
            CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Warlock", 2), 5f, 0f, 15f, 1f, warlockSpawnRate);

        bountyHunterSpawnRate = CustomOption.Create(Types.Impostor, new TranslationInfo(RoleId.BountyHunter, BountyHunter.color), rates,
            null, true);
        bountyHunterBountyDuration = CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-BountyHunter", 1),
            60f, 10f, 180f, 10f, bountyHunterSpawnRate);
        bountyHunterReducedCooldown = CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-BountyHunter", 2), 2.5f,
            0f, 30f, 2.5f, bountyHunterSpawnRate);
        bountyHunterPunishmentTime = CustomOption.Create(Types.Impostor,
            new TranslationInfo("Opt-BountyHunter", 3), 20f, 0f, 60f, 2.5f, bountyHunterSpawnRate);
        bountyHunterShowArrow = CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-BountyHunter", 4), true,
            bountyHunterSpawnRate);
        bountyHunterArrowUpdateIntervall = CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-BountyHunter", 5), 15f, 2.5f,
            60f, 2.5f, bountyHunterShowArrow);

        witchSpawnRate = CustomOption.Create(Types.Impostor, new TranslationInfo(RoleId.Witch, Witch.color), rates, null, true);
        witchCooldown = CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Witch", 1), 30f, 10f, 120f, 5f,
            witchSpawnRate);
        witchAdditionalCooldown = CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Witch", 2), 10f, 0f, 60f,
            5f, witchSpawnRate);
        witchCanSpellAnyone = CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Witch", 3), false, witchSpawnRate);
        witchSpellCastingDuration = CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Witch", 4), 1f, 0f, 10f, 1f,
            witchSpawnRate);
        witchTriggerBothCooldowns =
            CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Witch", 5), true, witchSpawnRate);
        witchVoteSavesTargets = CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Witch", 6), true,
            witchSpawnRate);

        ninjaSpawnRate = CustomOption.Create(Types.Impostor, new TranslationInfo(RoleId.Ninja, Ninja.color), rates, null, true);
        ninjaCooldown =
            CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Ninja", 1), 30f, 10f, 120f, 5f, ninjaSpawnRate);
        ninjaKnowsTargetLocation =
            CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Ninja", 2), true, ninjaSpawnRate);
        ninjaTraceTime = CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Ninja", 3), 5f, 1f, 20f, 0.5f, ninjaSpawnRate);
        ninjaTraceColorTime = CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Ninja", 4), 2f, 0f, 20f,
            0.5f, ninjaSpawnRate);
        ninjaInvisibleDuration = CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Ninja", 5), 3f, 0f, 20f,
            1f, ninjaSpawnRate);

        bomberSpawnRate = CustomOption.Create(Types.Impostor, new TranslationInfo(RoleId.Bomber, Bomber.color), rates, null, true);
        bomberBombDestructionTime = CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Bomber", 1), 20f, 2.5f, 120f,
            2.5f, bomberSpawnRate);
        bomberBombDestructionRange = CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Bomber", 2), 50f, 5f, 150f,
            5f, bomberSpawnRate);
        bomberBombHearRange =
            CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Bomber", 3), 60f, 5f, 150f, 5f, bomberSpawnRate);
        bomberDefuseDuration = CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Bomber", 4), 3f, 0.5f, 30f, 0.5f,
            bomberSpawnRate);
        bomberBombCooldown =
            CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Bomber", 5), 15f, 2.5f, 30f, 2.5f, bomberSpawnRate);
        bomberBombActiveAfter = CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-Bomber", 6), 3f, 0.5f, 15f, 0.5f,
            bomberSpawnRate);


        yoyoSpawnRate = CustomOption.Create(Types.Impostor, new TranslationInfo(RoleId.Yoyo, Yoyo.color), rates, null, true);
        yoyoBlinkDuration =
            CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-YoYo", 1), 20f, 2.5f, 120f, 2.5f, yoyoSpawnRate);
        yoyoMarkCooldown = CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-YoYo", 2), 20f, 2.5f, 120f, 2.5f,
            yoyoSpawnRate);
        yoyoMarkStaysOverMeeting = CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-YoYo", 3), true,
            yoyoSpawnRate);
        yoyoHasAdminTable = CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-YoYo", 4), true, yoyoSpawnRate);
        yoyoAdminTableCooldown = CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-YoYo", 5), 20f, 2.5f, 120f, 2.5f,
            yoyoHasAdminTable);
        yoyoSilhouetteVisibility = CustomOption.Create(Types.Impostor, new TranslationInfo("Opt-YoYo", 6),
            new[] { new TranslationInfo("0%"), new TranslationInfo("10%"), new TranslationInfo("20%"), new TranslationInfo("30%"), new TranslationInfo("40%"), new TranslationInfo("50%") }, yoyoSpawnRate);


        guesserSpawnRate = CustomOption.Create(Types.Neutral, new TranslationInfo(RoleId.NiceGuesser, Guesser.color), rates, null, true);
        guesserIsImpGuesserRate = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Guesser", 1),
            rates, guesserSpawnRate);
        guesserNumberOfShots = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Guesser", 2), 2f, 1f, 15f, 1f,
            guesserSpawnRate);
        guesserHasMultipleShotsPerMeeting = CustomOption.Create(Types.Neutral,
            new TranslationInfo("Opt-Guesser", 3), false, guesserSpawnRate);
        guesserKillsThroughShield =
            CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Guesser", 4), true, guesserSpawnRate);
        guesserEvilCanKillSpy =
            CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Guesser", 5), true, guesserSpawnRate);
        guesserSpawnBothRate =
            CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Guesser", 6), rates, guesserSpawnRate);
        guesserCantGuessSnitchIfTaksDone = CustomOption.Create(Types.Neutral,
            new TranslationInfo("Opt-Guesser", 7), true, guesserSpawnRate);

        jesterSpawnRate = CustomOption.Create(Types.Neutral, new TranslationInfo(RoleId.Jester, Jester.color), rates, null, true);
        jesterCanCallEmergency =
            CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Jester", 1), true, jesterSpawnRate);
        jesterHasImpostorVision =
            CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Jester", 2), false, jesterSpawnRate);

        arsonistSpawnRate = CustomOption.Create(Types.Neutral, new TranslationInfo(RoleId.Arsonist, Arsonist.color), rates, null, true);
        arsonistCooldown = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Arsonist", 1), 12.5f, 2.5f, 60f, 2.5f,
            arsonistSpawnRate);
        arsonistDuration = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Arsonist", 2), 3f, 1f, 10f, 1f,
            arsonistSpawnRate);

        jackalSpawnRate = CustomOption.Create(Types.Neutral, new TranslationInfo(RoleId.Jackal, Jackal.color), rates, null, true);
        jackalKillCooldown = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Jackal", 1), 30f, 10f, 60f,
            2.5f, jackalSpawnRate);
        jackalCanUseVents = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Jackal", 3), true, jackalSpawnRate);
        jackalCanSabotageLights =
            CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Jackal", 4), true, jackalSpawnRate);
        jackalCanCreateSidekick =
            CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Jackal", 5), false, jackalSpawnRate);
        jackalCreateSidekickCooldown = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Jackal", 2), 30f,
    10f, 60f, 2.5f, jackalCanCreateSidekick);
        sidekickPromotesToJackal = CustomOption.Create(Types.Neutral,
            new TranslationInfo("Opt-Jackal", 6), false, jackalCanCreateSidekick);
        sidekickCanKill = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Jackal", 7), false, jackalCanCreateSidekick);
        sidekickCanUseVents =
            CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Jackal", 8), true, jackalCanCreateSidekick);
        sidekickCanSabotageLights = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Jackal", 9), true,
            jackalCanCreateSidekick);
        jackalPromotedFromSidekickCanCreateSidekick = CustomOption.Create(Types.Neutral,
            new TranslationInfo("Opt-Jackal", 10), true, sidekickPromotesToJackal);
        jackalCanCreateSidekickFromImpostor = CustomOption.Create(Types.Neutral,
            new TranslationInfo("Opt-Jackal", 11), true, jackalCanCreateSidekick);
        jackalAndSidekickHaveImpostorVision = CustomOption.Create(Types.Neutral,
            new TranslationInfo("Opt-Jackal", 12), false, jackalSpawnRate);

        vultureSpawnRate = CustomOption.Create(Types.Neutral, new TranslationInfo(RoleId.Vulture, Vulture.color), rates, null, true);
        vultureCooldown =
            CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Vulture", 1), 15f, 10f, 60f, 2.5f, vultureSpawnRate);
        vultureNumberToWin = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Vulture", 2), 4f, 1f,
            10f, 1f, vultureSpawnRate);
        vultureCanUseVents = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Vulture", 3), true, vultureSpawnRate);
        vultureShowArrows = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Vulture", 4), true,
            vultureSpawnRate);

        lawyerSpawnRate = CustomOption.Create(Types.Neutral, new TranslationInfo(RoleId.Lawyer, Lawyer.color), rates, null, true);
        lawyerIsProsecutorChance = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Lawyer", 1),
            rates, lawyerSpawnRate);
        lawyerVision = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Lawyer", 2), 1f, 0.25f, 3f, 0.25f, lawyerSpawnRate);
        lawyerKnowsRole = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Lawyer", 3), false,
            lawyerSpawnRate);
        lawyerCanCallEmergency = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Lawyer", 4),
            true, lawyerSpawnRate);
        lawyerTargetCanBeJester =
            CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Lawyer", 5), false, lawyerSpawnRate);
        pursuerCooldown = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Lawyer", 6), 30f, 5f, 60f, 2.5f,
            lawyerSpawnRate);
        pursuerBlanksNumber = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Lawyer", 7), 5f, 1f, 20f, 1f,
            lawyerSpawnRate);

        mayorSpawnRate = CustomOption.Create(Types.Crewmate, new TranslationInfo(RoleId.Mayor, Mayor.color), rates, null, true);
        mayorCanSeeVoteColors =
            CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Mayor", 1), false, mayorSpawnRate);
        mayorTasksNeededToSeeVoteColors = CustomOption.Create(Types.Crewmate,
            new TranslationInfo("Opt-Mayor", 2), 5f, 0f, 20f, 1f, mayorCanSeeVoteColors);
        mayorMeetingButton = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Mayor", 3), true, mayorSpawnRate);
        mayorMaxRemoteMeetings = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Mayor", 4), 1f, 1f, 5f, 1f,
            mayorMeetingButton);
        mayorChooseSingleVote = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Mayor", 5),
            new[] { new TranslationInfo("Opt-General", 69), new TranslationInfo("Opt-Mayor", 101), new TranslationInfo("Opt-Mayor", 102) }, mayorSpawnRate);

        engineerSpawnRate = CustomOption.Create(Types.Crewmate, new TranslationInfo(RoleId.Engineer, Engineer.color), rates, null, true);
        engineerNumberOfFixes = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Engineer", 1), 1f, 1f, 3f, 1f,
            engineerSpawnRate);
        engineerHighlightForImpostors = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Engineer", 2), true,
            engineerSpawnRate);
        engineerHighlightForTeamJackal = CustomOption.Create(Types.Crewmate,
            new TranslationInfo("Opt-Engineer", 3), true, engineerSpawnRate);

        sheriffSpawnRate = CustomOption.Create(Types.Crewmate, new TranslationInfo(RoleId.Sheriff, Sheriff.color), rates, null, true);
        sheriffCooldown =
            CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Sheriff", 1), 30f, 10f, 60f, 2.5f, sheriffSpawnRate);
        sheriffCanKillNeutrals =
            CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Sheriff", 2), false, sheriffSpawnRate);
        deputySpawnRate = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Sheriff", 3), rates, sheriffSpawnRate);
        deputyNumberOfHandcuffs = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Sheriff", 4), 3f, 1f, 10f,
            1f, deputySpawnRate);
        deputyHandcuffCooldown =
            CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Sheriff", 5), 30f, 10f, 60f, 2.5f, deputySpawnRate);
        deputyHandcuffDuration =
            CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Sheriff", 6), 15f, 5f, 60f, 2.5f, deputySpawnRate);
        deputyKnowsSheriff = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Sheriff", 7), true,
            deputySpawnRate);
        deputyGetsPromoted = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Sheriff", 8),
            new[] { new TranslationInfo("Opt-General", 69), new TranslationInfo("Opt-Sheriff", 101), new TranslationInfo("Opt-Sheriff", 102) }, deputySpawnRate);
        deputyKeepsHandcuffs = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Sheriff", 9), true,
            deputyGetsPromoted);

        lighterSpawnRate = CustomOption.Create(Types.Crewmate, new TranslationInfo(RoleId.Lighter, Lighter.color), rates, null, true);
        lighterModeLightsOnVision = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Lighter", 1), 1.5f, 0.25f, 5f,
            0.25f, lighterSpawnRate);
        lighterModeLightsOffVision = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Lighter", 2), 0.5f, 0.25f, 5f,
            0.25f, lighterSpawnRate);
        lighterFlashlightWidth = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Lighter", 3), 0.3f, 0.1f, 1f, 0.1f,
            lighterSpawnRate);

        detectiveSpawnRate =
            CustomOption.Create(Types.Crewmate, new TranslationInfo(RoleId.Detective, Detective.color), rates, null, true);
        detectiveAnonymousFootprints =
            CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Detective", 1), false, detectiveSpawnRate);
        detectiveFootprintIntervall = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Detective", 2), 0.5f, 0.25f, 10f,
            0.25f, detectiveSpawnRate);
        detectiveFootprintDuration = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Detective", 3), 5f, 0.25f, 10f,
            0.25f, detectiveSpawnRate);
        detectiveReportNameDuration = CustomOption.Create(Types.Crewmate,
            new TranslationInfo("Opt-Detective", 4), 0, 0, 60, 2.5f, detectiveSpawnRate);
        detectiveReportColorDuration = CustomOption.Create(Types.Crewmate,
            new TranslationInfo("Opt-Detective", 5), 20, 0, 120, 2.5f, detectiveSpawnRate);

        timeMasterSpawnRate =
            CustomOption.Create(Types.Crewmate, new TranslationInfo(RoleId.TimeMaster, TimeMaster.color), rates, null, true);
        timeMasterCooldown = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-TimeMaster", 1), 30f, 10f, 120f, 2.5f,
            timeMasterSpawnRate);
        timeMasterRewindTime =
            CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-TimeMaster", 2), 3f, 1f, 10f, 1f, timeMasterSpawnRate);
        timeMasterShieldDuration = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-TimeMaster", 3), 3f, 1f, 20f,
            1f, timeMasterSpawnRate);

        medicSpawnRate = CustomOption.Create(Types.Crewmate, new TranslationInfo(RoleId.Medic, Medic.color), rates, null, true);
        medicShowShielded = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Medic", 1),
            new[] { new TranslationInfo("Opt-Medic", 100), new TranslationInfo("Opt-Medic", 101), new TranslationInfo("Opt-Medic", 102) }, medicSpawnRate);
        medicShowAttemptToShielded = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Medic", 2),
            false, medicSpawnRate);
        medicSetOrShowShieldAfterMeeting = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Medic", 3),
            new[] { new TranslationInfo("Opt-Medic", 110), new TranslationInfo("Opt-Medic", 111), new TranslationInfo("Opt-Medic", 112) }, medicSpawnRate);

        medicShowAttemptToMedic = CustomOption.Create(Types.Crewmate,
            new TranslationInfo("Opt-Medic", 4), false, medicSpawnRate);

        swapperSpawnRate = CustomOption.Create(Types.Crewmate, new TranslationInfo(RoleId.Swapper, Swapper.color), rates, null, true);
        swapperCanCallEmergency = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Swapper", 1), false,
            swapperSpawnRate);
        swapperCanOnlySwapOthers =
            CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Swapper", 2), false, swapperSpawnRate);

        swapperSwapsNumber =
            CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Swapper", 3), 1f, 0f, 5f, 1f, swapperSpawnRate);
        swapperRechargeTasksNumber = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Swapper", 4),
            2f, 1f, 10f, 1f, swapperSpawnRate);


        seerSpawnRate = CustomOption.Create(Types.Crewmate, new TranslationInfo(RoleId.Seer, Seer.color), rates, null, true);
        seerMode = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Seer", 1),
            new[] { new TranslationInfo("Opt-Seer", 100), new TranslationInfo("Opt-Seer", 101), new TranslationInfo("Opt-Seer", 102) }, seerSpawnRate);
        seerLimitSoulDuration =
            CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Seer", 2), false, seerSpawnRate);
        seerSoulDuration = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Seer", 3), 15f, 0f, 120f, 5f,
            seerLimitSoulDuration);

        hackerSpawnRate = CustomOption.Create(Types.Crewmate, new TranslationInfo(RoleId.Hacker, Hacker.color), rates, null, true);
        hackerCooldown = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Hacker", 1), 30f, 5f, 60f, 5f, hackerSpawnRate);
        hackerHackeringDuration =
            CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Hacker", 2), 10f, 2.5f, 60f, 2.5f, hackerSpawnRate);
        hackerOnlyColorType =
            CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Hacker", 3), false, hackerSpawnRate);
        hackerToolsNumber = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Hacker", 4), 5f, 1f, 30f, 1f,
            hackerSpawnRate);
        hackerRechargeTasksNumber = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Swapper", 4),
            2f, 1f, 5f, 1f, hackerSpawnRate);
        hackerNoMove = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Hacker", 6), true,
            hackerSpawnRate);

        trackerSpawnRate = CustomOption.Create(Types.Crewmate, new TranslationInfo(RoleId.Tracker, Tracker.color), rates, null, true);
        trackerUpdateIntervall = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Tracker", 1), 5f, 1f, 30f, 1f,
            trackerSpawnRate);
        trackerResetTargetAfterMeeting = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Tracker", 2),
            false, trackerSpawnRate);
        trackerCanTrackCorpses =
            CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Tracker", 3), true, trackerSpawnRate);
        trackerCorpsesTrackingCooldown = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Tracker", 4), 30f, 5f,
            120f, 5f, trackerCanTrackCorpses);
        trackerCorpsesTrackingDuration = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Tracker", 5), 5f, 2.5f,
            30f, 2.5f, trackerCanTrackCorpses);
        trackerTrackingMethod = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Tracker", 6),
            new[] { new TranslationInfo("Opt-Tracker", 100), new TranslationInfo("Opt-Tracker", 101), new TranslationInfo("Opt-Tracker", 102) }, trackerSpawnRate);

        snitchSpawnRate = CustomOption.Create(Types.Crewmate, new TranslationInfo(RoleId.Snitch, Snitch.color), rates, null, true);
        snitchLeftTasksForReveal = CustomOption.Create(Types.Crewmate,
            new TranslationInfo("Opt-Snitch", 1), 5f, 0f, 25f, 1f, snitchSpawnRate);
        snitchMode = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Snitch", 2), new[] { new TranslationInfo("Opt-Snitch", 100), new TranslationInfo("Opt-Snitch", 101), new TranslationInfo("Opt-Snitch", 102), new TranslationInfo("Opt-Snitch", 103) },
            snitchSpawnRate);
        snitchTargets = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Snitch", 3),
            new[] { new TranslationInfo("Opt-Snitch", 110), new TranslationInfo("Opt-Snitch", 111) }, snitchSpawnRate);

        spySpawnRate = CustomOption.Create(Types.Crewmate, new TranslationInfo(RoleId.Spy, Spy.color), rates, null, true);
        spyCanDieToSheriff = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Spy", 1), false, spySpawnRate);
        spyImpostorsCanKillAnyone = CustomOption.Create(Types.Crewmate,
            new TranslationInfo("Opt-Spy", 2), true, spySpawnRate);
        spyCanEnterVents = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Spy", 3), false, spySpawnRate);
        spyHasImpostorVision = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Spy", 4), false, spySpawnRate);

        portalmakerSpawnRate =
            CustomOption.Create(Types.Crewmate, new TranslationInfo(RoleId.Portalmaker, Portalmaker.color), rates, null, true);
        portalmakerCooldown = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Portalmaker", 1), 30f, 10f, 60f, 2.5f,
            portalmakerSpawnRate);
        portalmakerUsePortalCooldown = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Portalmaker", 2), 30f, 10f, 60f,
            2.5f, portalmakerSpawnRate);
        portalmakerLogOnlyColorType = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Portalmaker", 3),
            true, portalmakerSpawnRate);
        portalmakerLogHasTime = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Portalmaker", 4), true, portalmakerSpawnRate);
        portalmakerCanPortalFromAnywhere = CustomOption.Create(Types.Crewmate,
            new TranslationInfo("Opt-Portalmaker", 5), true, portalmakerSpawnRate);

        securityGuardSpawnRate = CustomOption.Create(Types.Crewmate, new TranslationInfo(RoleId.SecurityGuard, SecurityGuard.color),
            rates, null, true);
        securityGuardCooldown = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-SecurityGuard", 1), 30f, 10f, 60f, 2.5f,
            securityGuardSpawnRate);
        securityGuardTotalScrews = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-SecurityGuard", 2), 7f, 1f,
            15f, 1f, securityGuardSpawnRate);
        securityGuardCamPrice = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-SecurityGuard", 3), 2f, 1f, 15f, 1f,
            securityGuardSpawnRate);
        securityGuardVentPrice = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-SecurityGuard", 4), 1f, 1f, 15f, 1f,
            securityGuardSpawnRate);
        securityGuardCamDuration = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-SecurityGuard", 5), 10f, 2.5f, 60f,
            2.5f, securityGuardSpawnRate);
        securityGuardCamMaxCharges = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-SecurityGuard", 6), 5f, 1f, 30f, 1f,
            securityGuardSpawnRate);
        securityGuardCamRechargeTasksNumber = CustomOption.Create(Types.Crewmate,
            new TranslationInfo("Opt-SecurityGuard", 7), 3f, 1f, 10f, 1f, securityGuardSpawnRate);
        securityGuardNoMove = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-SecurityGuard", 8), true,
            securityGuardSpawnRate);

        mediumSpawnRate = CustomOption.Create(Types.Crewmate, new TranslationInfo(RoleId.Medium, Medium.color), rates, null, true);
        mediumCooldown = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Medium", 1), 30f, 5f, 120f, 5f,
            mediumSpawnRate);
        mediumDuration = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Medium", 2), 3f, 0f, 15f, 1f,
            mediumSpawnRate);
        mediumOneTimeUse = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Medium", 3), false,
            mediumSpawnRate);
        mediumChanceAdditionalInfo = CustomOption.Create(Types.Crewmate,
            new TranslationInfo("Opt-Medium", 4), rates, mediumSpawnRate);

        thiefSpawnRate = CustomOption.Create(Types.Neutral, new TranslationInfo(RoleId.Thief, Thief.color), rates, null, true);
        thiefCooldown = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Thief", 1), 30f, 5f, 120f, 5f, thiefSpawnRate);
        thiefCanKillSheriff = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Thief", 2), true, thiefSpawnRate);
        thiefHasImpVision = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Thief", 3), true, thiefSpawnRate);
        thiefCanUseVents = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Thief", 4), true, thiefSpawnRate);
        thiefCanStealWithGuess = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-Thief", 5),
            false, thiefSpawnRate);

        schrodingersCatSpawnRate = CustomOption.Create(Types.Neutral, new TranslationInfo(RoleId.SchrodingersCat, SchrodingersCat.color), rates, null, true);
        schrodingersCatKillCooldown = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-SchrodingersCat", 1), 20f, 1f, 60f, 0.5f, schrodingersCatSpawnRate);
        schrodingersCatKillsKiller = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-SchrodingersCat", 3), false, schrodingersCatSpawnRate);
        schrodingersCatCantKillUntilLastOne = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-SchrodingersCat", 4), false, schrodingersCatSpawnRate);
        schrodingersCatExileType = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-SchrodingersCat", 5), new[] { new TranslationInfo("Opt-SchrodingersCat", 100), new TranslationInfo("Opt-SchrodingersCat", 101), new TranslationInfo("Opt-SchrodingersCat", 102) }, schrodingersCatSpawnRate);
        schrodingersCatHideRole = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-SchrodingersCat", 6), false, schrodingersCatSpawnRate);
        schrodingersCatCanChooseTeam = CustomOption.Create(Types.Neutral, new TranslationInfo("Opt-SchrodingersCat", 7), false, schrodingersCatHideRole);

        trapperSpawnRate = CustomOption.Create(Types.Crewmate, new TranslationInfo(RoleId.Trapper, Trapper.color), rates, null, true);
        trapperCooldown =
            CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Trapper", 1), 30f, 5f, 120f, 5f, trapperSpawnRate);
        trapperMaxCharges =
            CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Trapper", 2), 5f, 1f, 15f, 1f, trapperSpawnRate);
        trapperRechargeTasksNumber = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Trapper", 3),
            2f, 1f, 15f, 1f, trapperSpawnRate);
        trapperTrapNeededTriggerToReveal = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Trapper", 4), 3f,
            2f, 10f, 1f, trapperSpawnRate);
        trapperAnonymousMap = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Trapper", 5), false, trapperSpawnRate);
        trapperInfoType = CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Trapper", 6),
            new[] { new TranslationInfo("Opt-Trapper", 100), new TranslationInfo("Opt-Trapper", 101), new TranslationInfo("Opt-Trapper", 102) }, trapperSpawnRate);
        trapperTrapDuration =
            CustomOption.Create(Types.Crewmate, new TranslationInfo("Opt-Trapper", 7), 5f, 1f, 15f, 1f, trapperSpawnRate);

        // Modifier (1000 - 1999)
        modifiersAreHidden = CustomOption.Create(Types.Modifier,
            new TranslationInfo("Opt-General", 62, Color.yellow), true, null, true,
            heading: new TranslationInfo("Opt-Heading", 3, Color.yellow));

        modifierBloody = CustomOption.Create(Types.Modifier, new TranslationInfo(RoleId.Bloody, Color.yellow), rates, null, true);
        modifierBloodyQuantity = CustomOption.Create(Types.Modifier, new TranslationInfo("Opt-Bloody", 1, Color.yellow),
            ratesModifier, modifierBloody);
        modifierBloodyDuration =
            CustomOption.Create(Types.Modifier, new TranslationInfo("Opt-Bloody", 2), 10f, 3f, 60f, 1f, modifierBloody);

        modifierAntiTeleport =
            CustomOption.Create(Types.Modifier, new TranslationInfo(RoleId.AntiTeleport, Color.yellow), rates, null, true);
        modifierAntiTeleportQuantity = CustomOption.Create(Types.Modifier,
            new TranslationInfo("Opt-AntiTeleport", 1, Color.yellow), ratesModifier, modifierAntiTeleport);

        modifierTieBreaker =
            CustomOption.Create(Types.Modifier, new TranslationInfo(RoleId.Tiebreaker, Color.yellow), rates, null, true);

        modifierBait = CustomOption.Create(Types.Modifier, new TranslationInfo(RoleId.Bait, Color.yellow), rates, null, true);
        modifierBaitQuantity = CustomOption.Create(Types.Modifier, new TranslationInfo("Opt-Bait", 1, Color.yellow),
            ratesModifier, modifierBait);
        modifierBaitReportDelayMin =
            CustomOption.Create(Types.Modifier, new TranslationInfo("Opt-Bait", 2), 0f, 0f, 10f, 1f, modifierBait);
        modifierBaitReportDelayMax =
            CustomOption.Create(Types.Modifier, new TranslationInfo("Opt-Bait", 3), 0f, 0f, 10f, 1f, modifierBait);
        modifierBaitShowKillFlash =
            CustomOption.Create(Types.Modifier, new TranslationInfo("Opt-Bait", 4), true, modifierBait);

        modifierLover = CustomOption.Create(Types.Modifier, new TranslationInfo(RoleId.Lover, Color.yellow), rates, null, true);
        modifierLoverImpLoverRate = CustomOption.Create(Types.Modifier, new TranslationInfo("Opt-Lovers", 1),
            rates, modifierLover);
        modifierLoverBothDie = CustomOption.Create(Types.Modifier, new TranslationInfo("Opt-Lovers", 2), true, modifierLover);
        modifierLoverEnableChat = CustomOption.Create(Types.Modifier, new TranslationInfo("Opt-Lovers", 3), true, modifierLover);

        modifierSunglasses =
            CustomOption.Create(Types.Modifier, new TranslationInfo(RoleId.Sunglasses, Color.yellow), rates, null, true);
        modifierSunglassesQuantity = CustomOption.Create(Types.Modifier, new TranslationInfo("Opt-Sunglasses", 1, Color.yellow),
            ratesModifier, modifierSunglasses);
        modifierSunglassesVision = CustomOption.Create(Types.Modifier, new TranslationInfo("Opt-Sunglasses", 2),
            new[] { new TranslationInfo("-10%"), new TranslationInfo("-20%"), new TranslationInfo("-30%"), new TranslationInfo("-40%"), new TranslationInfo("-50%") }, modifierSunglasses);

        modifierMini = CustomOption.Create(Types.Modifier, new TranslationInfo(RoleId.Mini, Color.yellow), rates, null, true);
        modifierMiniGrowingUpDuration = CustomOption.Create(Types.Modifier, new TranslationInfo("Opt-Mini", 1), 400f,
            100f, 1500f, 100f, modifierMini);
        modifierMiniGrowingUpInMeeting =
            CustomOption.Create(Types.Modifier, new TranslationInfo("Opt-Mini", 2), true, modifierMini);
        if (EventUtility.canBeEnabled || EventUtility.isEnabled)
        {
            eventKicksPerRound = CustomOption.Create(Types.Modifier,
                new TranslationInfo("Opt-Mini", 3, Color.green), 4f, 0f, 14f, 1f, modifierMini);
            eventHeavyAge = CustomOption.Create(Types.Modifier, new TranslationInfo("Opt-Mini", 4, Color.green),
                12f, 6f, 18f, 0.5f, modifierMini);
            eventReallyNoMini = CustomOption.Create(Types.Modifier, new TranslationInfo("Opt-Mini", 5, Color.green), false,
                modifierMini, invertedParent: true);
        }

        modifierVip = CustomOption.Create(Types.Modifier, new TranslationInfo(RoleId.Vip, Color.yellow), rates, null, true);
        modifierVipQuantity = CustomOption.Create(Types.Modifier, new TranslationInfo("Opt-Vip", 1, Color.yellow), ratesModifier,
            modifierVip);
        modifierVipShowColor = CustomOption.Create(Types.Modifier, new TranslationInfo("Opt-Vip", 2), true, modifierVip);

        modifierInvert = CustomOption.Create(Types.Modifier, new TranslationInfo(RoleId.Invert, Color.yellow), rates, null, true);
        modifierInvertQuantity = CustomOption.Create(Types.Modifier, new TranslationInfo("Opt-Invert", 1, Color.yellow),
            ratesModifier, modifierInvert);
        modifierInvertDuration = CustomOption.Create(Types.Modifier, new TranslationInfo("Opt-Invert", 2), 3f, 1f, 15f,
            1f, modifierInvert);

        modifierChameleon = CustomOption.Create(Types.Modifier, new TranslationInfo(RoleId.Chameleon, Color.yellow), rates, null, true);
        modifierChameleonQuantity = CustomOption.Create(Types.Modifier, new TranslationInfo("Opt-Chameleon", 1, Color.yellow),
            ratesModifier, modifierChameleon);
        modifierChameleonHoldDuration = CustomOption.Create(Types.Modifier, new TranslationInfo("Opt-Chameleon", 2), 3f, 1f,
            10f, 0.5f, modifierChameleon);
        modifierChameleonFadeDuration = CustomOption.Create(Types.Modifier, new TranslationInfo("Opt-Chameleon", 3), 1f, 0.25f, 10f,
            0.25f, modifierChameleon);
        modifierChameleonMinVisibility = CustomOption.Create(Types.Modifier, new TranslationInfo("Opt-Chameleon", 4),
            new[] { new TranslationInfo("0%"), new TranslationInfo("10%"), new TranslationInfo("20%"), new TranslationInfo("30%"), new TranslationInfo("40%"), new TranslationInfo("50%") }, modifierChameleon);

        modifierArmored = CustomOption.Create(Types.Modifier, new TranslationInfo(RoleId.Armored, Color.yellow), rates, null, true);

        modifierShifter = CustomOption.Create(Types.Modifier, new TranslationInfo(RoleId.Shifter, Color.yellow), rates, null, true);
        modifierShifterShiftsMedicShield =
            CustomOption.Create(Types.Modifier, new TranslationInfo("Opt-Shifter", 1), false, modifierShifter);

        // Guesser Gamemode (2000 - 2999)
        guesserGamemodeCrewNumber = CustomOption.Create(Types.Guesser,
            new TranslationInfo("Opt-Guessers-General", 1, Guesser.color), 15f, 0f, 15f, 1f, null, true, heading: new TranslationInfo("Opt-Heading", 4));
        guesserGamemodeNeutralNumber = CustomOption.Create(Types.Guesser,
            new TranslationInfo("Opt-Guessers-General", 2, Guesser.color), 15f, 0f, 15f, 1f);
        guesserGamemodeImpNumber = CustomOption.Create(Types.Guesser,
            new TranslationInfo("Opt-Guessers-General", 3, Guesser.color), 15f, 0f, 15f, 1f);
        guesserForceJackalGuesser = CustomOption.Create(Types.Guesser, new TranslationInfo("Opt-Guessers-General", 4), false, null, true,
            heading: new TranslationInfo("Opt-Heading", 5));
        guesserGamemodeSidekickIsAlwaysGuesser =
            CustomOption.Create(Types.Guesser, new TranslationInfo("Opt-Guessers-General", 5), false);
        guesserForceThiefGuesser = CustomOption.Create(Types.Guesser, new TranslationInfo("Opt-Guessers-General", 6), false);
        guesserGamemodeHaveModifier = CustomOption.Create(Types.Guesser, new TranslationInfo("Opt-Guessers-General", 7), true,
            null, true, heading: new TranslationInfo("Opt-Heading", 6));
        guesserGamemodeNumberOfShots =
            CustomOption.Create(Types.Guesser, new TranslationInfo("Opt-Guesser", 2), 3f, 1f, 15f, 1f);
        guesserGamemodeHasMultipleShotsPerMeeting = CustomOption.Create(Types.Guesser,
            new TranslationInfo("Opt-Guesser", 3), false);
        guesserGamemodeCrewGuesserNumberOfTasks = CustomOption.Create(Types.Guesser,
            new TranslationInfo("Opt-Guessers-General", 10), 0f, 0f, 15f, 1f);
        guesserGamemodeKillsThroughShield =
            CustomOption.Create(Types.Guesser, new TranslationInfo("Opt-Guesser", 4), true);
        guesserGamemodeEvilCanKillSpy =
            CustomOption.Create(Types.Guesser, new TranslationInfo("Opt-Guesser", 5), true);
        guesserGamemodeCantGuessSnitchIfTaksDone = CustomOption.Create(Types.Guesser,
            new TranslationInfo("Opt-Guesser", 7), true);

        // Hide N Seek Gamemode (3000 - 3999)
        hideNSeekMap = CustomOption.Create(Types.HideNSeekMain, new TranslationInfo("Opt-Snitch", 101, Color.yellow),
            new[] { new TranslationInfo("Opt-General", 100), new TranslationInfo("Opt-General", 101), new TranslationInfo("Opt-General", 102), new TranslationInfo("Opt-General", 103), new TranslationInfo("Opt-General", 104), new TranslationInfo("Opt-General", 105), new TranslationInfo("Opt-General", 106) }, null, true, () =>
            {
                var map = hideNSeekMap.selection;
                if (map >= 3) map++;
                GameOptionsManager.Instance.currentNormalGameOptions.MapId = (byte)map;
            });
        hideNSeekHunterCount = CustomOption.Create(Types.HideNSeekMain, new TranslationInfo("Opt-HideNSeek-Main", 2, Color.yellow), 1f,
            1f, 3f, 1f);
        hideNSeekKillCooldown = CustomOption.Create(Types.HideNSeekMain, new TranslationInfo("Opt-HideNSeek-Main", 3, Color.yellow), 10f,
            2.5f, 60f, 2.5f);
        hideNSeekHunterVision = CustomOption.Create(Types.HideNSeekMain, new TranslationInfo("Opt-HideNSeek-Main", 4, Color.yellow), 0.5f,
            0.25f, 2f, 0.25f);
        hideNSeekHuntedVision = CustomOption.Create(Types.HideNSeekMain, new TranslationInfo("Opt-HideNSeek-Main", 5, Color.yellow), 2f,
            0.25f, 5f, 0.25f);
        hideNSeekCommonTasks =
            CustomOption.Create(Types.HideNSeekMain, new TranslationInfo("Opt-HideNSeek-Main", 6, Color.yellow), 1f, 0f, 4f, 1f);
        hideNSeekShortTasks =
            CustomOption.Create(Types.HideNSeekMain, new TranslationInfo("Opt-HideNSeek-Main", 7, Color.yellow), 3f, 1f, 23f, 1f);
        hideNSeekLongTasks =
            CustomOption.Create(Types.HideNSeekMain, new TranslationInfo("Opt-HideNSeek-Main", 8, Color.yellow), 3f, 0f, 15f, 1f);
        hideNSeekTimer =
            CustomOption.Create(Types.HideNSeekMain, new TranslationInfo("Opt-HideNSeek-Main", 9, Color.yellow), 5f, 1f, 30f, 0.5f);
        hideNSeekTaskWin =
            CustomOption.Create(Types.HideNSeekMain, new TranslationInfo("Opt-HideNSeek-Main", 10, Color.yellow), false);
        hideNSeekTaskPunish = CustomOption.Create(Types.HideNSeekMain,
            new TranslationInfo("Opt-HideNSeek-Main", 11, Color.yellow), 10f, 0f, 30f, 1f);
        hideNSeekCanSabotage =
            CustomOption.Create(Types.HideNSeekMain, new TranslationInfo("Opt-HideNSeek-Main", 12, Color.yellow), false);
        hideNSeekHunterWaiting = CustomOption.Create(Types.HideNSeekMain,
            new TranslationInfo("Opt-HideNSeek-Main", 13, Color.yellow), 15f, 2.5f, 60f, 2.5f);

        hunterLightCooldown = CustomOption.Create(Types.HideNSeekRoles, new TranslationInfo("Opt-HideNSeek-Roles", 1, Color.red),
            30f, 5f, 60f, 1f, null, true, heading: new TranslationInfo("Opt-Heading", 7));
        hunterLightDuration = CustomOption.Create(Types.HideNSeekRoles, new TranslationInfo("Opt-HideNSeek-Roles", 2, Color.red),
            5f, 1f, 60f, 1f);
        hunterLightVision = CustomOption.Create(Types.HideNSeekRoles, new TranslationInfo("Opt-HideNSeek-Roles", 3, Color.red), 3f,
            1f, 5f, 0.25f);
        hunterLightPunish = CustomOption.Create(Types.HideNSeekRoles, new TranslationInfo("Opt-HideNSeek-Roles", 4, Color.red),
            5f, 0f, 30f, 1f);
        hunterAdminCooldown = CustomOption.Create(Types.HideNSeekRoles, new TranslationInfo("Opt-HideNSeek-Roles", 5, Color.red),
            30f, 5f, 60f, 1f);
        hunterAdminDuration = CustomOption.Create(Types.HideNSeekRoles, new TranslationInfo("Opt-HideNSeek-Roles", 6, Color.red),
            5f, 1f, 60f, 1f);
        hunterAdminPunish = CustomOption.Create(Types.HideNSeekRoles, new TranslationInfo("Opt-HideNSeek-Roles", 7, Color.red),
            5f, 0f, 30f, 1f);
        hunterArrowCooldown = CustomOption.Create(Types.HideNSeekRoles, new TranslationInfo("Opt-HideNSeek-Roles", 8, Color.red),
            30f, 5f, 60f, 1f);
        hunterArrowDuration = CustomOption.Create(Types.HideNSeekRoles, new TranslationInfo("Opt-HideNSeek-Roles", 9, Color.red),
            5f, 0f, 60f, 1f);
        hunterArrowPunish = CustomOption.Create(Types.HideNSeekRoles, new TranslationInfo("Opt-HideNSeek-Roles", 10, Color.red),
            5f, 0f, 30f, 1f);

        huntedShieldCooldown = CustomOption.Create(Types.HideNSeekRoles, new TranslationInfo("Opt-HideNSeek-Roles", 11, Color.gray),
            30f, 5f, 60f, 1f, null, true, heading: new TranslationInfo("Opt-Heading", 8));
        huntedShieldDuration = CustomOption.Create(Types.HideNSeekRoles, new TranslationInfo("Opt-HideNSeek-Roles", 12, Color.gray),
            5f, 1f, 60f, 1f);
        huntedShieldRewindTime = CustomOption.Create(Types.HideNSeekRoles, new TranslationInfo("Opt-HideNSeek-Roles", 13, Color.gray),
            3f, 1f, 10f, 1f);
        huntedShieldNumber = CustomOption.Create(Types.HideNSeekRoles, new TranslationInfo("Opt-HideNSeek-Roles", 14, Color.gray), 3f,
            1f, 15f, 1f);

        // Prop Hunt General Options
        propHuntMap = CustomOption.Create(Types.PropHunt, new TranslationInfo("Opt-Snitch", 101, Color.yellow),
            new[] { new TranslationInfo("Opt-General", 100), new TranslationInfo("Opt-General", 101), new TranslationInfo("Opt-General", 102), new TranslationInfo("Opt-General", 103), new TranslationInfo("Opt-General", 104), new TranslationInfo("Opt-General", 105), new TranslationInfo("Opt-General", 106) }, null, true, () =>
            {
                var map = propHuntMap.selection;
                if (map >= 3) map++;
                GameOptionsManager.Instance.currentNormalGameOptions.MapId = (byte)map;
            });
        propHuntTimer = CustomOption.Create(Types.PropHunt, new TranslationInfo("Opt-HideNSeek-Main", 9, Color.yellow), 5f, 1f, 30f, 0.5f,
            null, true, heading: new TranslationInfo("Opt-Heading", 9));
        propHuntUnstuckCooldown = CustomOption.Create(Types.PropHunt, new TranslationInfo("Opt-PropHunt", 3, Color.yellow), 30f,
            2.5f, 60f, 2.5f);
        propHuntUnstuckDuration =
            CustomOption.Create(Types.PropHunt, new TranslationInfo("Opt-PropHunt", 4, Color.yellow), 2f, 1f, 60f, 1f);
        propHunterVision = CustomOption.Create(Types.PropHunt, new TranslationInfo("Opt-HideNSeek-Main", 4, Color.yellow), 0.5f, 0.25f, 2f,
            0.25f);
        propVision = CustomOption.Create(Types.PropHunt, new TranslationInfo("Opt-PropHunt", 6, Color.yellow), 2f, 0.25f, 5f, 0.25f);
        // Hunter Options
        propHuntNumberOfHunters = CustomOption.Create(Types.PropHunt, new TranslationInfo("Opt-HideNSeek-Main", 2, Color.red), 1f, 1f,
            5f, 1f, null, true, heading: new TranslationInfo("Opt-Heading", 10));
        hunterInitialBlackoutTime = CustomOption.Create(Types.PropHunt,
            new TranslationInfo("Opt-PropHunt", 8, Color.red), 10f, 5f, 20f, 1f);
        hunterMissCooldown = CustomOption.Create(Types.PropHunt, new TranslationInfo("Opt-PropHunt", 9, Color.red), 10f,
            2.5f, 60f, 2.5f);
        hunterHitCooldown = CustomOption.Create(Types.PropHunt, new TranslationInfo("Opt-PropHunt", 10, Color.red), 10f,
            2.5f, 60f, 2.5f);
        propHuntRevealCooldown = CustomOption.Create(Types.PropHunt, new TranslationInfo("Opt-PropHunt", 11, Color.red), 30f,
            10f, 90f, 2.5f);
        propHuntRevealDuration =
            CustomOption.Create(Types.PropHunt, new TranslationInfo("Opt-PropHunt", 12, Color.red), 5f, 1f, 60f, 1f);
        propHuntRevealPunish =
            CustomOption.Create(Types.PropHunt, new TranslationInfo("Opt-PropHunt", 13, Color.red), 10f, 0f, 1800f, 5f);
        propHuntAdminCooldown = CustomOption.Create(Types.PropHunt, new TranslationInfo("Opt-HideNSeek-Roles", 5, Color.red), 30f,
            2.5f, 1800f, 2.5f);
        propHuntFindCooldown =
            CustomOption.Create(Types.PropHunt, new TranslationInfo("Opt-PropHunt", 15, Color.red), 60f, 2.5f, 1800f, 2.5f);
        propHuntFindDuration =
            CustomOption.Create(Types.PropHunt, new TranslationInfo("Opt-PropHunt", 16, Color.red), 5f, 1f, 15f, 1f);
        // Prop Options
        propBecomesHunterWhenFound = CustomOption.Create(Types.PropHunt,
            new TranslationInfo("Opt-PropHunt", 17, Palette.CrewmateBlue), false, null, true, heading: new TranslationInfo("Opt-Heading", 11));
        propHuntInvisEnabled = CustomOption.Create(Types.PropHunt,
            new TranslationInfo("Opt-PropHunt", 18, Palette.CrewmateBlue), true, null, true);
        propHuntInvisCooldown = CustomOption.Create(Types.PropHunt,
            new TranslationInfo("Opt-PropHunt", 19, Palette.CrewmateBlue), 120f, 10f, 1800f, 2.5f, propHuntInvisEnabled);
        propHuntInvisDuration = CustomOption.Create(Types.PropHunt,
            new TranslationInfo("Opt-PropHunt", 20, Palette.CrewmateBlue), 5f, 1f, 30f, 1f, propHuntInvisEnabled);
        propHuntSpeedboostEnabled = CustomOption.Create(Types.PropHunt,
            new TranslationInfo("Opt-PropHunt", 21, Palette.CrewmateBlue), true, null, true);
        propHuntSpeedboostCooldown = CustomOption.Create(Types.PropHunt,
            new TranslationInfo("Opt-PropHunt", 22, Palette.CrewmateBlue), 60f, 2.5f, 1800f, 2.5f, propHuntSpeedboostEnabled);
        propHuntSpeedboostDuration = CustomOption.Create(Types.PropHunt,
            new TranslationInfo("Opt-PropHunt", 23, Palette.CrewmateBlue), 5f, 1f, 15f, 1f, propHuntSpeedboostEnabled);
        propHuntSpeedboostSpeed = CustomOption.Create(Types.PropHunt,
            new TranslationInfo("Opt-PropHunt", 24, Palette.CrewmateBlue), 2f, 1.25f, 5f, 0.25f, propHuntSpeedboostEnabled);


        // Other options
        maxNumberOfMeetings = CustomOption.Create(Types.General, new TranslationInfo("Opt-General", 26), 10,
            0, 15, 1, null, true, heading: new TranslationInfo("Opt-Heading", 12));
        anyPlayerCanStopStart = CustomOption.Create(Types.General,
            new TranslationInfo("Opt-General", 27, new Color(204f / 255f, 204f / 255f, 0, 1f)), false);
        blockSkippingInEmergencyMeetings =
            CustomOption.Create(Types.General, new TranslationInfo("Opt-General", 28), false);
        noVoteIsSelfVote = CustomOption.Create(Types.General, new TranslationInfo("Opt-General", 29), false,
            blockSkippingInEmergencyMeetings);
        hidePlayerNames = CustomOption.Create(Types.General, new TranslationInfo("Opt-General", 30), false);
        allowParallelMedBayScans = CustomOption.Create(Types.General, new TranslationInfo("Opt-General", 31), false);
        shieldFirstKill = CustomOption.Create(Types.General, new TranslationInfo("Opt-General", 32), false);
        finishTasksBeforeHauntingOrZoomingOut =
            CustomOption.Create(Types.General, new TranslationInfo("Opt-General", 33), true);
        deadImpsBlockSabotage = CustomOption.Create(Types.General, new TranslationInfo("Opt-General", 34), false);
        camsNightVision = CustomOption.Create(Types.General, new TranslationInfo("Opt-General", 35), false,
            null, true, heading: new TranslationInfo("Opt-Heading", 13));
        camsNoNightVisionIfImpVision = CustomOption.Create(Types.General,
            new TranslationInfo("Opt-General", 36), false, camsNightVision);

        dynamicMap = CustomOption.Create(Types.General, new TranslationInfo("Opt-General", 37), false, null, true,
            heading: new TranslationInfo("Opt-Heading", 16));
        dynamicMapEnableSkeld = CustomOption.Create(Types.General, new TranslationInfo("Opt-General", 100), rates, dynamicMap);
        dynamicMapEnableMira = CustomOption.Create(Types.General, new TranslationInfo("Opt-General", 101), rates, dynamicMap);
        dynamicMapEnablePolus = CustomOption.Create(Types.General, new TranslationInfo("Opt-General", 102), rates, dynamicMap);
        dynamicMapEnableAirShip = CustomOption.Create(Types.General, new TranslationInfo("Opt-General", 103), rates, dynamicMap);
        dynamicMapEnableFungle = CustomOption.Create(Types.General, new TranslationInfo("Opt-General", 104), rates, dynamicMap);
        dynamicMapEnableSubmerged = CustomOption.Create(Types.General, new TranslationInfo("Opt-General", 105), rates, dynamicMap);
        dynamicMapSeparateSettings =
            CustomOption.Create(Types.General, new TranslationInfo("Opt-General", 44), false, dynamicMap);

        blockedRolePairings.Add((byte)RoleId.Vampire, new[] { (byte)RoleId.Warlock });
        blockedRolePairings.Add((byte)RoleId.Warlock, new[] { (byte)RoleId.Vampire });
        blockedRolePairings.Add((byte)RoleId.Spy, new[] { (byte)RoleId.Mini });
        blockedRolePairings.Add((byte)RoleId.Mini, new[] { (byte)RoleId.Spy });
        blockedRolePairings.Add((byte)RoleId.Vulture, new[] { (byte)RoleId.Cleaner });
        blockedRolePairings.Add((byte)RoleId.Cleaner, new[] { (byte)RoleId.Vulture });
    }
}