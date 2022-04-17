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

namespace LocationsApp.Pages.Locations.Addresses
{
    public partial class AddressAddEditDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public AddressModel Address { get; set; } = new AddressModel();
        [Parameter] public bool InEditMode { get; set; }
        [Inject] public IDialogService DialogService { get; set; }
        [Inject] public ICountryRepository CountryRepository { get; set; }
        [Inject] public IStateRepository StateRepository { get; set; }
        [Inject] public ICityRepository CityRepository { get; set; }
        [Inject] public IDistrictRepository DistrictRepository { get; set; }

        public List<ModalWindowData> CountriesData { get; set; } = new List<ModalWindowData>();
        public List<ModalWindowData> StatesData { get; set; } = new List<ModalWindowData>();
        public List<ModalWindowData> CitiesData { get; set; } = new List<ModalWindowData>();
        public List<ModalWindowData> DistrictsData { get; set; } = new List<ModalWindowData>();

        private List<CountryModel> _countries = new List<CountryModel>();
        private List<StateModel> _states = new List<StateModel>();
        private List<CityModel> _cities = new List<CityModel>();
        private List<DistrictModel> _districts = new List<DistrictModel>();

        private bool _stateFirstRender = true;
        private bool _cityFirstRender = true;
        private bool _districtFirstRender = true;

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private void Ok()
        {
            MudDialog.Close(DialogResult.Ok(Address));
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
                        Address.CountryId = int.Parse(data.Id.ToString());
                        Address.Country = data.Description;
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
                Address.StateId = 0;
                Address.State = null;
                Address.CityId = 0;
                Address.City = null;
                Address.DistrictId = 0;
                Address.District = null;
            }
            _stateFirstRender = false;
        }

        private async Task GetStatesByCountry(SearchArgs data)
        {
            try
            {
                StatesData.Clear();
                var pagination = new Pagination { PageNumber = data.Page, PageSize = data.PageSize };
                _states = (await StateRepository.GetLookupListAsync(pagination, Address.CountryId, data.Filter1)).ToList();
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
                        Address.StateId = int.Parse(data.Id.ToString());
                        Address.State = data.Description;
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
                Address.CityId = 0;
                Address.City = null;
                Address.DistrictId = 0;
                Address.District = null;
            }
            _cityFirstRender = false;
        }

        private async Task GetCitiesByState(SearchArgs data)
        {
            try
            {
                CitiesData.Clear();
                var pagination = new Pagination { PageNumber = data.Page, PageSize = data.PageSize };
                _cities = (await CityRepository.GetLookupListAsync(pagination, Address.StateId, data.Filter1)).ToList();
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
                        Address.CityId = int.Parse(data.Id.ToString());
                        Address.City = data.Description;
                    }
                }
            }
            catch (Exception ex)
            {
                await DialogService.ShowMessageBox(AppMessages.Error, ex.Message);
            }
        }

        private void CityChanged(string id)
        {
            if (_districtFirstRender == false)
            {
                Address.DistrictId = 0;
                Address.District = null;
            }
            _districtFirstRender = false;
        }

        private async Task GetDistrictsByCity(SearchArgs data)
        {
            try
            {
                DistrictsData.Clear();
                var pagination = new Pagination { PageNumber = data.Page, PageSize = data.PageSize };
                _districts = (await DistrictRepository.GetLookupListAsync(pagination, Address.CityId, data.Filter1)).ToList();
                foreach (var district in _districts)
                {
                    DistrictsData.Add(new ModalWindowData { Id = district.Id, Description = district.Name });
                }
            }
            catch (Exception ex)
            {
                await DialogService.ShowMessageBox(AppMessages.Error, ex.Message);
            }
        }

        private async Task SelectDistrict()
        {
            try
            {
                var dialogParams = new DialogParameters();
                dialogParams.Add("Data", DistrictsData);
                dialogParams.Add("OnSearch", new EventCallbackFactory().Create<SearchArgs>(this, GetDistrictsByCity));

                var dialogOptions = new DialogOptions { MaxWidth = MaxWidth.Small };
                var dialog = DialogService.Show<AppModalWindowPicker>("Select District", dialogParams, dialogOptions);
                var result = await dialog.Result;
                if (result.Cancelled == false && result.Data != null)
                {
                    var data = result.Data as ModalWindowData;
                    if (data != null)
                    {
                        Address.DistrictId = int.Parse(data.Id.ToString());
                        Address.District = data.Description;
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
