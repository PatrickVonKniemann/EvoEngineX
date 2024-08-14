namespace DomainModels;

public static class Links
{
    public const string Home = "/home";
    public const string CodeBases = "code-bases";
    private const string CodeBaseDetail = "code-base/{CodeBaseId}";
    public const string CloudProfile = "cloud-profile";
    public const string UserProfile = "user-profile";
    public const string BuildRuns = "build-runs";
    public const string SignUp = "sign-up";
    public const string Admin = "admin";
    public const string UserDetail = "user-detail";
    
    public static string GetCodeBaseDetailLink(Guid codeBaseId)
    {
        return CodeBaseDetail.Replace("{CodeBaseId}", codeBaseId.ToString());
    }
    
    public static readonly Dictionary<string, string> SegmentAliases = new()
    {
        { "home", "Home" },
        { "code-bases", "Projects" },
        { "code-base", "Projects" },
        { "cloud-profile", "Cloud Profiles" },
        { "user-profile", "Settings" },
        { "admin", "Admin" },
    };

}