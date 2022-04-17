using Microsoft.AspNetCore.Components;

namespace LocationsApp.Shared
{
    public partial class AppPager
    {
        [Parameter] public bool PreviousDisabled { get; set; }
        [Parameter] public bool NextDisabled { get; set; }
        [Parameter] public int CurrentPage { get; set; }
        [Parameter] public EventCallback OnPreviousClick { get; set; }
        [Parameter] public EventCallback OnNextClick { get; set; }
        [Parameter] public int RecordsCount { get; set; }
    }
}
