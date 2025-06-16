# DumpertMCP

DumpertMCP is a .NET 8.0-powered Model Context Protocol (MCP) Server designed for seamless interaction with the Dumpert.nl API. It enables you to fetch the latest videos, comments, soundboard items, and more from Dumpert. Dumpert.nl is a popular Dutch video-sharing platform.

DumpertMCP is fully compatible with MCP-enabled large language models (LLMs), including popular options like OpenAI's ChatGPT, Mistral, Claude, GitHub Copilot, Microsoft Copilot and LLaMA. This makes it easy to integrate Dumpert content into your AI-driven workflows, chatbots, or analysis pipelines with minimal effort.

## Features
- Get Kudotoppers
- Fetch latest Dumpert TV episodes and metadata
- Retrieve and display comments (reaguursels) and authors
- Access Dumpert soundboard items
- Easily extensible for new endpoints

## Requirements
- .NET 8.0 SDK or later
- Internet connection (for API access)

## Testing
To tests, you can use the following command:

```bash
npx @modelcontextprotocol/inspector dotnet run
```

## Use Docker version (No need to download this whole repository)

Add the following MCP server to your LLM's configuration:

```json
"servers": {
 "Dumpert": {
        "command": "docker",
        "args": [
            "run",
            "-i",
            "--rm",
            "roffel/dumpertmcp"
        ],
        "env": {}
    }
}
```

## Run from source

Replace the path to the DumpertMCP.csproj file with the path to your local copy of the project. You can add the server to your LLM's configuration like this:

```json

"servers": {
    "Dumpert": {
        "type": "stdio",
        "command": "dotnet",
        "args": [
            "run",
            "--project",
            "/Users/rutger/github/dumpert-mcp/DumpertMCP.csproj"
        ]
    }
}
```


## Contributing
Pull requests and issues are welcome! Please open an issue to discuss your ideas or report bugs.

## License
This project is licensed under the MIT License.

---

*DumpertMCP is not affiliated with Dumpert.nl. This is an unofficial project for educational and personal use.*

