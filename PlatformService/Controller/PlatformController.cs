using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices;

namespace PlatformService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPlatformRepo _repo;
        private readonly ICommandDataClient _commandDataClient;

        public PlatformController(IPlatformRepo repo, IMapper mapper, ICommandDataClient commandDataClient)
        {
            _mapper = mapper;
            _repo = repo;
            _commandDataClient = commandDataClient; 
        }

        [HttpGet]
        public ActionResult<PlatformReadDto> GetPlatforms()
        {
            var result = _repo.GetAll();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(result));
        }

        [HttpGet("{id}", Name =nameof(GetPlatformById))]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var result = _repo.GetPlatformById(id);
            if (result != null)
            {
                return Ok(_mapper.Map<PlatformReadDto>(result));

            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> AddPlatform(PlatformWriteDto platformWriteDto)
        {

            var platform = _mapper.Map<Platform>(platformWriteDto);
            _repo.CreatePlatForm(platform);
            _repo.SaveChanges();
            var platformReadDto = _mapper.Map<PlatformReadDto>(platform);
            try
            {
                await _commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"---> Could not send platform Sync : {ex.Message}");
            }
            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platform.Id}, platformReadDto);
        }

    }
}
