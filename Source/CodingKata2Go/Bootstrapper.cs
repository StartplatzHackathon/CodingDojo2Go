using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodingKata2Go.Services;
using CodingKata2Go.ViewModel;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace CodingKata2Go
{
    public class Bootstrapper
    {
        public Bootstrapper()
        {
            SimpleIoc.Default.Register<UserService>();
            SimpleIoc.Default.Register<ExcerciseService>();
            SimpleIoc.Default.Register<DojoStateMachine>();
        }

        public ExcerciseService ExcerciseService
        {
            get { return ServiceLocator.Current.GetInstance<ExcerciseService>(); }
        }

        public DojoStateMachine DojoStateMachine
        {
            get { return ServiceLocator.Current.GetInstance<DojoStateMachine>(); }
        }
    }
}
