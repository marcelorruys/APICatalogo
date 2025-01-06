using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
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
            var produtos = _context.Produtos.ToList();
            if (produtos is null)
            {
                return NotFound("Produtos não encontrados!");
            }
            return produtos;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public ActionResult<Produto> GetById(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
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
    }
}
