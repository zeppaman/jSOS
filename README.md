# jSOS
a tool for adding features like file system access to JavaScript


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