# Overview

All in one finance messaging microservice

[![Build Status](https://dev.azure.com/gkamacharov/gkama-cicd/_apis/build/status/kamacharovs.aiof-messaging?branchName=main)](https://dev.azure.com/gkamacharov/gkama-cicd/_build/latest?definitionId=25&branchName=main)

## Documentation

Overall documentation for the aiof messaging microservice

### Sending a message

The main functionality is sending a message based on a message type - email, sms, etc. Currently supported types are: email

#### Endpoints

- Sending a message

`/api/message/send`

```json
{
    "type": "Email",
    "from": "test@email.com",
    "to": "gtest@email.com",
    "subject": "",
    "cc": [
        "test@test.com"
    ],
    "bcc": [

    ]
}
```

## How to run it

How to run the messaging microservice locally and through Docker

### Docker

Build it

```ps
docker build -t aiof-messaging .
```

Run it

```ps
docker run -it --rm -e ASPNETCORE_ENVIRONMENT='Development' -p 8003:80 aiof-messaging
```

(Optional) Clean up `none` images

```ps
docker rmi $(docker images -f "dangling=true" -q)
```
