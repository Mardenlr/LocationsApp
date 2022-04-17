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

namespace LocationsApp.Pages.Locations.Districts
{
    public partial class DistrictAddEditDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public DistrictModel District { get; set; } = new DistrictModel();
        [Parameter] public bool InEditMode { get; set; }
        [Inject] public IDialogService DialogService { get; set; }
        [Inject] public ICountryRepository CountryRepository { get; set; }
        [Inject] public IStateRepository StateRepository { get; set; }
        [Inject] public ICityRepository CityRepository { get; set; }

        public List<ModalWindowData> CountriesData { get; set; } = new List<ModalWindowData>();
        public List<ModalWindowData> StatesData { get; set; } = new List<ModalWindowData>();
        public List<ModalWindowData> CitiesData { get; set; } = new List<ModalWindowData>();

        private List<CountryModel> _countries = new List<CountryModel>();
        private List<StateModel> _states = new List<StateModel>();
        private List<CityModel> _cities = new List<CityModel>();

        private bool _stateFirstRender = true;
        private bool _cityFirstRender = true;

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private void Ok()
        {
            MudDialog.Close(DialogResult.Ok(District));
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
                        District.CountryId = int.Parse(data.Id.ToString());
                        District.Country = data.Description;
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
            if (_stateFirstRender == false)
            {
                District.StateId = 0;
                District.State = null;
                District.CityId = 0;
                District.City = null;
            }
            _stateFirstRender = false;
        }

        private async Task GetStatesByCountry(SearchArgs data)
        {
            try
            {
                StatesData.Clear();
                var pagination = new Pagination { PageNumber = data.Page, PageSize = data.PageSize };
                _states = (await StateRepository.GetLookupListAsync(pagination, District.CountryId, data.Filter1)).ToList();
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
                        District.StateId = int.Parse(data.Id.ToString());
                        District.State = data.Description;
                    }
                }
            }
            catch (Exception ex)
            {
                await DialogService.ShowMessageBox(AppMessages.Error, ex.Message);
            }
        }

        private void StateChanged(string id)
        {
            if (_cityFirstRender == false)
            {
                District.CityId = 0;
                District.City = null;
            }
            _cityFirstRender = false;
        }

        private async Task GetCitiesByState(SearchArgs data)
        {
            try
            {
                CitiesData.Clear();
                var pagination = new Pagination { PageNumber = data.Page, PageSize = data.PageSize };
                _cities = (await CityRepository.GetLookupListAsync(pagination, District.StateId, data.Filter1)).ToList();
                foreach (var city in _cities)
                {
                    CitiesData.Add(new ModalWindowData { Id = city.Id, Description = city.Name });
                }
            }
            catch (Exception ex)
            {
                await DialogService.ShowMessageBox(AppMessages.Error, ex.Message);
            }
        }

        private async Task SelectCity()
        {
            try
            {
                var dialogParams = new DialogParameters();
                dialogParams.Add("Data", CitiesData);
                dialogParams.Add("OnSearch", new EventCallbackFactory().Create<SearchArgs>(this, GetCitiesByState));

                var dialogOptions = new DialogOptions { MaxWidth = MaxWidth.Small };
                var dialog = DialogService.Show<AppModalWindowPicker>("Select City", dialogParams, dialogOptions);
                var result = await dialog.Result;
                if (result.Cancelled == false && result.Data != null)
                {
                    var data = result.Data as ModalWindowData;
                    if (data != null)
                    {
                        District.CityId = int.Parse(data.Id.ToString());
                        District.City = data.Description;
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
