using FaultReportingAPI.Core.Enums;

namespace FaultReportingAPI.BLL.Helpers
{
    public static class StatusHelper
    {
        private static readonly Dictionary<StatusEnum, List<StatusEnum>> _transitions = new()
        {
            { StatusEnum.New, new() { StatusEnum.InReview, StatusEnum.Cancelled } },
            { StatusEnum.InReview, new() { StatusEnum.Assigned, StatusEnum.Rejected, StatusEnum.Cancelled } },
            { StatusEnum.Assigned, new() { StatusEnum.InProgress, StatusEnum.Cancelled } },
            { StatusEnum.InProgress, new() { StatusEnum.Completed, StatusEnum.Cancelled } },
            { StatusEnum.Completed, new() },
            { StatusEnum.Cancelled, new() },
            { StatusEnum.Rejected, new() }
        };

        public static bool CanTransition(StatusEnum current, StatusEnum next)
        {
            return _transitions.ContainsKey(current) &&
                   _transitions[current].Contains(next);
        }
        public static bool IsTerminal(StatusEnum status)
        {
            return _transitions.ContainsKey(status) && _transitions[status].Count == 0;
        }
    }
}
