using HTools;
using Lavcode.IService;
using Newtonsoft.Json;
using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Lavcode.Service.GitHub
{
    public class ConService : IConService
    {
        private static readonly int _pageSize = 20;

        internal GitHubClient Client { get; private set; }
        internal User User { get; private set; }
        internal Gist Gist { get; private set; }
        internal List<GistFile> GistFiles { get; private set; }

        public async Task<bool> Connect(object args)
        {
            var exObj = DynamicHelper.ToExpandoObject(args);
            var token = exObj.Token as string;
            var gistId = exObj.GistId as string;
            var credentials = new Credentials(token, AuthenticationType.Oauth);
            Client = new GitHubClient(new ProductHeaderValue("Lavcode")) { Credentials = credentials };

            try
            {
                User = await Client.User.Current();
                Gist = await GetGist(gistId);
                GistFiles = Gist.Files.Select(item => item.Value).ToList();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        private async Task<Gist> GetGist(string gistId)
        {
            if (string.IsNullOrEmpty(gistId))
            {
                return await CreateGist();
            }

            try
            {
                return await Client.Gist.Get(gistId);
            }
            catch (NotFoundException)
            {
                return await CreateGist();
            }
        }

        private async Task<Gist> CreateGist()
        {
            return await Client.Gist.Create(new NewGist()
            {
                Public = false,
                Description = "LavcodeStorageFiles",
            });
        }

        private string GetFileName<T>(string key = null)
        {
            var result = $"{typeof(T).Name}\\";
            if (key != null) result += key;
            return result;
        }

        internal T[] GetItems<T>()
        {
            var str = Gist.Files.Select(item => item.Value).FirstOrDefault(file => file.Filename.StartsWith(GetFileName<T>())).Content;
            return JsonConvert.DeserializeObject<T[]>(str);
        }

        internal async Task UpdateItem<T>(T value, string key)
        {
            var existFile = GistFiles.FirstOrDefault(file => file.Filename == GetFileName<T>(key));
            if (existFile == default) return;

            var updateGists = new GistUpdate()
            {
                Description = Gist.Description,
            };
            GistFiles.ForEach(file =>
            {
                if (file == existFile)
                {
                    updateGists.Files.Add(file.Filename, new GistFileUpdate()
                    {
                        NewFileName = file.Filename,
                        Content = JsonConvert.SerializeObject(value)
                    });
                }
                else
                {
                    updateGists.Files.Add(file.Filename, new GistFileUpdate()
                    {
                        NewFileName = file.Filename,
                        Content = file.Content
                    });
                }
            });
            Gist = await Client.Gist.Edit(Gist.Id, updateGists);
            GistFiles = Gist.Files.Select(file => file.Value).ToList();
        }

        internal async Task DeleteComment<T>(string key)
        {
            var existFile = GistFiles.FirstOrDefault(file => file.Filename == GetFileName<T>(key));
            if (existFile == default) return;

            var updateGists = new GistUpdate()
            {
                Description = Gist.Description,
            };
            GistFiles.ForEach(file =>
            {
                if (file == existFile)
                {
                    updateGists.Files.Add(file.Filename, new GistFileUpdate()
                    {
                        NewFileName = file.Filename,
                        Content = JsonConvert.SerializeObject(value)
                    });
                }
                else
                {
                    updateGists.Files.Add(file.Filename, new GistFileUpdate()
                    {
                        NewFileName = file.Filename,
                        Content = file.Content
                    });
                }
            });
            Gist = await Client.Gist.Edit(Gist.Id, updateGists);
            GistFiles = Gist.Files.Select(file => file.Value).ToList();
        }

        internal async Task CreateComment<T>(T value)
        {
            var issues = GetIssues<T>();
            var newComment = await Client.Issue.Comment.Create(Gist.Id, issues.Issue.Number, JsonConvert.SerializeObject(value));
            issues.Comments.Add(new CommentItem<T>()
            {
                Comment = newComment,
                Value = value
            });
        }

        internal async Task UpsertComment<T>(T value, Func<T, T, bool> isEqual)
        {
            var issues = GetIssues<T>();
            if (issues.Comments.Any(item => isEqual(item.Value, value)))
            {
                await UpdateGist(value, isEqual);
            }
            else
            {
                await CreateComment(value);
            }
        }

        private async Task<IReadOnlyList<Issue>> GetIssues(int page)
        {
            return await Client.Issue.GetAllForRepository(User.Login, Gist.Name, new ApiOptions()
            {
                PageCount = 1,
                PageSize = _pageSize,
                StartPage = page,
            });
        }

        public void Dispose()
        {

        }
    }
}
