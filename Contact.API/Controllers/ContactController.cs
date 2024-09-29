using AutoMapper;
using Contact.API.Dtos;
using Contact.API.Infrastructure;
using Contact.API.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Contact.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;

        public ContactController(IContactService contactService, IMapper mapper)
        {
            _contactService = contactService;
            _mapper = mapper;
        }

        // GET: api/<ContactController>
        [HttpGet]
        public IEnumerable<ContactDto> Get()
        {
            var items=_contactService.GetAll();
            var dtos = _mapper.Map<IEnumerable<ContactDto>>(items);
            return dtos;
        }

        // GET api/<ContactController>/5
        [HttpGet("{id}")]
        public ContactDto Get(int id)
        {
            var item=_contactService.GetContactById(id);
            var dto=_mapper.Map<ContactDto>(item);
            return dto; 
        }

        // POST api/<ContactController>
        [HttpPost]
        public ContactDto Post([FromBody] ContactDto dto)
        {
            var model=_mapper.Map<ContactModel>(dto);
            _contactService.Add(model);
            var addedDto=_mapper.Map<ContactDto>(model);
            return addedDto;
        }

        // PUT api/<ContactController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ContactDto dto)
        {
            var model = _mapper.Map<ContactModel>(dto);
            var result=_contactService.Update(id, model);  
            if(result!=null)return Ok(result);
            return BadRequest();
        }

        // DELETE api/<ContactController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var hasDeleted=_contactService.Delete(id);
            if (hasDeleted) return NoContent();
            return BadRequest();
        }
    }
}
