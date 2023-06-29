using System;
using Microsoft.AspNetCore.Mvc;
using project_B_integracao.Models;
using project_B_integracao.Data;
using Microsoft.EntityFrameworkCore;

namespace project_B_integracao.Controllers 
{
    [ApiController]
    [Route("/folha/listar")]
    public class FolhaController : ControllerBase 
    {
        private readonly DataContext _context;

        public FolhaController(DataContext context) => _context = context;

        [HttpPost]
        public IActionResult Create([FromBody] Folha folha)
        {
            Console.WriteLine("_______ controller");
            _context.Folhas.Add(folha);
            _context.SaveChanges();
            return Created("Folha salva!", folha);
        }

        [HttpGet]
        public IActionResult List() => Ok(_context.Folhas.ToList()); 

        [HttpGet("total")]
        public int GetTotal()
        {
            var folhas = _context.Folhas.ToList();
            int valorTotal = folhas.Sum(folha => folha.liquido);
            return valorTotal;
        }

        [HttpGet("media")]
        public double GetMedia()
        {
            var folhas = _context.Folhas.ToList();
            double valorMedio = folhas.Average(folha => folha.liquido);
            return valorMedio;
        }
    }
}
