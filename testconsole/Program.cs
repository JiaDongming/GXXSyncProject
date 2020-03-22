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
          //  Project project = new Project(); 
          //  project= ProjectTransfer.LoadProjectInfo(5422); //Get project info via task id
            //update project to project space tab and update project team resource
         int result=   ProjectTransfer.UpdateProjectInfo(5422);
            Console.ReadLine();
        }
    }
}
