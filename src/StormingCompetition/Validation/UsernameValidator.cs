using System.Text.RegularExpressions;

namespace StormingCompetition.Validation;

public class UsernameValidator
{
    private static readonly Regex UserNameRegex = new Regex("^(?!.*[\\s])(?!.*[\\W_])[\\w\\s]{3,20}$");

    public static bool IsValidUserName(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            return false;

        return UserNameRegex.IsMatch(userName);
    }
}
