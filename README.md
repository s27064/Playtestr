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
      "questionId": 12,
      "text": "How readable was the route map?",
      "type": "Rating",
      "averageRating": 2.53,
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

The API runs on http://localhost:8080 and the docs are on http://localhost:8080/scalar

In development the app runs the migrations and loads sample data on startup, so you don't need to set anything up.

You can also run the API with `dotnet run`, which is what I do while working on it. You need the .NET 10 SDK and a Postgres database. I just start the database from Docker:

```
docker compose up -d db
```

If you have Postgres installed you can use that instead. The connection string in `appsettings.Development.json` expects a database `playtestr` on localhost:5432 with user and password `playtestr`. The password is in the repo on purpose, it's only for the local Docker database.

Then set a JWT key and run it:

```
dotnet user-secrets set "Jwt:Key" "some-long-secret-at-least-32-characters" --project src/Playtestr.Api
dotnet run --project src/Playtestr.Api
```

The API runs on http://localhost:5102

### Test accounts

The sample data (users, games, playtests and answers in `seed.sql`) was generated using an LLM.

All seeded users have the password `playtestr123`. For example `ember-studio` owns the game Ember Hollow and `gloom-games` owns Gloomwick, so you can log in as one of them to see the playtest results. Users like `alex`, `sam` or `jordan` are testers who already sent answers.

To call endpoints that need login in Scalar, use `POST /auth/login`, copy the token and paste it in the Bearer auth field.

For example, log in as `gloom-games` and call `GET /playtests/7/results` to see the summary of one of the Gloomwick playtests.

## Endpoints

Auth
- `POST /auth/register`
- `POST /auth/login`
- `GET /auth/me`

Games
- `GET /games`, `GET /games/{id}`
- `POST /games`
- `PUT /games/{id}`, `DELETE /games/{id}` (only the owner)

Playtests
- `GET /playtests`, `GET /playtests/{id}`, `GET /games/{gameId}/playtests`
- `POST /games/{gameId}/playtests`, `PUT /playtests/{id}`, `DELETE /playtests/{id}` (only the game owner)

Questions
- `GET /questions`, `GET /questions/{id}`, `GET /playtests/{playtestId}/questions`
- `POST /playtests/{playtestId}/questions`, `PUT /questions/{id}`, `DELETE /questions/{id}` (only the game owner)

Submissions
- `POST /playtests/{playtestId}/submissions` - send answers
- `GET /playtests/{playtestId}/submissions` - all submissions (only the game owner)
- `GET /submissions/{id}`, `DELETE /submissions/{id}` - the person who submitted or the game owner

Results
- `GET /playtests/{playtestId}/results` - summary of a playtest (only the game owner)

All GET requests for games, playtests and questions are public, everything else needs a token.

## Some notes

Endpoints don't contain logic, they call services. Services return an `OperationResult` with a status like `NotFound` or `Forbidden` and the endpoint turns it into the right HTTP response (errors are returned as ProblemDetails). I did it this way so the services don't depend on ASP.NET and it's easier to test them.

When someone submits answers I check that they didn't already submit for this playtest, that every question belongs to the playtest and is answered only once, and that the answer type is right (rating for rating questions, text for text questions).

You have to be logged in to submit, otherwise one person could spam answers. That's why I added the anonymous option, so the developer doesn't see who wrote it.

I didn't use ASP.NET Core Identity. I wrote registration, login and the JWT handling myself (only the password hashing uses Identity's `PasswordHasher`), because I wanted to understand how it works. For a real app I'd use Identity or an external provider, since they also handle things like lockout and password reset.

## Tests

```
dotnet test
```

For now there are tests for `GameService`. They use SQLite in memory instead of the EF InMemory provider, because it behaves more like a real database. GitHub Actions runs the build and tests on every push.

## TODO

- tests for the other services
- integration tests
- pagination