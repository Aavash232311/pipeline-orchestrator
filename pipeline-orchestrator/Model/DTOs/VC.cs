namespace pipeline_orchestrator.Model.DTOs
{
    // DTO (Data Transfoer Objects) for GitHub API'S


    /*
      
     GraphQL API response. (It's not fequent that I deal with graph so my head was spinning for a while, so it's important to understand this.

      "data": {
        "user": {
          "contributionsCollection": {
            "contributionCalendar": {
              "totalContributions": 581
                }
              }
            }
          }
        } 
     
    */

    public class ContributionResults
    {
        public Data data { get; set; }
        public record Data(User User);
        public record User(ContributionsCollection ContributionsCollection);
        public record ContributionsCollection(ContributionCalendar ContributionCalendar);
        public record ContributionCalendar(int TotalContributions);
    }

    // Merit DTO

    public class GraphQLApiScore
    {
        public int? CollobratorScore { get; set; } = 0;
    }
   
}
