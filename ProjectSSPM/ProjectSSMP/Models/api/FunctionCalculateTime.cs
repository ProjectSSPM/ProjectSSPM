using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSSMP.Models.api
{
    public class FunctionCalculateTime
    {
        public static void CalTimeHours(TimeSpan time1, TimeSpan time2)
        {

            TimeSpan Result;
            Result = time1 + time2;
            Console.WriteLine(Result);

        }
    }
}
