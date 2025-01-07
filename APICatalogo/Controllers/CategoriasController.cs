using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaRepository _repository;
    private readonly ILogger<CategoriasController> _logger;

    public CategoriasController(ILogger<CategoriasController> logger, ICategoriaRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<Categoria>> GetAll()
    {
        var categorias = _repository.GetAll();

        return Ok(categorias);
    }
    
    //[HttpGet("produtos")]
    //public ActionResult<IEnumerable<Categoria>> GetAllWithProducts()
    //{
            
    //}

    [HttpGet("{id:int:min(1)}", Name = "GetByIdCategoria")]
    public ActionResult<Categoria> GetById(int id)
    {
        var categoria = _repository.GetById(id);
        
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

            var categoriaCriada = _repository.Create(categoria);
            
            return new CreatedAtRouteResult("GetByIdCategoria",
                new { id = categoriaCriada.CategoriaId }, categoriaCriada);
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult Update(int id, Categoria categoria)
    {
        if (id != categoria.CategoriaId)
        {
            return BadRequest("Dados inválidos");
        }

        _repository.Update(categoria);
            
        return Ok(categoria);
    }

    [HttpDelete("{id:int:min(1)}")]
    public ActionResult Delete(int id)
    {
        var categoria = GetById(id);

        if (categoria == null)
        {
            return NotFound($"Categoria com id={id} não encontrada...");
        }

        var categoriaExcluida = _repository.Delete(id);

        return Ok(categoriaExcluida);
    }
}