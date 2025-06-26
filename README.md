[![GitHub](https://img.shields.io/github/v/release/DuncanMcPherson/vectra-vm)](https://github.com/DuncanMcPherson/vectra-vm)
[![Build](https://github.com/DuncanMcPherson/vectra-vm/actions/workflows/release.yaml/badge.svg)](https://github.com/DuncanMcPherson/vectra-vm)

# Vectra.VM

A lightweight virtual machine for executing Vectra bytecode programs.

## Overview

Vectra.VM is a .NET 9.0 virtual machine implementation designed to execute programs compiled to Vectra bytecode format (VBC). It provides an execution environment for interpreting VBC instructions with support for class instantiation, method calls, local variables, and basic arithmetic operations.

## Features

- **Virtual Machine Execution**: Interprets and executes VBC instructions
- **Object-Oriented Support**: Handles class instantiation and method calls
- **Variable Management**: Supports local variable storage and retrieval
- **Arithmetic Operations**: Basic arithmetic operations (add, subtract, multiply, divide)
- **Comparison Operations**: Equality and inequality checks, greater/less than comparisons
- **Extensible Host Interface**: Pluggable execution hosts for I/O operations

## Installation

Vectra.VM is available as a NuGet package:

```
dotnet add package Vectra.VM
```

## Usage

To run a Vectra bytecode program:

```csharp
// Load a bytecode program
var program = LoadVbcProgram(); // Your loading logic here

// Create a virtual machine instance (optionally with a host)
var vm = new VirtualMachine(program, new WindowsConsoleHost());

// Execute the program
vm.Run();
```

## Architecture

The virtual machine consists of several components:

- **VirtualMachine**: Core execution engine that interprets VBC instructions
- **ExecutionContext**: Maintains the state of method execution including locals and operand stack
- **ExecutionHost**: Interface for I/O operations (console output, user input)
- **VbcInstance**: Runtime representation of class instances

## Dependencies

- Vectra.Bytecode (>= 2.6.0): Provides the bytecode models and structures

## License

Please refer to the repository for license information.

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for release history.

## Contributing

Contributions are welcome. Please feel free to submit a Pull Request.
