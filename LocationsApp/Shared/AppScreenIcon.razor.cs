using Microsoft.AspNetCore.Components;

namespace LocationsApp.Shared
{
    public partial class AppScreenIcon
    {
        [Parameter] public string Icon { get; set; }

        [Parameter] public string Title { get; set; }
    }
}
