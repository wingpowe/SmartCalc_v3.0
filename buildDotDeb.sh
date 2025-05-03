#!/bin/bash

mkdir -p smartcalc/DEBIAN
mkdir -p smartcalc/usr/local/bin
mkdir -p smartcalc/usr/share/applications
mkdir -p smartcalc/usr/share/icons/hicolor/256x256/apps

echo "Package: smartcalc
Version: 1.0.0
Section: base
Priority: optional
Architecture: amd64
Depends: libgtk-3-0, libgdk-pixbuf2.0-0, libglib2.0-0
Maintainer: alyshaor <alyshaor@student.21-school.ru>
Description: SmartCalc Application
 A simple calculator application." > smartcalc/DEBIAN/control

dotnet publish -c Release -r linux-x64 --self-contained --output smartcalc/usr/local/bin/runtimes/linux-x64/native/ ./SmartCalc.Gui/SmartCalc.Gui.csproj


echo "[Desktop Entry]
Name=SmartCalc
Comment=A simple calculator application
Exec=/usr/local/bin/runtimes/linux-x64/native/SmartCalc.Gui
WorkingDirectory=/usr/local/bin/runtimes/linux-x64/native/
Icon=smartcalc
Terminal=false
Type=Application
Categories=Utility;Application;" > smartcalc/usr/share/applications/smartcalc.desktop

cp ./icon.png smartcalc/usr/share/icons/hicolor/256x256/apps/smartcalc.png

dpkg-deb --build smartcalc
