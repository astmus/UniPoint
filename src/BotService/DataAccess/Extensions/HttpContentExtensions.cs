using System.Runtime.CompilerServices;
using System.Text;

namespace BotService.DataAccess.Extensions
{

    internal static class HttpContentExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string EncodeUtf8(this string value) =>
                new(Encoding.UTF8.GetBytes(value).Select(c => Convert.ToChar(c)).ToArray());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void AddStreamContent(
            this MultipartFormDataContent multipartContent,
            Stream content,
            string name,
            string? fileName = default)
        {
            fileName ??= name;
            var contentDisposition = $@"form-data; name=""{name}""; filename=""{fileName}""".EncodeUtf8();

            // It will be dispose of after the request is made
#pragma warning disable CA2000
            var mediaPartContent = new StreamContent(content)
            {
                Headers =
            {
                {"Content-Type", "application/octet-stream"},
                {"Content-Disposition", contentDisposition}
            }
            };
#pragma warning restore CA2000

            multipartContent.Add(mediaPartContent, name, fileName);
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //internal static void AddContentIfInputFileStream(
        //    this MultipartFormDataContent multipartContent,
        //    params IInputMedia[] inputMedia)
        //{
        //    foreach (var input in inputMedia)
        //    {
        //        if (input.Media.FileType == FileType.Stream)
        //        {
        //            multipartContent.AddStreamContent(
        //                content: input.Media.Content!,
        //                name: input.Media.FileName!
        //            );
        //        }

        //        if (input is IInputMediaThumb mediaThumb &&
        //            mediaThumb.Thumb?.FileType == FileType.Stream)
        //        {
        //            multipartContent.AddStreamContent(
        //                content: mediaThumb.Thumb.Content!,
        //                name: mediaThumb.Thumb.FileName!
        //            );
        //        }
        //    }
        //}
    }

}
