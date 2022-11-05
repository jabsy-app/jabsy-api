# Jabsy API
This is the API for Jabsy

## Installation
You'll need the [dotnet-sdk](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) and [postgresql](https://www.postgresql.org/download/) for this to work. You'll need to set the `postgres` user's password to `pwd`. Next, run
```
psql -U postgres
```
and
```sql
CREATE DATABASE jabsyuserdb;
\c jabsyuserdb;
CREATE TABLE users(id SERIAL PRIMARY KEY, name VARCHAR(255), status VARCHAR(255), profilepicture TEXT, hero TEXT, email VARCHAR(255), password VARCHAR(255), privatekey VARCHAR(255), publickey VARCHAR(255));
```

Now, you should be good to go!