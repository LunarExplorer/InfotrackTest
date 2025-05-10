namespace InfotrackTest
{
    public interface ISeoStatsProvider
    {
        public Task<SeoStats> GetSeoStats(string[] keywords, string subjectUrl);
    }
}
