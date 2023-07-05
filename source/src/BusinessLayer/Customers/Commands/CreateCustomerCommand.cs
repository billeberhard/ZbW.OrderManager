﻿using BusinessLayer.Base.Command;
using BusinessLayer.Base.Services;
using BusinessLayer.Base.Stores;
using BusinessLayer.Customers.ViewModels;
using DataLayer.Customers.Models;
using System.ComponentModel;


namespace BusinessLayer.Customers.Commands
{
    public class CreateCustomerCommand : BaseAsyncCommand
    {
        public CreateCustomerCommand(ManagerStore managerStore, CreateCustomerViewModel createCustomerViewModel, NavigationService customerListingViewModelNavigationService)
        {
            _managerStore = managerStore;
            _createCustomerViewModel = createCustomerViewModel;
            _customerListingViewModelNavigationService = customerListingViewModelNavigationService;

            _createCustomerViewModel.ErrorsChanged += OnHasCustomerPropertyErrorChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            return !_createCustomerViewModel.HasErrors && base.CanExecute(parameter);
        }

        public void OnHasCustomerPropertyErrorChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }


        public async override Task ExecuteAsync(object? parameter)
        {
            try
            {
                Customer customer = new Customer(
                    await _managerStore.GetNextFreeCustomerIdAsync(),
                    _createCustomerViewModel.FirstName,
                    _createCustomerViewModel.LastName,
                    _createCustomerViewModel.StreetName,
                    _createCustomerViewModel.HouseNumber,
                    _createCustomerViewModel.City,
                    _createCustomerViewModel.PostalCode,
                    _createCustomerViewModel.EmailAddress,
                    _createCustomerViewModel.WebsiteUrl,
                    _createCustomerViewModel.Password
                    );

                await _managerStore.CreateCustomer(customer);
            }
            catch (Exception e)
            {
                //MessageBox.Show("Failed to Create Customer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            _customerListingViewModelNavigationService.Navigate();
        }

        private readonly ManagerStore _managerStore;
        private readonly CreateCustomerViewModel _createCustomerViewModel;
        private readonly NavigationService _customerListingViewModelNavigationService;
    }
}
