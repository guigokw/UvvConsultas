using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UvvConsultas.Data;
using UvvConsultas.Models;
using UvvConsultas.Models.ViewModels;

namespace UVVConsultas.Controllers
{
    public class UsuariosController : Controller
    {
        // Injeção de dependência do ApplicationDbContext para acessar o banco de dados
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Registro()
        {
            return View();
        }

        // POST: Usuarios/Registro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Verifica se o e-mail já está cadastrado no banco de dados
                var existe = await _context.Usuarios.AnyAsync(u => u.Email == model.Email);
                if (existe)
                {
                    ModelState.AddModelError("Email", "Este e-mail já está cadastrado.");
                    return View(model);
                }

                // Cria um novo usuário com os dados fornecidos no formulário de registro
                var usuario = new Usuario
                {
                    NomeUsuario = model.Nome,
                    Email = model.Email,
                    Senha = model.Senha,
                    DataCadastro = DateTime.Now
                };

                // adiciona ao banco de dados e salva as alterações
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // redireciona para a página de login após o registro bem-sucedido
                return RedirectToAction("Login", "Usuarios");
            }

            // retorna a view com os erros de validação caso o modelo não seja válido
            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        // POST: Usuarios/Login

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // recupera o usuário do banco de dados com base no e-mail e senha fornecidos
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Senha == model.Senha);

                if (usuario != null)
                {
                    var claims = new List<Claim>
                    {
                        // adicionando claims (que são informações sobre o usuário) para o usuário autenticado
                        new Claim(ClaimTypes.Name, usuario.NomeUsuario),
                        new Claim(ClaimTypes.Email, usuario.Email),
                        new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString())
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    // realiza o login do usuário criando um cookie de autenticação
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    // redireciona para a página de consultas após o login bem-sucedido
                    return RedirectToAction("Index", "Consultas");
                }

                ModelState.AddModelError("", "E-mail ou senha inválidos.");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
