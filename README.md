# SigSpec for SignalR Core

[![Azure DevOps](https://img.shields.io/azure-devops/build/rsuter/9023bd0a-b641-4e30-9c0f-a7c15e1e080e/22/master.svg)](https://dev.azure.com/rsuter/Namotion/_build?definitionId=22)
[![Azure DevOps](https://img.shields.io/azure-devops/coverage/rsuter/9023bd0a-b641-4e30-9c0f-a7c15e1e080e/22/master.svg)](https://dev.azure.com/rsuter/Namotion/_build?definitionId=22)
[![Nuget](https://img.shields.io/nuget/v/SigSpec.Core.svg)](https://www.nuget.org/packages?q=sigspec)

**Experimental API endpoint specification** and code generator for [SignalR Core](https://github.com/aspnet/SignalR).

Run SigSpec.Console to see a demo spec and the generated TypeScript code.

**Please let me know what you think [here](https://github.com/RSuter/SigSpec/issues/1).**

Based on [NJsonSchema](http://njsonschema.org) (see also: [NSwag](http://nswag.org)).

Original idea: https://github.com/RSuter/NSwag/issues/691

## Sample

Hub: 

```csharp
public class ChatHub : Hub<IChatClient>
{
    public Task Send(string message)
    {
        if (message == string.Empty)
        {
            return Clients.All.Welcome();
        }

        return Clients.All.Send(message);
    }

    public Task AddPerson(Person person)
    {
        return Task.CompletedTask;
    }

    public ChannelReader<Event> GetEvents()
    {
        var channel = Channel.CreateUnbounded<Event>();
        return channel.Reader;
    }
}

public class Event
{
    public string Type { get; set; }
}

public class Person
{
    [JsonProperty("firstName")]
    public string FirstName { get; set; }

    [JsonProperty("lastName")]
    public string LastName { get; set; }
}

public interface IChatClient
{
    Task Welcome();

    Task Send(string message);
}
```

Generated spec: 

```json
{
  "hubs": {
    "chat": {
      "name": "Chat",
      "description": "",
      "operations": {
        "Send": {
          "description": "",
          "parameters": {
            "message": {
              "type": [
                "null",
                "string"
              ],
              "description": ""
            }
          }
        },
        "AddPerson": {
          "description": "",
          "parameters": {
            "person": {
              "description": "",
              "oneOf": [
                {
                  "type": "null"
                },
                {
                  "$ref": "#/definitions/Person"
                }
              ]
            }
          }
        },
        "GetEvents": {
          "description": "",
          "parameters": {},
          "returntype": {
            "description": "",
            "oneOf": [
              {
                "type": "null"
              },
              {
                "$ref": "#/definitions/Event"
              }
            ]
          },
          "type": "Observable"
        }
      },
      "callbacks": {
        "Welcome": {
          "description": "",
          "parameters": {}
        },
        "Send": {
          "description": "",
          "parameters": {
            "message": {
              "type": [
                "null",
                "string"
              ],
              "description": ""
            }
          }
        }
      }
    }
  },
  "definitions": {
    "Person": {
      "type": "object",
      "additionalProperties": false,
      "properties": {
        "firstName": {
          "type": [
            "null",
            "string"
          ]
        },
        "lastName": {
          "type": [
            "null",
            "string"
          ]
        }
      }
    },
    "Event": {
      "type": "object",
      "additionalProperties": false,
      "properties": {
        "Type": {
          "type": [
            "null",
            "string"
          ]
        }
      }
    }
  }
}
```

Generated TypeScript code: 

```typescript
import { HubConnection, IStreamResult } from "@aspnet/signalr"

export class ChatHub {
    constructor(private connection: HubConnection) {
    }

    send(message: string): Promise<void> {
        return this.connection.invoke('Send', message);
    }

    addPerson(person: Person): Promise<void> {
        return this.connection.invoke('AddPerson', person);
    }

    getEvents(): IStreamResult<Event> {
        return this.connection.stream('GetEvents');
    }

    registerCallbacks(implementation: IChatHubCallbacks) {
        this.connection.on('Welcome', () => implementation.welcome());
        this.connection.on('Send', (message) => implementation.send(message));
    }

    unregisterCallbacks(implementation: IChatHubCallbacks) {
        this.connection.off('Welcome', () => implementation.welcome());
        this.connection.off('Send', (message) => implementation.send(message));
    }
}

export interface IChatHubCallbacks {
    welcome(): void;
    send(message: string): void;
}

export interface Person {
    firstName: string;
    lastName: string;
}

export interface Event {
    Type: string;
}
```

# Development

## Release new version

1. Update versions with [dnt bump-versions patch](https://github.com/RicoSuter/DNT#bump-versions)
2. Commit to "master" (via PR)
3. Merge into "release" to start nuget.org publish (via PR)
