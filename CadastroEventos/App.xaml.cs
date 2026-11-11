
using CadastroEventos.Views;

namespace CadastroEventos
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window window = new Window(new NavigationPage(new CadastroEvento()));

            window.Width = 400;
            window.Height = 600;

            return window;
        }
    }
}
