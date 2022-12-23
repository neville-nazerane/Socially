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

        public abstract void OnException(Exception ex);

        public override Task OnNavigatedAsync() => GetAsync();

        [RelayCommand]
        public Task GetAsync() => ExecuteAndValidate(async () => Model = await GetFromServerAsync());

        [RelayCommand]
        public Task SubmitAsync()
        {
            Validation.Clear();
            if (model is IValidatable validatable && !validatable.Validate(Validation))
                return Task.CompletedTask;
            return ExecuteAndValidate(() => SubmitToServerAsync(model));
        }

        async Task ExecuteAndValidate(Func<Task> func)
        {
            try
            {
                await func();
            }
            catch (ErrorForClientException clientException)
            {
                Validation = clientException.ToObservableCollection();
                if (!Validation.Any() || clientException.Errors.SelectMany(e => e.Errors).Sum(s => s.Length) == 0)
                    ErrorMessage = ErrorWhenBadRequestEmpty;
            }
            catch (Exception ex)
            {
                ErrorMessage = ErrorOnException;
                OnException(ex);
            }
        }

    }

}
