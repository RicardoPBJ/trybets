using TryBets.Odds.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Globalization;

namespace TryBets.Odds.Repository;

public class OddRepository : IOddRepository
{
    protected readonly ITryBetsContext _context;
    public OddRepository(ITryBetsContext context)
    {
        _context = context;
    }

    public Match Patch(int MatchId, int TeamId, string BetValue)
    {
        var match = _context.Matches.FirstOrDefault(m => m.MatchId == MatchId);

        string newBetValue = BetValue.Replace(",", ".");
        if (
            !Decimal.TryParse(
                newBetValue,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out decimal betValue
            )
        )
            throw new Exception("Invalid bet value");

        if (match.MatchTeamAId == TeamId)
            match.MatchTeamAValue += betValue;
        else if (match.MatchTeamBId == TeamId)
            match.MatchTeamBValue += betValue;
        else
            throw new Exception("Team is not in this match");

        _context.Matches.Update(match);
        _context.SaveChanges();

        return match;
    }
}