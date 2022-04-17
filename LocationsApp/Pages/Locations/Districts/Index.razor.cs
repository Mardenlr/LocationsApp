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

namespace LocationsApp.Pages.Locations.Districts
{
    public partial class Index
    {
        [Inject] public IDistrictRepository DistrictRepository { get; set; }
        [Inject] public ISnackbar Snackbar { get; set; }
        [Inject] public IDialogService DialogService { get; set; }

        private List<DistrictModel> _districts;
        private DistrictModel _district = new DistrictModel();
        private DistrictModel _districtToDelete;
        private AppMessageBoxDelete _appMessageBoxDelete;

        private int _totalRecords;
        private int _currentPage = 1;
        private int _registersPerPage = 10;
        private bool _previousPageDisabled;
        private bool _nextPageDisabled;

        private string _districtName = "";
        private string _cityName = "";
        private Pagination _pagination = new Pagination();

        protected override async Task OnInitializedAsync()
        {
            await GetTotalRecords();
            await GetDistricts();
        }

        private async Task GetTotalRecords()
        {
            try
            {
                _totalRecords = await DistrictRepository.CountAsync();
            }
            catch (Exception ex)
            {
                await DialogService.ShowMessageBox(AppMessages.Error, ex.Message);
            }
        }

        private async Task GetDistricts()
        {
            try
            {
                _pagination.PageNumber = _currentPage;
                _pagination.PageSize = _registersPerPage;
                _districts = (await DistrictRepository.GetDisplayListAsync(_pagination, _districtName, _cityName)).ToList();
                _previousPageDisabled = _currentPage == 1;
                _nextPageDisabled = _districts == null || _districts.Count() != _registersPerPage;
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
                var dialogParams = new DialogParameters { ["District"] = new DistrictModel(), ["InEditMode"] = false };
                var dialog = DialogService.Show<DistrictAddEditDialog>("Add District", dialogParams);
                var result = await dialog.Result;
                if (result.Cancelled == false && result.Data != null)
                {
                    _district = result.Data as DistrictModel;
                    var id = await DistrictRepository.InsertAsync(_district);
                    if (id > 0)
                    {
                        _district = await DistrictRepository.FirstOrDefaulAsync(id);
                        _districts.Insert(0, _district);
                        _district = new DistrictModel();
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

        private async Task Edit(DistrictModel model)
        {
            try
            {
                var modelToEdit = await DistrictRepository.FirstOrDefaulAsync(model.Id);

                if (modelToEdit != null)
                {
                    var dialogParams = new DialogParameters { ["District"] = modelToEdit, ["InEditMode"] = true };
                    var dialog = DialogService.Show<DistrictAddEditDialog>("Edit District", dialogParams);
                    var result = await dialog.Result;
                    if (result.Cancelled == false && result.Data != null)
                    {
                        var modelResult = result.Data as DistrictModel;
                        var success = await DistrictRepository.UpdateAsync(modelResult);
                        if (success > 0)
                        {
                            var inThisIndex = _districts.FindIndex(x => x.Id == model.Id);
                            _districts.Remove(model);
                            _districts.Insert(inThisIndex, modelResult);
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

        private async Task Delete(DistrictModel model)
        {
            try
            {
                _districtToDelete = model;
                var result = await _appMessageBoxDelete.Show();
                if (result == true)
                {
                    var success = await DistrictRepository.DeleteAsync(model.Id);
                    if (success > 0)
                    {
                        _districts.Remove(model);
                        _totalRecords--;
                        Snackbar.Add(AppMessages.RecordDeletedSuccessfully, Severity.Success);
                    }
                    else
                    {
                        await DialogService.ShowMessageBox(AppMessages.Information, AppMessages.UnableToDeleteRecord);
                    }
                }
                _districtToDelete = null;
            }
            catch (Exception ex)
            {
                await DialogService.ShowMessageBox(AppMessages.Error, ex.Message);
            }
        }

        private async void Search(string filter)
        {
            _cityName = filter;
            _districtName = filter;
            _currentPage = 1;
            await GetDistricts();
        }

        private async Task GoToPreviousPage()
        {
            _currentPage--;
            await GetDistricts();
        }

        private async Task GoToNextPage()
        {
            _currentPage++;
            await GetDistricts();
        }
    }
}