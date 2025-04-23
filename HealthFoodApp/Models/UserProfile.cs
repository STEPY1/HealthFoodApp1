namespace HealthFoodApp.Models;

public class UserProfile
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string ProfileImage { get; set; }
    public string MemberSince { get; set; }
    public HealthProfile HealthProfile { get; set; }
}

public class HealthProfile
{
    public string ActivityLevel { get; set; }
    public string SleepPattern { get; set; }
    public string EnergyLevel { get; set; }
}

public class HealthAchievement
{
    public string Title { get; set; }
    public string Icon { get; set; }
    public string Date { get; set; }
    public string Description { get; set; }
}

public class OrderHistoryItem
{
    public string OrderId { get; set; }
    public string Date { get; set; }
    public string Restaurant { get; set; }
    public List<string> Items { get; set; }
    public string Total { get; set; }
    public string Status { get; set; }
    public string StatusColor { get; set; }
}
