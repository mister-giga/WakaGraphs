using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WakaGraphs.Templates.Models;
using WakaGraphs.Utils;

var e = Directory.GetFiles("/usr/bin").Where(x=>x.Contains("git")).OrderBy(x => x).ToArray();

Console.WriteLine("ss");
foreach(var item in e)
{
    Console.WriteLine(item);
}
Console.WriteLine("ee");
//CliCommandRunner

return;


var eee = Directory.Exists("ss");
List<string> dirs = new();


Enu("ss");

Console.WriteLine();


void Enu(string path)
{
    foreach(var dir in Directory.GetDirectories(path))
    {
        dirs.Add(dir);
        Enu(dir);
    }

    foreach(var file in Directory.GetFiles(path))
    {
        dirs.Add(file);
    }
}


try
{

    var gitCredentials = new UsernamePasswordCredentials()
    {
        Username = "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",
        Password = ""
    };
    var gitCredentialsHandler = new CredentialsHandler(
            (url, usernameFromUrl, types) => gitCredentials);

    string repoName = EnvironmentHelpers.GetEnvVariable("GITHUB_REPOSITORY", required: true);
    var githubRepoUrl = $"https://github.com/{repoName}.git";

    var s = Repository.ListRemoteReferences(githubRepoUrl).ToArray();
   
    if(Directory.Exists("src"))
        Directory.Delete("src", true);
    await Bash("mkdir src");
    var exists = Directory.Exists("src");
    Repository.Clone(githubRepoUrl, "src");
    File.AppendAllLines("src/commited.txt", new string[] { DateTime.Now.ToString() });
    using(var repo = new Repository("src"))
    {
        Commands.Stage(repo, "*");
        var branch = repo.Branches.First(x => x.IsCurrentRepositoryHead);

        Signature author = new Signature("WakaGraphBot", "@wakagraphbot", DateTime.Now);
        Signature committer = author;

        // Commit to the repository

        try
        {
            Commit commit = repo.Commit("Here's a commit i made!", author, committer);
        }
        catch(LibGit2Sharp.EmptyCommitException)
        {
            return;
        }


        LibGit2Sharp.PushOptions options = new LibGit2Sharp.PushOptions();
        options.CredentialsProvider = gitCredentialsHandler;

        repo.Network.Push(branch, options);
    }

    var wakatimeKey = EnvironmentHelpers.GetEnvVariable("WAKATIME_KEY", required: true);
    var wakaApiClient = new WakaApiClient(wakatimeKey);
    var allTimeData = await wakaApiClient.GetAllTimeDataAsync();

    Console.WriteLine("SVG Content - START");
    Console.WriteLine(await new AllTimeData
    {
        LastActive = allTimeData.Data.Range.End,
        MemberSince = allTimeData.Data.Range.Start,
        TotalTimeSpent = TimeSpan.FromSeconds(allTimeData.Data.Total_seconds)
    }.GetStringAsync());
    Console.WriteLine("SVG Content - END");

    Console.WriteLine(DateTime.Now);

}
catch(Exception ex)
{
    Console.WriteLine(ex);
}








static Task<int> Bash(string cmd)
{
    var source = new TaskCompletionSource<int>();
    var escapedArgs = cmd.Replace("\"", "\\\"");
    var process = new Process
    {
        StartInfo = new ProcessStartInfo
        {
            FileName = "bash",
            Arguments = $"-c \"{escapedArgs}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        },
        EnableRaisingEvents = true
    };
    process.Exited += (sender, args) =>
    {
        if(process.ExitCode == 0)
        {
            source.SetResult(0);
        }
        else
        {
            source.SetException(new Exception($"Command `{cmd}` failed with exit code `{process.ExitCode}`"));
        }

        process.Dispose();
    };

    try
    {
        process.Start();
    }
    catch(Exception e)
    {
        source.SetException(e);
    }

    return source.Task;
}