﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace WorkSpeed.MvvmBaseLibrary
{
    public abstract class ViewModel : NotifyingObject, IDataErrorInfo
    {
        public string this[string columnName] => OnValidate(columnName);

        public string Error => throw new NotImplementedException($"{nameof(IDataErrorInfo)} shouldn't implement Error property for WPF error binding.");

        protected virtual string OnValidate(string propertyName)
        {
            ValidationContext validationContext = new ValidationContext(this) {DisplayName = propertyName};
            var errorsCollection = new Collection<ValidationResult>();

            if (Validator.TryValidateProperty(this, validationContext, errorsCollection)) {

                return errorsCollection.First().ErrorMessage;
            }

            return null;
        }

        protected static void Observe<TModel, TViewModel> (ReadOnlyObservableCollection<TModel> observable, 
                                                               ObservableCollection<TViewModel> observator, 
                                                                        Func<TViewModel,TModel> func)
        {
            ((INotifyCollectionChanged) observable).CollectionChanged += (s, e) =>
                {
                    if (e.NewItems[0] != null) {
                        observator.Add ((TViewModel)Activator.CreateInstance (typeof(TViewModel), new[] { (TModel)e.NewItems[0] }));
                    }

                    if (e.OldItems[0] != null) {
                        observator.Remove (observator.First(vm => ReferenceEquals (func(vm), e.OldItems[0])));
                    }

                    if (e.NewItems[0] == null && e.OldItems[0] == null) {
                        observator.Clear();
                    }
                };
        }
    }
}
