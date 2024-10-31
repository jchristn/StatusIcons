<img src="https://github.com/jchristn/StatusIcons/blob/main/Assets/icon.png?raw=true" height="48">

# StatusIcons

[![NuGet Version](https://img.shields.io/nuget/v/StatusIcons.svg?style=flat)](https://www.nuget.org/packages/StatusIcons/) [![NuGet](https://img.shields.io/nuget/dt/StatusIcons.svg)](https://www.nuget.org/packages/StatusIcons) 

Simple C# class library for adding Unicode status icons to console applications, or falling back to ASCII if the terminal doesn't support Unicode.

## Help or Feedback

First things first - do you need help or have feedback?  File an issue here!  We'd love to hear from you.

Have an idea for amending the default set of icons?  Let us know!

## New in v1.0.x

- Initial release

## It's Really Easy...  I Mean, REALLY Easy

```csharp
```

## Usage

Refer to the `Test` project for full usage details.  If Unicode is supported in your terminal, retrieving an icon via `StatusIcon["name"]` will return the Unicode icon, and if not, it will return the ASCII variant.

```csharp
using StatusIcons;

// Display an icon
StatusIcon icon = new StatusIcon();
Console.WriteLine("Success: " + icon["Success"]);

// Test your terminal for Unicode support
icon.TestTerminal();

// Add and use an icon
icon["Speaker"] = "🔊";  // add to whichever dictionary is in use
icon.AsciiIcons["Speaker"] = ":(";  // add only to the ASCII icon dictionary
icon.UnicodeIcons["Speaker"] = "🔊";  // add only to the Unicode icon dictionary
Console.WriteLine("Icon 'Speaker' : " + icon["Speaker"]);
```

## Default Icons

The following icons are added by default.

| Key       | Unicode | ASCII |
|-----------|---------|-------|
| Success   | ✓ | + |
| Error     | ✗ | X |
| Warning   | ⚠ | ! |
| Info      | ℹ | i |
| Working   | ⋯ | . |
| Bullet    | • | * |
| Arrow     | → | > |

## Version History

Please refer to CHANGELOG.md.
