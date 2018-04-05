# Friend Meet Friend

Restful API backend for a mini blog engine with token-based authentication
(using JWTs).

Technology
----------
* ASP.NET Core
* PostgreSQL (with Entity Framework)

Endpoints
---------

### Tags
| Method    | URI                                   | Action                                    |
|-----------|---------------------------------------|-------------------------------------------|
| `GET`     | `/api/tags`                           | `Retrieve all tags`                       |

### Categories
| Method    | URI                                   | Action                                    |
|-----------|---------------------------------------|-------------------------------------------|
| `GET`     | `/api/categories`                     | `Retrieve all categories`                 |

### Posts
| Method    | URI                                   | Action                                    |
|-----------|---------------------------------------|-------------------------------------------|
| `GET`     | `/api/posts`                          | `Retrieve all posts`                      |
| `GET`     | `/api/posts/{pid}`                    | `Retrieve post`                           |
| `POST`    | `/api/posts/`                         | `Create post`                             |
| `PUT`     | `/api/posts/{pid}`                    | `Edit post`                               |
| `DELETE`  | `/api/posts/{pid}`                    | `Delete post`                             |
| `GET`     | `/api/posts/{pid}/comments`           | `Retrieve all post comments`              |
| `GET`     | `/api/posts/{pid}/comments/{id}`      | `Retrieve post comment`                   |
| `POST`    | `/api/posts/{pid}/comments`           | `Create post comment`                     |
| `PUT`     | `/api/posts/{pid}/comments/{id}`      | `Edit post comment`                       |
| `DELETE`  | `/api/posts/{pid}/comments/{id}`      | `Delete post comment`                     |

// update schema to reflect pid

### Users
| Method    | URI                                   | Action                                    |
|-----------|---------------------------------------|-------------------------------------------|
| `GET`     | `/api/users     `                     | `Retrieve (logged in) user details`       |
| `GET`     | `/api/users/{username}`               | `Retrieve user          `                 |
| `PUT`     | `/api/users`                          | `Edit (logged in) user profile`           |

// Check difference between first and second

### Auth
| Method     | URI                                   | Action                                   |
|------------|---------------------------------------|------------------------------------------|
| `POST`     | `/auth/register`                      | `Register User`                          |
| `POST`     | `/auth/login`                         | `Login User`                             |


Sample Usage
---------------
`http post localhost:5000/api/register email=user@email.com username=user password=pass`

```
TODO
```

`http post localhost:5000/api/login email=user@email.com password=pass`
```
TODO
```

`http localhost:5000/api/users`

```
TODO
...
```

`http --auth-type=jwt --auth="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImRlbW9AZW1haWwuY29tIiwiZXhwIjoxNTIyNzQ3NjIxLCJ1c2VySWQiOjV9.M71uY55Za_PjUo4QdZIf3FI-t6mB9ySCMuzWql1BCsE" post localhost:5000/api/users/5/messages recipientId:=3 body="hey, let me know if you want to go to an art museum sometime."`

```
TODO
```

Run
---
If you have docker installed,
```
docker-compose build
docker-compose up
Go to http://localhost:5000 and visit one of the above endpoints (or /swagger)
```

Otherwise you will need the .NET Core 2.0 SDK. If you have the SDK installed,
then open `appsettings.json` and point the connection string to your server,
then run:
```
dotnet restore
dotnet ef database update
dotnet run
Go to http://localhost:5000 and visit one of the above endpoints (or /swagger)
```
