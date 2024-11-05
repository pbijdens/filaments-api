# FilemantsAPI

API server for a filament tracking front-ends.

## Installation

Requirements:
- MySQL server
- .NET Core 8 Runtime

## Install .NET Core 8

Just install .NET core 8 on your server.

## Configure MYSQL

Create a user:
```sql
CREATE DATABASE FilamentsAPI;
CREATE USER 'csuser'@'{host}' IDENTIFIED WITH mysql_native_password BY '{superSecretPassword!123}';
GRANT ALL PRIVILEGES ON FilamentsAPI.* TO 'csuser'@'%';
FLUSH PRIVILEGES;
```

You can now set-up a connection string in ```appsettings.json``` on your deployed instance like this:

```json
{
  ...,
  "ConnectionStrings": {
    "FilamentsAPIDatabase": "server={server-ip};database=FilamentsAPI;user=csuser;password={superSecretPassword!123}"
  }
}
```

The first time you use the software, the initial database schema will automatically be created.

## Deploy the software

Start with creating a user account that is used to run this software. This user needs no special privileges on the system.

```sh
sudo adduser --system csuser
```

Copy over the software to a temporary folder in the system. Then create a folder to host from. Make sure this is *not* owned by the system user you just created, make sure they can access it though.

```sh
sudo mkdir -p /var/www/FilamentsAPIapi
sudo cp -R /tmp/release /var/www/FilamentsAPIapi
sudo chown -R root:root /var/www/FilamentsAPIapi
sudo chmod -R a+rX /var/www/FilamentsAPIapi
```

If you don't have the file yet, copy /var/www/FilamentsAPIapi/appsettings.json to /var/www/FilamentsAPIapi/appsettings.Production.json and fill in the missing information. The Secret can just be some random string, the LoginSecret is an API password for some restricted functionality. *this is currently not used so set it to some default*.

Create a service definition file for your service, based on ```deployment-files/FilamentsAPIapi.service```. Note that you will need to adjust this for the location of your dotnet binary.

Store the updated file in ```/etc/systemd/system/FilamentsAPIapi.service``` then enable it.

```sh
sudo systemctl enable FilamentsAPIapi.service
```

Then start it and check if it all worked.

```sh
sudo systemctl start FilamentsAPIapi
sudo systemctl status FilamentsAPIapi
```

Congratulations, you should now have your API service running on ```http://<server-ip>:8062/```


## Updating

Stop the service:

```sh
sudo systemctl stop FilamentsAPIapi
```

Copy the new release into the runtime folder:

```sh
sudo rm -f /tmp/release/appsettings.Production.json
sudo cp -R /tmp/release /var/www/FilamentsAPIapi
sudo chown -R root:root /var/www/FilamentsAPIapi
sudo chmod -R a+rX /var/www/FilamentsAPIapi
```

If database updates are required, apply them now to your MySQL database.

Start the software:

```sh
sudo systemctl start FilamentsAPIapi
```

## Doing it all automatically

```sh
#!/bin/bash
systemctl stop filamentsapi
sleep 5

DOWNLOAD_URL=$(curl -s https://api.github.com/repos/pbijdens/filaments-api/releases/latest | grep browser_download_url | cut -d '"' -f4)
DB_SERVER="127.0.0.1"
DB_USER="csuser"
DB_NAME="FilamentsAPI"
DB_PASSWORD="SUPER-SECRET-PASSWORD"

useradd --system --home-dir /var/www --no-create-home csuser --shell /usr/sbin/nologin
mkdir -p /var/www/filamentsapi
rm -rf /tmp/release
mkdir /tmp/release
pushd /tmp/release
curl -sL $DOWNLOAD_URL | tar xvfz -
popd
rm -rf /var/www/filamentsapi/*
cp -R /tmp/release/* /var/www/filamentsapi
chown -R root:root /var/www/filamentsapi
chmod -R a+rX /var/www/filamentsapi

cat <<EOT > /etc/systemd/system/filamentsapi.service
[Unit]
Description=FilamentsAPI API Service (kestrel)

[Service]
WorkingDirectory=/var/www/filamentsapi
ExecStart=/usr/bin/dotnet /var/www/filamentsapi/FilamentsAPI.dll
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=filamentsapi
User=csuser
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://*:8062

[Install]
WantedBy=multi-user.target
EOT

cat <<EOT > /var/www/filamentsapi/appsettings.Production.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "FilamentsAPIDatabase": "server=$DB_SERVER;database=$DB_NAME;user=$DB_USER;password=$DB_PASSWORD"
  }
}
EOT

chown -R root:root /var/www/filamentsapi
chmod -R a+rX /var/www/filamentsapi

systemctl enable filamentsapi.service

systemctl start filamentsapi
sleep 5
systemctl status filamentsapi
```