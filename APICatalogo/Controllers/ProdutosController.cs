using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> GetAll() 
        {
            var produtos = _context.Produtos.AsNoTracking().ToList();
            if (produtos is null)
            {
                return NotFound("Produtos não encontrados!");
            }
            return produtos;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public ActionResult<Produto> GetById(int id)
        {
            var produto = _context.Produtos.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id);
            if (produto is null)
            {
                return NotFound("O Produto não encontrado!");
            }
            return produto;
        }

        [HttpPost]
        public IActionResult Create(Produto produto)
        {
            if (produto is null) 
            {
                return BadRequest("Produto Inválido!");
            }

            _context.Produtos.Add(produto);
            _context.SaveChanges();
            
            return new CreatedAtRouteResult("GetById", new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult<Produto> Update(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest("Códigos de produtos divergentes!");
            }

            if ((_context.Produtos.FirstOrDefault(p => p.ProdutoId == id)) is null)
            {
                return NotFound("Produto não encontrado!");
            }

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete("id:int")]
        public IActionResult Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            if(produto is null)
            {
                return NotFound("Produto não encontrado!");
            }

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
