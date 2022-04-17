using LocationsApp.DataAccess.Models;
using LocationsApp.DataAccess.Repository;
using LocationsApp.DataAccess.Utils;
using LocationsApp.Shared;
using LocationsApp.Utils;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationsApp.Pages.Locations.Addresses
{
    public partial class Index
    {
        [Inject] public IAddressRepository AddressRepository { get; set; }
        [Inject] public ISnackbar Snackbar { get; set; }
        [Inject] public IDialogService DialogService { get; set; }

        private List<AddressModel> _addresses;
        private AddressModel _address = new AddressModel();
        private AddressModel _addressToDelete;
        private AppMessageBoxDelete _appMessageBoxDelete;

        private int _totalRecords;
        private int _currentPage = 1;
        private int _registersPerPage = 10;
        private bool _previousPageDisabled;
        private bool _nextPageDisabled;

        private string _zipCode = "";
        private string _addressName = "";
        private string _cityName = "";
        private Pagination _pagination = new Pagination();

        protected override async Task OnInitializedAsync()
        {
            await GetTotalRecords();
            await GetAddresses();
        }

        private async Task GetTotalRecords()
        {
            try
            {
                _totalRecords = await AddressRepository.CountAsync();
            }
            catch (Exception ex)
            {
                await DialogService.ShowMessageBox(AppMessages.Error, ex.Message);
            }
        }

        private async Task GetAddresses()
        {
            try
            {
                _pagination.PageNumber = _currentPage;
                _pagination.PageSize = _registersPerPage;
                _addresses = (await AddressRepository.GetDisplayListAsync(_pagination, _zipCode, _addressName, _cityName)).ToList();
                _previousPageDisabled = _currentPage == 1;
                _nextPageDisabled = _addresses == null || _addresses.Count() != _registersPerPage;
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
                var dialogParams = new DialogParameters { ["Address"] = new AddressModel(), ["InEditMode"] = false };
                var dialog = DialogService.Show<AddressAddEditDialog>("Add Address", dialogParams);
                var result = await dialog.Result;
                if (result.Cancelled == false && result.Data != null)
                {
                    _address = result.Data as AddressModel;
                    var id = await AddressRepository.InsertAsync(_address);
                    if (id > 0)
                    {
                        _address = await AddressRepository.FirstOrDefaulAsync(id);
                        _addresses.Insert(0, _address);
                        _address = new AddressModel();
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

        private async Task Edit(AddressModel model)
        {
            try
            {
                var modelToEdit = await AddressRepository.FirstOrDefaulAsync(model.Id);

                if (modelToEdit != null)
                {
                    var dialogParams = new DialogParameters { ["Address"] = modelToEdit, ["InEditMode"] = true };
                    var dialog = DialogService.Show<AddressAddEditDialog>("Edit Address", dialogParams);
                    var result = await dialog.Result;
                    if (result.Cancelled == false && result.Data != null)
                    {
                        var modelResult = result.Data as AddressModel;
                        var success = await AddressRepository.UpdateAsync(modelResult);
                        if (success > 0)
                        {
                            var inThisIndex = _addresses.FindIndex(x => x.Id == model.Id);
                            _addresses.Remove(model);
                            _addresses.Insert(inThisIndex, modelResult);
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

        private async Task Delete(AddressModel model)
        {
            try
            {
                _addressToDelete = model;
                var result = await _appMessageBoxDelete.Show();
                if (result == true)
                {
                    var success = await AddressRepository.DeleteAsync(model.Id);
                    if (success > 0)
                    {
                        _addresses.Remove(model);
                        _totalRecords--;
                        Snackbar.Add(AppMessages.RecordDeletedSuccessfully, Severity.Success);
                    }
                    else
                    {
                        await DialogService.ShowMessageBox(AppMessages.Information, AppMessages.UnableToDeleteRecord);
                    }
                }
                _addressToDelete = null;
            }
            catch (Exception ex)
            {
                await DialogService.ShowMessageBox(AppMessages.Error, ex.Message);
            }
        }

        private async void Search(string filter)
        {
            _zipCode = filter;
            _addressName = filter;
            _cityName = filter;
            _currentPage = 1;
            await GetAddresses();
        }

        private async Task GoToPreviousPage()
        {
            _currentPage--;
            await GetAddresses();
        }

        private async Task GoToNextPage()
        {
            _currentPage++;
            await GetAddresses();
        }
    }
}