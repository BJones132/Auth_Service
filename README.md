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
Next, Run `dotnet dev-certs https -t` to trust the developer certificate.
For the changes to come into effect, your browser must be restarted.

### Step 2
Edit the EXAMPLE.env file to match your needs. The FINANCEDB variable needs your postgres hostname/ip address, a username (by default "postgres") and a database name. Depending on how your postgres database was set up, a Password field may need to be added with the corresponding password.\
Select your ASPNETCORE environment. Development or Production.\
Finally, set the CERTIFICATE_PASSWORD to the password you entered in the previous step.\
Ensure that `EXAMPLE.env` is renamed to `.env`.

### Step 3
Open a terminal in the Auth Service folder alongside the compose.yaml and Dockerfile files.\
Run `docker compose up`.\
\
This should build the docker container and allow API requests to be sent to `https://localhost:5000/`. In development mode, you can navigate to `https://localhost:5000/scalar/` to view the endpoints and both development and production mode will show an OpenAPI specification at `https://localhost:5000/openapi/v1.json`.
