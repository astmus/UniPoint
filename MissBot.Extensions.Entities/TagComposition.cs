namespace MissBot.Extensions.Response
{
	public static class TagComposition
	{
		public static string AsBTag(this string content) => $"<b>{content}</b>";
		public static string AsITag(this string content) => $"<i>{content}</i>";
		public static string AsCodeTag(this string content) => $"<code>{content}</code>";
		public static string AsStrikeTag(this string content) => $"<s>{content}</s>";
		public static string AsUnderTag(this string content) => $"<u>{content}</u>";
		public static string AsPreTag(this string content) => $"<pre>{content}</pre>";
		public static string AsLinkTag(this string url, string title) => $"<a href=\"{url}\">{title}</a>";
		//public static string AsLinkTag(this JSUrl url, string title) => $"<a href=\"{url}\">{title}</a>";
		public static string PreLineTag(this string content) => $"\n{content}";
		public static string LineTag(this string content) => $"{content}\n";
		//public static string LineTag(this JSUrl content) => $"{content}\n";
		public static string AsSectionTag(this string content, string taggedName = "•") => $"{taggedName}: {content}\n";
		public static string AsSection(this string content, string sectionName = "•") => $"{sectionName}: {content}\n";
		public static string AsBSectionTag(this string content, string sectionName = "•") => $"<b>{sectionName}</b>: {content}\n";
		public static string AsBSectionTag(this int content, string sectionName = "•") => $"<b>{sectionName}</b>: {content}\n";
		public static string Section(string sectionName = "•", string sectionValue = "•") => $"{sectionName}: {sectionValue}\n";
		public static string BSection(string sectionName = "•", string sectionValue = "•") => $"<b>{sectionName}:</b> {sectionValue}\n";		
	}

	public static class TimeTransformExtension
	{
		public static string ToTimeString(this TimeSpan span) => span.ToString(@"hh\:mm\:ss");
		public static string ToDiffNowTimeString(this TimeSpan span) => (DateTimeOffset.Now - span).ToString(@"hh\:mm\:ss");
		public static string ToTimeString(this DateTime span) => span.ToString(@"hh\:mm\:ss");
		public static string ToDiffNowTimeString(this DateTime span) => (DateTimeOffset.Now - span).ToString(@"hh\:mm\:ss");
		public static TimeSpan DiffNow(this DateTime span) => (DateTime.UtcNow - span.ToLocalTime());
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
				_  => $"{(int)span.TotalDays} days ago",			
				/*JenkinsJobState.Failed => pattern.Replace("!", "build_fail"),
				JenkinsJobState.Success => "http://astmus.com/gomm/builded_success_light.png",
				JenkinsJobState.Unstable => "http://astmus.com/gomm/build_unstable.png",
				_ => "http://astmus.com/gomm/build_disabled.png",*/
			};

	}
}
