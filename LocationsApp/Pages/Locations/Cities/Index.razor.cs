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

namespace LocationsApp.Pages.Locations.Cities
{
    public partial class Index
    {
        [Inject] public ICityRepository CityRepository { get; set; }
        [Inject] public ISnackbar Snackbar { get; set; }
        [Inject] public IDialogService DialogService { get; set; }

        private List<CityModel> _cities;
        private CityModel _city = new CityModel();
        private CityModel _cityToDelete;
        private AppMessageBoxDelete _appMessageBoxDelete;

        private int _totalRecords;
        private int _currentPage = 1;
        private int _registersPerPage = 10;
        private bool _previousPageDisabled;
        private bool _nextPageDisabled;

        private string _cityName = "";
        private string _stateName = "";
        private Pagination _pagination = new Pagination();

        protected override async Task OnInitializedAsync()
        {
            await GetTotalRecords();
            await GetCities();
        }

        private async Task GetTotalRecords()
        {
            try
            {
                _totalRecords = await CityRepository.CountAsync();
            }
            catch (Exception ex)
            {
                await DialogService.ShowMessageBox(AppMessages.Error, ex.Message);
            }
        }

        private async Task GetCities()
        {
            try
            {
                _pagination.PageNumber = _currentPage;
                _pagination.PageSize = _registersPerPage;
                _cities = (await CityRepository.GetDisplayListAsync(_pagination, _cityName, _stateName)).ToList();
                _previousPageDisabled = _currentPage == 1;
                _nextPageDisabled = _cities == null || _cities.Count() != _registersPerPage;
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
                var dialogParams = new DialogParameters { ["City"] = new CityModel(), ["InEditMode"] = false };
                var dialog = DialogService.Show<CityAddEditDialog>("Add City", dialogParams);
                var result = await dialog.Result;
                if (result.Cancelled == false && result.Data != null)
                {
                    _city = result.Data as CityModel;
                    var id = await CityRepository.InsertAsync(_city);
                    if (id > 0)
                    {
                        _city = await CityRepository.FirstOrDefaulAsync(id);
                        _cities.Insert(0, _city);
                        _city = new CityModel();
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

        private async Task Edit(CityModel model)
        {
            try
            {
                var modelToEdit = await CityRepository.FirstOrDefaulAsync(model.Id);

                if (modelToEdit != null)
                {
                    var dialogParams = new DialogParameters { ["City"] = modelToEdit, ["InEditMode"] = true };
                    var dialog = DialogService.Show<CityAddEditDialog>("Edit City", dialogParams);
                    var result = await dialog.Result;
                    if (result.Cancelled == false && result.Data != null)
                    {
                        var modelResult = result.Data as CityModel;
                        var success = await CityRepository.UpdateAsync(modelResult);
                        if (success > 0)
                        {
                            var inThisIndex = _cities.FindIndex(x => x.Id == model.Id);
                            _cities.Remove(model);
                            _cities.Insert(inThisIndex, modelResult);
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

        private async Task Delete(CityModel model)
        {
            try
            {
                _cityToDelete = model;
                var result = await _appMessageBoxDelete.Show();
                if (result == true)
                {
                    var success = await CityRepository.DeleteAsync(model.Id);
                    if (success > 0)
                    {
                        _cities.Remove(model);
                        _totalRecords--;
                        Snackbar.Add(AppMessages.RecordDeletedSuccessfully, Severity.Success);
                    }
                    else
                    {
                        await DialogService.ShowMessageBox(AppMessages.Information, AppMessages.UnableToDeleteRecord);
                    }
                }
                _cityToDelete = null;
            }
            catch (Exception ex)
            {
                await DialogService.ShowMessageBox(AppMessages.Error, ex.Message);
            }
        }

        private async void Search(string filter)
        {
            _stateName = filter;
            _cityName = filter;
            _currentPage = 1;
            await GetCities();
        }

        private async Task GoToPreviousPage()
        {
            _currentPage--;
            await GetCities();
        }

        private async Task GoToNextPage()
        {
            _currentPage++;
            await GetCities();
        }
    }
}