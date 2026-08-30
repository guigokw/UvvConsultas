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

        // injeção de dependência do ApplicationDbContext para acessar o banco de dados
        private readonly ApplicationDbContext _context;

        public ConsultasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Consultas
        public async Task<IActionResult> Index()
        {
            var usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier); // Pega o ID do usuário logado
            if (!int.TryParse(usuarioIdClaim, out int usuarioId))
            {
                return RedirectToAction("Login", "Usuarios");
            }

            var consultas = await _context.Consultas // Pega todas as consultas do usuário logado
                .Where(c => c.UsuarioId == usuarioId)
                .OrderBy(c => c.DataHora)
                .ToListAsync();

            return View(consultas); // Passa a lista de consultas para a view
        }

        // GET: Consultas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Consultas/Create
        [HttpPost]
        [ValidateAntiForgeryToken] // Protege contra ataques CSRF

        public async Task<IActionResult> Create([Bind("Especialidade,DataHora,Descricao")] Consulta consulta)
        {
            if (ModelState.IsValid)
            {
                var usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier); // Pega o ID do usuário logado
                if (!int.TryParse(usuarioIdClaim, out int usuarioId))
                {
                    return RedirectToAction("Login", "Usuarios"); // Redireciona para a página de login se o usuário não estiver logado
                }

                consulta.UsuarioId = usuarioId; // Atribui o ID do usuário logado à consulta
                _context.Consultas.Add(consulta); // Adiciona a consulta ao contexto do banco de dados
                await _context.SaveChangesAsync(); // Salva as alterações no banco de dados
                return RedirectToAction(nameof(Index)); // Redireciona para a lista de consultas
            }
            return View(consulta); // Se o modelo não for válido, retorna para a view de criação com os erros de validação
        }

        // GET: Consultas/Edit/{id}
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) // Se o ID não for fornecido, retorna NotFound
                return NotFound();

            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); // Pega o ID do usuário logado
            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c => c.IdConsulta == id && c.UsuarioId == usuarioId); 

            if (consulta == null) // Se a consulta não for encontrada ou não pertencer ao usuário logado, retorna NotFound
                return NotFound();

            return View(consulta); // Retorna a view de edição com a consulta encontrada
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
                    consulta.UsuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); // Atribui o ID do usuário logado à consulta
                    _context.Update(consulta); // Atualiza a consulta no contexto do banco de dados
                    await _context.SaveChangesAsync(); // Salva as alterações no banco de dados
                }
                catch (DbUpdateConcurrencyException)   // Trata exceções de concorrência, caso a consulta tenha sido modificada por outro usuário
                {
                    if (!ConsultaExists(consulta.IdConsulta))
                        return NotFound(); // Se a consulta não existir mais, retorna NotFound
                    throw;
                }
                return RedirectToAction(nameof(Index)); // Redireciona para a lista de consultas após a edição bem-sucedida
            }
            return View(consulta);
        }

        // GET: Consultas/Delete/{id}
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); // Pega o ID do usuário logado
            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c => c.IdConsulta == id && c.UsuarioId == usuarioId);

            if (consulta == null) // Se a consulta não for encontrada ou não pertencer ao usuário logado, retorna NotFound
                return NotFound();

            return View(consulta); // Retorna a view de confirmação de exclusão com a consulta encontrada
        }

        // GET: Consultas/Details/{id}
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); // Pega o ID do usuário logado
            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c => c.IdConsulta == id && c.UsuarioId == usuarioId);

            if (consulta == null) // Se a consulta não for encontrada ou não pertencer ao usuário logado, retorna NotFound
                return NotFound();

            return View(consulta); // Retorna a view de detalhes com a consulta encontrada
        }

        // POST: Consultas/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); // Pega o ID do usuário logado
            var consulta = await _context.Consultas
                .FirstOrDefaultAsync(c => c.IdConsulta == id && c.UsuarioId == usuarioId);

            if (consulta != null) // Se a consulta for encontrada e pertencer ao usuário logado, remove-a do banco de dados
            {
                _context.Consultas.Remove(consulta);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index)); // Redireciona para a lista de consultas após a exclusão
        }

        private bool ConsultaExists(int id) // Verifica se uma consulta existe no banco de dados
        {
            return _context.Consultas.Any(e => e.IdConsulta == id);
        }
    }
}
