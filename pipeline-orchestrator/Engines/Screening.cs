using System.Text.RegularExpressions;
using UglyToad.PdfPig;

namespace pipeline_orchestrator.Engines
{
    public class ExtractionTopic
    {
        public ExtractionTopic(string Experience, string Summary, string Skills, string Projects) 
        {
            this.Experience = Experience;
            this.Summary = Summary;
            this.Skills = Skills;
            this.Projects = Projects;

        }
        public string? Experience { get; set; }
        public string? Summary { get; set; }
        public string? Skills { get; set; }
        public string? Projects { get; set; }

    }
    // I will learn more about this to make it work for general cases, but for now I am focusing on LLM part
    public class Screening
    {
        private readonly string summaryPattern = @"(?i)\b(SUMMARY|PROFESSIONAL SUMMARY|OBJECTIVE|ABOUT ME)\b";
        private readonly string experiencePattern = @"(?i)\b(EXPERIENCE|WORK HISTORY|PROFESSIONAL EXPERIENCE|EMPLOYMENT)\b";
        private readonly string projectsPattern = @"(?i)\b(PROJECTS|PORTFOLIO|PERSONAL WORK)\b";
        private readonly string skillsPattern = @"(?i)\b(SKILLS|TECHNICAL SKILLS|TECHNOLOGIES|PROGRAMMING LANGUAGES|COMPETENCIES)\b";

        /* What this method wants to do is get the information out of pdf and then
            we will return a object which will be saved to database from API controller.
        */
        public ExtractionTopic MetaData(IFormFile input_pdf)
        {
            if (input_pdf == null || input_pdf.Length == 0) throw new Exception("no file");

            string fullText = "";

            // let's get the raw text from pdf
            using (var stream = input_pdf.OpenReadStream())
            using (var pdf = PdfDocument.Open(stream))
            {
                foreach (var page in pdf.GetPages())
                {
                    fullText += page.Text + "\n";
                }
            }

            // sections based on headers
            string summary = ExtractBetween(fullText, summaryPattern, new[] { experiencePattern, projectsPattern, skillsPattern });
            string experience = ExtractBetween(fullText, experiencePattern, new[] { projectsPattern, skillsPattern, summaryPattern });
            string projects = ExtractBetween(fullText, projectsPattern, new[] { summaryPattern, experiencePattern, @"(?i)\bEDUCATION\b" });
            string skills = ExtractBetween(fullText, @"(?i)TECHNICAL SKILLS", new[] { @"(?i)EXPERIENCE", @"(?i)PROJECTS" });

            var returnTopics = new ExtractionTopic(
               Clean(experience),
               Clean(summary),
               Clean(projects),
               Clean(skills)
             );

            // cleaned data for the DB and AI pipeline
            return returnTopics;
        }

        private string ExtractBetween(string text, string startPattern, string[] endPatterns)
        {
            var startMatch = Regex.Match(text, startPattern); // basically asking for patterns like experience 
            if (!startMatch.Success) return "Section Not Found";

            int startIndex = startMatch.Index + startMatch.Length;
            int minEndIndex = text.Length;

            foreach (var pattern in endPatterns)
            {
                var endMatch = Regex.Match(text.Substring(startIndex), pattern);
                if (endMatch.Success)
                {
                    int currentEndIndex = startIndex + endMatch.Index;
                    if (currentEndIndex < minEndIndex)
                        minEndIndex = currentEndIndex;
                }
            }

            return text.Substring(startIndex, minEndIndex - startIndex);
        }

        private string Clean(string input)
        {
            if (input == "Section Not Found") return input;
            return Regex.Replace(input.Replace("\n", " "), @"\s+", " ").Trim();
        }
    }
}