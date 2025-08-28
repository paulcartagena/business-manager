
namespace BusinessManager.Models.ViewModels;

public class PageHeaderViewModel
{
    public string Title { get; set; }
    public string ButtonText { get; set; }
    public string ButtonUrl { get; set; }
    public string ButtonClass { get; set; } = "";
    public string ButtonId { get; set; } = "";
    public string ButtonIcon { get; set; } = "";
}