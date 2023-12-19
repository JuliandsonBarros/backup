using Microsoft.EntityFrameworkCore;
using Sisat.Models;
using System.Linq;

namespace Sisat.ViewModels
{
    public class BaseViewModel
    {
        public ProjetoListViewModel ProjetoListViewModel { get; set; }
        public IEnumerable<PacotesAtualizacoes> PacotesAtualizacoes { get; set; }

        public List<ProjetoComUltimoPacote> HomeProjeto()
        {
            using (var _context = new SisatContext())
            {
                var projetosComUltimoPacote = _context.Projetos
                    .Select(proj => new ProjetoComUltimoPacote
                    {
                        Projeto = proj,
                        UltimoPacote = _context.PacotesAtualizacoes
                            .Where(pacote => pacote.IdProj == proj.IdProjeto)
                            .OrderBy(pacote => pacote.NumVersao)
                            .FirstOrDefault()
                    }).ToList();

                return projetosComUltimoPacote;
            }
        }

        public List<PacotesAtualizacoes> PacotesAtuais()
        {
            using (var _context = new SisatContext())
            {
                var ultimoPacote = _context.PacotesAtualizacoes
                                    .GroupBy(p => p.IdPacote)
                                    .Select(g => g.OrderByDescending(p => p.NumVersao).FirstOrDefault()).ToList();
                return ultimoPacote;
            }
        }
    }

    public class ProjetoComUltimoPacote
    {
        public Projetos Projeto { get; set; }
        public PacotesAtualizacoes UltimoPacote { get; set; }
    }
}



/*
 using Microsoft.EntityFrameworkCore;
using Sisat.Models;

namespace Sisat.ViewModels
{
    public class BaseViewModel
    {
        public List<Projetos> HomeProjeto()
        {
            var _context = new SisatContext();

            return _context.Projetos.OrderBy(n => n.NomProjeto).ToList();
        }

        public List<PacotesAtualizacoes> PacotesAtuais()
        {
            var _context = new SisatContext();

            var ultimoPacote = _context.PacotesAtualizacoes
                                .GroupBy(p => p.IdPacote)
                                .Select(g => g.OrderByDescending(p => p.NumVersao).FirstOrDefault()).ToList();
            return ultimoPacote;
        }
    }
}
*/