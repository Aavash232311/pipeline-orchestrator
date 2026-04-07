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
        public bool BasicScreening(Posting posting, Talent pool)
        {
            if (posting == null)
            {
                throw new Exception("Posting is null");
            }


            return true;
        }
    }
}
