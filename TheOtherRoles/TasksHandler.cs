using System;
using HarmonyLib;
using TheOtherRoles.Utilities;

namespace TheOtherRoles;

[HarmonyPatch]
public static class TasksHandler
{
    public static Tuple<int, int> taskInfo(NetworkedPlayerInfo playerInfo)
    {
        var TotalTasks = 0;
        var CompletedTasks = 0;
        if (playerInfo != null && !playerInfo.Disconnected && playerInfo.Tasks != null &&
            playerInfo.Object &&
            playerInfo.Role && playerInfo.Role.TasksCountTowardProgress &&
            !playerInfo.Object.hasFakeTasks() && !playerInfo.Role.IsImpostor
           )
            foreach (var playerInfoTask in playerInfo.Tasks.GetFastEnumerator())
            {
                if (playerInfoTask.Complete) CompletedTasks++;
                TotalTasks++;
            }

        return Tuple.Create(CompletedTasks, TotalTasks);
    }

    [HarmonyPatch(typeof(GameData), nameof(GameData.RecomputeTaskCounts))]
    private static class GameDataRecomputeTaskCountsPatch
    {
        private static bool Prefix(GameData __instance)
        {
            var totalTasks = 0;
            var completedTasks = 0;

            foreach (var playerInfo in GameData.Instance.AllPlayers.GetFastEnumerator())
            {
                if ((playerInfo.Object
                     && playerInfo.Object
                         .hasAliveKillingLover()) // Tasks do not count if a Crewmate has an alive killing Lover
                    || playerInfo.PlayerId == Lawyer.lawyer?.PlayerId // Tasks of the Lawyer do not count
                    || (playerInfo.PlayerId == Pursuer.pursuer?.PlayerId &&
                        Pursuer.pursuer.Data.IsDead) // Tasks of the Pursuer only count, if he's alive
                    || playerInfo.PlayerId ==
                    Thief.thief
                        ?.PlayerId // Thief's tasks only count after joining crew team as sheriff (and then the thief is not the thief anymore)
                   )
                    continue;
                var (playerCompleted, playerTotal) = taskInfo(playerInfo);
                totalTasks += playerTotal;
                completedTasks += playerCompleted;
            }

            // Temporary diagnostic: user hit a report-triggers-instant-task-win bug in Dev Mode that
            // isn't explained by anything found reading the code so far (bots always have 0 tasks by
            // design, so totals should only ever reflect the real player). Log every time the totals
            // actually change, so the next repro shows exactly which recompute call pushed
            // CompletedTasks >= TotalTasks and what GameData.AllPlayers looked like at that moment.
            if (totalTasks != __instance.TotalTasks || completedTasks != __instance.CompletedTasks)
            {
                TheOtherRolesPlugin.Logger.LogInfo(
                    $"[TaskDiag] RecomputeTaskCounts: {__instance.TotalTasks}/{__instance.CompletedTasks} -> {totalTasks}/{completedTasks}");
                foreach (var playerInfo in GameData.Instance.AllPlayers.GetFastEnumerator())
                {
                    var (playerCompleted, playerTotal) = taskInfo(playerInfo);
                    if (playerTotal == 0 && playerCompleted == 0) continue;
                    TheOtherRolesPlugin.Logger.LogInfo(
                        $"[TaskDiag]   {playerInfo.PlayerName} (role={playerInfo.Role?.name}, isImpostor={playerInfo.Role?.IsImpostor}, tasksCount={playerInfo.Role?.TasksCountTowardProgress}, fake={playerInfo.Object?.hasFakeTasks()}): {playerCompleted}/{playerTotal}");
                }
            }

            __instance.TotalTasks = totalTasks;
            __instance.CompletedTasks = completedTasks;
            return false;
        }
    }
}