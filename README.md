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
