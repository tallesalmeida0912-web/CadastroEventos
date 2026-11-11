
using System;
using CadastroEventos.Models;

namespace CadastroEventos.Views
{
    public partial class CadastroEvento : ContentPage
    {
        private Evento evento;

        private DateTime dataInicioAnterior;

        public CadastroEvento()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            evento = new Evento();

            BindingContext = evento;

            // Configuração da data de início
            dpDataInicio.MinimumDate = DateTime.Today;
            dpDataInicio.MaximumDate = DateTime.Today.AddMonths(6);

            // Configuração inicial da data de término
            dataInicioAnterior = dpDataInicio.Date.Value;

            dpDataTermino.MinimumDate = dataInicioAnterior;
            dpDataTermino.MaximumDate = dataInicioAnterior.AddDays(30);
        }

        // Atualização dinâmica da data de término
        private void OnDataInicioSelected(object sender, DateChangedEventArgs e)
        {
            DatePicker elemento = sender as DatePicker;

            DateTime dataSelecionada = elemento.Date.Value;

            DateTime novaDataMinima = dataSelecionada;
            DateTime novaDataMaxima = dataSelecionada.AddDays(30);

            // Atualiza os limites conforme a nova data inicial
            if (dataSelecionada > dataInicioAnterior)
            {
                dpDataTermino.MaximumDate = novaDataMaxima;
                dpDataTermino.MinimumDate = novaDataMinima;
            }
            else
            {
                dpDataTermino.MinimumDate = novaDataMinima;
                dpDataTermino.MaximumDate = novaDataMaxima;
            }

            // Corrige a data final se estiver fora dos limites
            if (dpDataTermino.Date.Value < novaDataMinima)
            {
                dpDataTermino.Date = novaDataMinima;
            }
            else if (dpDataTermino.Date.Value > novaDataMaxima)
            {
                dpDataTermino.Date = novaDataMaxima;
            }

            evento.DataInicio = dataSelecionada;
            evento.DataTermino = dpDataTermino.Date.Value;

            dataInicioAnterior = dataSelecionada;
        }

        // Cadastro do evento
        private async void OnCadastrarClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(evento.Nome) ||
                string.IsNullOrWhiteSpace(evento.Local))
            {
                await DisplayAlertAsync(
                    "Atenção",
                    "Preencha o nome e o local do evento.",
                    "OK");
                return;
            }

            if (evento.NumeroParticipantes <= 0)
            {
                await DisplayAlertAsync(
                    "Atenção",
                    "Informe um número válido de participantes.",
                    "OK");
                return;
            }

            if (evento.CustoPorParticipante < 0)
            {
                await DisplayAlertAsync(
                    "Atenção",
                    "Informe um custo válido.",
                    "OK");
                return;
            }

            if (evento.DataTermino.Date < evento.DataInicio.Date)
            {
                await DisplayAlertAsync(
                    "Atenção",
                    "A data de término não pode ser anterior à data de início.",
                    "OK");
                return;
            }

            await Navigation.PushAsync(new ResumoEvento(evento));
        }
    }
}
