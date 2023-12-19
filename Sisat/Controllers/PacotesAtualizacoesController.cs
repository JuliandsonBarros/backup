using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sisat.Models;
using Sisat.ViewModels;

namespace Sisat.Controllers
{
    public class PacotesAtualizacoesController : Controller
    {
        private readonly SisatContext _context;

        private readonly ProjetoListViewModel projetoListViewModel;

        public PacotesAtualizacoesController(SisatContext context, ProjetoListViewModel projetoListViewModel)
        {
            _context = context;
            this.projetoListViewModel = projetoListViewModel;
        }


        // GET: PacotesAtualizacoes
        public async Task<IActionResult> Index()
        {
            var sisatContext = _context.PacotesAtualizacoes.Include(p => p.IdProjNavigation);
            return View(await sisatContext.ToListAsync());
        }

        // GET: PacotesAtualizacoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PacotesAtualizacoes == null)
            {
                return NotFound();
            }

            var pacotesAtualizacoes = await _context.PacotesAtualizacoes
                .Include(p => p.IdProjNavigation)
                .FirstOrDefaultAsync(m => m.IdPacote == id);
            if (pacotesAtualizacoes == null)
            {
                return NotFound();
            }

            return View(pacotesAtualizacoes);
        }

        // GET: PacotesAtualizacoes/Create
        public IActionResult Create()
        {
            ViewData["IdProj"] = new SelectList(_context.Projetos, "IdProjeto", "IdProjeto");
            return View();
        }

        // POST: PacotesAtualizacoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjetoListViewModel projetoListViewModel, IFormFile arquivo)
        {
            if (arquivo != null && arquivo.Length > 0)
            {
                var diretorio = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "arquivos");
                Directory.CreateDirectory(diretorio);

                var caminhoArquivo = Path.Combine(diretorio, arquivo.FileName);
                using (var stream = new FileStream(caminhoArquivo, FileMode.Create))
                {
                    await arquivo.CopyToAsync(stream);
                }

                projetoListViewModel.Pacote.DirArquivo = caminhoArquivo;
                _context.PacotesAtualizacoes.Add(projetoListViewModel.Pacote);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Projetos", new { id = projetoListViewModel.Pacote.IdProj });

        }

            // GET: PacotesAtualizacoes/Edit/5
            public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PacotesAtualizacoes == null)
            {
                return NotFound();
            }

            var pacotesAtualizacoes = await _context.PacotesAtualizacoes.FindAsync(id);
            if (pacotesAtualizacoes == null)
            {
                return NotFound();
            }
            ViewData["IdProj"] = new SelectList(_context.Projetos, "IdProjeto", "IdProjeto", pacotesAtualizacoes.IdProj);
            return View(pacotesAtualizacoes);
        }

        // POST: PacotesAtualizacoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPacote,IdProj,NumVersao,RegistroAlteracoes,DtLancamento")] PacotesAtualizacoes pacotesAtualizacoes)
        {
            if (id != pacotesAtualizacoes.IdPacote)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pacotesAtualizacoes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacotesAtualizacoesExists(pacotesAtualizacoes.IdPacote))
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
            ViewData["IdProj"] = new SelectList(_context.Projetos, "IdProjeto", "IdProjeto", pacotesAtualizacoes.IdProj);
            return View(pacotesAtualizacoes);
        }

        // GET: PacotesAtualizacoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PacotesAtualizacoes == null)
            {
                return NotFound();
            }

            var pacotesAtualizacoes = await _context.PacotesAtualizacoes
                .Include(p => p.IdProjNavigation)
                .FirstOrDefaultAsync(m => m.IdPacote == id);
            if (pacotesAtualizacoes == null)
            {
                return NotFound();
            }

            return View(pacotesAtualizacoes);
        }

        // POST: PacotesAtualizacoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PacotesAtualizacoes == null)
            {
                return Problem("Entity set 'SisatContext.PacotesAtualizacoes'  is null.");
            }
            var pacotesAtualizacoes = await _context.PacotesAtualizacoes.FindAsync(id);
            if (pacotesAtualizacoes != null)
            {
                _context.PacotesAtualizacoes.Remove(pacotesAtualizacoes);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PacotesAtualizacoesExists(int id)
        {
          return (_context.PacotesAtualizacoes?.Any(e => e.IdPacote == id)).GetValueOrDefault();
        }

        // GET: PacotesAtualizacoes/Download/
        public async Task<IActionResult> Download(int? id)
        {
            if (id == null || _context.PacotesAtualizacoes == null)
            {
                return NotFound();
            }

            var pacote = await _context.PacotesAtualizacoes
                .FirstOrDefaultAsync(m => m.IdPacote == id);

            if (pacote == null || string.IsNullOrEmpty(pacote.DirArquivo))
            {
                return NotFound();
            }

            var filePath = pacote.DirArquivo;
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var memoria = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memoria);
            }
            memoria.Position = 0;

            return File(memoria, GetContentType(filePath), Path.GetFileName(filePath));
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
    {
        {".txt", "text/plain"},
        {".pdf", "application/pdf"},
        {".doc", "application/vnd.ms-word"},
        {".docx", "application/vnd.ms-word"},
        {".xls", "application/vnd.ms-excel"},
        {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
        {".png", "image/png"},
        {".jpg", "image/jpeg"},
        {".jpeg", "image/jpeg"},
        {".gif", "image/gif"},
        {".csv", "text/csv"}
    };
        }

    }
}
