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
            var robot = new RemoteRobot("138.227.54.195", "T_ROB_R");
            robot.RunProcedure("wavehigh").Wait();
            robot.RunProcedure("wavehigh").Wait();
            robot.RunProcedure("wavehigh").Wait();
            robot.RunProcedure("wavehigh").Wait();
            robot.RunProcedure("wavelow").Wait();

            Console.WriteLine("Done");

            Console.ReadKey();
        }
    }
}
