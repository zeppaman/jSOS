# jSOS
A tool for adding features like file system access to JavaScript

# Why
In a world where we are delivering  powefull javascript application, we would have the possibility to print, interact with file system to be able to develop full desktop application with javascript.
Often the parts that need such capabilities are very limited, so it is quite expensive creating and mantainig a desktop application or an emebdded application (electron).

# How it works
The simple idea behind jSOS is to create an host application that exposes all the feature available on the Desktop side (filesystem, os, printing, etc..) throught API. In that way your javascript application will be able to do what he need,  just using regular API call to localost.
The feature access is scoped and profiled for the app, so that the user will be able do allow for each application only the needed feature. In poor words, it is like the permission on you smartphone.
The JSOS app will be running in background, so each application that will need this tool will be able to interact without any friction.

# Security considerations
Mentinoned javascript limitation are here to prevent security issues and it is easy to understant that are needed. That's why introducing such extension open to security consideration.
The principle over all it that if your user allow an application to access some resources, he thrust it and is able to evalutate the impact on is device. Something like when you go to the app marketplace and install an app. If you thrust the cross word game to access you phonebook and your gallery... it is your decision.
Comparing to the scenario when you install a desktop applciation, you ave more control about what the application does, beacuse the web app will need to ask the proper permission to jSOS. Everything will be transparent.
The access to the API is limited to the local host by deafult, so other PC in the LAN cannot try to use it. Each call to the API is authorized by a token that is generated during registration and avoid that thirdy party can act in behalf of the app you thrusted.

**Note** All the pracutions listed above dosen't prevent the phisching schenario. If some malicius web site try to register an app called "google" and the user is not able to detect that this website is not google, he may thrust the wrong application. This is the big difference from smarphones store: in maketplaces app identy are granted by the vendor.




# Tutorial

## register an application

POST http://localhost:54320/app/register

```
{
    "AppName":"MyApp",
    "Needs":
    [
        "OS/READ",
        "OS/WRITE",
         "FILE/READ",
         "FILE/WRITE",
         "FILE/DELETE"
    ],
    "Async":false,
    "Token":"mysecret"
}
```

| Parameter  | Description |
| ------------- | ------------- |
| AppName  | The name of your app  |
| Token  | The secret used for ask for modification of permission (other websites cannot ask for permission removal or adding)  |
| Needs  | The list of permission asked (string list) |
| Async  | if true the request will register a new app without asking the user for approval with a prompt. The user will need to open the app and grant permission manually. If false the user will be asked for a prompt and request will wait the user response. In case of timeot the app is registered, so the user can manually change permission. |


## invoke a service
By convention each request must have these headers:

- Token: the secret used during app registration
- AppName: the name of the app used

Then the request must be compliant with the functional part of the service (ie. passing path to get files into a folder)

All paths in next part of the documentation omits the hostname part. So, if you read /filesystem/file/save you will have to use probably something like http://localhost:54320/filesystem/file/save


## File system API
This set of API exend the filesystem usage. There are 3 different authotization level:

- file/read (read access to the file system)
- file/write (write access to the file system, witout deletion)
- file/delete (delete access to the file system)

Such permissions are non inclusive, so if you want to create, update and delete a file you need all of them. The filesystem access is unrestricted and limited only by user limitation introduced by OS.
In future we should have different scope for a better app isolation ( like: access only user profile, access only an embedded folder for the app... ).

### POST /filesystem/file/save
Save a file starting from a base64 string

*QUERY PARAMS*
 -filepath

*FROM BODY*
- contentBase64

 *REQUIRED PERMISSION*
 - file/write

### GET /filesystem/file/read 
read file content as base64

*QUERY PARAMS*
 -filepath

*REQUIRED PERMISSION*
 - file/read

### DELETE /filesystem/file/delete
Delete a file

*QUERY PARAMS*
 -filepath

 *REQUIRED PERMISSION*
 - file/delete

### POST /filesystem/directory/create
Create a directory if not exists

*QUERY PARAMS*
 -path

*REQUIRED PERMISSION*
 - file/write

### DELETE /filesystem/directory/delete 
Delete a file

*QUERY PARAMS*
 -path

*REQUIRED PERMISSION*
 - file/delete

### GET /filesystem/directory/files
List all files in a folder, given a seach pattern. The search can be recursive.

*QUERY PARAMS*
 - path: folder path
 - searchPattern: search pattern, something lile *.*  or * to return all files 
 - recursive: if true search in subfolder

*REQUIRED PERMISSION*
 - file/read

## File system API
This set of api expose OS infos like current username, environment variables and performance counters.

### GET /OS/whoami
Return the full username (DOMAIN\USER)
*REQUIRED PERMISSION*
 - os/read

### GET /OS/username
Return the username
*REQUIRED PERMISSION*
 - os/read

### GET /OS/domainname
Return the domain name
*REQUIRED PERMISSION*
 - os/read

### GET /os/variable 
Return a system varibale given the name
*QUERY PARAMS*
 - variableName

*REQUIRED PERMISSION*
 - os/read


### GET /os/cpu
Return CPU usage
*REQUIRED PERMISSION*
 - os/read

### GET /os/ram
Return RAM usage
*REQUIRED PERMISSION*
 - os/read