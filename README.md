# Playtestr

[![Build and test](https://github.com/s27064/Playtestr/actions/workflows/build.yml/badge.svg)](https://github.com/s27064/Playtestr/actions/workflows/build.yml)

REST API for collecting feedback from game playtests. I made it to learn ASP.NET Core and EF Core.

The idea: a developer adds their game, creates a playtest and writes some questions (either a 1-5 rating or a text answer). Players log in and answer them, and they can choose to be anonymous. Then the developer can see a summary of the playtest with average ratings and all the text answers.

This is what the summary looks like (shortened):

```json
{
  "playtestId": 3,
  "title": "Closed beta",
  "submissionCount": 15,
  "questionSummaries": [
    {
      "questionId": 11,
      "text": "How well did co-op work with your group?",
      "type": "Rating",
      "averageRating": 4.33,
      "answerCount": 15,
      "textAnswers": []
    },
    {
      "questionId": 14,
      "text": "What would you change first?",
      "type": "Text",
      "averageRating": null,
      "answerCount": 11,
      "textAnswers": [
        "Difficulty spiked out of nowhere halfway through.",
        "Would be nice to have a way to skip dialogue."
      ]
    }
  ]
}
```

Built with .NET 10 (minimal APIs), EF Core + PostgreSQL, JWT auth, Scalar for API docs, xUnit for tests and Docker.

## How to run

The easiest way is Docker:

```
docker compose up --build
```

The API runs on http://localhost:8080 and the docs are on http://localhost:8080/scalar. The app runs the migrations and loads sample data on startup, so you don't need to set anything up.

To run it with `dotnet run` instead (needs the .NET 10 SDK), start only the database and set a JWT key:

```
docker compose up -d db
dotnet user-secrets set "Jwt:Key" "some-long-secret-at-least-32-characters" --project src/Playtestr.Api
dotnet run --project src/Playtestr.Api
```

The API then runs on http://localhost:5102

### Test accounts

The sample data in `seed.sql` was generated using an LLM. All users have the password `playtestr123`. To try it in Scalar, log in with `POST /auth/login` as `gloom-games`, paste the token into the Bearer auth field and call `GET /playtests/7/results`. Users like `alex` or `sam` are testers who already sent answers.

## Endpoints

There are endpoints for auth, games, playtests, questions, submissions and the playtest results. Register, login and reading games, playtests and questions are public, everything else needs a token. Only the owner of a game can change it, its playtests and questions, or see its submissions and results. A single submission can be seen or deleted by the person who sent it or the game owner. The full list is in Scalar.

## Some notes

Endpoints don't contain logic, they call services. Services return an `OperationResult` with a status like `NotFound` or `Forbidden` and the endpoint turns it into the right HTTP response (errors are returned as ProblemDetails). I did it this way so the services don't depend on ASP.NET and it's easier to test them.

When someone submits answers I check that they didn't already submit for this playtest, that every question belongs to the playtest and is answered only once, and that the answer type is right (rating for rating questions, text for text questions).

The most important rules are also in the database as constraints: one submission per user per playtest, ratings between 1 and 5, an answer is either a rating or a text, length limits, and usernames are unique ignoring case. The API validation gives the user a nice error, the constraints make sure bad data can't get in even if there's a bug in the code.

You have to be logged in to submit, otherwise one person could spam answers. That's why I added the anonymous option, so the developer doesn't see who wrote it.

I didn't use ASP.NET Core Identity. I wrote registration, login and the JWT handling myself (only the password hashing uses Identity's `PasswordHasher`), because I wanted to understand how it works. For a real app I'd use Identity or an external provider, since they also handle things like lockout and password reset.

## Tests

```
dotnet test
```

For now there are tests for `GameService` and for the database constraints. They use SQLite in memory instead of the EF InMemory provider, because it behaves more like a real database. GitHub Actions runs the build and tests on every push.

## TODO

- tests for the other services
- integration tests
- pagination