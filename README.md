# SmartCalc v3.0

A powerful calculator application developed in C# with cross-platform support for Windows, macOS, and Linux.

![SmartCalc v3.0](icon.png)

## Features

- **Standard Calculator Functions**: Supports basic arithmetic operations and complex mathematical expressions
- **Function Graphing**: Plot functions with variables to visualize mathematical relationships
- **Credit Calculator**: Calculate loan payments, total interest, and other loan-related metrics
- **Deposit Calculator**: Calculate deposit earnings with various parameters
- **Cross-Platform Support**: Works on Windows, macOS, and Linux
- **History Tracking**: Save and reload previous calculations
- **Configurable Settings**: Customize appearance and behavior
- **Logging**: Track operations with configurable log rotation

## Supported Operations

### Arithmetic Operations
- Addition (+)
- Subtraction (-)
- Multiplication (*)
- Division (/)
- Power (^)
- Modulus (mod)
- Unary plus (+)
- Unary minus (-)

### Mathematical Functions
- Cosine (cos)
- Sine (sin)
- Tangent (tan)
- Arccosine (acos)
- Arcsine (asin)
- Arctangent (atan)
- Square root (sqrt)
- Natural logarithm (ln)
- Common logarithm (log)

## System Requirements

- .NET 8.0 Runtime
- For Linux: GTK3
- Windows 10 or later
- macOS 10.15 or later
- Ubuntu 20.04 or compatible Linux distribution

## Installation

### Windows
1. Download the installer from [Windows Installer](https://cloud.mail.ru/public/aFBh/MEhwP1PMx)
2. Run the installer and follow the on-screen instructions
3. Launch the application from the Start menu

### macOS
1. Download the application from [macOS App](https://cloud.mail.ru/public/AUzE/kw56deANz)
2. Mount the disk image and drag the application to the Applications folder
3. Launch from the Applications folder

### Linux (Ubuntu/Debian)
1. Download the .deb package from [Ubuntu Package](https://cloud.mail.ru/public/WKNL/2qA8CwSuX)
2. Install with:
   ```
   sudo dpkg -i smartcalc.deb
   sudo apt-get install -f  # To resolve dependencies if needed
   ```
3. Launch from the Applications menu or run `SmartCalc.Gui` in terminal

## Building from Source

### Prerequisites
- .NET 8.0 SDK
- For Linux builds: GTK3 development packages

### Windows Build
```powershell
# Clone the repository
git clone <repository-url>
cd SmartCalcV3.0

# Build the project
dotnet build -c Release
dotnet publish -c Release -r win-x64 --self-contained
```

### Linux Build
```bash
# Clone the repository
git clone <repository-url>
cd SmartCalcV3.0

# Build using make
make

# Or build manually
dotnet build -c Release
dotnet publish -c Release -r linux-x64 --self-contained

# Create a .deb package
./buildDotDeb.sh
```

### macOS Build
```bash
# Clone the repository
git clone <repository-url>
cd SmartCalcV3.0

# Build the project
dotnet build -c Release
dotnet publish -c Release -r osx-x64 --self-contained

# Create an app bundle
./create_app.sh
```

## Configuration

The application can be configured by editing the `appsettings.json` file:

```json
{
  "Settings": {
    "BackgroundColor": "#FFFFFF",
    "FontSize": 33,
    "LogRotationPeriod": "Day"
  }
}
```

- **BackgroundColor**: Hex color code for the application background
- **FontSize**: Size of the font used in the application
- **LogRotationPeriod**: Period for log rotation (Hour/Day/Month)

## Architecture

The project follows the MVVM (Model-View-ViewModel) pattern:
- **Model**: C/C++ core with C# wrapper for calculations (SmartCalc.Core)
- **View**: User interface built with Avalonia UI (SmartCalc.Gui)
- **ViewModel**: Data binding and business logic (SmartCalc.Gui/ViewModels)

## License

[MIT License](LICENSE)

## Authors

- Anatoliy Podsokhin 