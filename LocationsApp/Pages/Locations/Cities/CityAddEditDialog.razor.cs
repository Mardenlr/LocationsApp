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

namespace LocationsApp.Pages.Locations.Cities
{
    public partial class CityAddEditDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public CityModel City { get; set; } = new CityModel();
        [Parameter] public bool InEditMode { get; set; }
        [Inject] public IDialogService DialogService { get; set; }
        [Inject] public ICountryRepository CountryRepository { get; set; }
        [Inject] public IStateRepository StateRepository { get; set; }

        public List<ModalWindowData> CountriesData { get; set; } = new List<ModalWindowData>();
        public List<ModalWindowData> StatesData { get; set; } = new List<ModalWindowData>();
        private List<CountryModel> _countries = new List<CountryModel>();
        private List<StateModel> _states = new List<StateModel>();
        private bool _cityFirstRender = true;

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private void Ok()
        {
            MudDialog.Close(DialogResult.Ok(City));
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
                        City.CountryId = int.Parse(data.Id.ToString());
                        City.Country = data.Description;
                    }
                }
            }
            catch (Exception ex)
            {
                await DialogService.ShowMessageBox(AppMessages.Error, ex.Message);
            }
        }

        private void CountryChanged(string id)
        {
            if (_cityFirstRender == false)
            {
                City.StateId = 0;
                City.State = null;
            }
            _cityFirstRender = false;
        }

        private async Task GetStatesByCountry(SearchArgs data)
        {
            try
            {
                StatesData.Clear();
                var pagination = new Pagination { PageNumber = data.Page, PageSize = data.PageSize };
                _states = (await StateRepository.GetLookupListAsync(pagination, City.CountryId, data.Filter1)).ToList();
                foreach (var state in _states)
                {
                    StatesData.Add(new ModalWindowData { Id = state.Id, Description = state.Name });
                }
            }
            catch (Exception ex)
            {
                await DialogService.ShowMessageBox(AppMessages.Error, ex.Message);
            }
        }

        private async Task SelectState()
        {
            try
            {
                var dialogParams = new DialogParameters();
                dialogParams.Add("Data", StatesData);
                dialogParams.Add("OnSearch", new EventCallbackFactory().Create<SearchArgs>(this, GetStatesByCountry));

                var dialogOptions = new DialogOptions { MaxWidth = MaxWidth.Small };
                var dialog = DialogService.Show<AppModalWindowPicker>("Select State", dialogParams, dialogOptions);
                var result = await dialog.Result;
                if (result.Cancelled == false && result.Data != null)
                {
                    var data = result.Data as ModalWindowData;
                    if (data != null)
                    {
                        City.StateId = int.Parse(data.Id.ToString());
                        City.State = data.Description;
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
