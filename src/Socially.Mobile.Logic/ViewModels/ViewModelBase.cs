using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Socially.Apps.Consumer.Exceptions;
using Socially.Mobile.Logic.Models;
using Socially.Mobile.Logic.Services;
using Socially.Mobile.Logic.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.ViewModels
{
    public partial class ViewModelBase : ObservableObject
    {

        [ObservableProperty]
        ObservableCollection<ValidationResult> validation;

        [ObservableProperty]
        string errorMessage;

        [ObservableProperty]
        bool isLoading;

        public ViewModelBase()
        {
            Validation = new();
        }

        public virtual Task OnNavigatedAsync() => Task.CompletedTask;
        
        public virtual Task OnValidationChangedAsync() => Task.CompletedTask;

    }

    public abstract partial class ViewModelBase<TModel> : ViewModelBase
        where TModel : new()
    {

        [ObservableProperty]
        TModel model;

        public virtual string ErrorOnException => "Failed to submit. Please try again";

        public virtual string ErrorWhenBadRequestEmpty => "Failed. Please try again";

        public ViewModelBase()
        {
            Model = new();
        }

        public virtual Task SubmitToServerAsync(TModel model, CancellationToken cancellationToken = default) => throw new NotImplementedException("Submit not implimented");

        public virtual Task<TModel> GetFromServerAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException("Get not implimented");

        public virtual void OnModelUpdated(TModel model) { }

        partial void OnModelChanged(TModel value) => OnModelUpdated(value);

        public abstract void OnException(Exception ex);

        [RelayCommand]
        public async Task GetAsync()
        {
            IsLoading = true;
            await ExecuteAndValidate(async () => Model = await GetFromServerAsync());
            IsLoading = false;
        }

        [RelayCommand]
        public Task SubmitAsync()
        {
            if (model is IValidatable validatable && !validatable.Validate(Validation))
            {
                OnPropertyChanged(nameof(Validation));
                OnValidationChangedAsync();
                return Task.CompletedTask;
            }
            return ExecuteAndValidate(() => SubmitToServerAsync(model));
        }

        async Task ExecuteAndValidate(Func<Task> func)
        {
            try
            {
                await func();
                await OnValidationChangedAsync();
            }
            catch (ErrorForClientException clientException)
            {
                Validation = clientException.ToObservableCollection();
                if (!Validation.Any() || clientException.Errors.SelectMany(e => e.Errors).Sum(s => s.Length) == 0)
                    ErrorMessage = ErrorWhenBadRequestEmpty;
                await OnValidationChangedAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = ErrorOnException;
                OnException(ex);
            }
        }

    }

}
