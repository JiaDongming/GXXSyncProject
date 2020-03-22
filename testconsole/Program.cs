using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using SyncProjectInfo;

namespace testconsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Project project = new Project(); 
            project= ProjectTransfer.LoadProjectInfo(5224); //Get project info via task id
            //update project to project space tab and update project team resource

            Console.ReadLine();
        }
    }
}
