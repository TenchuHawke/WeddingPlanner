EntityBase

A base project for connecting to the database using the Enity Framwork
Includes Files for JQuery and Bootstrap 

appdata.json will still need to be created with the connection settings containing:

{
    "DBInfo":
    {
        "Name": "MySQLconnect",
        "ConnectionString": "server=SERVERIPADDRESS;userid=USERID;password=USERPASSWORD;port=PORTNUMBER;database=DATABASENAME;SslMode=None"
    }
}


ALSO change user models to make it match what your DB fields are.