namespace StormingCompetition.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public int WrongPasswordCount { get; set; }
    public bool IsLocked { get; set; }
    public bool Win { get; set; }

    //Navigation Properties
    public ICollection<UserLog> UserLogs { get; set; }
}

public class UserLog
{
    public int Id { get; set; }
    public string Log { get; set; }

    //Navigation Properties
    public int UserId { get; set; }
    public User User { get; set; }
    public DateTime LogTime { get; set; }
}