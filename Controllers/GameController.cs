using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Domotica_API.Controllers;
using Domotica_API.Middleware;
using Domotica_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Domotica_API.Controllers
{
	[MiddlewareFilter(typeof(TokenAuthorize))]
	[Route(Config.App.API_ROOT_PATH + "/Game")]
	public class GameController : ApiController
    {
	    public GameController(DatabaseContext db) : base(db) { }

		[HttpPost("create")]
		public IActionResult CreateGame([FromBody] Validators.GameCreate gameCreate)
		{
			User user = (User)HttpContext.Items["user"];
			User user_2 = this.db.Users.SingleOrDefault(x => x.id == gameCreate.user_2_id);
			int status = (user_2 == null) ? GameStatus.waiting_join : GameStatus.waiting_invite;

			//Check if the user is trying to invite himself.
			if (user == user_2)
			{
				return BadRequest("You can't play against yourself.");
			}

			Game NewGame = new Game
			{
				user_1_id = user.id,
				user_2_id = user_2.id,
				status = status
			};

			this.db.Add(NewGame);
			this.db.SaveChanges();

			object result = new
			{
				status = NewGame.status,
				opponement = user_2.name,
				moves = NewGame.Moves,
			};

			return Ok(result);

	    }

	    [HttpPost("join")]
		public IActionResult JoinGame([FromBody] Validators.GameJoin gameJoin)
	    {
		    if (ModelState.IsValid == false)
		    {
			    return BadRequest("Incorrect post data");
		    }

		    User user = (User)HttpContext.Items["user"];
		    Game game = this.db.Games.SingleOrDefault(x => x.id == gameJoin.id);

			//Check if game exists.
		    if (game == null)
		    {
			    return BadRequest("Game does not exist.");
		    }

			//Check if the user is trying to join a game that he created.
		    if (game.user_1 == user)
		    {
			    return BadRequest("You can't play against yourself.");
		    }

			//Check if the game is waiting for an random user or an invited user.

			//Check if the game is waiting for an invited user.
		    if (game.status == GameStatus.waiting_invite)
		    {
				//Check if the user that tries to join is indeed the invited user.
			    if (game.user_2_id == user.id)
			    {
				    game.status = GameStatus.started;
				    this.db.SaveChanges();
				    return Ok(game);
			    }

			    return BadRequest("Can't join game, you were not invited.");
		    }

			//Check if the game is waiting for a random user.
		    if (game.status == GameStatus.waiting_join)
		    {
			    game.status = GameStatus.started;
			    this.db.SaveChanges();
			    return Ok(game);
		    }

			//Game status is probably started or finished.
			return BadRequest("Can't join game. Game status: " + game.status);
		}

		[HttpPost("move/create")]
	    public IActionResult AddMove([FromBody] Validators.Move move)
	    {
		    if (ModelState.IsValid == false)
		    {
			    return BadRequest("Incorrect post data.");
		    }

			//Check if game exists.
		    Game game = this.db.Games.SingleOrDefault(x => x.id == move.game_id);
		    if (game == null)
		    {
			    return BadRequest("Game does not exist.");
		    }

			//Check if the user is a player of this game.
			User user = (User)HttpContext.Items["user"];
		    if (game.user_1 != user && game.user_2 != user)
		    {
			    return BadRequest("This is not your game.");
		    }

			//Check if it's his turn.
		    if (user != this.DecideTurn(game))
		    {
			    return BadRequest("It's not your turn.");
		    }

			//Check if turn is valid(Position not already chosen).
		    if (CheckPosition(game, move.position) == false)
		    {
			    return BadRequest("Position already chosen.");
		    }

		    this.db.Add(new Move
		    {
			    user_id = user.id,
			    game_id = game.id,
			    position = move.position
		    });
		    this.db.SaveChanges();

		    return Ok(game);
	    }

	    private bool CheckPosition(Game game, int position)
	    {
		    List<Move> moves = this.db.Moves.Where(x => x.game_id == game.id).ToList();
		    foreach (Move move in moves)
		    {
				//If the position is already found in the existing moves then.
			    if (move.position == position)
			    {
				    return false;
			    }
		    }

		    return true;
	    }

	    private User DecideTurn(Game game)
	    {
		    int User1Moves = this.db.Moves.Where(x => x.game_id == game.id && x.user_id == game.user_1_id).ToList().Count;
		    int User2Moves = this.db.Moves.Where(x => x.game_id == game.id && x.user_id == game.user_2_id).ToList().Count;

		    //If the turns are are equal then it's player 1 his turn.
			//So for example when a game has started both turns are 0. This means player one can start.
		    if (User1Moves == User2Moves)
		    {
			    return game.user_1;
		    }
			//If the turn are not equal then it's player 2 his turn.
			//So when player 1 has 1 turn and player 2 has 0 turn then it's players 2 his turn.
		    else
		    {
			    return game.user_2;
		    }
		}
    }
}