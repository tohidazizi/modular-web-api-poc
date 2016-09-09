# Modular ASP.NET Web API - Proof of concept
Proof of concept that we can add new Web APIs to an ASP.NET Web API project while it is running and without restarting it.


## Steps to prove

In the following steps, {host} is the base URL of your host environment. For me, it is an IIS Express: "localhost:49603".

### 1. [Optional step] send a GET request to http://{host}/api/modules/

    curl -X POST -H "Content-Type: application/json" "http://{host}/api/modules/"

We have not added any module yet. So you should receive an empty array: [ ].

### 2. [Optional step] send a GET request to http://{host}/api/articles/

    curl -X GET -H "Content-Type: application/json" "http://localhost:49603/api/articles"

The "articles" module has been not added yet. So you should receive a 404 (Not Found) status.

### 3. send a POST request to http://{host}/api/modules/moduleOne

    curl -X POST -H "Content-Type: application/json" "http://localhost:49603/api/modules/moduleone"

The application tries to locate `ModuleOne.dll` in the `~/plugins` folder and load it to the `Application Domain`.
You should receive a 204 (No content) status.

### 4. [Optional step] again, send a GET request to http://{host}/api/modules/

    curl -X POST -H "Content-Type: application/json" "http://{host}/api/modules/"

This time, you will receive something like:

    [
      "ModuleOne, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    ]

### 5. [Optional step] send a GET request to http://{host}/api/articles/

    curl -X GET -H "Content-Type: application/json" "http://localhost:49603/api/articles"

This time, you will receive a 200 (OK) status with the following body:

    [
      {
        "Id": 101
      },
      {
        "Id": 102
      }
    ] 
