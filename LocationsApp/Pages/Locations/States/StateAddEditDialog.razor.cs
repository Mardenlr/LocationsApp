using LocationsApp.DataAccess.Models;
using LocationsApp.DataAccess.Repository.Interfaces;
using LocationsApp.DataAccess.Utils;
using LocationsApp.Shared;
using LocationsApp.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static LocationsApp.Shared.AppModalWindowPicker;

namespace LocationsApp.Pages.Locations.States
{
    public partial class StateAddEditDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public StateModel State { get; set; } = new StateModel();
        [Parameter] public bool InEditMode { get; set; }
        [Inject] public IDialogService DialogService { get; set; }
        [Inject] public ICountryRepository CountryRepository { get; set; }
        public List<ModalWindowData> CountriesData { get; set; } = new List<ModalWindowData>();
        private List<CountryModel> _countries = new List<CountryModel>();

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private void Ok()
        {
            MudDialog.Close(DialogResult.Ok(State));
        }

        private async Task GetCountries(SearchArgs data)
        {
            try
            {
                CountriesData.Clear();
                var pagination = new Pagination { PageNumber = data.Page, PageSize = data.PageSize };
                _countries = (await CountryRepository.GetLookupListAsync(pagination, data.Filter1)).ToList();
                foreach (var country in _countries)
                {
                    CountriesData.Add(new ModalWindowData { Id = country.Id, Description = country.Name });
                }
            }
            catch (Exception ex)
            {
                await DialogService.ShowMessageBox(AppMessages.Error, ex.Message);
            }
        }

        private async Task SelectCountry()
        {
            try
            {
                var dialogParams = new DialogParameters();
                dialogParams.Add("Data", CountriesData);
                dialogParams.Add("OnSearch", new EventCallbackFactory().Create<SearchArgs>(this, GetCountries));

                var dialogOptions = new DialogOptions { MaxWidth = MaxWidth.Small };
                var dialog = DialogService.Show<AppModalWindowPicker>("Select Country", dialogParams, dialogOptions);
                var result = await dialog.Result;
                if (result.Cancelled == false && result.Data != null)
                {
                    var data = result.Data as ModalWindowData;
                    if (data != null)
                    {
                        State.CountryId = int.Parse(data.Id.ToString());
                        State.Country = data.Description;
                    }
                }
            }
            catch (Exception ex)
            {
                await DialogService.ShowMessageBox(AppMessages.Error, ex.Message);
            }
        }
    }
}
