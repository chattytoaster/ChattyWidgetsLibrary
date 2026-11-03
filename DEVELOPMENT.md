# 🛠 Руководство разработчика виджетов для ChattyWidgets

Добро пожаловать в руководство по созданию виджетов для платформы **ChattyWidgets**! Здесь описано всё, что нужно для создания стильных, производительных и безопасных виджетов.

---

## 📑 Содержание
1. [Анатомия виджета](#-анатомия-виджета)
2. [Манифест виджета (`manifest.json`)](#-манифест-виджета-manifestjson)
3. [Стандарт пропорций и адаптивности (4:3)](#-стандарт-пропорций-и-адаптивности-43)
4. [Правила пространства имён и авторизации](#-правила-пространства-имён-и-авторизации)
5. [JavaScript Bridge API (`window.chatty`)](#-javascript-bridge-api-windowchatty)
6. [Стилизация и оформление (Glassmorphism)](#-стилизация-и-оформление-glassmorphism)
7. [Локальное тестирование и проверка](#-локальное-тестирование-и-проверка)
8. [Публикация в каталоге сообщества](#-публикация-в-каталоге-сообщества)

---

## 📦 Анатомия виджета

Виджет представляет собой обычную веб-страницу (HTML5, CSS3, JavaScript) или нативный XAML-компонент, упакованный в архив с расширением `.chw`.

Структура каталога виджета:
```text
widgets/com.github.<ваш_логин>.<имя_виджета>/
├── manifest.json       # Обязательно: конфигурация, размеры, метаданные
├── index.html          # Обязательно: точка входа веб-виджета
├── icon.png            # Обязательно: квадратная иконка (128x128 или 256x256 px)
├── style.css           # Ваши стили
└── script.js           # Ваша логика
```

---

## 📜 Манифест виджета (`manifest.json`)

Файл `manifest.json` описывает параметры окна, размеры и поведение виджета:

```json
{
  "id": "com.github.ivan.crypto-rates",
  "name": "Живые Котировки",
  "version": "1.0.0",
  "author": "@ivan",
  "description": "Мониторинг курсов Bitcoin, Ethereum и Gram в реальном времени.",
  "category": "Финансы",
  "type": "web",
  "entry": "index.html",
  "icon": "icon.png",
  "defaultWidth": 320,
  "defaultHeight": 240,
  "minWidth": 240,
  "minHeight": 180,
  "resizable": true,
  "defaultLayer": "desktop",
  "clickThrough": false,
  "permissions": ["network"],
  "menuItems": [
    { "id": "refresh-rates", "title": "Обновить котировки" },
    { "id": "toggle-currency", "title": "Сменить валюту (USD/RUB)" }
  ]
}
```

### Основные поля манифеста:
| Поле | Тип | Описание |
| :--- | :--- | :--- |
| `id` | `string` | Уникальный идентификатор виджета (см. правила ниже). |
| `name` | `string` | Отображаемое название виджета в магазине и окне. |
| `version` | `string` | Версия в формате SemVer (`1.0.0`). |
| `author` | `string` | Имя автора (автоматически проверяется и связывается с GitHub). |
| `type` | `string` | Тип виджета: `"web"` (HTML/JS) или `"native"` (XAML). |
| `entry` | `string` | Главный файл запуска (обычно `index.html` или `Widget.xaml`). |
| `defaultWidth` / `defaultHeight` | `number` | Начальные размеры окна в пикселях (**320 × 240** — единый стандарт пропорций 4:3). |
| `minWidth` / `minHeight` | `number` | Минимальные размеры окна (**240 × 180** — пропорция 4:3 для компактного режима 75%). |
| `defaultLayer` | `string` | Слой по умолчанию: `"desktop"` (на рабочем столе), `"normal"` (окно), `"top"` (поверх всех окон). |
| `clickThrough` | `boolean` | `true`, если клики должны проходить насквозь на рабочий стол. |
| `menuItems` | `array` | Пользовательские пункты контекстного меню ПКМ. |

---

## 📐 Стандарт пропорций и адаптивности (4:3)

Для безупречной эстетики рабочего стола платформа ChattyWidgets использует **единый стандарт пропорций 4:3**.

### Зачем нужна пропорциональность?
Пользователи часто выстраивают виджеты в **один аккуратный горизонтальный ряд** вдоль верхней или нижней границы экрана. Если виджеты имеют разную высоту или случайные пропорции (например, один 300×140, другой 280×220, третий 340×200), строка выглядит «лесенкой» и теряет аккуратный вид. 

Единый формат **320×240** гарантирует:
- Все карточки в одном ряду имеют строго одинаковую высоту и пропорции.
- Идеальное масштабирование через системные пресеты в меню ПКМ:
  - **75% (Компактный)**: `240 × 180 px`
  - **100% (Обычный)**: `320 × 240 px`
  - **125% (Увеличенный)**: `400 × 300 px`
  - **150% (Большой)**: `480 × 360 px`

### 💡 Правила адаптивной верстки (XAML и Web)

1. **Не используйте фиксированную высоту внутренних элементов**:
   - Избегайте жестких значений вроде `Height="240"` на внутренних контейнерах.
   - В XAML используйте сетки `Grid` с пропорциональными строками/колонками (`*` и `Auto`), `UniformGrid` или `DockPanel`.
   - В Web используйте Flexbox (`display: flex`) или CSS Grid с относительными единицами (`%`, `fr`, `clamp()`).

2. **Масштабирование круговых шкал и векторной графики (`Viewbox`)**:
   - Векторные элементы (круговые индикаторы CPU/RAM, аналоговые циферблаты, большие значки) оборачивайте в `<Viewbox Stretch="Uniform">`. Это гарантирует, что датчики плавно уменьшатся при переключении на режим 75% (`240×180`) и не обрежутся.

3. **Защита текста от переполнения**:
   - Длинные заголовки, названия песен и новости защищайте обрезкой с многоточием: `TextTrimming="CharacterEllipsis"` (в XAML) или `text-overflow: ellipsis; white-space: nowrap; overflow: hidden;` (в CSS).
   - Для бегущей строки используйте встроенное поведение: `controls:MarqueeBehavior.EnableMarquee="True"` (родительский контейнер обязан иметь `ClipToBounds="True"`).

4. **Идеальные скругления углов при уменьшении размера**:
   - Базовый радиус скругления внешней границы: `CornerRadius="20"`.
   - В WPF `Border.ClipToBounds="True"` не скругляет растровые изображения внутри контейнера. При использовании `Grid.OpacityMask` задавайте `Stretch="None" AlignmentX="Left" AlignmentY="Top"` с привязкой через `ElementName` и внутренним радиусом `CornerRadius="19"`:
     ```xaml
     <Grid x:Name="MediaGrid">
         <Grid.OpacityMask>
             <VisualBrush Stretch="None" AlignmentX="Left" AlignmentY="Top">
                 <VisualBrush.Visual>
                     <Border Background="Black" CornerRadius="19"
                             Width="{Binding ActualWidth, ElementName=MediaGrid}"
                             Height="{Binding ActualHeight, ElementName=MediaGrid}"/>
                 </VisualBrush.Visual>
             </VisualBrush>
         </Grid.OpacityMask>
     ```
   - Любые пристыкованные снизу панели обязаны иметь нижний радиус скругления (например, `CornerRadius="0,0,19,19"`), иначе их прямые углы будут вылезать за пределы рамки при динамическом сжатии.

---

## 🔒 Правила пространства имён и авторизации

Чтобы никто не мог выдать свой виджет за официальный или подделать имя другого автора, в платформе действуют строгие правила:

1. **Официальные виджеты платформы**:
   - Префикс `com.chatty.*` зарезервирован исключительно за разработчиками ChattyWidgets (`chattytoaster`).
   - Официальные виджеты подписываются закрытым криптографическим RSA-ключом.

2. **Виджеты сообщества**:
   - Идентификатор `id` **обязан** начинаться с вашего реального логина на GitHub:
     - `com.github.<ваш_логин>.<имя_виджета>` или `<ваш_логин>.<имя_виджета>`.
   - При создании Pull Request робот сверяет ID с вашим авторизованным GitHub-аккаунтом. Если они не совпадают, PR отклоняется автоматически.
   - Поле `author` при сборке каталога принудительно заменяется на `@<ваш_логин>` со ссылкой на ваш GitHub.

---

## ⚡ JavaScript Bridge API (`window.chatty`)

В каждый веб-виджет среда ChattyWidgets автоматически внедряет объект `window.chatty` с синхронными методами, реактивными событиями и удобными хелперами.

### 1. Реактивные события в реальном времени (Event Subscriptions)

Подписывайтесь на системные события без опроса в таймере:

```javascript
// Подписка на изменение метрик системы (CPU, RAM, GPU, сеть)
window.chatty.on('metrics', (metrics) => {
    console.log('Загрузка CPU:', metrics.CpuPercent, 'GPU:', metrics.GpuPercent);
});

// Подписка на смену трека или статуса воспроизведения (GSMTC)
window.chatty.on('media', (media) => {
    console.log('Сейчас играет:', media.Title, media.Artist, media.IsPlaying);
});

// Подписка на изменение настроек пользователем в окне параметров
window.chatty.on('settingsChanged', () => {
    const settings = window.chatty.getSettings();
});

// Отписка
const handler = window.chatty.on('metrics', (m) => { /* ... */ });
window.chatty.off('metrics', handler);
```

### 2. Мониторинг системы (Hardware & System API)

```javascript
// Быстрый опрос перегрузки процессора и видеокарты
const isOverloaded = window.chatty.isHardwareOverloaded(80.0); // true, если CPU >= 80% или GPU >= 80%
const cpu = window.chatty.getCpu();                           // % загрузки CPU
const cpuModel = window.chatty.getCpuModel();                 // Например: "Intel(R) Core(TM) i7-12700H"

// Оперативная память и накопитель
const ramPercent = window.chatty.getMemoryPercent();          // % загрузки RAM
const ramUsed = window.chatty.getMemoryUsed();                // Использовано RAM в ГБ
const ramTotal = window.chatty.getMemoryTotal();              // Всего RAM в ГБ
const diskPercent = window.chatty.getDiskPercent();           // % использования диска
const diskFree = window.chatty.getDiskFree();                 // Свободно на диске в ГБ

// Видеокарта (GPU)
const gpu = window.chatty.getGpuPercent();                    // % загрузки GPU
const gpuName = window.chatty.getGpuName();                   // Имя адаптера (например: "NVIDIA GeForce RTX 4070")
const gpuTemp = window.chatty.getGpuTemperature();            // Температура GPU в °C

// Сеть
const netDown = window.chatty.getNetDownload();               // Скорость загрузки (КБ/с)
const netUp = window.chatty.getNetUpload();                   // Скорость отдачи (КБ/с)

// Активность пользователя и ОС
const idleSeconds = window.chatty.getUserIdleSeconds();       // Секунд с момента последнего ввода (мышь/клавиатура)
const isIdle = window.chatty.isUserIdle(60.0);                // true, если пользователь бездействует > 60 сек
const osVer = window.chatty.getOsVersion();                   // "Windows 11 (Build 22631)"
const uptimeSec = window.chatty.getSystemUptimeSeconds();     // Время работы ПК в секундах

// Получить все метрики системы в виде готового JS-объекта
const metrics = window.chatty.getMetrics();
```

### 3. Мультимедиа (GSMTC Media API)

Управление воспроизведением треков (Spotify, Яндекс Музыка, VK Музыка, браузеры, системные плееры):

```javascript
// Проверка состояния
const isPlaying = window.chatty.isMusicPlaying();             // true, если музыка воспроизводится
const trackTitle = window.chatty.getTrackTitle();             // Название трека
const trackArtist = window.chatty.getTrackArtist();           // Исполнитель
const trackAlbum = window.chatty.getTrackAlbum();             // Альбом
const thumbnail = window.chatty.getTrackThumbnail();          // Обложка трека (base64 data URI)
const volume = window.chatty.getVolume();                     // Системная громкость (0-100)

// Управление
window.chatty.playPause();                                    // Пауза / Воспроизведение
window.chatty.nextTrack();                                    // Следующий трек
window.chatty.prevTrack();                                    // Предыдущий трек
window.chatty.volumeUp();                                     // Увеличить громкость
window.chatty.volumeDown();                                   // Уменьшить громкость
window.chatty.mute();                                         // Заглушить / включить звук

// Получить полный объект текущего медиа
const media = window.chatty.getMedia();
```

### 4. Звуковые эффекты и голосовой синтез (SFX & Speech API)

```javascript
// Воспроизведение системных звуков Windows или локального .wav файла
window.chatty.beep();                                         // Системный звуковой сигнал (Beep)
window.chatty.playSound("asterisk");                          // Системное уведомление (info)
window.chatty.playSound("exclamation");                       // Предупреждение (warning)
window.chatty.playSound("hand");                              // Звук ошибки (error)
window.chatty.playSound("sounds/purr.wav");                   // Воспроизведение своего WAV-файла из папки виджета

// Синтез речи (Text-to-Speech) через системный Windows SAPI voice
window.chatty.ttsSpeak("Привет! Я ваш новый рабочий виджет.");
```

### 5. Время и фазы суток (Time & Schedule API)

```javascript
// Проверка ночного режима (с 22:00 до 07:00)
const isNight = window.chatty.isNightTime();

// Полный объект времени
const time = window.chatty.getTime();
// time = { hour, minute, second, millisecond, day, month, year, dayOfWeek, dayName, isNightTime, timestamp, iso }
```

### 6. Управление окном, анимации и геометрия

```javascript
// Геометрия окна и экранов
const bounds = window.chatty.getBounds();                     // { x, y, width, height }
const screen = window.chatty.getScreen();                     // { primaryWidth, primaryHeight, currentWidth, currentHeight, workAreaWidth, workAreaHeight }

// Эффекты и позиционирование
window.chatty.shake(400);                                     // Тряска виджета (например, при клике или тревоге)
window.chatty.centerOnScreen();                               // Центрировать виджет на текущем экране
window.chatty.setPosition(100, 200);                          // Переместить окно по координатам
window.chatty.resize(320, 240);                               // Изменить размер виджета
window.chatty.dragMove();                                     // Начать перетаскивание окна мышью

// Режимы окна
window.chatty.setLocked(true);                                // Зафиксировать виджет от случайного перемещения
window.chatty.setClickThrough(true);                          // Включить режим "Клик насквозь"
window.chatty.setAutoHideDot(true);                           // Скрывать синюю точку управления (показывать только при hover)
window.chatty.setOpacity(0.9);                                // Настроить прозрачность
window.chatty.showContextMenu();                              // Открыть контекстное меню ChattyWidgets
window.chatty.closeWidget();                                  // Закрыть виджет
window.chatty.reload();                                       // Перезагрузить страницу виджета
window.chatty.openDevTools();                                 // Открыть DevTools Chromium (F12)
```

### 7. Файловое хранилище данных (Data Storage & File I/O)

Каждый экземпляр виджета изолирован и имеет безопасную постоянную директорию данных:

```javascript
// Быстрое сохранение ключ-значение
window.chatty.storageSet('highScore', '1500');
const score = window.chatty.storageGet('highScore', '0');
window.chatty.storageRemove('highScore');
const allStorage = window.chatty.getStorageAll();             // Все ключи хранилища в виде объекта
window.chatty.storageClear();                                 // Очистить хранилище

// Произвольные текстовые файлы (JSON, CSV, лог)
window.chatty.writeDataText('pet_stats.json', JSON.stringify({ hunger: 85, energy: 90 }));
const petStatsJson = window.chatty.readDataText('pet_stats.json', '{}');
```

### 8. Пользовательские параметры (Settings API)

```javascript
// Чтение и запись параметров из манифеста виджета
const petType = window.chatty.getSetting('PetType');
window.chatty.setSetting('PetType', 'cat');

// Пакетное получение всех настроек в виде JS-объекта
const allSettings = window.chatty.getSettings();
```

### 9. Системные утилиты и интеграция

```javascript
window.chatty.notify('Напоминание', 'Пора размяться!');        // Системный тост в трее Windows
window.chatty.clipboardWrite('Скопированный текст');          // Запись в буфер обмена Windows
const clip = window.chatty.clipboardRead();                   // Чтение из буфера обмена Windows
const lang = window.chatty.getLanguage();                     // "ru" или "en"
const isRu = window.chatty.isRussian();                       // true, если язык интерфейса русский
const accent = window.chatty.getWindowsAccentColor();         // Акцентный цвет Windows (например: "#6366F1")
const battery = window.chatty.getBatteryLevel();              // % заряда батареи (0-100)
const isCharging = window.chatty.isBatteryCharging();         // true, если ноутбук на зарядке
window.chatty.openUrl('https://github.com');                  // Открыть ссылку в браузере по умолчанию
```

### 10. Обработка пунктов контекстного меню

Когда пользователь выбирает ваш кастомный пункт меню (из `menuItems` в манифесте), в окне вызывается событие:
```javascript
window.addEventListener('chatty:menu', function(e) {
    const menuId = e.detail.id;
    if (menuId === 'refresh-rates') {
        fetchRates();
    }
});
```

---

## 🎨 Стилизация и оформление (Glassmorphism)

Приложение ChattyWidgets оформлено в стиле **темного матового стекла**. Рекомендуется придерживаться общего визуального стиля:

- **Фон карточки**: полупрозрачный темный фон с размытием:
  ```css
  background: rgba(18, 22, 34, 0.75);
  backdrop-filter: blur(20px);
  border: 1px solid rgba(255, 255, 255, 0.08);
  border-radius: 18px;
  ```
- **Шрифты**: современный системный стек sans-serif:
  ```css
  font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif;
  color: #F8FAFC;
  ```
- **Точка управления**:
  В правом верхнем углу (12 px от краев) располагается синяя точка управления. Не размещайте важные интерактивные кнопки впритык к правому верхнему углу (оставьте отступ 30-36 px).

---

## 🧪 Локальное тестирование и проверка

Перед отправкой виджета в репозиторий протестируйте его встроенными инструментами:

```bash
# 1. Проверка структуры манифеста, безопасности и лимитов
python scripts/validate_widgets.py

# 2. Локальная сборка .chw пакета и проверка catalog.json
python scripts/build_catalog.py
```

Также в самом приложении **ChattyWidgets** есть встроенная **«Мастерская Разработчика»**:
1. Откройте приложение ChattyWidgets.
2. Перейдите во вкладку **«Мастерская»**.
3. Нажмите **«Создать шаблон виджета»** или **«Собрать .chw»** для проверки вашего виджета на живом рабочем столе.

---

## 🚀 Публикация в каталоге сообщества

1. Сделайте **Fork** репозитория [`ChattyWidgetsLibrary`](https://github.com/chattytoaster/ChattyWidgetsLibrary).
2. Поместите папку с вашим виджетом в `widgets/com.github.<ваш_логин>.<имя_виджета>/`.
3. Создайте **Pull Request**.
4. GitHub Actions автоматически проверит структуру и безопасность виджета.
5. После одобрения модератором виджет будет собран и появится в Магазине ChattyWidgets у всех пользователей мира!
