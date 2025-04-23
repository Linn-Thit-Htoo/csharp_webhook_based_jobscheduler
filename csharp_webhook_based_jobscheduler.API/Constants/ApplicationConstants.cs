using TimeZoneConverter;

namespace csharp_webhook_based_jobscheduler.API.Constants
{
    public class ApplicationConstants
    {
        public static TimeZoneInfo GetMyanmarTimeZone() => TimeZoneInfo.FindSystemTimeZoneById("Myanmar Standard Time");
        public static TimeZoneInfo GetMyanmarTimeZoneV1() => TZConvert.GetTimeZoneInfo("Asia/Yangon");
    }
}
