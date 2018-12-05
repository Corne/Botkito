using Discord.WebSocket;
using GoogleApi;
using GoogleApi.Entities.Search.Image.Request;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Botkito.Commands
{
    public class Show : ICommand
    {
        public string[] Keys => new[] { "show", "find", "search" };

        public async Task Execute(SocketMessage message)
        {
            try
            {
                string[] args = message.Content.Split(' ');
                if (args.Length == 0)
                    return;

                string search = string.Join(' ', args.Skip(1));

                if (string.IsNullOrWhiteSpace(search))
                    return;

                string token = await File.ReadAllTextAsync("google-token.txt").ConfigureAwait(false);

                var request = new ImageSearchRequest()
                {
                    Query = search,
                    Key = token,
                    SearchEngineId = "005052579021300564037:vtmq_gkuoog"
                };
                var response = await GoogleSearch.ImageSearch.QueryAsync(request).ConfigureAwait(false);
                RootObject result = JsonConvert.DeserializeObject<RootObject>(response.RawJson);


                if (result.items.Count == 0)
                {
                    await message.Channel.SendMessageAsync("Beep bop, sorry could not find anything for your request");
                    return;
                }

                using (var webclient = new WebClient())
                {
                    var item = result.items[0];

                    string path = Path.GetTempPath();

                    string filename = Path.GetFileName(item.link.Split('?')[0]);
                    string filepath = Path.Combine(path, filename);
                    

                    await webclient.DownloadFileTaskAsync(item.link, filepath).ConfigureAwait(false);
                    await message.Channel.SendFileAsync(filepath).ConfigureAwait(false);

                    File.Delete(filepath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public class Url
        {
            public string type { get; set; }
            public string template { get; set; }
        }

        public class Request
        {
            public string title { get; set; }
            public string totalResults { get; set; }
            public string searchTerms { get; set; }
            public int count { get; set; }
            public int startIndex { get; set; }
            public string inputEncoding { get; set; }
            public string outputEncoding { get; set; }
            public string safe { get; set; }
            public string cx { get; set; }
            public string filter { get; set; }
            public string disableCnTwTranslation { get; set; }
            public string hl { get; set; }
            public string fileType { get; set; }
            public string rights { get; set; }
            public string searchType { get; set; }
        }

        public class NextPage
        {
            public string title { get; set; }
            public string totalResults { get; set; }
            public string searchTerms { get; set; }
            public int count { get; set; }
            public int startIndex { get; set; }
            public string inputEncoding { get; set; }
            public string outputEncoding { get; set; }
            public string safe { get; set; }
            public string cx { get; set; }
            public string filter { get; set; }
            public string disableCnTwTranslation { get; set; }
            public string hl { get; set; }
            public string fileType { get; set; }
            public string rights { get; set; }
            public string searchType { get; set; }
        }

        public class Queries
        {
            public List<Request> request { get; set; }
            public List<NextPage> nextPage { get; set; }
        }

        public class Context
        {
            public string title { get; set; }
        }

        public class SearchInformation
        {
            public double searchTime { get; set; }
            public string formattedSearchTime { get; set; }
            public string totalResults { get; set; }
            public string formattedTotalResults { get; set; }
        }

        public class Image
        {
            public string contextLink { get; set; }
            public int height { get; set; }
            public int width { get; set; }
            public int byteSize { get; set; }
            public string thumbnailLink { get; set; }
            public int thumbnailHeight { get; set; }
            public int thumbnailWidth { get; set; }
        }

        public class Item
        {
            public string kind { get; set; }
            public string title { get; set; }
            public string htmlTitle { get; set; }
            public string link { get; set; }
            public string displayLink { get; set; }
            public string snippet { get; set; }
            public string htmlSnippet { get; set; }
            public string mime { get; set; }
            public Image image { get; set; }
        }

        public class RootObject
        {
            public string kind { get; set; }
            public Url url { get; set; }
            public Queries queries { get; set; }
            public Context context { get; set; }
            public SearchInformation searchInformation { get; set; }
            public List<Item> items { get; set; }
        }
    }
}
