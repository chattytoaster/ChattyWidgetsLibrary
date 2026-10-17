# 🏪 ChattyWidgets Community Library & Catalog

> **Language / Язык:** **[🇬🇧 English](README.md)** • **[🇷🇺 Русский](README.ru.md)**

Official catalog and open-source community widget repository for **[ChattyWidgets](https://github.com/chattytoaster/ChattyWidgets)**.

Every widget in this repository is cryptographically signed with the official RSA key and available for instant 1-click installation from the in-app Store.

---

## 📦 Official Widgets Catalog (12 Widgets)

| Icon | Name | ID | Type | Description |
| :---: | :--- | :--- | :---: | :--- |
| ⏱ | **Glass Clock** | `com.chatty.clock-glass` | Native | Elegant digital clock with world timezones, localized date, and seconds toggle |
| 📅 | **Glass Calendar** | `com.chatty.calendar-glass` | Native | Desktop calendar with first day of week selection (Mon/Sun), week numbers, and notes |
| 🌤 | **Weather Card** | `com.chatty.weather-card` | Native | Detailed weather forecast via Open-Meteo API with global city search and standard units (m/s, km/h, mmHg) |
| 💎 | **Crypto & Currency** | `com.chatty.crypto-tracker` | Native | Real-time prices for crypto (BTC, ETH, SOL, GRAM, DOGE, XRP...) and fiat currencies (USD, EUR, RUB, CNY, KZT) |
| 🎵 | **Mini Music Player** | `com.chatty.music-player` | Native | Windows SMTC media controller (Spotify, Yandex.Music, browsers) with spinning vinyl and marquee text |
| ⏳ | **Timer & Pomodoro** | `com.chatty.pomodoro-timer` | Native | Regular countdown timer on 1st place with quick +/- buttons, Focus and Break modes with audio alerts |
| 📝 | **Sticky Notes & Tasks** | `com.chatty.sticky-notes` | Native | Compact desktop task checklist with persistent state, task counter, and 5 color themes |
| 🚀 | **Quick Dock / Launcher** | `com.chatty.quick-dock` | Native | Floating quick-launch dock for apps, terminal, file manager, and websites with file picker |
| ⚡ | **System Gauges** | `com.chatty.system-gauges` | Native | Ultra-lightweight hardware monitor for CPU, RAM, disk, and network. Circular rings (2x2 grid) & linear bars |
| 🎮 | **GPU Monitor** | `com.chatty.gpu-monitor` | Native | DirectX/DWM graphics monitor: 3D Core %, dedicated VRAM, and temperature with rings and bars |
| 🖼 | **Media Photo Frame** | `com.chatty.media-frame` | Native | Desktop photo frame with rounded corners, Drag & Drop, single photo / folder mode, and auto-hiding controls |
| 📰 | **News Feed & RSS** | `com.chatty.rss-reader` | Native | Live news ticker with auto-scroll and curated international channels (UN News in Russian, Euronews, Hacker News, Habr, NASA) |

---

## 🛠 How to Publish Your Own Widget

Anyone can contribute and publish a widget to this catalog!
Detailed step-by-step instructions are available in **[CONTRIBUTING.md](CONTRIBUTING.md)**.

### Quick Publishing Steps:
1. **Fork** this repository.
2. Create a folder for your widget in `widgets/<id>/` (e.g. `widgets/com.author.my-widget/`).
3. Add `manifest.json`, icon `icon.png` (64x64 or 128x128), and entry point (`Widget.xaml` for native widgets or `index.html` for web widgets).
4. Run catalog builder script:
   ```bash
   python scripts/build_catalog.py
   ```
5. Submit a **Pull Request** to `main`.
6. Once validated, GitHub Actions will automatically package the signed `.chw` file and update `catalog.json`. The widget will immediately appear in the in-app Store for all users!

---

## 🔗 Direct Catalog URL for ChattyWidgets

The ChattyWidgets application loads the latest catalog from:
```
https://raw.githubusercontent.com/chattytoaster/ChattyWidgetsLibrary/main/catalog.json
```
