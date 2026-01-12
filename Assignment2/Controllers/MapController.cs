using Assignment2.Models;
using Assignment2.Services;
using Microsoft.AspNetCore.Mvc;
using ShortestRouteApi.Filters;


namespace Assignment2.Controllers
{
    [ApiController]
    [Route("api/map")]
    public class MapController : ControllerBase
    {
        private readonly MapStateService _mapState;
        private readonly PathFindingService _pathFinder;

        public MapController(MapStateService mapState, PathFindingService pathFinder)
        {
            _mapState = mapState;
            _pathFinder = pathFinder;
        }


        [HttpPost("SetMap")]
        [ApiKeyAuth("FS_ReadWrite")]
        public IActionResult SetMap([FromBody] Graph map)
        {
            if (map == null || map.Nodes == null || map.Nodes.Count == 0)
            {
                return BadRequest("Map data is missing or empty.");
            }

            _mapState.CurrentMap = map;
            return Ok();
        }

        [HttpGet("GetMap")]
        [ApiKeyAuth("FS_Read")]
        public IActionResult GetMap()
        {
            if (_mapState.CurrentMap == null)
            {
                return BadRequest("Map has not been set.");
            }

            return Ok(_mapState.CurrentMap);
        }

        [HttpGet("ShortestRoute")]
        [ApiKeyAuth("FS_Read")]
        public IActionResult ShortestRoute([FromQuery] string from, [FromQuery] string to)
        {
            if (_mapState.CurrentMap == null)
                return BadRequest("Map has not been set.");

            if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
                return BadRequest("Missing parameters.");

            if (!_mapState.CurrentMap.Nodes.ContainsKey(from) ||
                !_mapState.CurrentMap.Nodes.ContainsKey(to))
                return BadRequest("Unknown node.");

            var result = _pathFinder.FindShortestPath(_mapState.CurrentMap, from, to);

            if (result == null)
                return BadRequest("No route found.");

            return Ok(string.Join("", result.Value.path));
        }

        [HttpGet("ShortestDistance")]
        [ApiKeyAuth("FS_Read")]
        public IActionResult ShortestDistance([FromQuery] string from, [FromQuery] string to)
        {
            if (_mapState.CurrentMap == null)
                return BadRequest("Map has not been set.");

            if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
                return BadRequest("Missing parameters.");

            if (!_mapState.CurrentMap.Nodes.ContainsKey(from) ||
                !_mapState.CurrentMap.Nodes.ContainsKey(to))
                return BadRequest("Unknown node.");

            var result = _pathFinder.FindShortestPath(_mapState.CurrentMap, from, to);

            if (result == null)
                return BadRequest("No route found.");

            return Ok(result.Value.distance);
        }

    }
}
