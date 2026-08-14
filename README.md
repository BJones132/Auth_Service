# Auth Service
This is a mostly standalone authentication service for use with my "Simple Finance App" project.\
This service is mostly standalone as it relies on an active postgres database server to perform any actions.

## About
This authentication service serves a REST API with three endpoints.\
`/register` which takes a username and password to generate a user on the database. The password supplied is hashed using the Argon2id algorithm.\
`/authenticate` which takes a username and password, compares with the data stored in the database and returns a uuid/guid token which stays active for 14 days to allow returning users to skip login.\
`id` which takes a valid token string and returns the corresponding user's ID.

## Instructions
### Docker Compose
Docker information can be found [Here](https://www.docker.com/) -- Docker is a requirement for the Docker Compose instructions.

### Step 1
Generate and export a HTTPS certificate in `C:\Users\<CURRENT_USER>\AppData\Roaming\ASP.NET\Https`. This directory may have to be created if it does not exist.\
Generating a certificate can be done in many ways but I will show the `dotnet dev-certs` method.\
First, you will need to open a terminal in `C:\Users\<CURRENT_USER>\AppData\Roaming\ASP.NET\Https`.\
Run the command `dotnet dev-certs https -ep ./Auth_Service.pfx -p <PASSWORD>`. Replace <PASSWORD> with a password of your choice. It is recommended to use a powershell new-guid or similar for a secure password.\
