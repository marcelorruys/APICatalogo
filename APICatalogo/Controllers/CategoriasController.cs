using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<CategoriasController> _logger;

    public CategoriasController(AppDbContext context, ILogger<CategoriasController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<Categoria>> GetAll()
    {
            return _context.Categorias.AsNoTracking().ToList();
    }
    
    [HttpGet("produtos")]
    public ActionResult<IEnumerable<Categoria>> GetAllWithProducts()
    {
            return _context.Categorias.Include(p => p.Produtos).ToList();
    }

    [HttpGet("{id:int:min(1)}", Name = "GetByIdCategoria")]
    public ActionResult<Categoria> GetById(int id)
    {
        var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
        
        if (categoria == null)
        {
            return NotFound($"Categoria com id= {id} não encontrada...");
        }
        
        return Ok(categoria);
    }

    [HttpPost]
    public ActionResult Create(Categoria categoria)
    {
            if (categoria is null)
                return BadRequest("Dados inválidos");

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("GetByIdCategoria",
                new { id = categoria.CategoriaId }, categoria);
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult Update(int id, Categoria categoria)
    {
        if (id != categoria.CategoriaId)
        {
            return BadRequest("Dados inválidos");
        }

        _context.Entry(categoria).State = EntityState.Modified;
        _context.SaveChanges();
            
        return Ok(categoria);
    }

    [HttpDelete("{id:int:min(1)}")]
    public ActionResult Delete(int id)
    {
        var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);

        if (categoria == null)
        {
            return NotFound($"Categoria com id={id} não encontrada...");
        }

        _context.Categorias.Remove(categoria);
        _context.SaveChanges();

        return Ok(categoria);
    }
}