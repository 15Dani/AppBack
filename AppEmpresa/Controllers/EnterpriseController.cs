using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppEmpresa.Domain;
using AppEmpresa.Dtos;
using AppEmpresa.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppEmpresa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnterpriseController : ControllerBase
    {
        private readonly IAppEmpresaRepository _repo;

        public IMapper _mapper { get; }

        public EnterpriseController(IAppEmpresaRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
          
        }

        //GET api/Enterprise
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var enterprises = await _repo.ObterTodosEnterpriseAsync();

                var results = _mapper.Map<EnterpriseDTo[]>(enterprises);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um falha ao acessar o banco de dados");
            }
        }

        // GET api/Enterprise/5
        [HttpGet("{EnterpriseId:int}")]
        public async Task<IActionResult> Get(int EnterpriseId)
        {
            try
            {
                var enterprise = await _repo.ObterEnterprisePorIdAsync(EnterpriseId);

                var result = _mapper.Map<EnterpriseDTo>(enterprise);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return this.StatusCode(StatusCodes.Status404NotFound, $"Empresa Nº {EnterpriseId} não localizado");
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }

        // GET api/Enterprise/Nome/nassau
        [HttpGet("Nome/{Nome}")]
        public async Task<IActionResult> GetNome(string Nome)
        {
            try
            {
                var enterprise = await _repo.ObterTodosEnterprisePorNomeAsync(Nome);
                var results = _mapper.Map<EnterpriseDTo[]>(enterprise);
                if (results != null)
                {
                    return Ok(results);
                }
                else
                {
                    return this.StatusCode(StatusCodes.Status404NotFound, $"Nome '{Nome}' não localizado");
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }

        // POST api/Enterprise
        [HttpPost]
        public async Task<IActionResult> Post(EnterpriseDTo model)
        {
            try
            {
                var enterprise = _mapper.Map<Enterprise>(model);
                _repo.Adicionar(enterprise);
                if (await _repo.SaveChangesAsync())
                {
                    return Created($"api/Enterprise/{enterprise.Id}", _mapper.Map<EnterpriseDTo>(enterprise));
                }
            }
            catch (System.Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um falha ao acessar o banco de dados " + e.Message);
            }

            return StatusCode(StatusCodes.Status403Forbidden, $"Ocorreu um erro ao inserir {model}");
        }

        // PUT api/Enterprise/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EnterpriseDTo model)
        {
            try
            {
                var result = await _repo.ObterEnterprisePorIdAsync(id);
                if (result == null) return NotFound();

                //Efetua o mapeamento das alterações de acordo com o item buscado
                _mapper.Map(model, result);

                _repo.Atualizar(result);
                if (await _repo.SaveChangesAsync())
                {
                    return Created($"api/Enterprise/{model.Id}", _mapper.Map<EnterpriseDTo>(result));
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
            return BadRequest();
        }

        // DELETE api/Enterprise/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var enterprise = await _repo.ObterEnterprisePorIdAsync(id);
                if (enterprise == null) return NotFound();

                _repo.Deletar(enterprise);
                if (await _repo.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
            return BadRequest();
        }
    }
}
