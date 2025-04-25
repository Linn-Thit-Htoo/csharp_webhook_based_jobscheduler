namespace csharp_webhook_based_jobscheduler.API.Extensions;

public static class Extension
{
    public static string ToJson(this object obj) =>
        JsonConvert.SerializeObject(obj, Formatting.Indented);

    public static T ToObject<T>(this string json) => JsonConvert.DeserializeObject<T>(json)!;
}
