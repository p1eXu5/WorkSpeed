using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WorkSpeed.MvvmBaseLibrary.Tests.UnitTests
{
    [TestFixture]
    public class ViewModelUnitTests
    {
        [Test]
        public void ViewModel_IsAbstract()
        {
            Assert.That(typeof(ViewModel).IsAbstract);
        }

        [Test]
        public void ViewModel_ByDefault_DerivesNotyfierObject()
        {
            var viewModel = GetViewModel();

            Assert.That(viewModel, Is.InstanceOf<NotifyingObject>());
        }

        [Test]
        public void ViewModel_ByDefault_DerivesIDataErrorInfo()
        {
            var viewModel = GetViewModel();

            Assert.That(viewModel, Is.InstanceOf<IDataErrorInfo>());
        }

        #region Factory

        private ViewModel GetViewModel()
        {
            return new FakeViewModel();
        }

        class FakeViewModel : ViewModel
        { }

        #endregion
    }
}
