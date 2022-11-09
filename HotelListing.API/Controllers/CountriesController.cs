using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using HotelListing.API.Models;
using AutoMapper;
using HotelListing.API.Models.DTOs.Country;
using HotelListing.API.Data.RepositoryInterfaces;
using Microsoft.AspNetCore.Authorization;
using HotelListing.API.Exceptions;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        //private readonly ApiDBX repo; // Replaced by Repo
        private readonly IMapper _mapper;
        private readonly ICountriesRepository repo;

        public CountriesController(ICountriesRepository _repo, IMapper mapper)
        {
            _mapper = mapper;
            repo = _repo;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryGetDto>>> GetCountries()
        {
            var countries = await repo.GetAllAsync();
            var records = _mapper.Map<List<CountryGetDto>>(countries);
            return Ok(records);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
            var country = await repo.GetDetails(id);

            if (country == null)
            {
                throw new NotFoundException(nameof(GetCountry), id);
            }

            CountryDto countryDto = _mapper.Map<CountryDto>(country);
            return countryDto;
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutCountry(int id, CountryPutDto putDto)
        {
            if (id != putDto.Id)
            {
                return BadRequest("Invalid Country Id");
            }

            //repo.Entry(putDto).State = EntityState.Modified;
            var country = await repo.GetAsync(id);
            if(country == null)
            {
                throw new NotFoundException(nameof(PutCountry), id);
            }
            _mapper.Map(putDto, country);

            try
            {
                await repo.saveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await repo.Exists(id)))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Country>> PostCountry(CountryPostDto countryDto)
        {
            Country country = _mapper.Map<Country>(countryDto);

            await repo.AddAsync(country);

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await repo.GetAsync(id);
            if (country == null)
            {
                throw new NotFoundException(nameof(DeleteCountry), id);
            }

            await repo.DeleteAsync(country.Id);

            return NoContent();
        }
    }
}
