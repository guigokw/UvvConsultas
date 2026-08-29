using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using UvvConsultas.Data;

// cria um builder para configurar a aplicação
var builder = WebApplication.CreateBuilder(args);

// adiciona serviços ao contêiner de injeção de dependência
builder.Services.AddControllersWithViews();


// adiciona o contexto do banco de dados usando SQL Server e a string de conexão definida no appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/Usuarios/Login";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.Cookie.Name = "UVVConsultasCookie";
    });

// adiciona o serviço de autorização para controlar o acesso a recursos com base em políticas e funções
builder.Services.AddAuthorization();

// constrói a aplicação a partir do builder configurado
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// configura o middleware para redirecionar requisições HTTP para HTTPS, servir arquivos estáticos, roteamento, autenticação e autorização
app.UseHttpsRedirection();

// configura o middleware para servir arquivos estáticos (como CSS, JavaScript e imagens) a partir da pasta wwwroot
app.UseStaticFiles();

// configura o middleware de roteamento para mapear URLs para controladores e ações
app.UseRouting();

// configura o middleware de autenticação e autorização para proteger recursos e controlar o acesso com base em políticas e funções
app.UseAuthentication();

// configura o middleware de autorização para proteger recursos e controlar o acesso com base em políticas e funções
app.UseAuthorization();

// configura o roteamento padrão para os controladores e ações, definindo o controlador padrão como "Home" e a ação padrão como "Index"
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// inicia a aplicação e começa a escutar requisições HTTP
app.Run();