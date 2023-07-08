namespace MissBot.Extensions
{
    public static class TimeTransformExtension
    {
        public static string ToTimeString(this TimeSpan span) => span.ToString(@"hh\:mm\:ss");
        public static string ToDiffNowTimeString(this TimeSpan span) => (DateTimeOffset.Now - span).ToString(@"hh\:mm\:ss");
        public static string ToTimeString(this DateTime span) => span.ToString(@"hh\:mm\:ss");
        public static string ToDiffNowTimeString(this DateTime span) => (DateTimeOffset.Now - span).ToString(@"hh\:mm\:ss");
        public static TimeSpan DiffNow(this DateTime span) => DateTime.UtcNow - span.ToLocalTime();
        public static string ToFormatAmount(this TimeSpan span)
            => span switch
            {
                _ when span.TotalSeconds < 60 => $"{(int)span.TotalSeconds} sec ago",
                _ when span.TotalMinutes < 60 => $"{(int)span.TotalMinutes} min ago",
                _ when span.TotalHours < 24 => $"{(int)span.TotalHours} hrs ago",
                _ when span.TotalDays < 30 => $"{(int)span.TotalDays} days ago",
                _ => $"long time ago",
                /*JenkinsJobState.Failed => pattern.Replace("!", "build_fail"),
				JenkinsJobState.Success => "http://astmus.com/gomm/builded_success_light.png",
				JenkinsJobState.Unstable => "http://astmus.com/gomm/build_unstable.png",
				_ => "http://astmus.com/gomm/build_disabled.png",*/
            };
        public static string ToCountedFormatAmount(this TimeSpan span)
            => span switch
            {
                _ when span.TotalMinutes < 60 => $"{(int)span.TotalMinutes} min ago",
                _ when span.TotalHours < 24 => $"{(int)span.TotalHours} hrs ago",
                _ => $"{(int)span.TotalDays} days ago",
                /*JenkinsJobState.Failed => pattern.Replace("!", "build_fail"),
				JenkinsJobState.Success => "http://astmus.com/gomm/builded_success_light.png",
				JenkinsJobState.Unstable => "http://astmus.com/gomm/build_unstable.png",
				_ => "http://astmus.com/gomm/build_disabled.png",*/
            };

    }
}
