﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Funq;
using Microsoft.Phone.Controls;

namespace MyTwit.Services
{
    public class ContainerLocator : IDisposable
    {
        private bool disposed;

        public ContainerLocator()
        {
            this.Container = new Container();
            this.ConfigureContainer();
        }

        public Container Container { get; private set; }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.Container.Dispose();
            }

            this.disposed = true;
        }

        private void ConfigureContainer()
        {
            this.Container.Register("ServiceUri", new Uri("http://127.0.0.1:8080/Survey/"));
            this.Container.Register("RegistrationServiceUri", new Uri("http://127.0.0.1:8080/Registration/"));
            this.Container.Register<ISettingsStore>(c => new SettingsStore());

            this.Container.Register<INavigationService>(_ =>
                new ApplicationFrameNavigationService(((App)Application.Current).RootFrame));

            // LocationService:
            // 1. Registration
            this.Container.Register<ILocationService>(c => new LocationService(c.Resolve<ISettingsStore>()));

            // 2. Starts the service by trying to get the current location
            this.Container.Resolve<ILocationService>().TryToGetCurrentLocation();

            // View Models
            //this.Container.Register(
            //    c => new AppSettingsViewModel(
            //             c.Resolve<ISettingsStore>(),
            //             c.Resolve<IRegistrationServiceClient>(),
            //             c.Resolve<INavigationService>()))
            //    .ReusedWithin(ReuseScope.None);

            //this.Container.Register<ISurveyStoreLocator>(
            //    c => new SurveyStoreLocator(
            //             c.Resolve<ISettingsStore>(),
            //             storeName => new SurveyStore(storeName)));

            //this.Container.Register(
            //    c => new FilterSettingsViewModel(
            //             c.Resolve<IRegistrationServiceClient>(),
            //             c.Resolve<INavigationService>(),
            //             c.Resolve<ISurveyStoreLocator>()))
            //    .ReusedWithin(ReuseScope.None);

            //this.Container.Register<ISurveysSynchronizationService>(
            //    c => new SurveysSynchronizationService(
            //             c.Resolve<ISurveysServiceClient>,
            //             c.Resolve<ISurveyStoreLocator>()));
            //this.Container.Register(
            //    c => new SurveyListViewModel(
            //             c.Resolve<ISurveyStoreLocator>(),
            //             c.Resolve<ISurveysSynchronizationService>(),
            //             c.Resolve<INavigationService>()));

            //// The ONLY_PHONE symbol is only defined in the "OnlyPhone" configuration to run the phone project standalone
//#if ONLY_PHONE
//            this.Container.Register<ISurveysServiceClient>(c => new SurveysServiceClientMock(c.Resolve<ISettingsStore>()));
//            this.Container.Register<IRegistrationServiceClient>(c => new RegistrationServiceClientMock(c.Resolve<ISettingsStore>()));
//#else
//            this.Container.Register<ISurveysServiceClient>(c => new SurveysServiceClient(c.ResolveNamed<Uri>("ServiceUri"), c.Resolve<ISettingsStore>()));
//            this.Container.Register<IRegistrationServiceClient>(c => new RegistrationServiceClient(c.ResolveNamed<Uri>("RegistrationServiceUri"), c.Resolve<ISettingsStore>()));
//#endif
        }
    }

}