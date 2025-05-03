#!/bin/bash
# Публикация проекта
dotnet publish -c Release -r osx-x64 --self-contained

# Название приложения
APP_NAME="SmartCalc"

# Путь к опубликованным файлам
PUBLISH_DIR="SmartCalc.Gui/bin/Release/net8.0/osx-x64/publish"

# Создание структуры директорий для .app пакета
mkdir -p "$APP_NAME.app/Contents/MacOS"
mkdir -p "$APP_NAME.app/Contents/Resources"

# Копирование опубликованных файлов в .app пакет
cp -R "$PUBLISH_DIR"/* "$APP_NAME.app/Contents/MacOS/"

# Создание Info.plist
cat <<EOL > "$APP_NAME.app/Contents/Info.plist"
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>CFBundleName</key>
    <string>$APP_NAME</string>
    <key>CFBundleDisplayName</key>
    <string>$APP_NAME</string>
    <key>CFBundleExecutable</key>
    <string>$APP_NAME</string>
    <key>CFBundleIdentifier</key>
    <string>com.yourcompany.$APP_NAME</string>
    <key>CFBundleVersion</key>
    <string>1.0</string>
    <key>CFBundlePackageType</key>
    <string>APPL</string>
    <key>CFBundleSignature</key>
    <string>MYAP</string>
    <key>CFBundleInfoDictionaryVersion</key>
    <string>6.0</string>
    <key>LSMinimumSystemVersion</key>
    <string>10.12</string>
</dict>
</plist>
EOL

echo "Структура директорий для $APP_NAME.app создана."

# Создание скрипта установки
mkdir -p scripts
cat <<EOL > scripts/postinstall
#!/bin/bash
INSTALL_DIR="/Applications/$APP_NAME.app"
mv "$HOME/Applications/$APP_NAME/Contents/MacOS/SmartCalc.Gui" "$HOME/Applications/$APP_NAME/Contents/MacOS/SmartCalc.app"
ln -sf "$HOME/Applications/$APP_NAME/Contents/MacOS/SmartCalc.app" "$HOME/Desktop/\$APP_NAME"
chmod -R 777 "$HOME/Desktop/\$APP_NAME"
chmod -R 777 $HOME/Applications/*
EOL

chmod +x scripts/postinstall

# Создание пакета
pkgbuild --root "$APP_NAME.app" --install-location $HOME/Applications/$APP_NAME --scripts ./scripts --identifier com.alyshaor.$APP_NAME --version 3.0 "$APP_NAME.pkg"

# Удаление временных файлов
rm -rf scripts

echo "Пакет создан: $APP_NAME.pkg"
echo "Для удаления используйте следующие команды:"
echo "rm -rf \"$HOME/Applications/$APP_NAME\""
echo "rm -f \"\$HOME/Desktop/$APP_NAME\""