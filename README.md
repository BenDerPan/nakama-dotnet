Nakama .NET
===========

> .NET client for Nakama server written in C#.

[Nakama](https://github.com/heroiclabs/nakama) is an open-source server designed to power modern games and apps. Features include user accounts, chat, social, matchmaker, realtime multiplayer, and [much more](https://heroiclabs.com).

This client implements the full API and socket options with the server. It's written in C# with minimal dependencies to support Unity, Xamarin, Godot, XNA, and other engines and frameworks.

Full documentation is online - https://heroiclabs.com/docs

## Getting Started

You'll need to setup the server and database before you can connect with the client. The simplest way is to use Docker but have a look at the [server documentation](https://github.com/heroiclabs/nakama#getting-started) for other options.

1. Install and run the servers. Follow these [instructions](https://heroiclabs.com/docs/install-docker-quickstart).

2. Download the client from the [releases page](https://github.com/heroiclabs/nakama-dotnet/releases) and import it into your project. You can also [build from source](#source-builds) with the cake build tool.

3. Use the connection credentials to build a client object.

    ```csharp
    // using Nakama;
    var client = new Client("defaultkey", "127.0.0.1", 7350, false);
    client.Timeout = 10000; // set timeout on requests (default is 5000).
    client.Retries = 5;     // set retries on requests (default is 3).
    ```

## Usage

The client object has many methods to execute various features in the server and also create sockets to open connections with the server.

### Authenticate

There's a variety of ways to [authenticate](https://heroiclabs.com/docs/authentication) with the server. Authentication can create a user if they don't already exist with those credentials. It's also easy to authenticate with a social profile from Google Play Games, Facebook, Game Center, etc.

```csharp
var email = "super@heroes.com";
var password = "batsignal";
var session = await client.AuthenticateEmailAsync(email, password);
System.Console.WriteLine(session);
```

### Sessions

When authenticated the server responds with an auth token (JWT) which contains useful properties and gets deserialized into a `Session` object.

```csharp
System.Console.WriteLine(session.AuthToken); // raw JWT token
System.Console.WriteLine(session.UserId);
System.Console.WriteLine(session.Username);
System.Console.WriteLine("Session has expired: {0}", session.IsExpired);
System.Console.WriteLine("Session expires at: {0}", session.ExpireTime);
```

It is recommended to store the auth token from the session and check at startup if it has expired. If the token has expired you must reauthenticate. The expiry time of the token can be changed as a setting in the server.

```csharp
var authtoken = "restored from somewhere";
var session = Session.Restore(authtoken);
if (session.IsExpired)
{
    System.Console.WriteLine("Session has expired. Must reauthenticate!");
}
```

### Requests

The client includes lots of builtin APIs for various features of the game server. These can be accessed with the async methods. It is also possible to call RPC functions for custom logic registered with the server as well as with a socket object.

All requests are sent with a session object which authorizes the client.

```csharp
var account = await client.GetAccountAsync(session);
System.Console.WriteLine(account.User.Id);
System.Console.WriteLine(account.User.Username);
System.Console.WriteLine(account.Wallet);
```

### Socket

The client can create one or more sockets with the server. Each socket can have it's own event listeners registered for responses received from the server.

```csharp
var appearOnline = false;
var reconnects = 5; // set reconnect attempts (default is 3).
var socket = client.CreateWebSocketAsync(session, appearOnline, reconnects);
```

## Contribute

The development roadmap is managed as GitHub issues and pull requests are welcome. If you're interested to enhance the code please open an issue to discuss the changes or drop in and discuss it in the [community chat](https://gitter.im/heroiclabs/nakama).

### Source Builds

The codebase can be built with [Cake](https://cakebuild.net). All dependencies are downloaded at build time with Nuget.

```shell
./build.sh --target=Build
```

### Run Tests

To run tests you will need to run the server and database. Most tests are written as integration tests which execute against the server. A quick approach we use with our test workflow is to use the Docker compose file described in the [documentation](https://heroiclabs.com/docs/install-docker-quickstart).

```shell
docker-compose -f ./docker-compose.yml up
./build.sh --target=Run-Unit-Tests
```

### License

This project is licensed under the [Apache-2 License](https://github.com/heroiclabs/nakama-dotnet/blob/master/LICENSE).

### Special Thanks

Thanks to Alex Parker (@zanders3) for the excellent [json](https://github.com/zanders3/json) library.
