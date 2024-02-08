A small project to learn Angular with.

The Angular frontend communicates with a WebApi.

The WebApi is writtin in C# using minimal api's.
The WebApi, in turn, get its data from a SQL database.

The WebApi is based on transaction scripts, with a service layer communicating with a database layer.

The database is simple.
There are three tables
    User
    Company
    Project

A Company can have multiple projects. A project can only belong to 1 and only 1 Company.
A Project can also have zero or 1 Managers. A manager is a User.

There are no views or triggers defined in the database.
