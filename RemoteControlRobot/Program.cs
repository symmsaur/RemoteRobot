using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RemoteRobotLib;

namespace RemoteControlRobot
{
    class Program
    {
        static void Main(string[] args)
        {
            //string url = "192.168.54.108";
            string url = "localhost";
            var rightArm = new RemoteRobot(url, "T_ROB_R", RobotClientProvider.GetHttpClient(url));
            var leftArm = new RemoteRobot(url, "T_ROB_L", RobotClientProvider.GetHttpClient(url));

            Task.WhenAll(
                rightArm.RunProcedure("wavehigh"),
                leftArm.RunProcedure("wavelow"))
                .Wait();
            Console.WriteLine("Flip");
            Task.Delay(2000).Wait();
            Task.WhenAll(
                rightArm.RunProcedure("wavelow"),
                leftArm.RunProcedure("wavehigh"))
                .Wait();

            Console.WriteLine("Done");

            Console.ReadKey();
        }
    }
}
