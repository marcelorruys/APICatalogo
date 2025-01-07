using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public CategoriasController(AppDbContext context,
                                IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpGet("LerArquivoConfiguracao")]
    public string GetValores()
    {
        var valor1 = _configuration["chave1"];
        var valor2 = _configuration["chave2"];

        var secao1 = _configuration["secao1:chave2"];

        return $"Chave1 = {valor1}  \nChave2 = {valor2}  \nSeção1 => Chave2 = {secao1}";
    }

    [HttpGet]
    public ActionResult<IEnumerable<Categoria>> GetAll()
    {
        try
        {
            return _context.Categorias.AsNoTracking().ToList();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Ocorreu um problema ao tratar a sua solicitação.");
        }
    }
    
    [HttpGet("produtos")]
    public ActionResult<IEnumerable<Categoria>> GetAllWithProducts()
    {
        try
        {
            return _context.Categorias.Include(p => p.Produtos).ToList();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Ocorreu um problema ao tratar a sua solicitação.");
        }
    }

    [HttpGet("{id:int:min(1)}", Name = "GetByIdCategoria")]
    public ActionResult<Categoria> GetById(int id)
    {
        try
        {
            var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);

            if (categoria == null)
            {
                return NotFound($"Categoria com id= {id} não encontrada...");
            }
            return Ok(categoria);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                       "Ocorreu um problema ao tratar a sua solicitação.");
        }
    }

    [HttpPost]
    public ActionResult Create(Categoria categoria)
    {
        try
        {
            if (categoria is null)
                return BadRequest("Dados inválidos");

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("GetByIdCategoria",
                new { id = categoria.CategoriaId }, categoria);

        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                      "Ocorreu um problema ao tratar a sua solicitação.");
        }
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult Update(int id, Categoria categoria)
    {
        try
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest("Dados inválidos");
            }
            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok(categoria);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                   "Ocorreu um problema ao tratar a sua solicitação.");
        }
    }

    [HttpDelete("{id:int:min(1)}")]
    public ActionResult Delete(int id)
    {
        try
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
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                           "Ocorreu um problema ao tratar a sua solicitação.");
        }
    }
}