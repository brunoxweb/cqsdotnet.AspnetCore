# CQSDotnet.AspnetCore

CQSDotnet.AspnetCore is an extension for the CQSDotnet package, designed to integrate Command Query Separation (CQS) with the Microsoft.Extensions.DependencyInjection for .NET applications. This extension simplifies the implementation of the CQS pattern by providing seamless integration with Microsoft.Extensions.DependencyInjection, allowing you to cleanly and efficiently manage commands and queries in your .NET projects.

## Getting Started
To use CQSDotnet.AspnetCore in your project, follow these simple steps:

## Installation

```
dotnet add package CQSDotnet
dotnet add package CQSDotnet.AspnetCore
```

## Usage
Create an instance of the IServiceCollection in your application:

```
var services = new ServiceCollection();
services.CQSDotnetRegister();
```
## Example

```
private readonly IQueryDispatcher queryDispatcher;
private readonly ICommandDispatcher commandDispatcher;

public ExampleController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
{
    this.queryDispatcher = queryDispatcher;
    this.commandDispatcher = commandDispatcher;
}

[HttpGet]
public async Task<IHttpActionResult> GetAsync()
{
    var query = new MyQuery();
    var response = await this.queryDispatcher
        .ExecuteAsync<MyQuery, IEnumerable<MyDto>>(query, CancellationToken.None);
    return Ok(response);
}

[HttpPost]
public async Task<IHttpActionResult> PostAsync()
{
    var command = new MyCommand();
    await this.commandDispatcher.ExecuteAsync<MyCommand>(command, CancellationToken.None);
    return Ok();
}

// Implement your queries and commands
public class MyQuery : IQuery<IEnumerable<MyDto>> { }
public class MyQueryHandler : IQueryHandler<MyQuery, IEnumerable<MyDto>> { }
public class MyCommand : ICommand { }
public class MyCommandHandler : ICommandHandler<MyCommand> { }
```
And that's it! You've successfully integrated CQSDotnet with AspnetCore in your application. Now you can use the CQS pattern to manage your commands and queries in a clean and organized way.

## Support and Issues
If you encounter any issues, have questions, or want to contribute, please visit the GitHub repository. We appreciate your feedback and contributions to help improve this library.

## License
CQSDotnet.AspnetCore is licensed under the MIT License.
