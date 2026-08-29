using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UvvConsultas.Data;
using UvvConsultas.Models;

namespace UvvConsultas.Controllers
{
    [Authorize] // Protege todas as ações deste controller
    public class ConsultasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConsultasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Consultas
        public async Task<IActionResult> Index()
        {
            var usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(usuarioIdClaim, out int usuarioId))
            {
                return RedirectToAction("Login", "Usuarios");
            }

            var consultas = await _context.Consultas
                .Where(c => c.UsuarioId == usuarioId)
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
                var usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!int.TryParse(usuarioIdClaim, out int usuarioId))
                {
                    return RedirectToAction("Login", "Usuarios");
                }

                consulta.UsuarioId = usuarioId;
                _context.Consultas.Add(consulta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(consulta);
        }

        // GET: Consultas/Edit/{id}
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c => c.IdConsulta == id && c.UsuarioId == usuarioId);

            if (consulta == null)
                return NotFound();

            return View(consulta);
        }

        // POST: Consultas/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdConsulta,Especialidade,DataHora,Descricao")] Consulta consulta)
        {
            if (id != consulta.IdConsulta)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    consulta.UsuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c => c.IdConsulta == id && c.UsuarioId == usuarioId);

            if (consulta == null)
                return NotFound();

            return View(consulta);
        }

        // GET: Consultas/Details/{id}
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c => c.IdConsulta == id && c.UsuarioId == usuarioId);

            if (consulta == null)
                return NotFound();

            return View(consulta);
        }

        // POST: Consultas/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c => c.IdConsulta == id && c.UsuarioId == usuarioId);

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
