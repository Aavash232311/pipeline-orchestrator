using pipeline_orchestrator.Data;
using pipeline_orchestrator.Model;
using pipeline_orchestrator.Model.Recruit;


/*
    Here using power of .NET runtime we will eliminate basic boolean fields.
    So that we wouldn't send much data to the ML model which is costly.
*/

namespace pipeline_orchestrator.Engines
{
    public class Screening
    {
        public Dictionary<string, bool> BasicScreening(Posting posting, Talent pool)
        {

            return new Dictionary<string, bool>();
        }
    }
}
