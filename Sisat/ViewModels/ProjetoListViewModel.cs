using Microsoft.EntityFrameworkCore;
using Sisat.Models;

namespace Sisat.ViewModels
{
    public class ProjetoListViewModel : BaseViewModel
    {
        public Projetos Projeto { get; set; }

        public List<Projetos> Projetos { get; set; }
        public PacotesAtualizacoes Pacote { get; set; }

        public List<PacotesAtualizacoes> Pacotes { get; set; }

        public ProjetoListViewModel()
        {
            Projeto = new Projetos();
            Pacotes = new List<PacotesAtualizacoes>();
            Pacote = new();
            Projetos = new List<Projetos>();
        }
    }
}
