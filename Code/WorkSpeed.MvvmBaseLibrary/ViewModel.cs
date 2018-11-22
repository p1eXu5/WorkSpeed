using System;
using System.Collections.ObjectModel;
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
    }
}
