# auth_app

```
make auth_run

```

## Service for Authorization and Authentication

Service for Authorization and Authentication with API and Swagger on host:port/swagger

## Methods

```
/About
Request: None
Response: String

```
```
/Auth/SignUp
Request: Accepts<User>("application/json")
Response: Produces<User>(statusCode: 200, contentType: "application/json")

```
```
/Auth/SignIn
Request: Accepts<Login>("application/json")
Response: Produces<Login>(statusCode: 200, contentType: "application/json")

```
```
/Auth/CurrentUserInfo
Request: Jwt token from Header
Response: Produces<User>(statusCode: 200, contentType: "application/json")

```
```
/Auth/TokenProlongate
Request: None
Response: None

```
```
/Auth/Logout
Request: Jwt token from Header
Response: Redirect

```
```
/Auth/TokenInfo
Request: Jwt token from Header
Response: Produces<Token>(statusCode: 200, contentType: "application/json")

```
