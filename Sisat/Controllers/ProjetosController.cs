using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Sisat.Models;
using Sisat.ViewModels;

namespace Sisat.Controllers
{
    public class ProjetosController : Controller
    {
        private readonly SisatContext _context;

        private readonly ProjetoListViewModel _projetoListViewModel;

        public ProjetosController(SisatContext context, ProjetoListViewModel projetoListViewModel)
        {
            _context = context;
            _projetoListViewModel = projetoListViewModel;
        }

        // GET: Projetos
        public async Task<IActionResult> Index()
        {
            return View(_projetoListViewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Projetos == null)
            {
                return NotFound();
            }

            var projetos = await _context.Projetos.OrderBy(p => p.IdProjeto).ToListAsync();

            projetos.Reverse();

            var projeto = projetos.FirstOrDefault(p => p.IdProjeto == id);
            if (projeto == null)
            {
                return NotFound();
            }

            var pacotes = await _context.PacotesAtualizacoes
                            .Where(p => p.IdProj == id)
                            .ToListAsync();

            pacotes.Reverse();

            if (pacotes == null)
            {
                return NotFound();
            }

            int projetoId = id ?? (int?)TempData["ProjetoId"] ?? 0;

            var projetoListViewModel = new ProjetoListViewModel
            {
                Projetos = projetos,
                Projeto = projeto,
                Pacotes = pacotes
            };

            return View(projetoListViewModel);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjetoListViewModel projetoListViewModel)
        {
            Projetos projeto = projetoListViewModel.Projeto;
            _context.Projetos.Add(projeto);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Projetos", new { id = projeto.IdProjeto });
        }

        // GET: Projetos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Projetos == null)
            {
                return NotFound();
            }

            var projetos = await _context.Projetos.FindAsync(id);
            if (projetos == null)
            {
                return NotFound();
            }
            return View(_projetoListViewModel);
        }

        // POST: Projetos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProjeto,NomProjeto")] Projetos projetos)
        {
            if (id != projetos.IdProjeto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projetos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjetosExists(projetos.IdProjeto))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(_projetoListViewModel);
        }

        // GET: Projetos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Projetos == null)
            {
                return NotFound();
            }

            var projetos = await _context.Projetos
                .FirstOrDefaultAsync(m => m.IdProjeto == id);
            if (projetos == null)
            {
                return NotFound();
            }

            return View(projetos);
        }

        // POST: Projetos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Projetos == null)
            {
                return Problem("Entity set 'SisatContext.Projetos'  is null.");
            }
            var projetos = await _context.Projetos.FindAsync(id);
            if (projetos != null)
            {
                _context.Projetos.Remove(projetos);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjetosExists(int id)
        {
            return (_context.Projetos?.Any(e => e.IdProjeto == id)).GetValueOrDefault();
        }

    }
}
