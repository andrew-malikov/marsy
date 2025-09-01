using System.Text;
using MarsY.Mission.Cli;
using MarsY.Robot;

using var stdin = Console.OpenStandardInput();
using var reader = new StreamReader(stdin, Encoding.UTF8);
var plan = reader.ReadToEnd();

var (missionPlan, validationResult) = MissionPlan.From(plan);
if (validationResult is not null)
{
    Console.WriteLine("Something wrong with input data. {0}", validationResult.ErrorMessage);
}

var mission = new ResearchMission(new RobotStdOutPrinter(), missionPlan!.Value);
mission.Accomplish();