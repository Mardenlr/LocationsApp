using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;

namespace LocationsApp.Shared
{
    public partial class AppMessageBoxDelete
    {
        [Parameter] public RenderFragment ChildContent { get; set; }

        private MudMessageBox _messageBox;

        public async Task<bool?> Show()
        {
            return await _messageBox.Show();
        }
    }
}
