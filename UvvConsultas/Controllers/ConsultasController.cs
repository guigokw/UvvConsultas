using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UvvConsultas.Data;
using UvvConsultas.Models;

namespace UVVConsultas.Controllers
{
    [Authorize] // Protege todas as ações deste controller
    public class ConsultasController : Controller
    {

        // configura a injeção de dependência do ApplicationDbContext para acessar o banco de dados
        private readonly ApplicationDbContext _context;

        public ConsultasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Consultas
        public async Task<IActionResult> Index()
        {
            // Obtém o ID do usuário logado a partir das claims
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var consultas = await _context.Consultas
                .Where(c => c.Usuario.IdUsuario == usuarioId)
                .OrderBy(c => c.DataHora)
                .ToListAsync();

            return View(consultas);
        }

        // GET: Consultas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Consultas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Especialidade,DataHora,Descricao")] Consulta consulta)
        {
            if (ModelState.IsValid)
            {
                // Atribui o ID do usuário logado à consulta antes de salvar
                consulta.Usuario.IdUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                _context.Consultas.Add(consulta); // Adiciona a nova consulta ao contexto
                await _context.SaveChangesAsync(); // Salva as alterações no banco de dados
                return RedirectToAction(nameof(Index)); // Redireciona para a lista de consultas após a criação bem-sucedida
            }
            return View(consulta);
        }

        // GET: Consultas/Edit/{id}
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            // Obtém o ID do usuário logado a partir das claims
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            // Busca a consulta pelo ID e pelo ID do usuário logado para garantir que o usuário só possa editar suas próprias consultas
            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c => c.IdConsulta == id && c.Usuario.IdUsuario == usuarioId);

            if (consulta == null)
                return NotFound();

            return View(consulta);
        }

        // POST: Consultas/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]

        // atualiza uma consulta existente, garantindo que o usuário só possa editar suas próprias consultas
        public async Task<IActionResult> Edit(int id, [Bind("Id,Especialidade,DataHora,Descricao")] Consulta consulta)
        {
            if (id != consulta.IdConsulta)
                return NotFound();

            if (ModelState.IsValid)
            {
                // Atribui o ID do usuário logado à consulta antes de salvar
                try
                {
                    consulta.Usuario.IdUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    _context.Update(consulta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsultaExists(consulta.IdConsulta))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(consulta);
        }

        // GET: Consultas/Delete/{id}

        // deleta uma consulta, garantindo que o usuário só possa deletar suas próprias consultas
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            // Obtém o ID do usuário logado a partir das claims
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c => c.IdConsulta == id && c.Usuario.IdUsuario == usuarioId);

            if (consulta == null)
                return NotFound();

            return View(consulta);
        }

        // POST: Consultas/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        // confirma a exclusão da consulta, garantindo que o usuário só possa excluir suas próprias consultas
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c => c.IdConsulta == id && c.Usuario.IdUsuario == usuarioId);

            if (consulta != null)
            {
                _context.Consultas.Remove(consulta);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ConsultaExists(int id)
        {
            return _context.Consultas.Any(e => e.IdConsulta == id);
        }
    }
}
