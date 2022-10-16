using Microsoft.AspNetCore.Mvc;
using Advanced.Models;
using Microsoft.EntityFrameworkCore;
using DataModel;

namespace Advanced.Controllers
{
    [ApiController]
    [Route("/api/people")]
    public class DataController : ControllerBase
    {
        private DataContext _dataContext;
        public DataController(DataContext dataContext)
        { 
            _dataContext = dataContext;
        }

        [HttpGet]
        public IEnumerable<Person> GetAll()
        {
            IEnumerable<Person> people = _dataContext.People.Include(p => p.Department).Include(p => p.Location);
            foreach (Person p in people)
            {
                if (p.Department?.People != null)
                    p.Department.People = null;
                if(p.Location?.People != null)
                    p.Location.People = null;
            }
            return people;
        }

        [HttpGet("{id}")]
        public async Task<Person> GetDetails(long id)
        {
            Person p = await _dataContext.People.Include(p => p.Department).Include(p => p.Location).FirstAsync(p => p.PersonId == id);
            if (p.Department?.People != null)
                p.Department.People = null;
            if (p.Location?.People != null)
                p.Location.People = null;
            return p;
        }

        [HttpPost]
        public async Task Save([FromBody] Person person)
        { 
            await _dataContext.People.AddAsync(person);
            await _dataContext.SaveChangesAsync();
        }

        [HttpPut]
        public async Task Update([FromBody] Person person)
        {
            _dataContext.People.Update(person);
            await _dataContext.SaveChangesAsync();
        }

        [HttpDelete("{id}")]
        public async Task Delete(long id)
        {
            _dataContext.People.Remove(new Person() { PersonId = id });
            await _dataContext.SaveChangesAsync();
        }

        [HttpGet("/api/locations")]
        public IAsyncEnumerable<Location> GetLocations() => _dataContext.Locations.AsAsyncEnumerable();

        [HttpGet("/api/departments")]
        public IAsyncEnumerable<Department> GetDepartments() => _dataContext.Departments.AsAsyncEnumerable();

    }
}
