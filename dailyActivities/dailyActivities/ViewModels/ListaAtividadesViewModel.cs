using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using dailyActivities.Models;
using Xamarin.Forms;

namespace dailyActivities.ViewModels
{
    class ListaAtividadesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        /*
        Pegando o que foi digitado pelo usuário
        */

        public string ParametroBusca { get; set; }
        /*
        Gerencia se mostra ao usuário o RefreshView
        */
        bool estaAtualizado = false;
        public bool EstaAtualizado
        {
            get => estaAtualizado;
            set
            {
                estaAtualizado = value;
                PropertyChanged(this, new PropertyChangedEventArgs("EstaAtualizaado"));
            }
        }
        /*
       Coleção que armazena as atividades do usuário
       */

        ObservableCollection<Atividade> listaAtividade = new ObservableCollection<Atividade>();

        public ObservableCollection<Atividade> ListaAtividades
        {
            get => listaAtividade;
            set => listaAtividade = value;
        }

        public ICommand AtualizarLista
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (EstaAtualizado)
                            return;

                        EstaAtualizado = true;
                        List<Atividade> tmp = await App.Database.GetAllRows();
                        ListaAtividades.Clear();
                        tmp.ForEach(i => ListaAtividades.Add(i));
                    }
                    catch (Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert("OPS at", ex.Message, "OK");
                    }
                    finally
                    {
                        EstaAtualizado = false;
                    }
                });
            }
        }

        public ICommand Buscar
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (EstaAtualizado)
                            return;

                        EstaAtualizado = true;
                        List<Atividade> tmp = await App.Database.Search(ParametroBusca);
                        ListaAtividades.Clear();
                        tmp.ForEach(i => ListaAtividades.Add(i));
                    }
                    catch (Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert("OPS ax", ex.Message, "OK");
                    }
                    finally
                    {
                        EstaAtualizado = false;
                    }
                });
            }
        }

        public ICommand AbrirDetalhesAtividade
        {
            get
            {
                return new Command<int>(async (int id) =>
                {
                    await Shell.Current.GoToAsync($"//CadastroAtividade?parametro_id={id}");                    
                });
            }
        }

        public ICommand Remover
        {
            get
            {
                return new Command<int>(async (int id) =>
                {
                    try
                    {
                        bool conf = await Application.Current.MainPage.DisplayAlert("Tem certeza?",
                                                                                    "Excluir", "Sim", "Não");
                        if (conf)
                        {
                            await App.Database.Delete(id);
                            AtualizarLista.Execute(null);
                        }
                    }
                    catch (Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert("OPS asd", ex.Message, "OK");
                    }
                    finally
                    {
                        EstaAtualizado = false;
                    }
                });
            }
        }

    }
}