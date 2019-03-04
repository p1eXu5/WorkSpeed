using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService.Entities
{
    public class CategoryViewModel : ViewModel
    {
        private readonly Category _category;

        public CategoryViewModel( Category category )
        {
            _category = category ?? throw new ArgumentNullException(nameof(category), @"Category cannot be null.");
        }

        public Category Category => _category;

        public double? MinVolume
        {
            get => _category.MinVolume;
            set {
                _category.MinVolume = value;
                OnPropertyChanged();
            }
        }

        public double? MaxVolume
        {
            get => _category.MaxVolume;
            set {
                _category.MaxVolume = value;
                OnPropertyChanged();
            }
        }
    }
}
