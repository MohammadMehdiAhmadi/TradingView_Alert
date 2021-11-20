using SimpleInjector;
using SimpleInjector.Diagnostics;
using System.Windows.Forms;
using TradingView_Example.Services.TradingView;

namespace TradingView_Example.Ioc
{
    public static class SimpleInjectorConfig
    {
        public static Container Container;

        #region Setup

        public static Container Setup()
        {
            Container = new Container();

            Container.Register<ITradingViewClient, TradingViewClient>(Lifestyle.Singleton);

            AutoRegisterWindowsForms(Container);

            Container.Verify();

            return Container;
        }

        #endregion

        #region AutoRegisterWindowsForms

        private static void AutoRegisterWindowsForms(Container container)
        {
            var types = container.GetTypesToRegister<Form>(typeof(Program).Assembly);

            foreach (var type in types)
            {
                var registration =
                    Lifestyle.Transient.CreateRegistration(type, container);

                registration.SuppressDiagnosticWarning(
                    DiagnosticType.DisposableTransientComponent,
                    "Forms should be disposed by app code; not by the container.");

                container.AddRegistration(type, registration);
            }
        }

        #endregion
    }
}
