namespace MissBot.Extensions
{
    public static class TagComposition
    {
        //public static string AsBTag(this string content) => $"<b>{content}</b>";
        public static string AsBTag(this string content, string title, string separator = ":") => $"<b>{title}{separator}</b> {content} ";
        public static string AsBTag(this string content) => $"<b>{content}</b> ";
        public static string AsBTag(this int content) => $"<b>{content}</b> ";
        public static string AsBTag(this double content) => $"<b>{content}</b> ";
        public static string AsBTag(this int content, string title, string separator = ":") => $"<b>{title}{separator}</b> {content} ";
        public static string AsBTag(this double content, string title, string separator = ":") => $"<b>{title}{separator}</b> {content} ";
        public static string AsITag(this string content) => $"<i>{content}</i>";
        public static string Shrink(this string content, short length) => content.PadRight(length)[..length];
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
}
