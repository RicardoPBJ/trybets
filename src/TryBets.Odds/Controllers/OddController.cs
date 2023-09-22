using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using TryBets.Odds.Repository;

namespace TryBets.Odds.Controllers;

[Route("[controller]")]
public class OddController : Controller
{
    private readonly IOddRepository _repository;
    public OddController(IOddRepository repository)
    {
        _repository = repository;
    }

    [HttpPatch("{MatchId:int}/{TeamId:int}/{BetValue}")]

    public IActionResult Patch(int MatchId, int TeamId, string BetValue)
    {
        try
        {
            var updatedMatch = _repository.Patch(MatchId, TeamId, BetValue);
            if (updatedMatch == null)
                return NotFound();

            return Ok(updatedMatch);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return BadRequest(new { message = ex.Message });
        }
    }
}