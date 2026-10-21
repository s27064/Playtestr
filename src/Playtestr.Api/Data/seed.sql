-- Example data for local testing.
-- WARNING: deletes everything in Games, Playtests, Questions, Submissions and Answers first.
-- Run from src/Playtestr.Api:  sqlite3 playtestr.db < Data/seed.sql

PRAGMA foreign_keys = ON;
BEGIN TRANSACTION;

DELETE FROM Answers;
DELETE FROM Submissions;
DELETE FROM Questions;
DELETE FROM Playtests;
DELETE FROM Games;
DELETE FROM sqlite_sequence WHERE name IN ('Games', 'Playtests', 'Questions', 'Submissions', 'Answers');

-- Games ---------------------------------------------------------------------

INSERT INTO Games (Id, Name, Description, ReleaseYear) VALUES
    (1, 'Ember Hollow',    'A cozy farming sim set in a village built inside a dormant volcano.',        2025),
    (2, 'Starlane Couriers', 'Co-op space delivery game where you and up to three friends haul cargo.',  2026),
    (3, 'Gloomwick',       'Dark fantasy roguelike with hand-drawn dungeons and permadeath.',            2024),
    (4, 'Pixel Pitch',     'Arcade football with 2v2 matches and power-ups.',                            2026),
    (5, 'The Quiet Archive', 'Narrative puzzle game about restoring a forgotten library.',               2027),
    (6, 'Turbo Tortoise',  'Kart racer for kids where every racer is a very determined animal.',         2026);

-- Playtests -----------------------------------------------------------------

INSERT INTO Playtests (Id, Title, Description, CreatedAt, GameId) VALUES
    (1,  'Alpha – first week',        'Covers the tutorial and the first in-game week.',                 '2026-06-02 10:00:00', 1),
    (2,  'Fishing update',            'Testing the new fishing minigame and rod upgrades.',              '2026-08-14 14:30:00', 1),
    (3,  'Closed beta',               'Four-player sessions on the first three delivery routes.',        '2026-07-01 18:00:00', 2),
    (4,  'Netcode stress test',       'Full lobbies with simulated high latency.',                       '2026-09-20 19:00:00', 2),
    (5,  'Combat feel',               'Focus on the new parry and dodge timing.',                        '2026-05-10 12:00:00', 3),
    (6,  'Boss rush',                 'All five bosses back to back with a fixed loadout.',              '2026-06-25 16:00:00', 3),
    (7,  'Difficulty curve',          'Floors 1 to 10 with the rebalanced enemy stats.',                 '2026-10-01 11:00:00', 3),
    (8,  'Controls prototype',        'Comparing the classic and the twin-stick control schemes.',       '2026-07-18 15:00:00', 4),
    (9,  'Tournament mode',           'Bracket mode with eight local players.',                          '2026-09-05 17:30:00', 4),
    (10, 'Chapter one',               'The prologue and the first wing of the library.',                 '2026-08-01 09:00:00', 5),
    (11, 'Puzzle hint system',        'Testing whether the three-tier hints are too generous.',          '2026-10-10 13:00:00', 5),
    (12, 'Kids focus group',          'Playtest with children aged 6–10 and their parents.',             '2026-09-12 10:00:00', 6);

-- Questions (Type: 0 = Rating, 1 = Text) -------------------------------------

INSERT INTO Questions (Id, Text, Type, PlaytestId) VALUES
    (1,  'How fun was the session overall?',                         0, 1),
    (2,  'How clear was the tutorial?',                              0, 1),
    (3,  'How relaxing did the game feel?',                          0, 1),
    (4,  'What did you enjoy most?',                                 1, 1),
    (5,  'What confused you?',                                       1, 1),

    (6,  'How fun was fishing?',                                     0, 2),
    (7,  'How fair did the catch rates feel?',                       0, 2),
    (8,  'Were the rod upgrades worth the cost?',                    0, 2),
    (9,  'Any other feedback on fishing?',                           1, 2),

    (10, 'How fun was the session overall?',                         0, 3),
    (11, 'How well did co-op work with your group?',                 0, 3),
    (12, 'How readable was the route map?',                          0, 3),
    (13, 'How likely are you to play again?',                        0, 3),
    (14, 'What would you change first?',                             1, 3),

    (15, 'How stable was the connection?',                           0, 4),
    (16, 'How noticeable was input lag?',                            0, 4),
    (17, 'Describe any desyncs or disconnects you saw.',             1, 4),

    (18, 'How responsive did combat feel?',                          0, 5),
    (19, 'How fair was the parry window?',                           0, 5),
    (20, 'How satisfying was dodging?',                              0, 5),
    (21, 'What felt off about combat?',                              1, 5),

    (22, 'How hard were the bosses overall?',                        0, 6),
    (23, 'How fun were the boss fights?',                            0, 6),
    (24, 'Which boss was your favourite and why?',                   1, 6),
    (25, 'Which boss felt unfair?',                                  1, 6),

    (26, 'How smooth was the difficulty curve?',                     0, 7),
    (27, 'How rewarding were the item drops?',                       0, 7),
    (28, 'How often did you feel stuck?',                            0, 7),
    (29, 'On which floor did you quit or die most, and why?',        1, 7),

    (30, 'How intuitive were the classic controls?',                 0, 8),
    (31, 'How intuitive were the twin-stick controls?',              0, 8),
    (32, 'Which scheme do you prefer and why?',                      1, 8),

    (33, 'How fun was the tournament overall?',                      0, 9),
    (34, 'How balanced were the power-ups?',                         0, 9),
    (35, 'How easy was it to set up the bracket?',                   0, 9),
    (36, 'What would make tournaments better?',                      1, 9),

    (37, 'How engaging was the story?',                              0, 10),
    (38, 'How satisfying were the puzzles?',                         0, 10),
    (39, 'How much did you like the art style?',                     0, 10),
    (40, 'Which puzzle did you like most?',                          1, 10),
    (41, 'Where did you get stuck?',                                 1, 10),

    (42, 'How helpful was the first hint?',                          0, 11),
    (43, 'Did the hints spoil the puzzle?',                          0, 11),
    (44, 'How would you change the hint system?',                    1, 11),

    (45, 'How much did your child enjoy it?',                        0, 12),
    (46, 'How easy was it for your child to steer?',                 0, 12),
    (47, 'How appropriate was the difficulty for your child?',       0, 12),
    (48, 'Which animal did your child pick and why?',                1, 12),
    (49, 'Anything a parent should know about?',                     1, 12);

