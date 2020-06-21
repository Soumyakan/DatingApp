using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public DataContext _dataContext;
        public ValuesController(DataContext data){
            _dataContext = data;
        }
        // GET api/values
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var data = await _dataContext.Values.ToListAsync();
            return Ok(data);

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetData(int id)
        {
            var data = await _dataContext.Values.FirstOrDefaultAsync( x => x.Id == id);
            return Ok(data);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
