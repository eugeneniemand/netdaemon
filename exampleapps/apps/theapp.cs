using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using JoySoftware.HomeAssistant.NetDaemon.Common;
/// <summary>
///     Greets (or insults) people when coming home :)
/// </summary>
public class TheApp : NetDaemonApp
{

    #region -- Config properties --
    public string? StringConfig { get; set; } = null;
    public int? IntConfig { get; set; } = null;
    public IEnumerable<string>? EnumerableConfig { get; set; } = null;

    #endregion

    public static string PrettyPrintDictData(IDictionary<string, object>? dict)
    {

        if (dict == null)
            return string.Empty;

        var builder = new StringBuilder(100);
        foreach (var key in dict.Keys)
        {
            builder.AppendLine($"{key}:{dict[key]}");
        }
        return builder.ToString();
    }
    public override Task InitializeAsync()
    {
        ListenServiceCall("scene", "turn_on", async (data) =>
        {
            Log("Service call!");
            var prettyData = PrettyPrintDictData(data as IDictionary<string, object>);
            Log(prettyData);


        });

        // var time = "07:24:23";
        // // Do nothing
        // Schedule("14:41:01");
        // Schedule("14:41:02");
        // Schedule("14:41:03");
        // Schedule("14:41:04");
        // Schedule("14:41:05");
        // Schedule("14:41:06");
        // Schedule("14:41:07");
        // Schedule("14:41:08");
        // Schedule("14:41:09");
        // Schedule("14:41:10");
        // Schedule("14:41:11");
        // Schedule("14:41:12");
        // Schedule("14:41:13");
        // Schedule("14:41:14");
        // Scheduler.RunDaily(time, new DayOfWeek[]
        // {
        //     DayOfWeek.Sunday,
        // }, async () => Log($"This is correct time! {DateTime.Now}"));
        // Scheduler.RunEveryMinute(30, async () => Log($"{DateTime.Now}"));
        return Task.CompletedTask;
    }

    private void Schedule(string time)
    {

        Scheduler.RunDaily(time, new DayOfWeek[]
      {
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday,
            DayOfWeek.Saturday,
            DayOfWeek.Sunday,
      }, async () => Log($"Time! {time}, {DateTime.Now}"));

    }

    // [HomeAssistantServiceCall]
    // public async Task CallMeFromHass(dynamic data)
    // {
    //     Log("A call from hass!");
    // }
}