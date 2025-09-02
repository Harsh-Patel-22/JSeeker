namespace Backend.Util;

public static class StringCleaner {
    public static string GetCleanJsonString(string json) {
        var cleanedString = json.Replace("```json", "").Replace("```", "").Replace("\n", "").Trim();
        int firstBrace = cleanedString.IndexOf('{');
        if (firstBrace >= 0)
            cleanedString = cleanedString.Substring(firstBrace);

        return cleanedString;
    }

    public static string GetCleanDateString(string githubFormatDateString) {
        return githubFormatDateString.Substring(0, githubFormatDateString.IndexOf('T'));
    }

    public static string GetCleanCSVString(string inputString) {
        return null;
    }
}