using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WakaGraphs.Templates.Models;
using WakaGraphs.Utils;

string repoName = EnvironmentHelpers.GetRepoName(out var userName);
string ghToken = EnvironmentHelpers.GetEnvVariable("GH_TOKEN", required: true);
string statsDir = EnvironmentHelpers.GetEnvVariable("STATS_DIR", def: "stats");
string wakaApiKey = EnvironmentHelpers.GetEnvVariable("WAKATIME_KEY", required: true);
string branch = EnvironmentHelpers.GetEnvVariable("BRANCH", required: true);
string readmeFilePath = "README.md";


var wakaApiClient = new WakaApiClient(wakaApiKey);

Console.WriteLine("START");
CliCommandRunner.Git($"clone --branch {branch} https://github.com/{userName}/{repoName}.git", Console.WriteLine);

Directory.SetCurrentDirectory(repoName);
Directory.CreateDirectory(statsDir);


List<TemplateModelBase> templates = new List<TemplateModelBase>();


var allTimeData = await wakaApiClient.GetAllTimeDataAsync();

Console.WriteLine("api resp: ");
Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(allTimeData, new JsonSerializerOptions { WriteIndented = true }));

Console.WriteLine("SVG Content - START");

templates.Add(new AllTimeData
{
    LastActive = allTimeData.Data.Range.End,
    MemberSince = allTimeData.Data.Range.Start,
    TotalTimeSpent = TimeSpan.FromSeconds(allTimeData.Data.Total_seconds)
});

var samplesFilePath = Path.Combine(statsDir, "sample.md");
if(File.Exists(samplesFilePath))
    File.Delete(samplesFilePath);

foreach(var template in templates)
{
    await template.GenerateAsync(statsDir);
    File.AppendAllText(samplesFilePath, template.GetSample(userName, repoName, branch, statsDir));
}

Console.WriteLine("SVG Content - END");




//if(File.Exists(readmeFilePath))
//{
//    var readmeContent = File.ReadAllLines(readmeFilePath);

//    var srcStartContent = $"src='https://raw.githubusercontent.com/{userName}/{repoName}/";
//    var srcEndContent = ".svg'";

//    for(int i = 0; i < readmeContent.Length; i++)
//    {
//        var line = readmeContent[i];
//        var srcStartIndex = line.IndexOf(srcStartContent);
//        if(srcStartIndex < 0)
//            continue;

//        var endIndex = line.IndexOf(srcEndContent, srcStartIndex);
//        if(endIndex < 0)
//            continue;

//        var firstPart = line.Substring(0, srcStartIndex + srcStartContent.Length);
//        var midPart = line.Substring(srcStartIndex + srcStartContent.Length, endIndex);
//        var endPart = line.Substring(endIndex);

//        var fileName = midPart.Split('/').Last();

//        //todo

//        readmeContent[i] = string.Concat(firstPart, midPart, ".svg", endPart);
//    }

//    File.WriteAllLines(readmeFilePath, readmeContent);
//}
//else
//{
//    Console.WriteLine($"{readmeFilePath} does not exist");
//}


CliCommandRunner.Git("config user.name \"WakaGraphsBot\"", Console.WriteLine);
CliCommandRunner.Git("config user.email @wakagraphsbot", Console.WriteLine);

CliCommandRunner.Git("add .", Console.WriteLine);
CliCommandRunner.Git("commit -m\"WakaGraphs generated\"", Console.WriteLine);
CliCommandRunner.Git($"push https://{ghToken}@github.com/{userName}/{repoName}.git", Console.WriteLine);

//CliCommandRunner.Run("mkdir", "/usr/tmpsrc", Console.WriteLine);

Console.WriteLine("END");