-- Submissions ---------------------------------------------------------------
-- 8 to 15 submissions per playtest, spread out after the playtest was created.
-- About one in five testers stays anonymous (TesterName is NULL).

CREATE TEMP TABLE TesterNames (N INTEGER PRIMARY KEY, Name TEXT NOT NULL);
INSERT INTO TesterNames (N, Name) VALUES
    (0, 'Alex'), (1, 'Sam'), (2, 'Jordan'), (3, 'Taylor'), (4, 'Riley'),
    (5, 'Casey'), (6, 'Morgan'), (7, 'Jamie'), (8, 'Avery'), (9, 'Quinn'),
    (10, 'Robin'), (11, 'Charlie'), (12, 'Skyler'), (13, 'Dana'), (14, 'Kai'),
    (15, 'Noor'), (16, 'Mika'), (17, 'Ola'), (18, 'Yuki'), (19, 'Ezra');

WITH RECURSIVE Seq(N) AS (
    SELECT 1 UNION ALL SELECT N + 1 FROM Seq WHERE N < 15
),
Rows AS (
    SELECT p.Id AS PlaytestId, p.CreatedAt, s.N
    FROM Playtests p
    JOIN Seq s ON s.N <= 8 + (p.Id * 5) % 8
),
Numbered AS (
    SELECT ROW_NUMBER() OVER (ORDER BY PlaytestId, N) AS Id, PlaytestId, CreatedAt, N
    FROM Rows
)
INSERT INTO Submissions (Id, SubmittedAt, TesterName, PlaytestId)
SELECT
    n.Id,
    strftime('%Y-%m-%d %H:%M:%S', n.CreatedAt, '+' || (n.N / 2) || ' days', '+' || ((n.Id * 37) % 600) || ' minutes'),
    CASE WHEN n.Id % 5 = 0 THEN NULL ELSE (SELECT Name FROM TesterNames WHERE N = (n.Id * 7) % 20) END,
    n.PlaytestId
FROM Numbered n;

-- Answers -------------------------------------------------------------------
-- Every submission rates every Rating question. Each question gets its own average
-- (somewhere between ~2 and ~4.5) so the results endpoint shows different numbers.
-- Roughly one in four text questions is skipped, like a tester leaving it blank.

CREATE TEMP TABLE TextAnswers (N INTEGER PRIMARY KEY, Text TEXT NOT NULL);
INSERT INTO TextAnswers (N, Text) VALUES
    (0,  'Loved the atmosphere, the music really sells it.'),
    (1,  'It took me a while to figure out what I was supposed to do next.'),
    (2,  'The controls felt a bit floaty at first but I got used to them.'),
    (3,  'Honestly the best part was playing with friends.'),
    (4,  'Menus are confusing, I kept opening the wrong tab.'),
    (5,  'Too short! I wanted to keep playing.'),
    (6,  'Some text was too small to read on my laptop.'),
    (7,  'The art style is gorgeous.'),
    (8,  'I got stuck and had to restart, not sure if it was a bug.'),
    (9,  'Difficulty spiked out of nowhere halfway through.'),
    (10, 'Sound effects are satisfying, especially the UI clicks.'),
    (11, 'Would be nice to have a way to skip dialogue.'),
    (12, 'Camera got caught on walls a few times.'),
    (13, 'Great pacing, never felt bored.'),
    (14, 'I did not understand the upgrade system at all.'),
    (15, 'Frame rate dropped when there was a lot happening on screen.'),
    (16, 'The ending of the session felt abrupt.'),
    (17, 'Characters have a lot of personality.'),
    (18, 'Please add controller remapping.'),
    (19, 'Nothing major, it was a lot of fun.'),
    (20, 'Tutorial explained too much, let me figure things out.'),
    (21, 'Saving was not obvious, I lost some progress.'),
    (22, 'Rewards felt too stingy early on.'),
    (23, 'The colour palette made some enemies hard to see.'),
    (24, 'I would definitely recommend this to friends.');

INSERT INTO Answers (QuestionId, Rating, SubmissionId, Text)
SELECT
    q.Id,
    CASE WHEN q.Type = 0
        THEN MIN(5, MAX(1, 2 + q.Id % 3 + (s.Id * 7 + q.Id * 3) % 3 - (s.Id + q.Id) % 2))
    END,
    s.Id,
    CASE WHEN q.Type = 1
        THEN (SELECT Text FROM TextAnswers WHERE N = (s.Id * 11 + q.Id * 5) % 25)
    END
FROM Submissions s
JOIN Questions q ON q.PlaytestId = s.PlaytestId
WHERE q.Type = 0 OR (s.Id + q.Id) % 4 <> 0
ORDER BY s.Id, q.Id;

DROP TABLE TesterNames;
DROP TABLE TextAnswers;

COMMIT;
