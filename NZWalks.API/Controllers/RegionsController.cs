using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/regions")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;

        public RegionsController(NZWalksDbContext dbContext,IRegionRepository regionRepository)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regionsDomains = await regionRepository.GetAllAsync();

            var regionsDTO = new List<RegionDto>();

            foreach (var regionsDomain in regionsDomains)
            {
                regionsDTO.Add(new RegionDto()
                {
                    Id = regionsDomain.Id,
                    Code = regionsDomain.Code,
                    Name = regionsDomain.Name,
                    RegionImageURL = regionsDomain.RegionImageURL
                });
            }

            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route( "{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var regionsDomain = await regionRepository.GetByIdAsync(id);
            if(regionsDomain == null)
            {
                return NotFound();
            }
            var regionDTO = new RegionDto()
            {
                Id = regionsDomain.Id,
                Code = regionsDomain.Code,
                Name = regionsDomain.Name,
                RegionImageURL = regionsDomain.RegionImageURL
            };
            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            var regionDomainModel = new Region()
            {
                Code = addRegionRequestDTO.Code,
                Name = addRegionRequestDTO.Name,
                RegionImageURL = addRegionRequestDTO.RegionImageURL
            };

            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            var regionDTO = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageURL = regionDomainModel.RegionImageURL
            };

            return CreatedAtAction(nameof(GetById), new {id = regionDomainModel.Id},regionDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO) {

            var regionDomainModel = new Region()
            {
                Code = updateRegionRequestDTO.Code,
                Name = updateRegionRequestDTO.Name,
                RegionImageURL = updateRegionRequestDTO.RegionImageURL  
            };
            
            var regionsDomain = await regionRepository.UpdateAsync(id,regionDomainModel);
            if (regionsDomain == null)
            {
                return NotFound();
            }
            var regionDTO = new RegionDto()
            {
                Id = regionsDomain.Id,
                Code = regionsDomain.Code,
                Name = regionsDomain.Name,
                RegionImageURL = regionsDomain.RegionImageURL
            };
            return Ok(regionDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionsDomain = await regionRepository.DeleteAsync(id);
            if (regionsDomain == null)
            {
                return NotFound();
            }
            var regionDTO = new RegionDto()
            {
                Id = regionsDomain.Id,
                Code = regionsDomain.Code,
                Name = regionsDomain.Name,
                RegionImageURL = regionsDomain.RegionImageURL
            };
            return Ok(regionDTO);
        }
    }
}
