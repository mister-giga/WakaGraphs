using System;
using WakaGraphs.Templates.Models;
using WakaGraphs.Utils;
using Runtime = System.Runtime.InteropServices.RuntimeInformation;
Console.WriteLine(Runtime.FrameworkDescription);
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
