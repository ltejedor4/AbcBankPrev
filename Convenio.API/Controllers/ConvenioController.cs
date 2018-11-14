using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convenio.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Convenio.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ConvenioController : ControllerBase
    {
        private readonly ConvenioContext _convenioContext;
        public ConvenioController(ConvenioContext context)
        {
            _convenioContext = context ?? throw new ArgumentException(nameof(context));
        }

        /// <summary>
        /// Get all convenios
        /// </summary>
        /// <returns>List of convenio</returns>
        [HttpGet]
        public ActionResult<List<Convenio>> Get()
        {
            var convenios = _convenioContext.Convenios.ToList();
            return convenios;
        }

        /// <summary>
        /// Get convenio by id
        /// </summary>
        /// <param name="id">integer convenio id</param>
        /// <returns>name of convenio</returns>
        [HttpGet]
        [Route("{id}")]        
        public ActionResult GetById(int id)
        {
            var convenio = _convenioContext.Convenios.FirstOrDefault(x => x.Id == id);
            return Ok(convenio);
        }

        /// <summary>
        /// Get convenio by convenio number
        /// </summary>
        /// <param name="cod">it is the bill's first four number</param>
        /// <param name="accion">it is the action that identified kind of request</param>
        /// <returns>name of convenio</returns>
        [HttpGet]
        [Route("{cod}/{accion}")]
        public ActionResult GetByCodigo(string cod, string accion)
        {
            var convenio = _convenioContext.Convenios.FirstOrDefault(x => x.Codigo == cod && x.Accion==accion);
            return Ok(convenio);
        }

        /// <summary>
        /// Get url base
        /// </summary>
        /// <param name="cod">it is the bill's first four number</param>
        /// <returns>string url base</returns>
        [HttpGet]
        [Route("{cod}/url")]
        public ActionResult<string> GetUrlConvenio(string cod)
        {
            var convenio = _convenioContext.Convenios.Where(x => x.Codigo == cod).Select(x=>x.BaseUrl).FirstOrDefault();
            return Ok(convenio);
        }

        /// <summary>
        /// Method that allow you to create a new convenio
        /// </summary>
        /// <param name="convenio">object nameof covenio</param>
        /// <returns>convenio created</returns>
        [HttpPost]
        public ActionResult<Convenio> Post(Convenio convenio)
        {
            _convenioContext.Convenios.Add(convenio);
            _convenioContext.SaveChanges();
            return convenio;
        }

        /// <summary>
        /// Methot to disable a convenio
        /// </summary>
        /// <param name="id">Integer convenio id</param>
        /// <returns>name of convenio</returns>
        [HttpPut]
        public ActionResult InactivateConvenio(int id)
        {
            var convenio = _convenioContext.Convenios.FirstOrDefault(x => x.Id == id);
            if(convenio==null)
                return NotFound(new { Message = $"Convenio with id {id} not found." });

            convenio.Estado = false;
            _convenioContext.SaveChanges();

            return Ok(convenio);
        }
    }
}