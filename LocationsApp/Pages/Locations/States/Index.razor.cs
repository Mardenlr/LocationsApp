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

namespace LocationsApp.Pages.Locations.States
{
    public partial class Index
    {
        [Inject] public IStateRepository StateRepository { get; set; }
        [Inject] public ISnackbar Snackbar { get; set; }
        [Inject] public IDialogService DialogService { get; set; }

        private List<StateModel> _states;
        private StateModel _state = new StateModel();
        private StateModel _stateToDelete;
        private AppMessageBoxDelete _appMessageBoxDelete;

        private int _totalRecords;
        private int _currentPage = 1;
        private int _registersPerPage = 10;
        private bool _previousPageDisabled;
        private bool _nextPageDisabled;

        private string _stateName = "";
        private string _countryName = "";
        private Pagination _pagination = new Pagination();

        protected override async Task OnInitializedAsync()
        {
            await GetTotalRecords();
            await GetStates();
        }

        private async Task GetTotalRecords()
        {
            try
            {
                _totalRecords = await StateRepository.CountAsync();
            }
            catch (Exception ex)
            {
                await DialogService.ShowMessageBox(AppMessages.Error, ex.Message);
            }
        }

        private async Task GetStates()
        {
            try
            {
                _pagination.PageNumber = _currentPage;
                _pagination.PageSize = _registersPerPage;
                _states = (await StateRepository.GetDisplayListAsync(_pagination, _stateName, _countryName)).ToList();
                _previousPageDisabled = _currentPage == 1;
                _nextPageDisabled = _states == null || _states.Count() != _registersPerPage;
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
                var dialogParams = new DialogParameters { ["State"] = new StateModel(), ["InEditMode"] = false };
                var dialog = DialogService.Show<StateAddEditDialog>("Add State", dialogParams);
                var result = await dialog.Result;
                if (result.Cancelled == false && result.Data != null)
                {
                    _state = result.Data as StateModel;
                    var id = await StateRepository.InsertAsync(_state);
                    if (id > 0)
                    {
                        _state = await StateRepository.FirstOrDefaulAsync(id);
                        _states.Insert(0, _state);
                        _state = new StateModel();
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

        private async Task Edit(StateModel model)
        {
            try
            {
                var modelToEdit = await StateRepository.FirstOrDefaulAsync(model.Id);

                if (modelToEdit != null)
                {
                    var dialogParams = new DialogParameters { ["State"] = modelToEdit, ["InEditMode"] = true };
                    var dialog = DialogService.Show<StateAddEditDialog>("Edit State", dialogParams);
                    var result = await dialog.Result;
                    if (result.Cancelled == false && result.Data != null)
                    {
                        var modelResult = result.Data as StateModel;
                        var success = await StateRepository.UpdateAsync(modelResult);
                        if (success > 0)
                        {
                            var inThisIndex = _states.FindIndex(x => x.Id == model.Id);
                            _states.Remove(model);
                            _states.Insert(inThisIndex, modelResult);
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

        private async Task Delete(StateModel model)
        {
            try
            {
                _stateToDelete = model;
                var result = await _appMessageBoxDelete.Show();
                if (result == true)
                {
                    var success = await StateRepository.DeleteAsync(model.Id);
                    if (success > 0)
                    {
                        _states.Remove(model);
                        _totalRecords--;
                        Snackbar.Add(AppMessages.RecordDeletedSuccessfully, Severity.Success);
                    }
                    else
                    {
                        await DialogService.ShowMessageBox(AppMessages.Information, AppMessages.UnableToDeleteRecord);
                    }
                }
                _stateToDelete = null;
            }
            catch (Exception ex)
            {
                await DialogService.ShowMessageBox(AppMessages.Error, ex.Message);
            }
        }

        private async void Search(string filter)
        {
            _countryName = filter;
            _stateName = filter;
            _currentPage = 1;
            await GetStates();
        }

        private async Task GoToPreviousPage()
        {
            _currentPage--;
            await GetStates();
        }

        private async Task GoToNextPage()
        {
            _currentPage++;
            await GetStates();
        }
    }
}