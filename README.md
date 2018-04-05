# Friend Meet Friend

Restful API backend for a mini blog engine with token-based authentication
(using JWTs).

Technology
----------
* ASP.NET Core
* PostgreSQL (with Entity Framework)

Endpoints
---------

### Posts
| Method     | URI                                   | Action                                    |
|------------|---------------------------------------|-------------------------------------------|
| `GET`      | `/api/users`                          | `Retrieve all user profiles`              |
| `GET`      | `/api/users/{uid}`                    | `Retrieve user profile`                   |
| `PATCH`    | `/api/users/{uid`                     | `Partially update logged in user's profile`         |

### Messages
| Method     | URI                                   | Action                                    |
|------------|---------------------------------------|-------------------------------------------|
| `GET`      | `/api/users/{uid}/messages/{id}`      | `Retrieve user message`                   |
| `GET`      | `/api/users/{uid}/messages/sent`      | `Retrieve user's sent messages`           |
| `GET`      | `/api/users/{uid}/messages/recieved`  | `Retrieve user's recieved messages`       |
| `POST`     | `/api/users/{uid}/messages`           | `Send message to another user`            |

### Following
| Method     | URI                                   | Action                                    |
|------------|---------------------------------------|-------------------------------------------|
| `POST`     | `/api/users/{uid}/follow`             | `Follow user`                             |
| `POST`     | `/api/users/{uid}/unfollow`           | `Unfollow user`                           |

### Auth
| Method     | URI                                   | Action                                    |
|------------|---------------------------------------|-------------------------------------------|
| `GET`      | `/api/status`                         | `Check Login Status `                     |
| `POST`     | `/api/login`                          | `Login User`                              |
| `POST`     | `/api/register`                       | `Register User`                           |

Sample Usage
---------------
`http post localhost:5000/api/register email=user@email.com username=user password=pass`

```
{
    "email": "user@email.com", 
    "id": 5, 
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImRlbW9AZW1haWwuY29tIiwiZXhwIjoxNTIyNzQ3NTA1LCJ1c2VySWQiOjV9.jIcIwq8hA1uSLDFyuytr1lGwQ9WNnvkubzz0qrPN7SQ"
}

```

`http post localhost:5000/api/login email=user@email.com password=pass`
```
{
    "email": "user@email.com", 
    "id": 5, 
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImRlbW9AZW1haWwuY29tIiwiZXhwIjoxNTIyNzQ3NjIxLCJ1c2VySWQiOjV9.M71uY55Za_PjUo4QdZIf3FI-t6mB9ySCMuzWql1BCsE"
}

```

`http localhost:5000/api/users`

```
[
    {
        "borough": "manhattan", 
        "createdOn": "2018-03-31T00:39:49.243078Z", 
        "followees": [
            {
                "id": 1, 
                "name": "michael"
            }
        ], 
        "followers": [
            {
                "id": 4, 
                "name": "amanda"
            }, 
            {
                "id": 1, 
                "name": "michael"
            }
        ], 
        "id": 2, 
        "interests": "bmx bikes", 
        "lastActive": "2018-03-31T00:39:49.243078Z", 
        "name": "mick"
    }, 
...
```

`http --auth-type=jwt --auth="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImRlbW9AZW1haWwuY29tIiwiZXhwIjoxNTIyNzQ3NjIxLCJ1c2VySWQiOjV9.M71uY55Za_PjUo4QdZIf3FI-t6mB9ySCMuzWql1BCsE" post localhost:5000/api/users/5/messages recipientId:=3 body="hey, let me know if you want to go to an art museum sometime."`

```
{
    "body": "hey, let me know if you want to go to an art museum sometime.", 
    "recipient": {}, 
    "recipientId": 3, 
    "sender": {}, 
    "senderId": 5, 
    "timestamp": "2018-04-03T00:34:13.920188005-04:00"
}

```

Run
---
If you have docker installed,
```
docker-compose build
docker-compose up
Go to http://localhost:5000 and visit one of the above endpoints
```

Otherwise, create a database, run the migration scripts, and then open `main.go`
and point the PostgreSQL URI to your server,

After all that has been taken care of,
```
dotnet restore
dotnet ef database update
dotnet run
Go to http://localhost:5000 and visit one of the above endpoints
```

TODO
----
Update last active timestamp upon user login  
Add unit tests  
Add an eloquent config management package
