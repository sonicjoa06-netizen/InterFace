using InterFace.Data;
using InterFace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace InterFace.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(AppDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                await _context.Database.EnsureCreatedAsync();
                ViewBag.Clientes = await _context.Clientes
                    .OrderByDescending(cliente => cliente.CriadoEm)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Nao foi possivel conectar ao MySQL.");
                ViewBag.Clientes = new List<Cliente>();
                ViewBag.ErroBanco = "Nao foi possivel conectar ao MySQL. Confira usuario e senha em appsettings.json.";
            }

            return View(new Cliente());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Clientes = await _context.Clientes
                    .OrderByDescending(item => item.CriadoEm)
                    .ToListAsync();

                return View("Index", cliente);
            }

            try
            {
                await _context.Database.EnsureCreatedAsync();
                cliente.CriadoEm = DateTime.Now;
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();
                TempData["Mensagem"] = "Cliente cadastrado com sucesso.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Nao foi possivel salvar o cliente no MySQL.");
                TempData["ErroBanco"] = "Nao foi possivel salvar. Confira a conexao com o MySQL.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remover(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente is not null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
                TempData["Mensagem"] = "Cliente removido com sucesso.";
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
