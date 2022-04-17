using LocationsApp.DataAccess.Models;
using LocationsApp.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LocationsApp.Pages.Locations.Countries
{
    public partial class CountryAddEditDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public CountryModel Country { get; set; } = new CountryModel();
        [Parameter] public bool InEditMode { get; set; }
        [Inject] public IDialogService DialogService { get; set; }
        [Inject] public ICountryRepository CountryRepository { get; set; }

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private void Ok()
        {
            MudDialog.Close(DialogResult.Ok(Country));
        }
    }
}
