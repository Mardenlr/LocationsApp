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

namespace LocationsApp.Pages.Locations.Countries
{
    public partial class Index
    {
        [Inject] public ICountryRepository CountryRepository { get; set; }
        [Inject] public ISnackbar Snackbar { get; set; }
        [Inject] public IDialogService DialogService { get; set; }

        private List<CountryModel> _countries;
        private CountryModel _country = new CountryModel();
        private CountryModel _countryToDelete;
        private AppMessageBoxDelete _appMessageBoxDelete;

        private int _totalRecords;
        private int _currentPage = 1;
        private int _registersPerPage = 10;
        private bool _previousPageDisabled;
        private bool _nextPageDisabled;

        private string _countryName = "";
        private Pagination _pagination = new Pagination();

        protected override async Task OnInitializedAsync()
        {
            await GetTotalRecords();
            await GetCountries();
        }

        private async Task GetTotalRecords()
        {
            try
            {
                _totalRecords = await CountryRepository.CountAsync();
            }
            catch (Exception ex)
            {
                await DialogService.ShowMessageBox(AppMessages.Error, ex.Message);
            }
        }

        private async Task GetCountries()
        {
            try
            {
                _pagination.PageNumber = _currentPage;
                _pagination.PageSize = _registersPerPage;
                _countries = (await CountryRepository.GetDisplayListAsync(_pagination, _countryName)).ToList();
                _previousPageDisabled = _currentPage == 1;
                _nextPageDisabled = _countries == null || _countries.Count() != _registersPerPage;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                await DialogService.ShowMessageBox(AppMessages.Error, ex.Message);
            }
        }

        private async Task Add()
        {
            try
            {
                var dialogParams = new DialogParameters { ["Country"] = new CountryModel(), ["InEditMode"] = false };
                var dialog = DialogService.Show<CountryAddEditDialog>("Add Country", dialogParams);
                var result = await dialog.Result;
                if (result.Cancelled == false && result.Data != null)
                {
                    _country = result.Data as CountryModel;
                    var id = await CountryRepository.InsertAsync(_country);
                    if (id > 0)
                    {
                        _country = await CountryRepository.FirstOrDefaulAsync(id);
                        _countries.Insert(0, _country);
                        _country = new CountryModel();
                        _totalRecords++;
                        Snackbar.Add(AppMessages.RecordCreatedSucessfully, Severity.Success);
                    }
                    else
                    {
                        await DialogService.ShowMessageBox(AppMessages.Information, AppMessages.UnableToAddRecord);
                    }
                }
            }
            catch (Exception ex)
            {
                await DialogService.ShowMessageBox(AppMessages.Error, ex.Message);
            }
        }

        private async Task Edit(CountryModel model)
        {
            try
            {
                var modelToEdit = await CountryRepository.FirstOrDefaulAsync(model.Id);

                if (modelToEdit != null)
                {
                    var dialogParams = new DialogParameters { ["Country"] = modelToEdit, ["InEditMode"] = true };
                    var dialog = DialogService.Show<CountryAddEditDialog>("Edit Country", dialogParams);
                    var result = await dialog.Result;
                    if (result.Cancelled == false && result.Data != null)
                    {
                        var modelResult = result.Data as CountryModel;
                        var success = await CountryRepository.UpdateAsync(modelResult);
                        if (success > 0)
                        {
                            var inThisIndex = _countries.FindIndex(x => x.Id == model.Id);
                            _countries.Remove(model);
                            _countries.Insert(inThisIndex, modelResult);
                            Snackbar.Add(AppMessages.RecordUpdatedSucessfully, Severity.Success);
                        }
                        else
                        {
                            await DialogService.ShowMessageBox(AppMessages.Information, AppMessages.UnableToUpdateRecord);
                        }
                    }
                }
                else
                {
                    await DialogService.ShowMessageBox(AppMessages.Information, AppMessages.RecordDontExistsInDatabase);
                }
            }
            catch (Exception ex)
            {
                await DialogService.ShowMessageBox(AppMessages.Error, ex.Message);
            }
        }

        private async Task Delete(CountryModel model)
        {
            try
            {
                _countryToDelete = model;
                var result = await _appMessageBoxDelete.Show();
                if (result == true)
                {
                    var success = await CountryRepository.DeleteAsync(model.Id);
                    if (success > 0)
                    {
                        _countries.Remove(model);
                        _totalRecords--;
                        Snackbar.Add(AppMessages.RecordDeletedSuccessfully, Severity.Success);
                    }
                    else
                    {
                        await DialogService.ShowMessageBox(AppMessages.Information, AppMessages.UnableToDeleteRecord);
                    }
                }
                _countryToDelete = null;
            }
            catch (Exception ex)
            {
                await DialogService.ShowMessageBox(AppMessages.Error, ex.Message);
            }
        }

        private async void Search(string filter)
        {
            _countryName = filter;
            _currentPage = 1;
            await GetCountries();
        }

        private async Task GoToPreviousPage()
        {
            _currentPage--;
            await GetCountries();
        }

        private async Task GoToNextPage()
        {
            _currentPage++;
            await GetCountries();
        }
    }
}