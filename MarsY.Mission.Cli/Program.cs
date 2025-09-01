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
    return 1;
}

var mission = new ResearchMission(new RobotStdOutPrinter(), missionPlan!.Value);
var missionValidationResult = mission.Accomplish();
if (missionValidationResult is not null)
{
    Console.WriteLine("Something wrong has happened during the mission. {0}", missionValidationResult.ErrorMessage);
    return 1;
}


return 0;