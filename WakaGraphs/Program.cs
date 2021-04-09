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

//var e = Directory.GetFiles("/usr/bin").Where(x=>x.Contains("git")).OrderBy(x => x).ToArray();


//CliCommandRunner
Console.WriteLine("START");
//CliCommandRunner.Run("/usr/bin/git", "clone https://github.com/mister-giga/temporary.git", Console.WriteLine);

//var txt = File.ReadAllText("temporary/commited.txt");

//Directory.SetCurrentDirectory("temporary");



//File.AppendAllLines("commited.txt", new string[] { DateTime.Now.ToString() });


//CliCommandRunner.Run("/usr/bin/git", "config --global user.name \"WakaGraphsBot\"", Console.WriteLine);
//CliCommandRunner.Run("/usr/bin/git", "config --global user.email @wakagraphsbot", Console.WriteLine);
//CliCommandRunner.Run("/usr/bin/git", "config --global user.username \"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\"", Console.WriteLine);
//CliCommandRunner.Run("/usr/bin/git", "config --global user.password \"\"", Console.WriteLine);
//
//
//
//CliCommandRunner.Run("/usr/bin/git", "add .", Console.WriteLine);
//CliCommandRunner.Run("/usr/bin/git", "commit -m\"temp message\"", Console.WriteLine);
//CliCommandRunner.Run("/usr/bin/git", "push", Console.WriteLine);

//CliCommandRunner.Run("mkdir", "/usr/tmpsrc", Console.WriteLine);

Console.WriteLine("END");


Directory.SetCurrentDirectory("/usr");


try
{
    string repoName = EnvironmentHelpers.GetEnvVariable("GITHUB_REPOSITORY", required: true);
    var githubRepoUrl = $"https://github.com/{repoName}.git";

    var s = Repository.ListRemoteReferences(githubRepoUrl).ToArray();

    if(Directory.Exists("tempsource"))
        Directory.Delete("tempsource", true);
    
    Repository.Clone(githubRepoUrl, "tempsource");

    Directory.SetCurrentDirectory("tempsource");

    File.AppendAllLines("commited.txt", new string[] { DateTime.Now.ToString() });


    //Directory.SetCurrentDirectory("../");
    using(var repo = new Repository("./"))
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
        options.CredentialsProvider = new CredentialsHandler(
            (url, usernameFromUrl, types) => new UsernamePasswordCredentials()
            {
                Username = "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",
                Password = ""
            });

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