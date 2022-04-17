using LocationsApp.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationsApp.Shared
{
    public partial class AppModalWindowPicker
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public List<ModalWindowData> Data { get; set; }
        [Parameter] public EventCallback<SearchArgs> OnSearch { get; set; }
        [Parameter] public int RegistersPerPage { get; set; } = 5;

        private ModalWindowData _selectedDataRow;
        private int _currentPage = 1;
        private bool _previousPageDisabled;
        private bool _nextPageDisabled;
        private string _filter = "";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender == true)
            {
                await Search(_filter);
            }
        }

        private async Task Search(string filter)
        {
            _filter = filter;
            _currentPage = 1;
            await OnSearch.InvokeAsync(new SearchArgs 
            { 
                Filter1 = _filter, 
                Page = _currentPage, 
                PageSize = RegistersPerPage 
            });
            EnableDisablePaginationButtons();
        }

        private async Task Paginate()
        {
            await OnSearch.InvokeAsync(new SearchArgs 
            { 
                Filter1 = _filter, 
                Page = _currentPage, 
                PageSize = RegistersPerPage 
            });
            EnableDisablePaginationButtons();
        }

        private void EnableDisablePaginationButtons()
        {
            _previousPageDisabled = _currentPage == 1;
            _nextPageDisabled = Data == null || Data.Count() != RegistersPerPage;
            StateHasChanged();
        }

        private async Task PreviousPageClicked()
        {
            _currentPage--;
            await Paginate();
        }

        private async Task NextPageClicked()
        {
            _currentPage++;
            await Paginate();
        }

        private void SelectDataRow(ModalWindowData row)
        {
            _selectedDataRow = row;
        }

        private void DialogCancel()
        {
            MudDialog.Cancel();
        }

        private void DialogOk()
        {
            MudDialog.Close(DialogResult.Ok(_selectedDataRow));
        }

        public class ModalWindowData
        {
            public object Id { get; set; }
            public string Description { get; set; }
        }
    }
}
