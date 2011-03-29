using System;
using MyTwit.Services;

namespace MyTwit.ViewModels
{
    public class ViewModelLocator : IDisposable
    {
        private readonly ContainerLocator _containerLocator;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelLocator"/> class.
        /// </summary>
        public ViewModelLocator()
        {
            this._containerLocator = new ContainerLocator();
        }

        /// <summary>
        /// Gets the app settings view model.
        /// </summary>
        /// <value>The app settings view model.</value>
        public AppSettingsViewModel AppSettingsViewModel
        {
            get { return this._containerLocator.Container.Resolve<AppSettingsViewModel>(); }
        }

        /// <summary>
        /// Gets the filter settings view model.
        /// </summary>
        /// <value>The filter settings view model.</value>
        public FilterSettingsViewModel FilterSettingsViewModel
        {
            get { return this._containerLocator.Container.Resolve<FilterSettingsViewModel>(); }
        }

        //public SurveyListViewModel SurveyListViewModel
        //{
        //    get
        //    {
        //        return this.containerLocator.Container.Resolve<SurveyListViewModel>();
        //    }
        //}

        //public TakeSurveyViewModel TakeSurveyViewModel
        //{
        //    get
        //    {
        //        var templateViewModel = this.SurveyListViewModel.SelectedSurveyTemplate;
        //        var vm = new TakeSurveyViewModel(this.containerLocator.Container.Resolve<INavigationService>())
        //        {
        //            SurveyStoreLocator = this.containerLocator.Container.Resolve<ISurveyStoreLocator>(),
        //            LocationService = this.containerLocator.Container.Resolve<ILocationService>(),
        //            TemplateViewModel = templateViewModel,
        //            SurveyAnswer = this.containerLocator.Container.Resolve<ISurveyStoreLocator>().GetStore().GetCurrentAnswerForTemplate(templateViewModel.Template) ??
        //                           templateViewModel.Template.CreateSurveyAnswers(this.containerLocator.Container.Resolve<ILocationService>())
        //        };
        //        vm.Initialize();
        //        return vm;
        //    }
        //}

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed)
            {
                return;
            }

            if (disposing)
            {
                this._containerLocator.Dispose();
            }

            this._disposed = true;
        }
    }
}
